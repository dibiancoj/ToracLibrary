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
        /// Html Tag Removal Test. First overload with no replace value
        /// </summary>
        [InlineData("<html>Test</html>", "Test")]
        [InlineData("<html><jason>Test</jason></html>", "Test")]
        [InlineData("<html id=\"5\"><jason>Test</jason></html>", "Test")]
        [InlineData("<html id=\"5\"><jason txt=\"123\">Test</jason></html>", "Test")]
        [InlineData("<html id=\"5\">htmltag<jason txt=\"123\">Test</jason></html>", "htmltagTest")]
        [Theory]
        public void HtmlTagRemovalTest1(string HtmlToTest, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, HtmlTagRemoval.StripHtmlTags(HtmlToTest));
        }

        /// <summary>
        /// Html Tag Removal Test. Second overload with the replace value
        /// </summary>
        [InlineData("<html>Test</html>", "h", "hTesth")]
        [Theory]
        public void HtmlTagRemovalTest2(string HtmlToTest, string ReplaceValue, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, HtmlTagRemoval.StripHtmlTags(HtmlToTest, ReplaceValue));
        }

        #endregion

        #region NumberParser

        /// <summary>
        /// Test the regular expression parser functionanlity. First overload with no replace value
        /// </summary>
        [InlineData("jason123", "jason")]
        [InlineData("123jason123", "jason")]
        [InlineData("123jas123on123", "jason")]
        [Theory]
        public void NumberParserTest1(string TestValueToParse, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, NumberParser.ParseStringAndLeaveOnlyNumbers(TestValueToParse));
        }

        /// <summary>
        /// Test the regular expression parser functionanlity. Second Overload with replace value
        /// </summary>
        [InlineData("123jas1on123", "zzzjaszonzzz", "z")]
        [Theory]
        public void NumberParserTest2(string TestValueToParse, string ShouldBeValue, string ReplaceWithValue)
        {
            Assert.Equal(ShouldBeValue, NumberParser.ParseStringAndLeaveOnlyNumbers(TestValueToParse, ReplaceWithValue));
        }

        #endregion

    }

}