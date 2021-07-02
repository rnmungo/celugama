using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class PasswordContainer
    {
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
