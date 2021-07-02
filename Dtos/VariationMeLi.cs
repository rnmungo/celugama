using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class VariationMeLi
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("attribute_combinations")]
        public AttributeCombinationMeLi[] AttributeCombinations { get; set; }
    }
}
