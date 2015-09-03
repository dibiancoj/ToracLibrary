using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Security.Encryption
{

    /// <summary>
    /// Class Used To Encrypt And Decrypt String Values Using The MD5 Hash Protocol
    /// </summary>
    /// <remarks>Class Is Immutable With Properties</remarks>
    public class MD5HashSecurityEncryption : ISecurityEncryption
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="KeyToBaseEncryptionOffOf">Key which the MD5Hash is based off of</param>
        public MD5HashSecurityEncryption(string KeyToBaseEncryptionOffOf)
        {
            //Validate
            if (string.IsNullOrEmpty(KeyToBaseEncryptionOffOf))
            {
                throw new ArgumentNullException("Key Must Not Be Null");
            }

            //Is a valid key, set the property
            Key = KeyToBaseEncryptionOffOf;
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Holds The Key Which The MD5Hash Is Based Off Of
        /// </summary>
        private string Key { get; }

        #endregion

        #region Encryption

        /// <summary>
        /// Encrypt A String Value
        /// </summary>
        /// <param name="TextToEncrypt">Text To Encrypt</param>
        /// <returns>The Encrypted String Value</returns>
        public string Encrypt(string TextToEncrypt)
        {
            //Validate First
            if (string.IsNullOrEmpty(TextToEncrypt))
            {
                throw new ArgumentNullException("Text To Encrypt Can't Be Null.");
            }

            //create the triple des encryption
            using (var DESCrypto = new TripleDESCryptoServiceProvider())
            {
                //create md5
                using (var MD5Provider = new MD5CryptoServiceProvider())
                {
                    //Set the security properties
                    DESCrypto.Key = MD5Provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));
                    DESCrypto.Mode = CipherMode.ECB;

                    //get the buffer
                    byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(TextToEncrypt);

                    //return the encrypted value
                    return Convert.ToBase64String(DESCrypto.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
                }
            }
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypt An Encrypted String
        /// </summary>
        /// <param name="TextToDecrypt">Text To Decrypt</param>
        /// <returns>The Decrypted String Value</returns>
        public string Decrypt(string TextToDecrypt)
        {
            //Validate
            if (string.IsNullOrEmpty(TextToDecrypt))
            {
                throw new ArgumentNullException("Text To Decrypt Can't Be Null.");
            }

            //create the TripleDESCryptoServiceProvider 
            using (var DESCrypto = new TripleDESCryptoServiceProvider())
            {
                //create the md 5
                using (var MD5Provider = new MD5CryptoServiceProvider())
                {
                    //Set the security properties
                    DESCrypto.Key = MD5Provider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));
                    DESCrypto.Mode = CipherMode.ECB;

                    //get the buffer
                    byte[] Buffer = Convert.FromBase64String(TextToDecrypt);

                    //return the encrypted value
                    return ASCIIEncoding.ASCII.GetString(DESCrypto.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
                }
            }
        }

        #endregion

    }

}
