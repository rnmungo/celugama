using System;

namespace CeluGamaSystem.Dtos
{
    public class JWToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string Username { get; set; }
    }
}
