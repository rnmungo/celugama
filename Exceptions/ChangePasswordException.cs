using System;

namespace CeluGamaSystem.Exceptions
{
    public class ChangePasswordException : Exception
    {
        private int _StatusCode { get; } = 500;
        public int StatusCode { get { return this._StatusCode; } }

        public ChangePasswordException(string message) : base(message) { }
    }
}
