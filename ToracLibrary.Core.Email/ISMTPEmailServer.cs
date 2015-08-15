using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Email
{

    /// <summary>
    /// Public interface for the smtp email server. Allow this to be used with Dependency injection.
    /// </summary>
    public interface ISMTPEmailServer
    {

        /// <summary>
        /// Send an E-mail.
        /// </summary>
        /// <param name="ToEmailAddress">To Address --> IEnumerable-string</param>
        /// <param name="CCEmailAddress">CC Address --> IEnumerable-string</param>
        /// <param name="BCCEmailAddress">BCC Address --> IEnumerable-string</param>
        /// <param name="FromEmailAddress">From Address</param>
        /// <param name="Subject">Subject Of E-mail</param>
        /// <param name="Body">Body Of E-mail</param>
        /// <param name="BodyContainsHTML">Does The Body Of The E-mail Contain HTML</param>
        /// <param name="Priority">Priority Of The Mail When It Gets Sent</param>
        /// <param name="FileAttachments">Dictionary of file names and value of file bytes</param>
        /// <returns>Task. Check if the task is successful by checking the IsFaulted property of the task</returns>
        /// <remarks>Pass in null if you don't want to have any cc email address or bcc emails address</remarks>
        Task SendEmail(IEnumerable<string> ToEmailAddress,
                              IEnumerable<string> CCEmailAddress,
                              IEnumerable<string> BCCEmailAddress,
                              string FromEmailAddress,
                              string Subject,
                              string Body,
                              bool BodyContainsHTML,
                              MailPriority Priority,
                              IDictionary<string, byte[]> FileAttachments);

    }

}
