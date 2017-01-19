using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ToracLibrary.AspNet.Mocking
{

    /// <summary>
    /// Fake the http context which is tough to mock. This is for legacy asp.net and httpcontext.current
    /// </summary>
    public static class FakeHttpContext
    {

        /// <summary>
        /// Build a fake http context and return it
        /// </summary>
        /// <param name="Url">Url to fake</param>
        /// <returns>Build up httpcontext</returns>
        public static HttpContext BuildFakeHttpContext(string Url)
        {
            //grab the uri to use
            var UriToUse = new Uri(Url);

            //build up the request
            var HttpRequestToUse = new HttpRequest(string.Empty, UriToUse.ToString(), UriToUse.Query.TrimStart('?'));

            //build the string writer
            var StringWriterToUse = new StringWriter();

            //respose to use
            var HttpResponseToUse = new HttpResponse(StringWriterToUse);

            //build the context with the request and response
            var HttpContextToUse = new HttpContext(HttpRequestToUse, HttpResponseToUse);

            //add a dummy session state. You can use HttpContext.Current.Session["bla"] =....
            var SessionContainerToUse = new HttpSessionStateContainer("id",
                                            new SessionStateItemCollection(),
                                            new HttpStaticObjectsCollection(),
                                            10, true, HttpCookieMode.AutoDetect,
                                            SessionStateMode.InProc, false);

            //Add session state to the context
            SessionStateUtility.AddHttpSessionStateToContext(HttpContextToUse, SessionContainerToUse);

            //return the context
            return HttpContextToUse;
        }

    }

}
