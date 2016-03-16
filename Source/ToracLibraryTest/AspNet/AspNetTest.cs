using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.AspNet.SessionState;
using ToracLibrary.AspNet.URLHelperMethods;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNet
{

    /// <summary>
    /// Unit test for common asp.net methods
    /// </summary>
    [TestClass]
    public class AspNetTest : IDependencyInject
    {

        #region Framework

        #region Constants

        /// <summary>
        /// Holds the mock request for the ExportSessionStateTest1
        /// </summary>
        private const string MockFactoryNameForSessionStateExportWithBaseHttpRequest = "MockFactoryNameForSessionStateExportWithBaseHttpRequest";

        /// <summary>
        /// Session State Id to test with
        /// </summary>
        private const string SessionIdToTest = "SessionIdToTestCarryOverWith";

        #endregion

        #region IDependency Injection Methods

        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //add a mocked request (simple mock request) [BaseHttpRequest]
            DIContainer.Register<MockHttpRequest>()
                .WithFactoryName(MockFactoryNameForSessionStateExportWithBaseHttpRequest)
                .WithConstructorImplementation((di) => new MockHttpRequest(null, null, null, new HttpCookieCollection() { new HttpCookie(ExportCurrentSessionState.SessionStateCookieName, SessionIdToTest) }, null, null));
        }

        #endregion

        #endregion

        #region Unit Tests

        #region Url Helpers

        [TestCategory("AspNet.URLHelpers")]
        [TestCategory("AspNet")]
        [TestMethod]
        public void QueryStringAppendToUrlTest1()
        {
            Assert.AreEqual("http://www.test.com:80/Index?Id=1", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.AreEqual("http://www.test.com:80/Index?Id=1&Id2=5", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.AreEqual("http://www.test.com:80/Index?Id=1", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));
        }

        #endregion

        #region Export Session State

        [TestCategory("AspNet.SessionState")]
        [TestCategory("AspNet")]
        [TestMethod]
        public void ExportSessionStateTest1()
        {
            //test domain name
            const string CookieDomainNameToUse = "TestDomainName.Jason.com";

            //grab the request and let's use that
            var BuiltCookie = ExportCurrentSessionState.ExportSessionState(DIUnitTestContainer.DIContainer.Resolve<MockHttpRequest>(MockFactoryNameForSessionStateExportWithBaseHttpRequest), CookieDomainNameToUse);

            //check the result now
            Assert.AreEqual(CookieDomainNameToUse, BuiltCookie.Domain);
            Assert.AreEqual(ExportCurrentSessionState.SessionStateCookieName, BuiltCookie.Name);
            Assert.AreEqual(SessionIdToTest, BuiltCookie.Value);
        }

        #endregion

        #endregion

    }

}
