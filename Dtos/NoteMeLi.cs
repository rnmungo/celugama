using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class NoteMeLi
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("date_created")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("date_last_updated")]
        public DateTimeOffset DateLastUpdated { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
