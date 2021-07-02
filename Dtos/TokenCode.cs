using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class TokenCode
    {
        public TokenCode() { }

        [JsonProperty("Code")]
        public String Code { get; set; }
    }
}
