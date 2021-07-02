using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeluGamaSystem.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public long OrderIDML { get; set; }
        public string ItemID { get; set; }
        public string Title { get; set; }
        public string CategoryID { get; set; }
        public string VariationID { get; set; }
        public string VariationColor { get; set; }
        public string SellerSKU { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public int OrderID { get; set; }
    }
}
