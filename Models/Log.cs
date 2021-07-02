using System;

namespace CeluGamaSystem.Models
{
    public class Log
    {
        public int ID { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Resource { get; set; }
        public string Level { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
