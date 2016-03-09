using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ToracLibrary.AspNet.SessionState
{

    /// <summary>
    /// Carry session state over in a web request.
    /// </summary>
    public static class SessionStateCarryOverInWebRequest
    {

        /// <summary>
        /// Builds the cookie from the request which you can add to the cookie container. So the cookie gets put into the container. The cookie container gets set on the web request. Then when the webrequest is made it see's that cookie with the same session id and it hooks the session is automatically.
        /// essentially we are hijacking session state
        /// </summary>
        /// <param name="Request">Request where we can pull in the session state cookie so we can transfer it</param>
        /// <returns>The cookie which you can add to the cookie container. CookieContainer.Add(Result)</returns>
        public static Cookie CarrySessionStateOverInWebRequest(HttpRequest Request)
        {
            //remember this is only for in proc session state. in a web server farm, with stick sessions you could land on the other server
            //this will transfer your asp.net session id cookie which will get picked up when the web request is made

            //this only works if your calling the same server with the same domain!!!

            //grab the session id cookie
            var SessionStateCookie = Request.Cookies["ASP.NET_SessionId"];

            //make sure we have that cookie
            if (SessionStateCookie == null)
            {
                throw new NullReferenceException("Can't Find The ASP.NET_SessionId Cookie In The Requests Cookie List");
            }

            // Convert between the System.Net.Cookie to a System.Web.HttpCookie... and return it
            return new Cookie
            {
                //use the host name here.
                Domain = Request.Url.Host,
                Expires = SessionStateCookie.Expires,
                Name = SessionStateCookie.Name,
                Path = SessionStateCookie.Path,
                Secure = SessionStateCookie.Secure,
                Value = SessionStateCookie.Value
            };
        }

    }

}
