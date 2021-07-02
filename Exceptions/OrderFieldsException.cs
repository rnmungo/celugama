using System;

namespace CeluGamaSystem.Exceptions
{
    public class OrderFieldsException : Exception
    {
        private int _StatusCode { get; set; }
        public int StatusCode { get { return this._StatusCode; } }

        public OrderFieldsException(string message, int statusCode) : base(message)
        {
            _StatusCode = statusCode;
        }
    }
}
