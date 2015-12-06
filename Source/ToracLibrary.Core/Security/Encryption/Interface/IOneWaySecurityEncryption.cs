using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Security.Encryption
{
    
    /// <summary>
    /// Interface for the encryption api that will only encrypt 1 way
    /// </summary>
    public interface IOneWaySecurityEncryption
    {

        /// <summary>
        /// Encrypt A Value
        /// </summary>
        /// <param name="ValueToEncrypt">Value To Encypt</param>
        /// <returns>Encrypted Value</returns>
        string Encrypt(string ValueToEncrypt);

    }

}
