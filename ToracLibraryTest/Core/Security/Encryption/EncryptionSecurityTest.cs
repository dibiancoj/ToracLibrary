using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.Security.Encryption;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test the Encryption
    /// </summary>
    [TestClass]
    public class EncryptionSecurityTest
    {

        #region Constants

        /// <summary>
        /// Value to test
        /// </summary>
        private const string ValueToTest = "test123";

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the MD5 Hash Encryption
        /// </summary>
        [TestMethod]
        public void EncryptionMD5HashTest1()
        {
            //create the implementation of the interface
            ISecurityEncryption EncryptImplementation = new MD5HashSecurityEncryption("Test");

            //go encrypt the value
            var EncryptedValue = EncryptImplementation.Encrypt(ValueToTest);

            //is it what we are expecting
            Assert.AreEqual("6Ktjr0b7Wj0=", EncryptedValue);

            //go decrypt it
            var DecryptedValue = EncryptImplementation.Decrypt(EncryptedValue);

            //check the decrypted value
            Assert.AreEqual(ValueToTest, DecryptedValue);
        }

        /// <summary>
        /// Test the Rijndael Encrytion
        /// </summary>
        [TestMethod]
        public void EncryptionRijndaelSecurityTest1()
        {
            //create the implementation of the interface
            ISecurityEncryption EncryptImplementation = new RijndaelSecurityEncryption("1234567891123456", "1234567891123456");

            //go encrypt the value
            var EncryptedValue = EncryptImplementation.Encrypt(ValueToTest);

            //is it what we are expecting
            Assert.AreEqual("bo1JgQZZcRDRqmjNK47h2Q==", EncryptedValue);

            //go decrypt it
            var DecryptedValue = EncryptImplementation.Decrypt(EncryptedValue);

            //check the decrypted value
            Assert.AreEqual(ValueToTest, DecryptedValue);
        }

        #endregion

    }

}