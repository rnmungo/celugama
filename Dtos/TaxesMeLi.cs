using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class TaxesMeLi
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
    }
}
