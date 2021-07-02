using System;

namespace CeluGamaSystem.Dtos
{
    public class OrderTransportDto
    {
        public int ID { get; set; }
        public long OrderIDML { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountWithShipping { get; set; }
        public decimal PaidAmount { get; set; }
        public long? PackId { get; set; }
        public long? ShippingId { get; set; }
        public string ShippingStatus { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverZipCode { get; set; }
        public string ReceiverCity { get; set; }
        public string ReceiverState { get; set; }
        public string ReceiverCountry { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverComment { get; set; }
        public bool IsPrintable { get; set; }
        public bool IsFulfillment { get; set; }
        public int Items { get; set; }
    }
}
