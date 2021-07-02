using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ShippingOptionMeLi
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("shipping_method_id")]
        public long ShippingMethodId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("currency_id")]
        public string CurrencyId { get; set; }

        [JsonProperty("cost")]
        public decimal? Cost { get; set; }

        [JsonProperty("list_cost")]
        public decimal? ListCost { get; set; }
    }
}
