using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.AspNet.SessionState;
using ToracLibrary.AspNet.SessionState.Cache;
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

        /// <summary>
        /// Contains normal port numbers
        /// </summary>
        [Fact]
        public void QueryStringAppendToUrlTest1()
        {
            //http version
            Assert.Equal(new Uri("http://www.test.com"), URLHelpers.AppendQueryStringToUrl("http://www.test.com/"));
            Assert.Equal(new Uri("http://www.test.com/Index"), URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index"));
            Assert.Equal(new Uri("http://www.test.com/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.Equal(new Uri("http://www.test.com/Index?Id=1&Id2=5"), URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.Equal(new Uri("http://www.test.com/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("http://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));

            //https version
            Assert.Equal(new Uri("https://www.test.com"), URLHelpers.AppendQueryStringToUrl("https://www.test.com/"));
            Assert.Equal(new Uri("https://www.test.com/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("https://www.test.com/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.Equal(new Uri("https://www.test.com/Index?Id=1&Id2=5"), URLHelpers.AppendQueryStringToUrl("https://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.Equal(new Uri("https://www.test.com/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("https://www.test.com/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));
        }


        /// <summary>
        /// Contains non normal port numbers
        /// </summary>
        [Fact]
        public void QueryStringAppendToUrlTest2()
        {
            //http version
            Assert.Equal(new Uri("http://www.test.com:123"), URLHelpers.AppendQueryStringToUrl("http://www.test.com:123/"));
            Assert.Equal(new Uri("http://www.test.com:123/Index"), URLHelpers.AppendQueryStringToUrl("http://www.test.com:123/Index"));
            Assert.Equal(new Uri("http://www.test.com:123/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("http://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.Equal(new Uri("http://www.test.com:123/Index?Id=1&Id2=5"), URLHelpers.AppendQueryStringToUrl("http://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.Equal(new Uri("http://www.test.com:123/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("http://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));

            //https version
            Assert.Equal(new Uri("https://www.test.com:123"), URLHelpers.AppendQueryStringToUrl("https://www.test.com:123/"));
            Assert.Equal(new Uri("https://www.test.com:123/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("https://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1")));
            Assert.Equal(new Uri("https://www.test.com:123/Index?Id=1&Id2=5"), URLHelpers.AppendQueryStringToUrl("https://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id2", "5")));
            Assert.Equal(new Uri("https://www.test.com:123/Index?Id=1"), URLHelpers.AppendQueryStringToUrl("https://www.test.com:123/Index", new KeyValuePair<string, string>("Id", "1"), new KeyValuePair<string, string>("Id", "5")));
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

        #region Session State Cache Model

        [Fact]
        public void SessionStateCacheModelIsExpiredTest1()
        {
            //values to test with. Can't pass dates in attributes. Item1 = Expiration Date. Item 2 = Expected Results
            var ValuesToTest = new Tuple<DateTime?, bool>[]
            {
                new Tuple<DateTime?, bool>(null, false),
                new Tuple<DateTime?, bool>(DateTime.MinValue, true),
                new Tuple<DateTime?, bool>(DateTime.Now.AddDays(-1), true ),
                new Tuple<DateTime?, bool>(DateTime.Now.AddDays(1), false)
            };

            //check the values now
            foreach(var TestValue in ValuesToTest)
            {
                //create the model to test with
                Assert.Equal(TestValue.Item2, new SessionStateCacheModel<string>(TestValue.Item1, "TestSessionStateCache").CacheIsExpired());
            }
        }

        #endregion

        #endregion

    }

}
