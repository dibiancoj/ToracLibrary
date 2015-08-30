using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNetMVC.CustomValueProviderFactory;
using ToracLibrary.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNetMVC.CustomValueProviderFactory
{

    /// <summary>
    /// Unit test for a the json net value provider factory
    /// </summary>
    [TestClass]
    public class JsonNetCustomValueProviderFactoryTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //let's mock up the controller for each of these tests

            //now let's register the actual html helper we are going to mock
            DIContainer.Register<JsonNetCustomValueProviderFactoryControllerTest>()
                .WithFactoryName(JsonNetCustomValueProviderFactoryName)
                .WithConstructorImplementation((di) => JsonNetCustomValueProviderFactoryControllerTest.MockController(di));
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        private const string JsonNetCustomValueProviderFactoryName = "JsonNetCustomValueProviderFactoryName";

        #endregion

        #endregion

        #region Framework

        #region Test Controller

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        private class JsonNetCustomValueProviderFactoryControllerTest : Controller
        {

            #region DI Controller Creation

            public static JsonNetCustomValueProviderFactoryControllerTest MockController(ToracDIContainer DIContainer)
            {
                //create the controller
                var MockedController = new JsonNetCustomValueProviderFactoryControllerTest();

                //create the Mock controller
                MockedController.ControllerContext = new MockControllerContext(MockedController);

                //set the request
                MockedController.Request = new MockHttpRequest(string.Empty, null, null, null);

                //set the content type 
                MockedController.Request.ContentType = "application/json";

                //return the controller now
                return MockedController;
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.CustomValueProviderFactory.JsonNet")]
        [TestCategory("AspNetMVC.CustomValueProviderFactory")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JsonNetCustomValueProviderFactoryTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonNetCustomValueProviderFactoryControllerTest>(JsonNetCustomValueProviderFactoryName);

            //grab the new factory 
            var TestProvider = new JsonNetValueProviderFactory();

            //let's go execute the action result
            var Result = TestProvider.GetValueProvider(TestController.ControllerContext);

            //let's check the result now
            Assert.AreEqual("{\"JsonId\":5,\"JsonDescription\":\"Description5\",\"CreatedDate\":\"2015-09-01T00:00:00\"}", ((MockHttpResponse)TestController.Response).HtmlOutput.ToString());
        }

        #endregion

    }

}
