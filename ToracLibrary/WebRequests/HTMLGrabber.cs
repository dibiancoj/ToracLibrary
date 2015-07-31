using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.WebRequests
{

    /// <summary>
    /// Gather The HTML Or Encoding For A Web Site.
    /// </summary>
    /// <remarks>Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class HTMLGrabber
    {

        #region Documentation

        /* Example of how to call the class
         *  private void Form1_Load(object sender, EventArgs e)
            {
         *     If you want to set the proxy credentials: (pass the object below in)
         *     myRequest.Proxy.Credentials = new NetworkCredential() { UserName = @"corp\jdibianco", Password = "welcome2" };
         *     
                string HtmlResult = HTMLGrabber.GatherHTMLForWebSite("http://www.google.com");
            }
        */

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        public static string GatherHTMLForWebSite(string URL)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, null, null);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="ProxyCredentials">Proxy Credentials - Null If You Don't Want To Set It (NetworkCredential Class Implements ICredentials)</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        public static string GatherHTMLForWebSite(string URL, ICredentials ProxyCredentials)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, null, ProxyCredentials);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="Timeout">Web Request Timeout in milliseconds</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        public static string GatherHTMLForWebSite(string URL, int Timeout)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, Timeout, null);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="UserAgent">User Agent</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        public static string GatherHTMLForWebSite(string URL, string UserAgent)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, UserAgent, null, null);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="Timeout">Web Request Timeout in milliseconds</param>
        /// <param name="UserAgent">User Agent</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        public static string GatherHTMLForWebSite(string URL, int Timeout, string UserAgent)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, UserAgent, Timeout, null);
        }

        #endregion

        #region Private Main Helper Method

        /// <summary>
        /// Gather the HTML Or Encoding. Call Could be lengthy and may freeze UI. Please Consider Using GatherHTMLForWebSiteTask Which Will Return A Task Of String
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="UserAgent">User Agent. Pass In Null To Not Set It</param>
        /// <param name="CommandTimeout">Command Timeout In Milliseconds</param>
        /// <param name="ProxyCredentials">Proxy Credentials - Null If You Don't Want To Set It (NetworkCredential Class Implements ICredentials)</param>
        /// <returns>String -- > HTML Of URL</returns>
        private static string GatherHTMLForWebSiteHelper(string URL, string UserAgent, Nullable<int> CommandTimeout, ICredentials ProxyCredentials)
        {
            //Validation
            if (string.IsNullOrEmpty(URL))
            {
                throw new ArgumentNullException("URL Can't Be Null.");
            }
            //End of validation

            //go create the web request with the url   
            var RequestToMake = WebRequest.Create(URL);

            //if the user agent is not set then set it
            if (!string.IsNullOrEmpty(UserAgent))
            {
                //set the user agent
                ((HttpWebRequest)RequestToMake).UserAgent = UserAgent;
            }

            //if there is a command timeout then set it now (in milliseconds)
            if (CommandTimeout.HasValue)
            {
                RequestToMake.Timeout = CommandTimeout.Value;
            }

            //if we have proxy credentials then set it here
            if (ProxyCredentials != null)
            {
                //set the credentials
                RequestToMake.Proxy.Credentials = ProxyCredentials;
            }

            //Get the response.
            using (var WebResponse = (HttpWebResponse)RequestToMake.GetResponse())
            {
                //Get the stream containing content returned by the server.
                using (Stream DataStream = WebResponse.GetResponseStream())
                {
                    //Open the stream using a StreamReader for easy access.
                    using (StreamReader DataStreamReader = new StreamReader(DataStream))
                    {
                        //Read the content.
                        return DataStreamReader.ReadToEnd();
                    }
                }
            }
        }

        #endregion

    }

}
