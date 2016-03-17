using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer;
using ToracLibrary.HtmlParsing;
using ToracLibraryTest.Framework;
using static ToracLibrary.HtmlParsing.HtmlParserWrapperExtensionMethods;

namespace ToracLibraryTest.UnitsTest.HtmlParsing
{

    /// <summary>
    /// Unit test for common methods in the html parsing
    /// </summary>
    [TestClass]
    public class HtmlParsingTest : IDependencyInject
    {

        #region DI Container Framework

        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //let's register my dummy cache container
            DIContainer.Register<HtmlParserWrapper>()
                .WithFactoryName(HtmlParserFactoryName)
                .WithConstructorImplementation((di) => new HtmlParserWrapper(string.Format($"<html><span class='{ClassNameInSpan}'>{SpanInnerTextValue}</span></html>")));
        }

        #endregion

        #region Constants

        /// <summary>
        /// factory name for the di container
        /// </summary>
        private const string HtmlParserFactoryName = "HtmlParserFactoryName";

        /// <summary>
        /// Class name in span that we can test with
        /// </summary>
        private const string ClassNameInSpan = "TestClass1";

        /// <summary>
        /// Hold the inner text for the span
        /// </summary>
        private const string SpanInnerTextValue = "SpanText";

        #endregion

        #region Unit Test

        [TestCategory("HtmlParsing")]
        [TestMethod]
        public void ElementHasClassValueTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //does this have a positive class
            Assert.IsTrue(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //negative result
            Assert.IsFalse(SpanToTest.ElementHasClassValue("noclassmatch"));
        }

        [TestCategory("HtmlParsing")]
        [TestMethod]
        public void ElementAddClassValueTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //make sure we see test class 1
            Assert.IsTrue(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //add a class that is already there (make sure it doesn't blow up
            SpanToTest.AddClassValueToClass(ClassNameInSpan);

            //make sure it's still there
            Assert.IsTrue(SpanToTest.ElementHasClassValue(ClassNameInSpan));

            //use a variable
            const string ClassNameToAdd = "testclass100";

            //add another class
            SpanToTest.AddClassValueToClass(ClassNameToAdd);

            //make sure it was added
            Assert.IsTrue(SpanToTest.ElementHasClassValue(ClassNameToAdd));
        }

        [TestCategory("HtmlParsing")]
        [TestMethod]
        public void SetElementTextTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").First();

            //make sure it has the span text that we started with
            Assert.AreEqual(SpanInnerTextValue, SpanToTest.InnerText);

            //value to change the inner text to
            const string NewValueOfInnerText = "NewValue";

            //go change the value
            SpanToTest.SetTextElement(NewValueOfInnerText);

            //test the value that it has changed
            Assert.AreEqual(NewValueOfInnerText, SpanToTest.InnerText);
        }

        #endregion

    }

}
