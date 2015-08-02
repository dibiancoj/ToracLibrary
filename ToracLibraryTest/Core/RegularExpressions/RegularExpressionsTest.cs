using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ToracLibrary.Core.RegularExpressions;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for regular expressions that we have in the library
    /// </summary>
    [TestClass]
    public class RegularExpressionsTest
    {

        #region Html Tag Removal Tests

        /// <summary>
        /// Html Tag Removal Test
        /// </summary>
        [TestMethod]
        public void HtmlTagRemovalTest1()
        {
            //test some html
            Assert.AreEqual("Test", HtmlTagRemoval.StripHtmlTags("<html>Test</html>"));
            Assert.AreEqual("Test", HtmlTagRemoval.StripHtmlTags("<html><jason>Test</jason></html>"));
            Assert.AreEqual("Test", HtmlTagRemoval.StripHtmlTags("<html id=\"5\"><jason>Test</jason></html>"));
            Assert.AreEqual("Test", HtmlTagRemoval.StripHtmlTags("<html id=\"5\"><jason txt=\"123\">Test</jason></html>"));
            Assert.AreEqual("htmltagTest", HtmlTagRemoval.StripHtmlTags("<html id=\"5\">htmltag<jason txt=\"123\">Test</jason></html>"));
            Assert.AreEqual("hTesth", HtmlTagRemoval.StripHtmlTags("<html>Test</html>", "h"));
        }

        #endregion

        #region NumberParser

        /// <summary>
        /// Test the regular expression parser functionanlity
        /// </summary>
        [TestMethod]
        public void NumberParserTest1()
        {
            Assert.AreEqual("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("jason123"));
            Assert.AreEqual("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("123jason123"));
            Assert.AreEqual("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("123jas123on123"));
            Assert.AreEqual("zzzjaszonzzz", NumberParser.ParseStringAndLeaveOnlyNumbers("123jas1on123", "z"));
        }

        #endregion

    }

}