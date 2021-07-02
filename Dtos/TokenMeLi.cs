using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class TokenMeLi
    {
        public TokenMeLi() { }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

    }
}
