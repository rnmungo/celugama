using System;

namespace CeluGamaSystem.Exceptions
{
    public class OrdersNotFoundException : Exception
    {
        private int _StatusCode { get; } = 404;
        public int StatusCode { get { return this._StatusCode; } }

        public OrdersNotFoundException(string message) : base(message) { }
    }
}
