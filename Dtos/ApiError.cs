using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ApiError
    {
        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
