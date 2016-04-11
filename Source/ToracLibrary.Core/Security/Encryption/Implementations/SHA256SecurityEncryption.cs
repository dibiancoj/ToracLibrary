using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.ByteArrayExtensions;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.Security.Encryption
{

    /// <summary>
    /// Class Used To Encrypt And Decrypt String Values Using The SHA256 Hash Protocol. This is a 1 way hash!
    /// </summary>
    /// <remarks>Class Is Immutable With Properties</remarks>
    public class SHA256SecurityEncryption : IOneWaySecurityEncryption
    {

        #region Encryption

        /// <summary>
        /// Encrypt A String Value
        /// </summary>
        /// <param name="TextToEncrypt">Text To Encrypt</param>
        /// <returns>The Encrypted String Value</returns>
        public string Encrypt(string TextToEncrypt)
        {
            //Validate First
            if (TextToEncrypt.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Text To Encrypt Can't Be Null.");
            }

            //create the hasher
            using (var Hasher = new SHA256Managed())
            {
                //go return the hash
                return Hasher.ComputeHash(new UTF8Encoding().GetBytes(TextToEncrypt)).ToByteArrayToHexadecimalString(false);
            }
        }

        #endregion

    }

}
