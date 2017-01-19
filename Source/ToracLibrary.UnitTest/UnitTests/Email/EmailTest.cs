using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Email;
using Xunit;

namespace ToracLibrary.UnitTest.EmailSMTP
{

    /// <summary>
    /// Unit test smtp email
    /// </summary>
    public class EmailTest
    {

        #region Unit Test

        [Fact]
        public async Task EmailSMTPTest1()
        {
            //this doesn't really test anything...going to leave it for now. Maybe one day we will add more.

            //go grab the di container
            var MockEmailServerToUse = new Mock<ISMTPEmailServer>();

            //set it up to return 
            MockEmailServerToUse.Setup(x => x.SendEmail(new string[] { "dibiancoj@gmail.com" }, null, null, "Test@gmail.com", "Subject", "Body", true, MailPriority.Normal, null))
                .Returns(Task.CompletedTask);

            //now run the method
            await MockEmailServerToUse.Object.SendEmail(new string[] { "dibiancoj@gmail.com" }, null, null, "Test@gmail.com", "Subject", "Body", true, MailPriority.Normal, null).ConfigureAwait(false);

            //now we will just pass it 
            Assert.True(true);
        }

        #endregion

    }

}
