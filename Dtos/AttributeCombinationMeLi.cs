using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class AttributeCombinationMeLi
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value_id")]
        public string ValueId { get; set; }

        [JsonProperty("value_name")]
        public string ValueName { get; set; }
    }
}
