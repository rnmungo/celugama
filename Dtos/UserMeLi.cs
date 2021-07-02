using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class UserMeLi
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
