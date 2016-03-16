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
                .WithConstructorImplementation((di) => new HtmlParserWrapper("<html><span class='TestClass1'>SpanText</span></html>"));
        }

        #endregion

        #region Constants

        /// <summary>
        /// factory name for the di container
        /// </summary>
        private const string HtmlParserFactoryName = "HtmlParserFactoryName";

        #endregion

        #region Unit Test

        [TestCategory("HtmlParsing")]
        [TestMethod]
        public void ElementHasClassValueTest1()
        {
            //build up my test html
            var HDoc = DIUnitTestContainer.DIContainer.Resolve<HtmlParserWrapper>(HtmlParserFactoryName);

            //grab my span
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").FirstOrDefault();

            //make sure we have an element
            Assert.IsNotNull(SpanToTest);

            //does this have a positive class
            Assert.IsTrue(SpanToTest.ElementHasClassValue("testclass1"));

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
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").FirstOrDefault();

            //make sure we have an element
            Assert.IsNotNull(SpanToTest);

            //make sure we see test class 1
            Assert.IsTrue(SpanToTest.ElementHasClassValue("testclass1"));

            //add a class that is already there (make sure it doesn't blow up
            SpanToTest.AddClassValueToClass("testclass1");

            //make sure it's still there
            Assert.IsTrue(SpanToTest.ElementHasClassValue("testclass1"));

            //use a variable
            var ClassNameToAdd = "testclass100";

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
            var SpanToTest = HDoc.HtmlDoc.DocumentNode.Descendants("span").FirstOrDefault();

            //make sure we have an element
            Assert.IsNotNull(SpanToTest);

            //does this have a positive class
            Assert.IsTrue(SpanToTest.ElementHasClassValue("testclass1"));

            //negative result
            Assert.IsFalse(SpanToTest.ElementHasClassValue("noclassmatch"));
        }

        #endregion

    }

}
