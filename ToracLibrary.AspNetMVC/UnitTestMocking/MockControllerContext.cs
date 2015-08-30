using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Enabled a user to unit test an mvc controller with session and other http type functionality
    /// </summary>
    public class MockControllerContext : ControllerContext
    {

        /// <summary>
        /// Mock a controller context
        /// </summary>
        public MockControllerContext(
                IController ControllerToSet,
                MockPrincipal PrincipalToSet,
                MockIdentity IdentityToSet,
                MockHttpRequest RequestToSet,
                MockHttpResponse ResponseToSet,
                MockHttpSessionState SessionStateToSet)
            : base(new MockHttpContext(PrincipalToSet, RequestToSet, ResponseToSet, SessionStateToSet), new RouteData(), (ControllerBase)ControllerToSet)
        {
        }

    }

}
