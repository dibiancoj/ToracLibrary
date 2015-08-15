using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.Email
{

    public partial class SMTPEmailServer
    {

        /// <summary>
        /// Helper Class Which Makes Sure All The Resources Are Released After Sending The E-mail
        /// </summary>
        private class SMTPEmailMessageHelper : IDisposable
        {

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ToEmailAddress">E-mails To Send Too</param>
            /// <param name="CCEmailAddress">CC E-mails To Send Too</param>
            /// <param name="BCCEmailAddress">Bcc E-mails To Send Too</param>
            /// <param name="FromEmailAddress">From E-mail which we send from</param>
            /// <param name="Subject">Subtract Of E-mail</param>
            /// <param name="EmailBody">E-mail Body</param>
            /// <param name="BodyIsHTML">Is The Body Html Or Just Plain Text?</param>
            /// <param name="Priority">Mail Message Priority</param>
            /// <param name="FileAttachments">Dictionary of file names and value of file bytes</param>
            /// <remarks>Class is immutable</remarks>
            public SMTPEmailMessageHelper(IEnumerable<string> ToEmailAddress,
                                          IEnumerable<string> CCEmailAddress,
                                          IEnumerable<string> BCCEmailAddress,
                                          string FromEmailAddress,
                                          string Subject,
                                          string EmailBody,
                                          bool BodyIsHTML,
                                          MailPriority Priority,
                                          IDictionary<string, byte[]> FileAttachments)
            {
                //first let's create a new instace of the property
                MailMessageToSend = new MailMessage();

                //Validate The List Of People To Send The E-mail To
                if (!ToEmailAddress.AnyWithNullCheck() && !CCEmailAddress.AnyWithNullCheck() && !BCCEmailAddress.AnyWithNullCheck())
                {
                    throw new ArgumentNullException("Need To The E-mail Message To Atleast 1 Person");
                }

                //if you have a from access then add it. Otherwise if you pass in null it will grab it from the web config (if available - error's if it can't load the web.config)
                if (!string.IsNullOrEmpty(FromEmailAddress))
                {
                    //add the from e-mail address
                    MailMessageToSend.From = new MailAddress(FromEmailAddress);

                    //Add the reply to
                    MailMessageToSend.ReplyToList.Add(new MailAddress(FromEmailAddress));
                }

                //Add the subject
                MailMessageToSend.Subject = Subject;

                //add the body
                MailMessageToSend.Body = EmailBody;

                //add the MailPriority
                MailMessageToSend.Priority = Priority;

                //does the body contain html
                MailMessageToSend.IsBodyHtml = BodyIsHTML;

                //if we have an attachment then attach it now
                if (FileAttachments.AnyWithNullCheck())
                {
                    //let's loop through each of the attachments and add it to the email
                    foreach (KeyValuePair<string, byte[]> FileToAttach in FileAttachments)
                    {
                        //add this attachment
                        MailMessageToSend.Attachments.Add(new Attachment(BuildAttachmentMemoryStream(FileToAttach.Value), FileToAttach.Key));
                    }
                }

                //*****************Add The Recipients*****************

                //Add the To E-mail Address's if the list is valid
                if (ToEmailAddress.AnyWithNullCheck())
                {
                    foreach (string ToEmail in ToEmailAddress)
                    {
                        //validate it's a valid e-mail address
                        if (!ToEmail.IsValidEmailAddress())
                        {
                            throw new ArgumentOutOfRangeException("ToEmailAddress", ToEmail, string.Format($"{ToEmail} Is An Invalid E-mail Address In The ToEmailAddress List"));
                        }

                        //it's a valid email address..add it
                        MailMessageToSend.To.Add(ToEmail);
                    }
                }

                //Add the CC's if the list is valid
                if (CCEmailAddress.AnyWithNullCheck())
                {
                    foreach (string CCEmail in CCEmailAddress)
                    {
                        //validate it's a valid e-mail address
                        if (!CCEmail.IsValidEmailAddress())
                        {
                            throw new ArgumentOutOfRangeException("ToEmailAddress", CCEmail, string.Format($"{CCEmail} Is An Invalid E-mail Address In The CCEmailAddress List"));
                        }

                        //it's a valid email address..add it
                        MailMessageToSend.CC.Add(CCEmail);
                    }
                }

                //Add the BCC if the list is valid
                if (BCCEmailAddress.AnyWithNullCheck())
                {
                    foreach (string BCCEmail in BCCEmailAddress)
                    {
                        //validate it's a valid e-mail address
                        if (!BCCEmail.IsValidEmailAddress())
                        {
                            throw new ArgumentOutOfRangeException("ToEmailAddress", BCCEmail, string.Format($"{BCCEmail} Is An Invalid E-mail Address In The BCCEmailAddress List"));
                        }

                        //it's a valid email address..add it
                        MailMessageToSend.Bcc.Add(BCCEmail);
                    }
                }

                //*****************End Of The Recipients*****************
            }

            #endregion

            #region Properties

            /// <summary>
            /// Holds a flag if the class has been disposed yet or called to be disposed yet
            /// </summary>
            /// <remarks>Used IDisposable</remarks>
            private bool disposed { get; set; }

            /// <summary>
            /// Mail Message That Was Built
            /// </summary>
            /// <remarks>Make the variable immutable</remarks>
            internal MailMessage MailMessageToSend { get; }

            #endregion

            #region Helper Methods

            /// <summary>
            /// Builds a memory stream for a file attachment
            /// </summary>
            /// <param name="thisAttachment">This File Attachment</param>
            /// <returns>MemoryStream</returns>
            private static MemoryStream BuildAttachmentMemoryStream(byte[] thisAttachment)
            {
                //*** we need this special method because we don't want to dispose of it until after we dispose of this message.
                // otherwise it will blow up that its been disposed when we go to send the email. Otherwise i would have this in some generic method in IO.

                //memory stream item that will be returned (is disposed when SMTP Email Message Gets disposed)
                var ms = new MemoryStream();

                //write the byte array
                ms.Write(thisAttachment, 0, thisAttachment.Length);

                //reset it back to the beg.
                ms.Position = 0;

                //return the memory stream
                return ms;
            }

            #endregion

            #region Dispose Method

            /// <summary>
            /// Disposes My Object
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Dispose Overload. Ensures my database connection is closed
            /// </summary>
            private void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        //if we have attachments...then go dispose of them
                        if (MailMessageToSend.Attachments.AnyWithNullCheck())
                        {
                            //loop through the attachments
                            foreach (Attachment thisAttachment in MailMessageToSend.Attachments)
                            {
                                //dipose of the attachments
                                thisAttachment.Dispose();
                            }
                        }

                        //now let's dispose of the mail message
                        MailMessageToSend.Dispose();
                    }
                }
                disposed = true;
            }

            #endregion

        }

    }

}
