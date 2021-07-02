using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ShippingMeLi
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("base_cost")]
        public decimal? BaseCost { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("substatus")]
        public string Substatus { get; set; }

        [JsonProperty("shipping_option")]
        public ShippingOptionMeLi ShippingOption { get; set; }

        [JsonProperty("logistic_type")]
        public string LogisticType { get; set; }

        [JsonProperty("receiver_address")]
        public ReceiverAddressMeLi ReceiverAddress { get; set; }
    }
}
