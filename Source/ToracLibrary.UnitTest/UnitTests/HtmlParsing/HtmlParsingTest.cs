using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.HtmlParsing;
using ToracLibrary.UnitTest.Framework;
using Xunit;
using static ToracLibrary.HtmlParsing.HtmlParserWrapperExtensionMethods;

namespace ToracLibrary.UnitTestUnitsTest.HtmlParsing
{

    /// <summary>
    /// Unit test for common methods in the html parsing
    /// </summary>
    public class HtmlParsingTest
    {

        #region Constants

        /// <summary>
        /// factory name for the di container
        /// </summary>
        internal const string HtmlParserFactoryName = "HtmlParserFactoryName";

        /// <summary>
        /// Class name in span that we can test with
        /// </summary>
        internal const string ClassNameInSpan = "TestClass1";

        /// <summary>
        /// Hold the inner text for the span
        /// </summary>
        internal const string SpanInnerTextValue = "SpanText";

        #endregion

        #region Unit Test

        [Fact]
        public void ElementHasClassValueTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //does this have a positive class
            Assert.True(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //negative result
            Assert.False(SpanToTest.ElementHasClassValue("noclassmatch"));
        }

        [Fact]
        public void ElementAddClassValueTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //make sure we see test class 1
            Assert.True(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //add a class that is already there (make sure it doesn't blow up
            SpanToTest.AddClassValueToClass(ClassNameInSpan);

            //make sure it's still there
            Assert.True(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //use a variable
            const string ClassNameToAdd = "testclass100";

            //add another class
            SpanToTest.AddClassValueToClass(ClassNameToAdd);

            //make sure it was added
            Assert.True(SpanToTest.ElementHasClassValue(ClassNameToAdd));
        }

        [Fact]
        public void SetElementTextTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //make sure it has the span text that we started with
            Assert.Equal(SpanInnerTextValue, SpanToTest.InnerText);

            //value to change the inner text to
            const string NewValueOfInnerText = "NewValue";

            //go change the value
            SpanToTest.SetTextElement(NewValueOfInnerText);

            //test the value that it has changed
            Assert.Equal(NewValueOfInnerText, SpanToTest.InnerText);
        }

        #endregion

    }

}
