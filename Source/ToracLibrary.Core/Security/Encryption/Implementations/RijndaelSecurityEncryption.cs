using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.Security.Encryption
{

    /// <summary>
    /// Class Used To Encrypt And Decrypt String Values Using The RijndaelManaged Protocol
    /// </summary>
    /// <remarks>Class Is Immutable With Properties</remarks>
    public class RijndaelSecurityEncryption : ITwoWaySecurityEncryption
    {

        #region Constructor

        /// <summary>
        /// Constructor The Class
        /// </summary>
        /// <param name="InitializedVectorToSet">rgbIV: The initialization vector to use for the symmetric algorithm. Must Be 16 Bytes / Characters</param>
        /// <param name="PublicKeyToSet">rgbKey: The secret key to use for the symmetric algorithm.</param>
        public RijndaelSecurityEncryption(string InitializedVectorToSet, string PublicKeyToSet)
        {
            //Validate The Parameters For The Constructor
            if (InitializedVectorToSet.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Initialized Vector Must Not Be Blank");
            }

            if (PublicKeyToSet.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Public Key Must Not Be Blank");
            }

            //go valid the length of the keys it will raise an error if it find's an error
            IsValidKey(InitializedVectorToSet);

            //go validate the key now
            IsValidKey(PublicKeyToSet);

            //End of Validation

            //Set the properties
            InitVector = InitializedVectorToSet;
            PublicKey = PublicKeyToSet;
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// The secret key to use for the symmetric algorithm.
        /// </summary>
        private string PublicKey { get; }
       
        /// <summary>
        /// The initialization vector to use for the symmetric algorithm.
        /// </summary>
        private string InitVector { get; }

        #endregion

        #region Main Calls

        #region Encrypt

        /// <summary>
        /// Encrypt A String Value Based On The Keys You Passed In The Constructor. Uses the RijndaelManaged Protocol Method
        /// </summary>
        /// <param name="ValueToEncrypt">Value To Encrypt</param>
        /// <returns>The encrypted value in a string</returns>
        public string Encrypt(string ValueToEncrypt)
        {
            //Validate the string to encrypt
            if (ValueToEncrypt.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Value To Encrypt Can't Be Null.");
            }

            //declare the memory stream
            using (var EncryptMemoryStream = new MemoryStream())
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                //Create a encryptor to perform the stream transform.
                using (var EncryptCryptoStream = new CryptoStream(EncryptMemoryStream,
                                                                  new RijndaelManaged().CreateEncryptor(
                                                                  Encoding.UTF8.GetBytes(PublicKey),
                                                                  Encoding.UTF8.GetBytes(InitVector)),
                                                                  CryptoStreamMode.Write))
                {
                    //Create the stream writer to write the data
                    using (var swEncrypt = new StreamWriter(EncryptCryptoStream))
                    {
                        swEncrypt.Write(ValueToEncrypt);
                    }
                }

                //return the encrypted string 
                return Convert.ToBase64String(EncryptMemoryStream.ToArray());
            }
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypt A String Value Based On The Keys You Passed In The Constructor. Uses the RijndaelManaged Protocol Method
        /// </summary>
        /// <param name="EncryptedText">Value To Decrypt</param>
        /// <returns>The decrypted value in a string</returns>
        public string Decrypt(string EncryptedText)
        {
            //Validate the text to encrypt
            if (EncryptedText.IsNullOrEmpty())
            {
                throw new ArgumentNullException("Value To Decrypt Can't Be Null.");
            }

            //declare the memory stream
            using (var DecryptMemoryStream = new MemoryStream(Convert.FromBase64String(EncryptedText)))
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                // Create a decryptor to perform the stream transform.
                using (var DescryptCryptoStream = new CryptoStream(DecryptMemoryStream,
                                                                   new RijndaelManaged().CreateDecryptor(
                                                                   Encoding.UTF8.GetBytes(PublicKey),
                                                                   Encoding.UTF8.GetBytes(InitVector)),
                                                                   CryptoStreamMode.Read))
                {
                    //Create the stream writer to write the data
                    using (var srDecrypt = new StreamReader(DescryptCryptoStream))
                    {
                        //return the decrypted string 
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Supporting Calls

        /// <summary>
        /// Check to see if the Initialized Vector Is The Correct Length. Is A Private Static Method Which Is Called In The Constructor
        /// </summary>
        /// <param name="Key">Key Value To Check</param>
        /// <returns>Boolean if its a valid size</returns>
        private static void IsValidKey(string Key)
        {
            //set the size of the key
            int SizeOfKey = Encoding.UTF8.GetBytes(Key).Length;

            //check the length of the key passed in
            if (SizeOfKey != 16)
            {
                throw new IndexOutOfRangeException("The Initialized Vector Must Be 16 Bytes (16 characters). Your Current Size Is " + SizeOfKey.ToString());
            }
        }

        #endregion

    }

}
