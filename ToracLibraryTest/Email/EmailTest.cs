using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Email;
using ToracLibrary.DIContainer;
using System.Net.Mail;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.EmailSMTP
{

    /// <summary>
    /// Unit test smtp serialization
    /// </summary>
    [TestClass]
    public class EmailTest : IDependencyInject
    {

        //I want to be able to mock it up in the unit test, so i can ensure it works with DI.
        //Can't really test this easily, as I don't want to have to check emails to ensure it completes successfully

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //let's register the di container for the readonly EF Data provider
            DIContainer.Register<ISMTPEmailServer, MockSMTPEmailServer>();
        }

        #endregion

        #region Framework

        /// <summary>
        /// Mock up of the smtp email server
        /// </summary>
        private class MockSMTPEmailServer : ISMTPEmailServer
        {
            public Task SendEmail(IEnumerable<string> ToEmailAddress, IEnumerable<string> CCEmailAddress, IEnumerable<string> BCCEmailAddress, string FromEmailAddress, string Subject, string Body, bool BodyContainsHTML, MailPriority Priority, IDictionary<string, byte[]> FileAttachments)
            {
                return Task.FromResult<object>(null);
            }
        }

        #endregion

        #region Unit Test

        [TestCategory("Email")]
        [TestMethod]
        public async Task EmailSMTPTest1()
        {
            //go grab the di container
            var EmailServerToUse = DIUnitTestContainer.DIContainer.Resolve<ISMTPEmailServer>();

            //make sure we have an email server
            Assert.IsNotNull(EmailServerToUse);

            //now run the method
            await EmailServerToUse.SendEmail(new string[] { "dibiancoj@gmail.com" }, null, null, "Test@gmail.com", "Subject", "Body", true, MailPriority.Normal, null);

            //now we will just pass it 
            Assert.AreEqual(true, true);
        }

        #endregion

    }

}
