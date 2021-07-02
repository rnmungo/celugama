using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using RestSharp;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Platform.Clients;
using CeluGamaSystem.Services.Interfaces;

namespace CeluGamaSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly CeluGamaDbContext _context;

        public NotificationService(CeluGamaDbContext context)
        {
            _context = context;
        }

        public void ProcessNotification(Notification notification)
        {
            if (notification.Topic.Equals(Constants.ORDERS) || notification.Topic.Equals(Constants.ORDERS_V2))
            {
                SaveOrder(notification);
            }
            else if (notification.Topic.Equals(Constants.SHIPMENTS))
            {
                SaveShipment(notification);
            }
        }

        private void SaveOrder(Notification notification)
        {
            try
            {
                OrderMeLi OrderMeLi = GetOrderMeli(notification.Resource);

                if (OrderMeLi != null)
                {
                    using (IDbContextTransaction Transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Order Order = new Order();
                            bool Exists = _context.Orders.Any(o => o.OrderIDML == OrderMeLi.Id);
                            if (Exists)
                            {
                                Order = _context.Orders.Where(o => o.OrderIDML == OrderMeLi.Id).First();
                            }

                            LoadDBOrder(Order, OrderMeLi);
                            LoadNoteFromOrder(Order, notification.Resource);
                            _context.Entry(Order).State = Exists ? EntityState.Modified : EntityState.Added;
                            _context.SaveChanges();

                            foreach (OrderItemMeLi ItemMeli in OrderMeLi.OrderItems)
                            {
                                OrderItem OrderItem = new OrderItem();
                                bool ItemExists = _context.OrderItems.Any(i => i.OrderIDML == OrderMeLi.Id && i.ItemID == ItemMeli.Item.Id);
                                if (ItemExists)
                                {
                                    OrderItem = _context.OrderItems.Where(i => i.OrderIDML == OrderMeLi.Id && i.ItemID == ItemMeli.Item.Id).First();
                                }

                                LoadDBOrderItem(OrderItem, ItemMeli, Order.ID, OrderMeLi.Id);
                                _context.Entry(OrderItem).State = ItemExists ? EntityState.Modified : EntityState.Added;
                                _context.SaveChanges();
                            }

                            foreach (PaymentMeLi PaymentMeli in OrderMeLi.Payments)
                            {
                                Payment Payment = new Payment();
                                bool PayExists = _context.Payments.Any(p => p.OrderIDML == OrderMeLi.Id && p.PaymentID == PaymentMeli.Id);
                                if (PayExists)
                                {
                                    Payment = _context.Payments.Where(p => p.OrderIDML == OrderMeLi.Id && p.PaymentID == PaymentMeli.Id).First();
                                }

                                LoadDBPayment(Payment, PaymentMeli, Order.ID, OrderMeLi.Id);
                                _context.Entry(Payment).State = PayExists ? EntityState.Modified : EntityState.Added;
                                _context.SaveChanges();
                            }

                            Transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            Transaction.Rollback();
                            LogOnDB(500, "internal_server_error", e.Message, e.StackTrace, $"/orders/{OrderMeLi.Id}", "Error");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogOnDB(500, "internal_server_error", e.Message, e.StackTrace, notification.Resource, "Error");
            }
        }

        private void SaveShipment(Notification notification)
        {
            try
            {
                string[] segments = notification.Resource.Split('/');
                long.TryParse(segments[2], out long shippingId);
                ShippingMeLi shipment = GetShipping(shippingId);
                if (shipment != null)
                {
                    bool Exists = _context.Orders.Any(o => o.ShippingId == shippingId);
                    if (Exists)
                    {
                        List<Order> Orders = _context.Orders.Where(o => o.ShippingId == shippingId).ToList();
                        using (IDbContextTransaction Transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (Order order in Orders)
                                {
                                    order.ShippingStatus = shipment.Status;
                                    order.ShippingSubstatus = shipment.Substatus;
                                    order.ShippingType = shipment.LogisticType;
                                    if (shipment.ReceiverAddress != null && shipment.ReceiverAddress.Id.HasValue)
                                    {
                                        order.ShippingAddress = shipment.ReceiverAddress.AddressLine;
                                        order.ShippingStreetName = shipment.ReceiverAddress.StreetName;
                                        order.ShippingStreetNumber = shipment.ReceiverAddress.StreetNumber;
                                        order.ShippingLongitude = shipment.ReceiverAddress.Longitude.Value;
                                        order.ShippingLatitude = shipment.ReceiverAddress.Latitude.Value;
                                        order.ShippingComment = shipment.ReceiverAddress.Comment;
                                        order.ShippingZipCode = shipment.ReceiverAddress.ZipCode;
                                        order.ShippingReceiverName = shipment.ReceiverAddress.ReceiverName;
                                        order.ShippingReceiverPhone = shipment.ReceiverAddress.ReceiverPhone;
                                        if (shipment.ReceiverAddress.City != null)
                                        {
                                            order.ShippingCity = shipment.ReceiverAddress.City.Name;
                                        }

                                        if (shipment.ReceiverAddress.State != null)
                                        {
                                            order.ShippingState = shipment.ReceiverAddress.State.Name;
                                        }

                                        if (shipment.ReceiverAddress.Country != null)
                                        {
                                            order.ShippingCountry = shipment.ReceiverAddress.Country.Name;
                                        }

                                        if (shipment.ReceiverAddress.Neighborhood != null)
                                        {
                                            order.ShippingNeighborhood = shipment.ReceiverAddress.Neighborhood.Name;
                                        }
                                    }

                                    _context.Entry(order).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }

                                Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                Transaction.Rollback();
                                LogOnDB(500, "internal_server_error", e.Message, e.StackTrace, notification.Resource, "Error");
                            }
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                LogOnDB(500, "internal_server_error", e.Message, e.StackTrace, notification.Resource, "Error");
            }
        }

        private void LoadNoteFromOrder(Order order, string notificationResource)
        {
            order.Observations = "";
            MercadoLibreClient client = GetMELIClient();
            Token meliToken = _context.Tokens.First();
            IRestResponse response = client.GetNotesFromOrder(notificationResource, meliToken.AccessToken);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                // Acumulo las notas separadas por punto y coma.
                List<NotesMeLiContainer> ListNotes = JsonConvert.DeserializeObject<List<NotesMeLiContainer>>(response.Content);
                foreach(NoteMeLi Note in ListNotes[0].Results)
                {
                    order.Observations += $"{Note.Note};";
                }

                // Quito el último punto y coma, esto es para poder separar las notas posteriormente por punto y coma.
                if (!string.IsNullOrEmpty(order.Observations))
                {
                    order.Observations = order.Observations.Substring(0, order.Observations.Length - 1);
                }
            }
            else
            {
                LogOnDB((int)response.StatusCode, response.StatusDescription, response.Content, response.ErrorMessage, notificationResource, "Warning");
            }
        }

        private MercadoLibreClient GetMELIClient()
        {
            string AppID = "";
            string SecretKey = "";
            Token meliToken = _context.Tokens.FirstOrDefault();
            if (meliToken != null)
            {
                AppID = meliToken.AppID;
                SecretKey = meliToken.SecretKey;
            }

            return new MercadoLibreClient(AppID, SecretKey);
        }

        private OrderMeLi GetOrderMeli(string notificationResource)
        {
            OrderMeLi order = null;

            MercadoLibreClient client = GetMELIClient();
            Token meliToken = _context.Tokens.First();
            IRestResponse response = client.GetOrder(notificationResource, meliToken.AccessToken);

            if (response.StatusCode.Equals(HttpStatusCode.OK) || response.StatusCode.Equals(HttpStatusCode.PartialContent))
            {
                order = JsonConvert.DeserializeObject<OrderMeLi>(response.Content);
                if (order.Shipping != null && order.Shipping.Id.HasValue)
                {
                    order.Shipping = GetShipping(order.Shipping.Id);
                }
            }
            else
            {
                string content = response.Content != "" ? response.Content : response.ErrorMessage;
                LogOnDB((int)response.StatusCode, response.StatusDescription, content, "", notificationResource, "Error");
            }

            return order;
        }

        private string GetVariation(string ItemID, long VariationID)
        {
            MercadoLibreClient client = GetMELIClient();
            string fullVariation = "";
            IRestResponse response = client.GetItemVariations(ItemID, VariationID);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                VariationMeLi variation = JsonConvert.DeserializeObject<VariationMeLi>(response.Content);
                foreach (AttributeCombinationMeLi attribute in variation.AttributeCombinations)
                {
                    if (!string.IsNullOrEmpty(attribute.Name) && !string.IsNullOrEmpty(attribute.ValueName))
                    {
                        fullVariation += $"{attribute.Name}: {attribute.ValueName};";
                    }
                }

                if (!string.IsNullOrEmpty(fullVariation))
                {
                    fullVariation = fullVariation.Substring(0, fullVariation.Length - 1);
                }
            }
            else
            {
                LogOnDB((int)response.StatusCode, response.StatusDescription, response.Content, response.ErrorMessage, $"/items/{ItemID}/variations/{VariationID}", "Warning");
            }

            return fullVariation;
        }

        private ShippingMeLi GetShipping(long? shippingId)
        {
            MercadoLibreClient client = GetMELIClient();
            Token meliToken = _context.Tokens.First();
            IRestResponse ResponseShipping = client.GetShipping(shippingId.Value.ToString(), meliToken.AccessToken);
            if (ResponseShipping.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonConvert.DeserializeObject<ShippingMeLi>(ResponseShipping.Content);
            }

            return null;
        }

        private void LogOnDB(int StatusCode, string Error, string Message, string StackTrace, string Resource, string Level)
        {
            Log Log = new Log()
            {
                StatusCode = StatusCode,
                Error = Error,
                Message = Message,
                StackTrace = StackTrace,
                Resource = Resource,
                Level = Level,
                CreatedAt = DateTime.UtcNow
            };

            _context.Logs.Add(Log);
            _context.SaveChanges();
        }

        private void LoadDBOrder(Order Order, OrderMeLi OrderMeli)
        {
            Order.OrderIDML = OrderMeli.Id;
            Order.Status = OrderMeli.Status;
            Order.DateCreated = OrderMeli.DateCreated.UtcDateTime;
            Order.DateClosed = OrderMeli.DateClosed.UtcDateTime;

            if (OrderMeli.Buyer != null)
            {
                Order.Name = OrderMeli.Buyer.FirstName;
                Order.LastName = OrderMeli.Buyer.LastName;
                Order.Nickname = OrderMeli.Buyer.Nickname;
            }

            if (OrderMeli.Shipping != null && OrderMeli.Shipping.Id != null)
            {
                Order.ShippingId = OrderMeli.Shipping.Id;
                Order.ShippingStatus = OrderMeli.Shipping.Status;
                Order.ShippingSubstatus = OrderMeli.Shipping.Substatus;
                Order.ShippingType = OrderMeli.Shipping.LogisticType;
                if (OrderMeli.Shipping.ReceiverAddress != null && OrderMeli.Shipping.ReceiverAddress.Id.HasValue)
                {
                    Order.ShippingAddress = OrderMeli.Shipping.ReceiverAddress.AddressLine;
                    Order.ShippingStreetName = OrderMeli.Shipping.ReceiverAddress.StreetName;
                    Order.ShippingStreetNumber = OrderMeli.Shipping.ReceiverAddress.StreetNumber;
                    Order.ShippingLongitude = OrderMeli.Shipping.ReceiverAddress.Longitude.Value;
                    Order.ShippingLatitude = OrderMeli.Shipping.ReceiverAddress.Latitude.Value;
                    Order.ShippingComment = OrderMeli.Shipping.ReceiverAddress.Comment;
                    Order.ShippingZipCode = OrderMeli.Shipping.ReceiverAddress.ZipCode;
                    Order.ShippingReceiverName = OrderMeli.Shipping.ReceiverAddress.ReceiverName;
                    Order.ShippingReceiverPhone = OrderMeli.Shipping.ReceiverAddress.ReceiverPhone;
                    if (OrderMeli.Shipping.ReceiverAddress.City != null)
                    {
                        Order.ShippingCity = OrderMeli.Shipping.ReceiverAddress.City.Name;
                    }

                    if (OrderMeli.Shipping.ReceiverAddress.State != null)
                    {
                        Order.ShippingState = OrderMeli.Shipping.ReceiverAddress.State.Name;
                    }

                    if (OrderMeli.Shipping.ReceiverAddress.Country != null)
                    {
                        Order.ShippingCountry = OrderMeli.Shipping.ReceiverAddress.Country.Name;
                    }

                    if (OrderMeli.Shipping.ReceiverAddress.Neighborhood != null)
                    {
                        Order.ShippingNeighborhood = OrderMeli.Shipping.ReceiverAddress.Neighborhood.Name;
                    }
                }
            }

            Order.PackId = OrderMeli.PackId;
            Order.TotalAmount = OrderMeli.TotalAmount;
            Order.TotalAmountWithShipping = OrderMeli.TotalAmountWithShipping();
            Order.PaidAmount = OrderMeli.PaidAmount;
        }

        private void LoadDBOrderItem(OrderItem OrderItem, OrderItemMeLi ItemMeli, int OrderID, long OrderIDML)
        {
            OrderItem.CategoryID = ItemMeli.Item.CategoryId;
            OrderItem.ItemID = ItemMeli.Item.Id;
            OrderItem.OrderID = OrderID;
            OrderItem.OrderIDML = OrderIDML;
            OrderItem.Quantity = (int)ItemMeli.Quantity;
            OrderItem.SellerSKU = ItemMeli.Item.SellerSku;
            OrderItem.Title = ItemMeli.Item.Title;
            OrderItem.UnitPrice = ItemMeli.UnitPrice;
            if (ItemMeli.Item.VariationId.HasValue)
            {
                OrderItem.VariationID = ItemMeli.Item.VariationId.Value.ToString();
                OrderItem.VariationColor = GetVariation(ItemMeli.Item.Id, ItemMeli.Item.VariationId.Value);
            }
        }

        private void LoadDBPayment(Payment Payment, PaymentMeLi PaymentMeli, int OrderID, long OrderIDML)
        {
            Payment.CardID = PaymentMeli.CardId;
            Payment.OperationType = PaymentMeli.OperationType;
            Payment.OrderID = OrderID;
            Payment.OrderIDML = OrderIDML;
            Payment.OverpaidAmount = PaymentMeli.OverpaidAmount;
            Payment.PayerID = PaymentMeli.PayerId;
            Payment.PaymentID = PaymentMeli.Id;
            Payment.PaymentMethod = PaymentMeli.PaymentMethodId;
            Payment.PaymentType = PaymentMeli.PaymentType;
            Payment.Reason = PaymentMeli.Reason;
            Payment.ShippingCost = PaymentMeli.ShippingCost;
            Payment.Status = PaymentMeli.Status;
            Payment.StatusDetail = PaymentMeli.StatusDetail;
            Payment.TaxesAmount = PaymentMeli.TaxesAmount;
            Payment.TotalPaidAmount = PaymentMeli.TotalPaidAmount;
            Payment.TransactionAmount = PaymentMeli.TransactionAmount;
        }
    }
}
