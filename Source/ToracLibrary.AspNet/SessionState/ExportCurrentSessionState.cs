using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.AspNet.SessionState
{

    /// <summary>
    /// Carry session state over in a web request.
    /// </summary>
    public static class ExportCurrentSessionState
    {

        #region Constants

        /// <summary>
        /// Holds the session state cookie name.
        /// </summary>
        public const string SessionStateCookieName = "ASP.NET_SessionId";

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the cookie from the request which you can add to the cookie container. So the cookie gets put into the container. The cookie container gets set on the web request. Then when the webrequest is made it see's that cookie with the same session id and it hooks the session is automatically.
        /// essentially we are hijacking session state
        /// </summary>
        /// <param name="Request">Request where we can pull in the session state cookie so we can transfer it</param>
        /// <param name="DomainNameOfCookieToUse">Domain name to use Use Request.Url.Host or something else (generally a sub domain so you can share the cookies).</param>
        /// <returns>The cookie which you can add to the cookie container. CookieContainer.Add(Result)</returns>
        [MethodIsNotTestable("HttpRequest Has Readonly Cookies. Not Sure How To Mock It. As Long As We Test The HttpRequestBase Overload We Should Be Ok Since We Are Calling That Overload For The Main Logic Function.")]
        public static Cookie ExportSessionState(HttpRequest Request, string DomainNameOfCookieToUse)
        {
            //remember this is only for in proc session state. in a web server farm, with stick sessions you could land on the other server
            //this will transfer your asp.net session id cookie which will get picked up when the web request is made

            //this only works if your calling the same server with the same domain!!!

            //use the httprequestbase which is unit tested...Testing / mocking the HttpRequest is hard because HttpCookieCollection is readonly and the class is sealed
            return ExportSessionState(new HttpRequestWrapper(Request), DomainNameOfCookieToUse);
        }

        /// <summary>
        /// Builds the cookie from the request which you can add to the cookie container. So the cookie gets put into the container. The cookie container gets set on the web request. Then when the webrequest is made it see's that cookie with the same session id and it hooks the session is automatically.
        /// essentially we are hijacking session state
        /// </summary>
        /// <param name="Request">Request where we can pull in the session state cookie so we can transfer it</param>
        /// <param name="DomainNameOfCookieToUse">Domain name to use Use Request.Url.Host or something else (generally a sub domain so you can share the cookies).</param>
        /// <returns>The cookie which you can add to the cookie container. CookieContainer.Add(Result)</returns>
        public static Cookie ExportSessionState(HttpRequestBase Request, string DomainNameOfCookieToUse)
        {
            //grab the session id cookie and return it
            return BuildCookie(Request.Cookies[SessionStateCookieName], DomainNameOfCookieToUse);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Builds the cookie from the http cookie.
        /// </summary>
        /// <param name="SessionStateCookie">Session state cookie to duplicate</param>
        /// <param name="DomainNameOfCookieToUse">Domain name to use Use Request.Url.Host or something else (generally a sub domain so you can share the cookies).</param>
        /// <returns></returns>
        private static Cookie BuildCookie(HttpCookie SessionStateCookie, string DomainNameOfCookieToUse)
        {
            //make sure we have that cookie
            if (SessionStateCookie == null)
            {
                throw new NullReferenceException("Can't Find The ASP.NET_SessionId Cookie In The Requests Cookie List");
            }

            //go return the cookie
            return new Cookie
            {
                //use the host name here.
                Domain = DomainNameOfCookieToUse,//Request.Url.Host,
                Expires = SessionStateCookie.Expires,
                Name = SessionStateCookie.Name,
                Path = SessionStateCookie.Path,
                Secure = SessionStateCookie.Secure,
                Value = SessionStateCookie.Value
            };
        }

        #endregion

    }

}
