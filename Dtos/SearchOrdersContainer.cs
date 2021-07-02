using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class SearchOrdersContainer
    {
        [JsonProperty("results")]
        public List<OrderTransportDto> Results { get; set; }

        [JsonProperty("shipping_type")]
        public string ShippingType { get; set; }
    }
}
