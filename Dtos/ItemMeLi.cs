using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ItemMeLi
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("category_id")]
        public string CategoryId { get; set; }
        [JsonProperty("variation_id")]
        public long? VariationId { get; set; }
        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }
    }
}
