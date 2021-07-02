using System;
using Newtonsoft.Json;

namespace CeluGamaSystem.Dtos
{
    public class ReceiverAddressMeLi
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("address_line")]
        public string AddressLine { get; set; }

        [JsonProperty("street_name")]
        public string StreetName { get; set; }

        [JsonProperty("street_number")]
        public string StreetNumber { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }

        [JsonProperty("receiver_name")]
        public string ReceiverName { get; set; }

        [JsonProperty("receiver_phone")]
        public string ReceiverPhone { get; set; }

        [JsonProperty("city")]
        public ReceiverAddressAdditionalMeLi City { get; set; }

        [JsonProperty("state")]
        public ReceiverAddressAdditionalMeLi State { get; set; }

        [JsonProperty("country")]
        public ReceiverAddressAdditionalMeLi Country { get; set; }

        [JsonProperty("neighborhood")]
        public ReceiverAddressAdditionalMeLi Neighborhood { get; set; }
    }
}
