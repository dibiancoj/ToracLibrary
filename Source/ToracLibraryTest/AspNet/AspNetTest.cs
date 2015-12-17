using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.URLHelperMethods;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNet
{

    /// <summary>
    /// Unit test for common asp.net methods
    /// </summary>
    [TestClass]
    public class UrlHelpersTest
    {

        #region Unit Tests

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

    }

}
