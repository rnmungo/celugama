using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Exceptions;
using System.Drawing;
using OfficeOpenXml.ConditionalFormatting.Contracts;

namespace CeluGamaSystem.Services
{
    public class OrderExcelReportService
    {
        public string GetRandomFileName()
        {
            return string.Format("{0}.xlsx", Guid.NewGuid().ToString());
        }

        private Dictionary<string, PackOrdersMeLi> MapOrdersToPacks(List<Order> Orders)
        {
            Dictionary<string, PackOrdersMeLi> HashPacks = new Dictionary<string, PackOrdersMeLi>();
            foreach(Order o in Orders)
            {
                string HashKey;
                
                if (o.PackId.HasValue)
                {
                    HashKey = o.PackId.Value.ToString();
                }
                else if (o.ShippingId.HasValue)
                {
                    HashKey = o.ShippingId.Value.ToString();
                }
                else
                {
                    HashKey = o.OrderIDML.ToString();
                }

                List<ItemRowMeLi> Rows = new List<ItemRowMeLi>();
                foreach (OrderItem Item in o.OrderItem)
                {
                    Rows.Add(new ItemRowMeLi()
                    {
                        SellerSKU = Item.SellerSKU,
                        VariationColor = Item.VariationColor,
                        Title = Item.Title,
                        Quantity = Item.Quantity
                    });
                }

                if (HashPacks.ContainsKey(HashKey))
                {
                    HashPacks[HashKey].Orders.Add(new OrderRowMeLi()
                    {
                        DateCreated = o.DateCreated,
                        TotalAmount = o.TotalAmount,
                        Observations = o.Observations,
                        OrderIDML = o.OrderIDML,
                        Items = Rows
                    });
                }
                else
                {
                    List<OrderRowMeLi> NewListOrders = new List<OrderRowMeLi>();
                    NewListOrders.Add(new OrderRowMeLi()
                    {
                        DateCreated = o.DateCreated,
                        TotalAmount = o.TotalAmount,
                        Observations = o.Observations,
                        OrderIDML = o.OrderIDML,
                        Items = Rows
                    });

                    string Address = "";
                    if (!string.IsNullOrEmpty(o.ShippingAddress))
                    {
                        Address = $"{o.ShippingAddress}, C.P. {o.ShippingZipCode}";
                        if (!string.IsNullOrEmpty(o.ShippingCity))
                        {
                            Address = $"{Address}, {o.ShippingCity}";
                        }

                        if (!string.IsNullOrEmpty(o.ShippingState))
                        {
                            Address = $"{Address}, {o.ShippingState}";
                        }

                        if (!string.IsNullOrEmpty(o.ShippingCountry))
                        {
                            Address = $"{Address}, {o.ShippingCountry}";
                        }
                    }


                    HashPacks.Add(HashKey, new PackOrdersMeLi()
                    {
                        PackId = o.PackId,
                        ShippingId = o.ShippingId,
                        ShippingStatus = o.ShippingStatus,
                        ShippingType = o.ShippingType,
                        ReceiverAddress = Address,
                        Orders = NewListOrders
                    });
                }

            }

            return HashPacks;
        }

        private Color GetColor(int Index)
        {
            switch(Index)
            {
                case 0:
                    return Color.LightBlue;
                case 1:
                    return Color.LightCoral;
                case 2:
                    return Color.LightGoldenrodYellow;
                case 3:
                    return Color.LightGreen;
                case 4:
                    return Color.LightSalmon;
                case 5:
                    return Color.LightSeaGreen;
                case 6:
                    return Color.LightSkyBlue;
                case 7:
                    return Color.LightSlateGray;
                default:
                    return Color.LightBlue;
            }
        }

