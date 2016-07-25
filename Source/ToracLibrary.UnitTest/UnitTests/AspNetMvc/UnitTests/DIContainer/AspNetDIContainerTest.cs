using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using ToracLibrary.DIContainer;
using Xunit;

namespace ToracLibrary.UnitTestUnitsTest.AspNet.AspNetMVC.DIContainer
{

    /// <summary>
    /// Unit test to ensure the di container works in asp.net mvc
    /// </summary>
    public class AspNetDIContainerTest
    {

        #region Framework

        private static class ToracDIBootstrapper
        {

            public static void Configure(ToracDIContainer ContainerToConfigure)
            {
                ContainerToConfigure.Register<HomeControllerDIContainerTest>();
                ContainerToConfigure.Register<StringLogger>();
            }

        }

        private class ToracDIDefaultControllerFactory : DefaultControllerFactory
        {

            #region Constructor

            public ToracDIDefaultControllerFactory(ToracDIContainer ContainerToSet)
            {
                this.Container = ContainerToSet;
            }

            #endregion

            #region Properties

            private ToracDIContainer Container { get; }

            #endregion

            #region Overrides

            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                //make sure the this controller is registered
                if (Container.AllRegistrationSelectLazy(controllerType).Any())
                {
                    return (Controller)Container.Resolve(controllerType);
                }

                return base.GetControllerInstance(requestContext, controllerType);
            }

            public IController GetControllerInstanceMock(RequestContext requestContext, Type controllerType)
            {
                //we need a method because we need to grab it from another class. That's why we have this as public calling a protected method
                return GetControllerInstance(requestContext, controllerType);
            }

            #endregion

        }

        private class HomeControllerDIContainerTest : Controller
        {

            public HomeControllerDIContainerTest(StringLogger LoggerToUse)
            {
                Logger = LoggerToUse;
            }

            internal StringLogger Logger { get; }
        }

        private class StringLogger
        {
            public string WriteToLog(string DescriptionToLog)
            {
                return DescriptionToLog;
            }
        }

        #endregion

        #region Unit Tests

        [Fact]
        public void AspMvcWorksWithDIContainerTest1()
        {
            //first thing we need to do is declare the container
            var DIContainer = new ToracDIContainer();

            //let's go configure the bootstrapper
            ToracDIBootstrapper.Configure(DIContainer);

            //let's go create the default controller factory
            var MockedDefaultControllerFactory = new ToracDIDefaultControllerFactory(DIContainer);

            //let's go grab mock the request context
            var ControllerToUse = (HomeControllerDIContainerTest)MockedDefaultControllerFactory.GetControllerInstanceMock(new RequestContext(), typeof(HomeControllerDIContainerTest));

            //description to log
            const string DescriptionToLog = "Test123";

            //now go check that value
            Assert.Equal(DescriptionToLog, ControllerToUse.Logger.WriteToLog(DescriptionToLog));
        }

        #endregion

    }

}
