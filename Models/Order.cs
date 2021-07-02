using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeluGamaSystem.Models
{
    public class Order
    {
        public int ID { get; set; }
        public long OrderIDML { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateClosed { get; set; }
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmountWithShipping { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PaidAmount { get; set; }
        public Nullable<long> PackId { get; set; }
        public Nullable<long> ShippingId { get; set; }
        public string ShippingStatus { get; set; }
        public string ShippingSubstatus { get; set; }
        public string ShippingType { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingStreetName { get; set; }
        public string ShippingStreetNumber { get; set; }
        [Column(TypeName = "decimal(12,8)")]
        public Nullable<decimal> ShippingLongitude { get; set; }
        [Column(TypeName = "decimal(12,8)")]
        public Nullable<decimal> ShippingLatitude { get; set; }
        public string ShippingComment { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingReceiverName { get; set; }
        public string ShippingReceiverPhone { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingNeighborhood { get; set; }
        public string Observations { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
        public ICollection<Payment> Payment { get; set; }
    }
}
