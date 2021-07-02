using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class Notification
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }
        [JsonProperty("application_id")]
        public long ApplicationID { get; set; }
        [JsonProperty("topic")]
        public string Topic { get; set; }
    }
}
