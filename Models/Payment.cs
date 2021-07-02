using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeluGamaSystem.Models
{
    public class Payment
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public long OrderIDML { get; set; }
        public long PaymentID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public string OperationType { get; set; }
        public string Reason { get; set; }
        public Nullable<long> CardID { get; set; }
        public long PayerID { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TransactionAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Nullable<decimal> TaxesAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Nullable<decimal> ShippingCost { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public Nullable<decimal> OverpaidAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPaidAmount { get; set; }
        public Order Order { get; set; }
    }
}
