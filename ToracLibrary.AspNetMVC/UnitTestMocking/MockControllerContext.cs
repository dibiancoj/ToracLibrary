using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Enabled a user to unit test an mvc controller with session and other http type functionality
    /// </summary>
    public class MockControllerContext : ControllerContext
    {

        #region Constructor Overloads

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        public MockControllerContext(IController ControllerToSet)
            : this(ControllerToSet, string.Empty, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="CookiesToSet">Cookies to be used in the controller</param>
        public MockControllerContext(IController ControllerToSet, HttpCookieCollection CookiesToSet)
            : this(ControllerToSet, string.Empty, null, null, null, null, CookiesToSet, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="SessionItemsToSet">Session variables to use in the controller</param>
        public MockControllerContext(IController ControllerToSet, SessionStateItemCollection SessionItemsToSet)
            : this(ControllerToSet, string.Empty, null, null, null, null, null, SessionItemsToSet)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="FormParamsToSet">Form parameters to use in the controller</param>
        public MockControllerContext(IController ControllerToSet, NameValueCollection FormParamsToSet)
            : this(ControllerToSet, string.Empty, null, null, FormParamsToSet, null, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="FormParamsToSet">Form parameters to use in the controller</param>
        /// <param name="QueryStringParamsToSet">Query string items to be used in the controller</param>
        public MockControllerContext(IController ControllerToSet, NameValueCollection FormParamsToSet, NameValueCollection QueryStringParamsToSet)
            : this(ControllerToSet, string.Empty, null, null, FormParamsToSet, QueryStringParamsToSet, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="UserNameToSet">User Name To Use</param>
        public MockControllerContext(IController ControllerToSet, string UserNameToSet)
            : this(ControllerToSet, string.Empty, UserNameToSet, null, null, null, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ControllerToSet">Controller To Mock Up</param>
        /// <param name="UserNameToSet">User Name To Use</param>
        /// <param name="RolesToSet">Roles to use in the controller</param>
        public MockControllerContext(IController ControllerToSet, string UserNameToSet, IEnumerable<string> RolesToSet)
            : this(ControllerToSet, string.Empty, UserNameToSet, RolesToSet, null, null, null, null)
        {
        }

        #endregion

        #region Main Constructor Helper

        /// <summary>
        /// Make constructor helper
        /// </summary>
        /// <param name="ControllerToSet">controller to add</param>
        /// <param name="RelativeUrlToSet"></param>
        /// <param name="UserNameToSet"></param>
        /// <param name="RolesToSet"></param>
        /// <param name="FormParamsToSet"></param>
        /// <param name="QueryStringParamsToSet"></param>
        /// <param name="CookiesToSet"></param>
        /// <param name="SessionItemsToSet"></param>
        public MockControllerContext(
                IController ControllerToSet,
                string RelativeUrlToSet,
                string UserNameToSet,
                IEnumerable<string> RolesToSet,
                NameValueCollection FormParamsToSet,
                NameValueCollection QueryStringParamsToSet,
                HttpCookieCollection CookiesToSet,
                SessionStateItemCollection SessionItemsToSet)
            : base(new MockHttpContext(RelativeUrlToSet, new MockPrincipal(new MockIdentity(UserNameToSet), RolesToSet), FormParamsToSet, QueryStringParamsToSet, CookiesToSet, SessionItemsToSet), new RouteData(), (ControllerBase)ControllerToSet)
        {
        }

        #endregion

    }

}
