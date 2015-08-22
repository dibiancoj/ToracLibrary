using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ToracLibrary.AspNetMVC.ExtensionMethods.ControllerExtensions;
using ToracLibrary.AspNetMVC.RazorViewToString;
using ToracLibrary.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNetMVC
{

    /// <summary>
    /// Unit test for mvc controller extension methods
    /// </summary>
    [TestClass]
    public class ControllerExtensionTest : IDependencyInject
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
            DIContainer.Register<ControllerExtensionTestController>()
                .WithFactoryName(ControllerExtensionFactoryName)
                .WithConstructorImplementation((di) => ControllerExtensionTestController.MockController(di));

            //add the mock view engine
            DIContainer.Register<IViewEngine, MockIViewEngine>()
                .WithFactoryName(ControllerExtensionFactoryName);
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        private const string ControllerExtensionFactoryName = "ControllerExtensionFactoryName";

        #endregion

        #endregion

        #region Framework

        #region Test Controller

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        internal class ControllerExtensionTestController : Controller
        {

            #region DI Controller Creation

            public static ControllerExtensionTestController MockController(ToracDIContainer DIContainer)
            {
                //create the controller
                var MockedController = new ControllerExtensionTestController();

                //we need some route data for this
                //var routeData = new RouteData();
                //routeData.Values.Add("controller", "someValue");

                //create the Mock controller
                MockedController.ControllerContext = new MockControllerContext(MockedController);// { RouteData = routeData };

                //return the controller now
                return MockedController;
            }

            #endregion

            #region Methods

            public string ViewToString()
            {
                //the view to use
                const string View = "ViewToString123";

                return this.RenderViewToString(ViewTypeToLoad.ViewTypeToRender.View, "_TestView", View, string.Empty);
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void RazorViewToStringTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<ControllerExtensionTestController>(ControllerExtensionFactoryName);

            //we will need to mock a view engine
            ViewEngines.Engines.Clear();

            //now add the new mock view engine
            ViewEngines.Engines.Add(DIUnitTestContainer.DIContainer.Resolve<IViewEngine>(ControllerExtensionFactoryName));

            //call the method now to test
            var Results = TestController.ViewToString();

            Assert.Fail();
        }

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void RazorPartialViewToStringTest1()
        {
            Assert.Fail();
        }

        #endregion

    }

}
