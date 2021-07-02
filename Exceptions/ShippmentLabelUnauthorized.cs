using System;

namespace CeluGamaSystem.Exceptions
{
    public class ShippmentLabelUnauthorized : Exception
    {
        private int _StatusCode { get; } = 403;
        public int StatusCode { get { return this._StatusCode; } }

        public ShippmentLabelUnauthorized(string message) : base(message) { }
    }
}
