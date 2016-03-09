using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.Core.WebRequests
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

        //to see how you can carry session state over in a web request, view ToracLibrary.AspNet.SessionState.SessionStateCarryOverInWebRequest

        #region Public Static Methods

        #region Post

        /// <summary>
        /// Make a web request using a post
        /// </summary>
        /// <param name="UrlToPost">url to post to</param>
        /// <param name="PostBodyParameters">Parameters into the post. If using application/x-www-form-urlencoded then key value pair ("UserName=peterppp&Password=password1"). Else if json, it would be json</param>
        /// <param name="ContentType">content type such as application/x-www-form-urlencoded or json</param>
        /// <param name="CookieJar">CookieCollection so we can keep session on multiple requests. use new CookieCollection and keep passing in the parameters</param>
        /// <returns>Html of the web request</returns>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string PostWebRequest(string UrlToPost, string PostBodyParameters, string ContentType, CookieContainer CookieJar)
        {
            // string postData = "UserName=peterppp&Password=password1";

            //go create the web request
            var PostRequest = (HttpWebRequest)WebRequest.Create(UrlToPost);

            //set the mehtod
            PostRequest.Method = "POST";

            //grab the post data in bytes
            byte[] PostDataInBytes = new ASCIIEncoding().GetBytes(PostBodyParameters);

            // Set the content type of the data being posted.
            PostRequest.ContentType = ContentType;

            // Set the content length of the string being posted.
            PostRequest.ContentLength = PostDataInBytes.Length;

            //set the cookie container
            PostRequest.CookieContainer = CookieJar;

            //go crewate the request stream so we can write the parameters for the method
            using (var BodyParameterStream = PostRequest.GetRequestStream())
            {
                //write the parameters
                BodyParameterStream.Write(PostDataInBytes, 0, PostDataInBytes.Length);
            }

            //go grab the response
            using (var PostResponse = (HttpWebResponse)PostRequest.GetResponse())
            {
                //go read the response and return it
                return new StreamReader(PostResponse.GetResponseStream()).ReadToEnd();
            }
        }

        #endregion

        #region Generic 

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, null, null, new CookieContainer());
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL, CookieContainer CookieJar)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, null, null, CookieJar);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="ProxyCredentials">Proxy Credentials - Null If You Don't Want To Set It (NetworkCredential Class Implements ICredentials)</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL, ICredentials ProxyCredentials, CookieContainer CookieJar)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, null, ProxyCredentials, CookieJar);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="Timeout">Web Request Timeout in milliseconds</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL, int Timeout, CookieContainer CookieJar)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, null, Timeout, null, CookieJar);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="UserAgent">User Agent</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL, string UserAgent, CookieContainer CookieJar)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, UserAgent, null, null, CookieJar);
        }

        /// <summary>
        /// Gather the Html That Is Received When Loading This Url
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="Timeout">Web Request Timeout in milliseconds</param>
        /// <param name="UserAgent">User Agent</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>Returns The HTML Of Web Request</returns>
        /// <remarks>HTML</remarks>
        [MethodIsNotTestable("Not going to simulate a web request")]
        public static string GatherHTMLForWebSite(string URL, int Timeout, string UserAgent, CookieContainer CookieJar)
        {
            //call the base method
            return GatherHTMLForWebSiteHelper(URL, UserAgent, Timeout, null, CookieJar);
        }

        #endregion

        #endregion

        #region Private Main Helper Method

        /// <summary>
        /// Gather the HTML Or Encoding. Call Could be lengthy and may freeze UI. Please Consider Using GatherHTMLForWebSiteTask Which Will Return A Task Of String
        /// </summary>
        /// <param name="URL">URL To Gather HTML From</param>
        /// <param name="UserAgent">User Agent. Pass In Null To Not Set It</param>
        /// <param name="CommandTimeout">Command Timeout In Milliseconds</param>
        /// <param name="ProxyCredentials">Proxy Credentials - Null If You Don't Want To Set It (NetworkCredential Class Implements ICredentials)</param>
        /// <param name="CookieJar">Cookie container so the session and everything will be passed along as you go page to page or request to request. Pass in null or use new CookieContainer(). Then just keep passing it in if you want to make more then 1 web request</param>
        /// <returns>String -- > HTML Of URL</returns>
        [MethodIsNotTestable("Not going to simulate a web request")]
        private static string GatherHTMLForWebSiteHelper(string URL, string UserAgent, int? CommandTimeout, ICredentials ProxyCredentials, CookieContainer CookieJar)
        {
            //to see how you can carry session state over in a web request, view ToracLibrary.AspNet.SessionState.SessionStateCarryOverInWebRequest.CarrySessionStateOverInWebRequest

            //Validation
            if (string.IsNullOrEmpty(URL))
            {
                throw new ArgumentNullException("URL Can't Be Null.");
            }
            //End of validation

            //go create the web request with the url   
            var RequestToMake = WebRequest.Create(URL);

            //if we have a cookie container then add it now
            if (CookieJar != null)
            {
                //add the cookie container
                ((HttpWebRequest)RequestToMake).CookieContainer = CookieJar;
            }

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
