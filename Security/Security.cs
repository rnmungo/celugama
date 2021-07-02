using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CeluGamaSystem.Security
{
    internal static class Security
    {
        /// <summary>
        /// Encripta con el algoritmo SHA256 el texto solicitado.
        /// </summary>
        /// <param name="original">El texto a encriptar</param>
        /// <returns>El texto encriptado</returns>
        internal static string stringToSHA256(string original)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();
            byte[] inputBytes = Encoding.UTF8.GetBytes(original);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder hash = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                hash.Append(hashedBytes[i].ToString("x2"));

            return hash.ToString();
        }
    }
}
