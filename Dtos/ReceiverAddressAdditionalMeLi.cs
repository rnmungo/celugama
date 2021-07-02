using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ReceiverAddressAdditionalMeLi
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
