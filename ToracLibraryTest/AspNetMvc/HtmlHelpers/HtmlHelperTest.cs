using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNetMVC.HtmlHelpers;

namespace ToracLibraryTest.UnitsTest.AspNetMVC
{

    /// <summary>
    /// Unit test for asp.net mvc html helpers
    /// </summary>
    [TestClass]
    public class HtmlHelperTest
    {

        [TestCategory("AspNetMVC.HtmlHelpers")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void CustomOutputAttributeTest1()
        {
            //test list
            int[] ContainsTest = { 1, 2, 3, 4, 5 };

            //positive result where we output the attribute
            Assert.AreEqual("data-id=5", new HtmlHelper().CustomOutputAttribute<int>(x => ContainsTest.Any(3), "data-id=5"));
        }

    }

}
