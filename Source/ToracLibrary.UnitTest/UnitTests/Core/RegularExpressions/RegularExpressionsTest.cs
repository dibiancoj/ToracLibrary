using System;
using System.Linq;
using ToracLibrary.Core.RegularExpressions;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for regular expressions that we have in the library
    /// </summary>
    public class RegularExpressionsTest
    {

        #region Html Tag Removal Tests

        /// <summary>
        /// Html Tag Removal Test
        /// </summary>
        [Fact]
        public void HtmlTagRemovalTest1()
        {
            //test some html
            Assert.Equal("Test", HtmlTagRemoval.StripHtmlTags("<html>Test</html>"));
            Assert.Equal("Test", HtmlTagRemoval.StripHtmlTags("<html><jason>Test</jason></html>"));
            Assert.Equal("Test", HtmlTagRemoval.StripHtmlTags("<html id=\"5\"><jason>Test</jason></html>"));
            Assert.Equal("Test", HtmlTagRemoval.StripHtmlTags("<html id=\"5\"><jason txt=\"123\">Test</jason></html>"));
            Assert.Equal("htmltagTest", HtmlTagRemoval.StripHtmlTags("<html id=\"5\">htmltag<jason txt=\"123\">Test</jason></html>"));
            Assert.Equal("hTesth", HtmlTagRemoval.StripHtmlTags("<html>Test</html>", "h"));
        }

        #endregion

        #region NumberParser

        /// <summary>
        /// Test the regular expression parser functionanlity
        /// </summary>
        [Fact]
        public void NumberParserTest1()
        {
            Assert.Equal("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("jason123"));
            Assert.Equal("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("123jason123"));
            Assert.Equal("jason", NumberParser.ParseStringAndLeaveOnlyNumbers("123jas123on123"));
            Assert.Equal("zzzjaszonzzz", NumberParser.ParseStringAndLeaveOnlyNumbers("123jas1on123", "z"));
        }

        #endregion

    }

}