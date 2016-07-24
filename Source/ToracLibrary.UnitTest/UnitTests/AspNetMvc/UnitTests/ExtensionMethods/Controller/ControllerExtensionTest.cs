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
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC.ExtensionMethods.ControllerExtensions
{

    /// <summary>
    /// Unit test for mvc controller extension methods
    /// </summary>
    public class ControllerExtensionTest
    {

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        internal const string ControllerExtensionFactoryName = "ControllerExtensionFactoryName";

        #endregion

        #region Framework

        #region Test View

        internal class CustomView : IView
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

        #region Unit Tests

        [Fact]
        public void RazorViewToStringTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<ControllerExtensionTestController>(ControllerExtensionFactoryName);

            //call the method now to test
            var RenderedHtml = TestController.ViewOrPartialToString(ViewTypeToLoad.ViewTypeToRender.View);

            //make sure we have the rendered html
            Assert.Equal(CustomView.WriteHtmlValue, RenderedHtml);
        }

        [Fact]
        public void RazorPartialViewToStringTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<ControllerExtensionTestController>(ControllerExtensionFactoryName);

            //call the method now to test
            var RenderedHtml = TestController.ViewOrPartialToString(ViewTypeToLoad.ViewTypeToRender.PartialView);

            //make sure we have the rendered html
            Assert.Equal(CustomView.WriteHtmlValue, RenderedHtml);
        }

        #endregion

    }

}
