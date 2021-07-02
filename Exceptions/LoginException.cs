using System;
using System.Collections.Generic;
using System.Text;

namespace CeluGamaSystem.Exceptions
{
    public class LoginException : Exception
    {
        private int _StatusCode { get; set; }
        public int StatusCode { get { return this._StatusCode; } }

        public LoginException(string message, int statusCode) : base(message)
        {
            _StatusCode = statusCode;
        }
    }
}
