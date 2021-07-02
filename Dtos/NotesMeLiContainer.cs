using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class NotesMeLiContainer
    {
        [JsonProperty("results")]
        public NoteMeLi[] Results { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }
    }
}
