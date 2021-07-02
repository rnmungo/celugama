using System;
using System.Linq;
using System.Collections.Generic;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;

namespace CeluGamaSystem.Services
{
    public class OrderService : IOrderService
    {
        string[] OnMyWaySubStatus = new string[] { 
            "dropped_off",
            "picked_up",
            "in_hub",
            "measures_ready",
            "waiting_for_carrier_authorization",
            "authorized_by_carrier",
            "in_packing_list",
            "waiting_for_last_mile_authorization"
        };

        private readonly CeluGamaDbContext _context;

        public OrderService(CeluGamaDbContext context)
        {
            _context = context;
        }

        public Order GetOrder(long orderId)
        {
            Order order = _context.Orders.Where(o => o.OrderIDML == orderId).FirstOrDefault();
            if (order != null)
            {
                _context.Entry(order).Collection(o => o.OrderItem).Load();
            }

            return order;           
        }

        public List<Order> SearchOrders(SearchOrder searchOrder)
        {
            List<Order> Orders = _context.Orders
                .Where(o => o.ShippingStatus == "ready_to_ship")
                .Where(o => !OnMyWaySubStatus.Contains(o.ShippingSubstatus))
                .Where(o => o.Status != "cancelled")
                .Where(o => o.ShippingType == searchOrder.ShippingType)
                .OrderBy(o => o.DateCreated).ThenBy(o => o.PackId).ThenBy(o => o.ShippingId).ToList();

            foreach (Order Order in Orders)
            {
                _context.Entry(Order).Collection(o => o.OrderItem).Load();
            }
            
            return Orders;
        }

        public List<OrderTransportDto> GetOrdersToPrepare(SearchOrder filters)
        {
            List<Order> Orders = _context.Orders
                .Where(o => o.ShippingStatus == "ready_to_ship")
                .Where(o => !OnMyWaySubStatus.Contains(o.ShippingSubstatus))
                .Where(o => o.Status != "cancelled")
                .Where(o => o.ShippingType == filters.ShippingType)
                .OrderBy(o => o.DateCreated).ThenBy(o => o.PackId).ThenBy(o => o.ShippingId).ToList();

            return MapOrdersToTransport(Orders);
        }

        private List<OrderTransportDto> MapOrdersToTransport(List<Order> Orders)
        {
            List<OrderTransportDto> OrdersTransport = new List<OrderTransportDto>();
            
            foreach (Order Order in Orders)
            {
                _context.Entry(Order).Collection(o => o.OrderItem).Load();
                OrdersTransport.Add(MapOrderToTransport(Order));
            }

            return OrdersTransport;
        }

        private OrderTransportDto MapOrderToTransport(Order Order)
        {
            int TotalItems = 0;

            foreach (OrderItem OrderItem in Order.OrderItem)
            {
                TotalItems += OrderItem.Quantity;
            }

            return new OrderTransportDto()
            {
                ID = Order.ID,
                DateCreated = Order.DateCreated,
                OrderIDML = Order.OrderIDML,
                Name = Order.Name,
                LastName = Order.LastName,
                TotalAmount = Order.TotalAmount,
                TotalAmountWithShipping = Order.TotalAmountWithShipping,
                PaidAmount = Order.PaidAmount,
                PackId = Order.PackId,
                ShippingId = Order.ShippingId,
                ShippingStatus = Order.ShippingStatus,
                ReceiverAddress = Order.ShippingAddress,
                ReceiverZipCode = Order.ShippingZipCode,
                ReceiverCity = Order.ShippingCity,
                ReceiverState = Order.ShippingState,
                ReceiverCountry = Order.ShippingCountry,
                ReceiverName = Order.ShippingReceiverName,
                ReceiverPhone = Order.ShippingReceiverPhone,
                ReceiverComment = Order.ShippingComment,
                IsFulfillment = Order.ShippingType == "fulfillment",
                IsPrintable = Order.ShippingStatus == "ready_to_ship" && Order.ShippingType != "fulfillment",
                Items = TotalItems
            };
        }
    }
}
