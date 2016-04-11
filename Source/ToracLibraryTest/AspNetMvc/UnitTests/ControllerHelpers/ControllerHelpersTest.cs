using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.GenericControllerHelpers;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC.GenericControllerHelpers
{

    /// <summary>
    /// Unit test for mvc generic controller helpers
    /// </summary>
    [TestClass]
    public class ControllerHelpersTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //now let's register the mock http context
            DIContainer.Register<MockHttpContext>()
                .WithFactoryName(ControllerHelperFactoryName)
                .WithConstructorImplementation((di) => new MockHttpContext(
                        di.Resolve<MockPrincipal>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                        di.Resolve<MockHttpRequest>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                        di.Resolve<MockHttpResponse>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                        di.Resolve<MockHttpSessionState>(AspNetDIContainerSharedMock.AspNetMockFactoryName)));
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        private const string ControllerHelperFactoryName = "ControllerHelperFactoryName";

        #endregion

        #endregion

        #region Framework

        #region Test Controller

        /// <summary>
        /// Test controller to create
        /// </summary>
        private class ControllerGenericTestController : Controller
        {
        }

        #endregion

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.GenericControllerHelpers.ControllerHelpers")]
        [TestCategory("AspNetMVC.GenericControllerHelpers")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void CreateControllerWithContextTest1()
        {
            // resolve the mocked httpcontext
            var HttpContext = DIUnitTestContainer.DIContainer.Resolve<MockHttpContext>(ControllerHelperFactoryName);

            //call the method to test
            var CreatedController = ControllerHelpers.CreateControllerWithContext<ControllerGenericTestController>(HttpContext.Request.RequestContext);// HttpContext.ApplicationInstance.Context);

            //make sure we have the controller
            Assert.IsNotNull(CreatedController);

            //also make sure we have a controller context so the RenderPartialView will work
            Assert.IsNotNull(CreatedController.ControllerContext);
        }

        #endregion

    }

}