        public MemoryStream WriteExcelFile(List<Order> Orders, string shippingType)
        {
            if (Orders.Count.Equals(0))
            {
                throw new OrdersNotFoundException("No se encontraron resultados en la búsqueda");
            }

            Dictionary<string, PackOrdersMeLi> Hash = MapOrdersToPacks(Orders);
            MemoryStream stream = new MemoryStream();
            ExcelPackage Excel = new ExcelPackage(stream);
            ExcelWorksheet Sheet = Excel.Workbook.Worksheets.Add("PEDIDOS");

            bool isFlex = shippingType.Equals("self_service");
            int columnAmount = isFlex ? 10 : 9;

            Sheet.Cells[1, 1].Value = "Nr. Pack";
            Sheet.Cells[1, 2].Value = "Nr. Pedido";
            Sheet.Cells[1, 3].Value = "Nr. Envío";
            Sheet.Cells[1, 4].Value = "Cantidad";
            Sheet.Cells[1, 5].Value = "Código";
            Sheet.Cells[1, 6].Value = "Descripción";
            Sheet.Cells[1, 7].Value = "Variantes";
            Sheet.Cells[1, 8].Value = "Observaciones";
            if (isFlex)
            {
                Sheet.Cells[1, 9].Value = "Dirección";
            }

            Sheet.Cells[1, columnAmount].Value = "Monto ($)";

            int StartRow = 2;
            int IndexColor = 0;
            foreach (KeyValuePair<string, PackOrdersMeLi> HashItem in Hash)
            {
                if (IndexColor % 8 == 0)
                {
                    IndexColor = 0;
                }

                int ToCell = StartRow + HashItem.Value.Orders.Count - 1;
                Sheet.Cells[StartRow, 1, ToCell, 1].Merge = true;
                string PackId = HashItem.Value.PackId.HasValue ? HashItem.Value.PackId.Value.ToString() : "-";
                string ShippingId = HashItem.Value.ShippingId.HasValue ? HashItem.Value.ShippingId.Value.ToString() : "-";
                Sheet.Cells[StartRow, 1, ToCell, 1].Value = PackId;
                Sheet.Cells[StartRow, 1, ToCell, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                Sheet.Cells[StartRow, 1, ToCell, 1].Style.Font.Bold = true;
                Sheet.Cells[StartRow, 1, ToCell, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheet.Cells[StartRow, 1, ToCell, 1].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                
                foreach (OrderRowMeLi Order in HashItem.Value.Orders)
                {
                    ToCell = StartRow + Order.Items.Count - 1;
                    Sheet.Cells[StartRow, 2, ToCell, 2].Merge = true;
                    Sheet.Cells[StartRow, 2, ToCell, 2].Value = Order.OrderIDML.ToString();
                    Sheet.Cells[StartRow, 2, ToCell, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    Sheet.Cells[StartRow, 2, ToCell, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[StartRow, 2, ToCell, 2].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                    Sheet.Cells[StartRow, 3, ToCell, 3].Value = ShippingId;
                    Sheet.Cells[StartRow, 3, ToCell, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    Sheet.Cells[StartRow, 3, ToCell, 3].Style.Font.Bold = true;
                    Sheet.Cells[StartRow, 3, ToCell, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[StartRow, 3, ToCell, 3].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                    foreach (ItemRowMeLi Item in Order.Items)
                    {
                        Sheet.Cells[StartRow, 4].Value = Item.Quantity;
                        Sheet.Cells[StartRow, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[StartRow, 4].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                        Sheet.Cells[StartRow, 5].Value = Item.SellerSKU;
                        Sheet.Cells[StartRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[StartRow, 5].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));

                        Sheet.Cells[StartRow, 6].Value = Item.Title;
                        Sheet.Cells[StartRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[StartRow, 6].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                        Sheet.Cells[StartRow, 7].Value = string.IsNullOrEmpty(Item.VariationColor) ? "" : Item.VariationColor;
                        Sheet.Cells[StartRow, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[StartRow, 7].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                    }

                    Sheet.Cells[StartRow, 8, ToCell, 8].Merge = true;
                    Sheet.Cells[StartRow, 8, ToCell, 8].Value = Order.Observations;
                    Sheet.Cells[StartRow, 8, ToCell, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    Sheet.Cells[StartRow, 8, ToCell, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[StartRow, 8, ToCell, 8].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));

                    
                    if (isFlex)
                    {
                        Sheet.Cells[StartRow, 9, ToCell, 9].Merge = true;
                        Sheet.Cells[StartRow, 9, ToCell, 9].Value = HashItem.Value.ReceiverAddress;
                        Sheet.Cells[StartRow, 9, ToCell, 9].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        Sheet.Cells[StartRow, 9, ToCell, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        Sheet.Cells[StartRow, 9, ToCell, 9].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                    }

                    Sheet.Cells[StartRow, columnAmount, ToCell, columnAmount].Value = Order.TotalAmount;
                    Sheet.Cells[StartRow, columnAmount, ToCell, columnAmount].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    Sheet.Cells[StartRow, columnAmount, ToCell, columnAmount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[StartRow, columnAmount, ToCell, columnAmount].Style.Fill.BackgroundColor.SetColor(GetColor(IndexColor));
                    Sheet.Cells[StartRow, columnAmount, ToCell, columnAmount].Style.Numberformat.Format = "#,##0.00";

                    StartRow += 1;
                }

                IndexColor += 1;
            }
            Sheet.Cells.AutoFitColumns(50.0, 200.0);
            Excel.Save();
            stream.Position = 0;
            return stream;
        }
    }
}
