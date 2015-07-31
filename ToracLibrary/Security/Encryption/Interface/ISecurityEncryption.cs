﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Security.Encryption
{
    
    /// <summary>
    /// Interface for the encryption api you want to use.
    /// </summary>
    public interface ISecurityEncryption
    {

        /// <summary>
        /// Encrypt A Value
        /// </summary>
        /// <param name="ValueToEncrypt">Value To Encypt</param>
        /// <returns>Encrypted Value</returns>
        string Encrypt(string ValueToEncrypt);

        /// <summary>
        /// Decrypt An Encrypted String To A Readable String
        /// </summary>
        /// <param name="EncryptedText">Encrypted Text</param>
        /// <returns>A Readable unencrypted string</returns>
        string Decrypt(string EncryptedText);

    }

}
