using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.AspNet.SessionState;
using ToracLibrary.AspNet.URLHelperMethods;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNet
{

    /// <summary>
    /// Unit test for common asp.net methods
    /// </summary>
    public class AspNetTest
    {

        #region Framework

        #region Constants

        /// <summary>
        /// Holds the mock request for the ExportSessionStateTest1
        /// </summary>
        internal const string MockFactoryNameForSessionStateExportWithBaseHttpRequest = "MockFactoryNameForSessionStateExportWithBaseHttpRequest";

        /// <summary>
        /// Session State Id to test with
        /// </summary>
        internal const string SessionIdToTest = "SessionIdToTestCarryOverWith";

        #endregion

        #endregion

        #region Unit Tests

        #region Url Helpers

        [Fact]
        public void QueryStringAppendToUrlTest1()
        {
            Assert.Equal("http://www.test.com:80/Index?Id=1", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.Equal("http://www.test.com:80/Index?Id=1&Id2=5", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.Equal("http://www.test.com:80/Index?Id=1", URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));
        }

        #endregion

        #region Export Session State

        [Fact]
        public void ExportSessionStateTest1()
        {
            //test domain name
            const string CookieDomainNameToUse = "TestDomainName.Jason.com";

            //grab the request and let's use that
            var BuiltCookie = ExportCurrentSessionState.ExportSessionState(DIUnitTestContainer.DIContainer.Resolve<MockHttpRequest>(MockFactoryNameForSessionStateExportWithBaseHttpRequest), CookieDomainNameToUse);

            //check the result now
            Assert.Equal(CookieDomainNameToUse, BuiltCookie.Domain);
            Assert.Equal(ExportCurrentSessionState.SessionStateCookieName, BuiltCookie.Name);
            Assert.Equal(SessionIdToTest, BuiltCookie.Value);
        }

        #endregion

        #endregion

    }

}
