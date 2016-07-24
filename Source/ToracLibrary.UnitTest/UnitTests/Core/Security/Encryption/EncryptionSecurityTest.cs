using System;
using ToracLibrary.Core.Security.Encryption;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to test the Encryption
    /// </summary>
    public class EncryptionSecurityTest
    {

        #region Constants

        /// <summary>
        /// Holds the md5 Di container name
        /// </summary>
        internal const string MD5DIContainerName = "MD5";

        /// <summary>
        /// Rijndael container name for the di container
        /// </summary>
        internal const string RijndaelDIContainerName = "Rijndael";

        /// <summary>
        /// Sha 256 container name
        /// </summary>
        internal const string SHA256ContainerName = "SHA256";

        /// <summary>
        /// Value to test
        /// </summary>
        private const string ValueToTest = "test123";

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the MD5 Hash Encryption
        /// </summary>
        [Fact]
        public void EncryptionMD5HashTest1()
        {
            //create the implementation of the interface
            var EncryptImplementation = DIUnitTestContainer.DIContainer.Resolve<ITwoWaySecurityEncryption>(MD5DIContainerName);

            //go encrypt the value
            var EncryptedValue = EncryptImplementation.Encrypt(ValueToTest);

            //is it what we are expecting
            Assert.Equal("6Ktjr0b7Wj0=", EncryptedValue);

            //go decrypt it
            var DecryptedValue = EncryptImplementation.Decrypt(EncryptedValue);

            //check the decrypted value
            Assert.Equal(ValueToTest, DecryptedValue);
        }

        /// <summary>
        /// Test the Rijndael Encrytion
        /// </summary>
        [Fact]
        public void EncryptionRijndaelSecurityTest1()
        {
            //create the implementation of the interface
            var EncryptImplementation = DIUnitTestContainer.DIContainer.Resolve<ITwoWaySecurityEncryption>(RijndaelDIContainerName);

            //go encrypt the value
            var EncryptedValue = EncryptImplementation.Encrypt(ValueToTest);

            //is it what we are expecting
            Assert.Equal("bo1JgQZZcRDRqmjNK47h2Q==", EncryptedValue);

            //go decrypt it
            var DecryptedValue = EncryptImplementation.Decrypt(EncryptedValue);

            //check the decrypted value
            Assert.Equal(ValueToTest, DecryptedValue);
        }

        /// <summary>
        /// Test the SHA256 Encrytion
        /// </summary>
        [Fact]
        public void EncryptionSHA256SecurityTest1()
        {
            //create the implementation of the interface
            var EncryptImplementation = DIUnitTestContainer.DIContainer.Resolve<IOneWaySecurityEncryption>(SHA256ContainerName);

            //go encrypt the value
            var EncryptedValue = EncryptImplementation.Encrypt(ValueToTest);

            //is it what we are expecting
            Assert.Equal("ECD71870D1963316A97E3AC3408C9835AD8CF0F3C1BC703527C30265534F75AE", EncryptedValue);
        }

        #endregion

    }

}