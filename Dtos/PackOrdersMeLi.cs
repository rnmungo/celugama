using System;
using System.Collections.Generic;

namespace CeluGamaSystem.Dtos
{
    public class PackOrdersMeLi
    {
        public long? PackId { get; set; }
        public long? ShippingId { get; set; }
        public string ShippingStatus { get; set; }
        public string ShippingType { get; set; }
        public string ReceiverAddress { get; set; }
        public List<OrderRowMeLi> Orders { get; set; }
    }
}
