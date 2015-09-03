using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.HtmlHelpers;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.AspNet.AspNetMVC.HtmlHelpers
{

    /// <summary>
    /// Unit test for asp.net mvc html helpers
    /// </summary>
    [TestClass]
    public class HtmlHelperTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //to create an html helper we need to mock a view context and a view data container

            //register the view context
            DIContainer.Register<ViewContext>()
                .WithFactoryName(HtmlHelperTestDIFactoryName)
                .WithConstructorOverload();

            //register the IViewDataContainer
            DIContainer.Register<IViewDataContainer, MockIViewDataContainer>()
                .WithFactoryName(HtmlHelperTestDIFactoryName)
                .WithConstructorOverload();

            //now let's register the actual html helper we are going to mock
            DIContainer.Register<HtmlHelper<HtmlHelperTestViewModel>>()
                .WithFactoryName(HtmlHelperTestDIFactoryName);
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        private const string HtmlHelperTestDIFactoryName = "HtmlHelperMocks";

        #endregion

        #endregion

        #region Test Model

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        internal class HtmlHelperTestViewModel
        {

            #region Constructor

            public HtmlHelperTestViewModel()
            {
                Id = IdValueToUseForTest;
            }

            #endregion

            #region Constants

            internal const int IdValueToUseForTest = 5;

            #endregion

            #region Properties

            public int Id { get; }

            #endregion

        }

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.HtmlHelpers")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void CustomOutputAttributeTest1()
        {
            //test attribute value to output
            const string AttributeToOutput = "data-id=5";

            //mock TModel which is present in the view (no need to put this into the di container)
            var MockedModel = new HtmlHelperTestViewModel();

            //we have everything we need to mock up the html helper with the DI...so grab the mocked HtmlHelper from the di container
            var MockedHtmlHelper = DIUnitTestContainer.DIContainer.Resolve<HtmlHelper<HtmlHelperTestViewModel>>(HtmlHelperTestDIFactoryName);

            //let's test the html helper now
            Assert.AreEqual(AttributeToOutput, MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == HtmlHelperTestViewModel.IdValueToUseForTest, AttributeToOutput));

            //let's test if the value is not correct. So we pass in -9999 which should evaluate to false and return null
            Assert.IsNull(MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == -9999, AttributeToOutput));
        }

        #endregion

    }

}
