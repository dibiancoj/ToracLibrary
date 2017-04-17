using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.Email
{

    //  for testing you can put the following in your web config. Instead of using an actual email, it will save it to the local directory. Just double click and it will load in outlook just like you sent it
    //  <system.net>
    //  <mailSettings>
    //    <!-- dev, production -->
    //    <!--<smtp>
    //      <network host="exchsmtp2" port="25"/>
    //    </smtp>-->
    //    <!-- localhost -->
    //    <smtp deliveryMethod="SpecifiedPickupDirectory" from="NeedToFillInMaybeOnServer@navg.com">
    //      <specifiedPickupDirectory pickupDirectoryLocation="C:\Logs\Guru\email" />
    //    </smtp>
    //  </mailSettings>
    //</system.net>

    /// <summary>
    /// Sends E-mail via .NET SMTP
    /// </summary>
    /// <remarks>Class is immutable </remarks>
    /// <example>
    /// Gmail (Need to set your settings for the e-mail account to allow remote SMTP in your options)
    /// Port = 587
    /// Host = "smtp.gmail.com"
    /// Enable SSL = True
    /// </example>
    [Obsolete("use MailKit if possible. Microsoft just came out and said to use MailKit because its better and faster. https://github.com/jstedfast/MailKit")]
    public partial class SMTPEmailServer : ISMTPEmailServer
    {

        #region Constructor

        /// <summary>
        /// Constructor. (Overloaded when using default credentials) If You don't want to pass in your own SMTP client then you can do it by passing in your parameters
        /// </summary>
        /// <param name="EmailServerAddressToSet">The Server IP Or DNS Name - Example = "smtp.gmail.com"</param>
        /// <param name="PortNumberToSet">Port Number Of the Server - Example = 587</param>
        /// <param name="UseSSLConnectionToSet">Enable SSL. Does The SMTP Server Require SSL Connection</param>
        public SMTPEmailServer(string EmailServerAddressToSet, int PortNumberToSet, bool UseSSLConnectionToSet)
        {
            //Validate
            if (string.IsNullOrEmpty(EmailServerAddressToSet))
            {
                throw new ArgumentNullException("No Email Server Address Specified.");
            }

            if (PortNumberToSet == 0)
            {
                throw new ArgumentNullException("No Port Number Specified.");
            }
            //End of Validation

            //Set the properties
            HostAddress = EmailServerAddressToSet;
            PortNumber = PortNumberToSet;
            EnableSSLConnection = UseSSLConnectionToSet;
            UseDefaultCredentials = true;
        }

        /// <summary>
        /// Constructor. If You don't want to pass in your own SMTP client then you can do it by passing in your parameters
        /// </summary>
        /// <param name="EmailServerAddressToSet">The Server IP Or DNS Name - Example = "smtp.gmail.com"</param>
        /// <param name="PortNumberToSet">Port Number Of the Server - Example = 587</param>
        /// <param name="UseSSLConnectionToSet">Enable SSL. Does The SMTP Server Require SSL Connection</param>
        /// <param name="EmailServerUserNameToSet">The User Name Who The E-mail Is Coming From</param>
        /// <param name="EmailServerUserPWToSet">The User's Password Who The E-mail Is Coming From. Need This To Log Into The Server</param>
        public SMTPEmailServer(string EmailServerAddressToSet, int PortNumberToSet, bool UseSSLConnectionToSet, string EmailServerUserNameToSet, string EmailServerUserPWToSet)
        {
            //Validate
            if (EmailServerAddressToSet.IsNullOrEmpty())
            {
                throw new ArgumentNullException("No Email Server Address Specified.");
            }

            if (PortNumberToSet == 0)
            {
                throw new ArgumentNullException("No Port Number Specified.");
            }

            if (EmailServerUserNameToSet.IsNullOrEmpty())
            {
                throw new ArgumentNullException("No Email User Name Specified.");
            }

            if (EmailServerUserPWToSet.IsNullOrEmpty())
            {
                throw new ArgumentNullException("No Email User's Password Specified.");
            }
            //End of Validation

            //Set the properties
            HostAddress = EmailServerAddressToSet;
            PortNumber = PortNumberToSet;
            EnableSSLConnection = UseSSLConnectionToSet;
            EmailServerUserName = EmailServerUserNameToSet;
            EmailServerUserPassword = EmailServerUserPWToSet;
        }

        /// <summary>
        /// Constructor. You are using the web.config file for the settings. => system.net.mailSettings. DONT USE THIS IF YOU AREN'T USING THE WEB CONFIG AND YOU DON'T KNOW YOUR SETTINGS
        /// </summary>
        public SMTPEmailServer()
        {
            //set the property to use the web config
            UseWebConfigForSettings = true;
        }

        #endregion

        #region Immutable Variables

        /// <summary>
        /// Holds The Port Number Of The E-Mail Server
        /// </summary>
        private int PortNumber { get; }

        /// <summary>
        /// Holds The Server IP Or DNS Address Of The E-mail Server
        /// </summary>
        private string HostAddress { get; }

        /// <summary>
        /// Does The E-mail Provider Require A SSL Connection?
        /// </summary>
        private bool EnableSSLConnection { get; }

        /// <summary>
        /// User Name To Log Into The E-mail Server
        /// </summary>
        private string EmailServerUserName { get; }

        /// <summary>
        /// User Name's Password To Log Into The E-mail Server
        /// </summary>
        private string EmailServerUserPassword { get; }

        /// <summary>
        /// Use when you want to use windows authentication. Don't need to specify user name and pw
        /// </summary>
        private bool UseDefaultCredentials { get; }

        /// <summary>
        /// Do we want to use the web config for the settings?
        /// </summary>
        private bool UseWebConfigForSettings { get; }

        #endregion

        #region Public Methods

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
        public Task SendEmail(IEnumerable<string> ToEmailAddress,
                              IEnumerable<string> CCEmailAddress,
                              IEnumerable<string> BCCEmailAddress,
                              string FromEmailAddress,
                              string Subject,
                              string Body,
                              bool BodyContainsHTML,
                              MailPriority Priority,
                              IDictionary<string, byte[]> FileAttachments)
        {
            //Return the task.
            return Task.Factory.StartNew(() =>
            {
                //Create the instance of the e-mail server. Set the properties to the parameters passed in
                using (var SmtpClientToUse = new SmtpClient(HostAddress, PortNumber) { EnableSsl = EnableSSLConnection })
                {
                    //create the email message with a using so we know we are disposing of it. We need to cleanup the attached files
                    using (var Email = new SMTPEmailMessageHelper(ToEmailAddress, CCEmailAddress, BCCEmailAddress, FromEmailAddress, Subject, Body, BodyContainsHTML, Priority, FileAttachments))
                    {
                        //See if we are getting the settings from the web config
                        if (!UseWebConfigForSettings)
                        {
                            //Not Using The Web Config For The Settings
                            //If we want to use default credentials set it here
                            SmtpClientToUse.UseDefaultCredentials = UseDefaultCredentials;

                            //if we are not using default credentials set the user name and pw
                            if (!UseDefaultCredentials)
                            {
                                SmtpClientToUse.Credentials = new NetworkCredential(EmailServerUserName, EmailServerUserPassword);
                            }
                        }

                        //Send the e-mail.
                        SmtpClientToUse.Send(Email.MailMessageToSend);
                    }
                }
            });
        }

        #endregion

    }

}
