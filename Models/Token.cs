using System;

namespace CeluGamaSystem.Models
{
    public class Token
    {
        public int ID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Nullable<DateTimeOffset> DueDateTime { get; set; }
        public string AppID { get; set; }
        public string SecretKey { get; set; }
    }
}
