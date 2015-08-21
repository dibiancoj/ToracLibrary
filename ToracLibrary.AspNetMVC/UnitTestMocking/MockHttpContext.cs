using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Class used to mock a HttpContextBase
    /// </summary>
    public class MockHttpContext : HttpContextBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RelativeUrlToSet">Relative Url</param>
        public MockHttpContext(string RelativeUrlToSet)
            : this(RelativeUrlToSet, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RelativeUrlToSet">Relative Url</param>
        /// <param name="Principal">Principal</param>
        /// <param name="FormParams">FormParams</param>
        /// <param name="QueryStringParams">QueryStringParams</param>
        /// <param name="Cookies">Cookies</param>
        /// <param name="SessionItems">SessionItems</param>
        public MockHttpContext(string RelativeUrlToSet, MockPrincipal PrincipalToSet, NameValueCollection FormParamsToSet, NameValueCollection QueryStringParamsToSet, HttpCookieCollection CookiesToSet, SessionStateItemCollection SessionItemsToSet)
        {
            RelativeUrl = RelativeUrlToSet;
            Principal = PrincipalToSet;
            FormParams = FormParamsToSet;
            QueryStringParams = QueryStringParamsToSet;
            Cookies = CookiesToSet;
            SessionItems = SessionItemsToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Relative Url
        /// </summary>
        private string RelativeUrl { get; }

        /// <summary>
        /// Principal
        /// </summary>
        private MockPrincipal Principal { get;  }

        /// <summary>
        /// Forms Params
        /// </summary>
        private NameValueCollection FormParams { get;  }

        /// <summary>
        /// Query string
        /// </summary>
        private NameValueCollection QueryStringParams { get;  }

        /// <summary>
        /// Cookies
        /// </summary>
        private HttpCookieCollection Cookies { get;  }

        /// <summary>
        /// Session variables
        /// </summary>
        private SessionStateItemCollection SessionItems { get;  }

        #endregion

        #region Override HttpContextBase Methods

        /// <summary>
        /// Request
        /// </summary>
        public override HttpRequestBase Request
        {
            get
            {
                return new MockHttpRequest(RelativeUrl, FormParams, QueryStringParams, Cookies);
            }
        }

        /// <summary>
        /// User
        /// </summary>
        public override IPrincipal User
        {
            get
            {
                return Principal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Session State
        /// </summary>
        public override HttpSessionStateBase Session
        {
            get
            {
                return new MockHttpSessionState(SessionItems);
            }
        }

        #endregion

    }

}
