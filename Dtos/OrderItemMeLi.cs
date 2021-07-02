using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class OrderItemMeLi
    {
        [JsonProperty("item")]
        public ItemMeLi Item { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }
    }
}
