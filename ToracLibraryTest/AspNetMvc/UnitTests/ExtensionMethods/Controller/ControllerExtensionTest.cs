using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.ExtensionMethods.ControllerExtensions;
using ToracLibrary.AspNet.AspNetMVC.RazorViewToString;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC
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
                .WithFactoryName(ControllerExtensionFactoryName)
                .WithConstructorImplementation((di) => new MockIViewEngine(new Dictionary<string, IView>() { { "_TestView", new CustomView() } }));
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        private const string ControllerExtensionFactoryName = "ControllerExtensionFactoryName";

        #endregion

        #endregion

        #region Framework

        #region Test View

        private class CustomView : IView
        {
            internal const string WriteHtmlValue = "<label>Test123<label>";

            public void Render(ViewContext viewContext, TextWriter writer)
            {
                writer.Write(WriteHtmlValue);
            }
        }

        #endregion

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

                //create the Mock controller
                MockedController.ControllerContext = new MockControllerContext(MockedController,
                                                                               DIContainer.Resolve<MockPrincipal>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockIdentity>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpRequest>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpResponse>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpSessionState>(AspNetDIContainerSharedMock.AspNetMockFactoryName));

                //return the controller now
                return MockedController;
            }

            #endregion

            #region Methods

            public string ViewOrPartialToString(ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad)
            {
                return this.RenderViewToString(ViewTypeToLoad, "_TestView", "ViewToString123", string.Empty);
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Test Initalize

        [ClassInitialize()]
        public static void ClassInit(TestContext Context)
        {
            //we will need to mock a view engine
            ViewEngines.Engines.Clear();

            //now add the new mock view engine
            ViewEngines.Engines.Add(DIUnitTestContainer.DIContainer.Resolve<IViewEngine>(ControllerExtensionFactoryName));
        }

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.ExtensionMethods.Controller")]
        [TestCategory("AspNetMVC.ExtensionMethods")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void RazorViewToStringTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<ControllerExtensionTestController>(ControllerExtensionFactoryName);

            //call the method now to test
            var RenderedHtml = TestController.ViewOrPartialToString(ViewTypeToLoad.ViewTypeToRender.View);

            //make sure we have the rendered html
            Assert.AreEqual(CustomView.WriteHtmlValue, RenderedHtml);
        }

        [TestCategory("AspNetMVC.ExtensionMethods.Controller")]
        [TestCategory("AspNetMVC.ExtensionMethods")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void RazorPartialViewToStringTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<ControllerExtensionTestController>(ControllerExtensionFactoryName);

            //call the method now to test
            var RenderedHtml = TestController.ViewOrPartialToString(ViewTypeToLoad.ViewTypeToRender.PartialView);

            //make sure we have the rendered html
            Assert.AreEqual(CustomView.WriteHtmlValue, RenderedHtml);
        }

        #endregion

    }

}
