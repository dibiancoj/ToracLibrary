using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.Security.Encryption;
using ToracLibraryTest.Framework;
using ToracLibrary.DIContainer;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test the Encryption
    /// </summary>
    [TestClass]
    public class EncryptionSecurityTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //let's register the di container now (md5)
            DIContainer.Register<ISecurityEncryption, MD5HashSecurityEncryption>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(MD5DIContainerName)
                .WithConstructorImplementation(() => new MD5HashSecurityEncryption("Test"));

            //let's register the rijndael container now
            DIContainer.Register<ISecurityEncryption, RijndaelSecurityEncryption>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(RijndaelDIContainerName)
                .WithConstructorImplementation(() => new RijndaelSecurityEncryption("1234567891123456", "1234567891123456"));
        }

        #endregion

        #region Constants

        /// <summary>
        /// Holds the md5 Di container name
        /// </summary>
        private const string MD5DIContainerName = "MD5";

        /// <summary>
        /// Rijndael container name for the di container
        /// </summary>
        private const string RijndaelDIContainerName = "Rijndael";

        /// <summary>
        /// Value to test
        /// </summary>
        private const string ValueToTest = "test123";

        #endregion

        #region Unit Tests

        /// <summary>
        /// Test the MD5 Hash Encryption
        /// </summary>
        [TestCategory("Core.Security.Encryption")]
        [TestCategory("Core.Security")]
        [TestCategory("Core")]
        [TestMethod]
        public void EncryptionMD5HashTest1()
        {
            //create the implementation of the interface
            var EncryptImplementation = DIUnitTestContainer.DIContainer.Resolve<ISecurityEncryption>(MD5DIContainerName);

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
        [TestCategory("Core.Security.Encryption")]
        [TestCategory("Core.Security")]
        [TestCategory("Core")]
        [TestMethod]
        public void EncryptionRijndaelSecurityTest1()
        {
            //create the implementation of the interface
            var EncryptImplementation = DIUnitTestContainer.DIContainer.Resolve<ISecurityEncryption>(RijndaelDIContainerName);

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