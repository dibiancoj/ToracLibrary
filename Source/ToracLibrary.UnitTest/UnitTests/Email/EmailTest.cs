using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Email;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.EmailSMTP
{

    /// <summary>
    /// Unit test smtp serialization
    /// </summary>
    public class EmailTest
    {

        //I want to be able to mock it up in the unit test, so i can ensure it works with DI.
        //Can't really test this easily, as I don't want to have to check emails to ensure it completes successfully

        #region Framework

        /// <summary>
        /// Mock up of the smtp email server
        /// </summary>
        internal class MockSMTPEmailServer : ISMTPEmailServer
        {
            public Task SendEmail(IEnumerable<string> ToEmailAddress, IEnumerable<string> CCEmailAddress, IEnumerable<string> BCCEmailAddress, string FromEmailAddress, string Subject, string Body, bool BodyContainsHTML, MailPriority Priority, IDictionary<string, byte[]> FileAttachments)
            {
                return Task.FromResult<object>(null);
            }
        }

        #endregion

        #region Unit Test

        [Fact]
        public async Task EmailSMTPTest1()
        {
            //go grab the di container
            var EmailServerToUse = DIUnitTestContainer.DIContainer.Resolve<ISMTPEmailServer>();

            //make sure we have an email server
            Assert.NotNull(EmailServerToUse);

            //now run the method
            await EmailServerToUse.SendEmail(new string[] { "dibiancoj@gmail.com" }, null, null, "Test@gmail.com", "Subject", "Body", true, MailPriority.Normal, null).ConfigureAwait(false);

            //now we will just pass it 
            Assert.True(true);
        }

        #endregion

    }

}
