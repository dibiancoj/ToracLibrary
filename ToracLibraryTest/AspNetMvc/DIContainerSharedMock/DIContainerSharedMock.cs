using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNetMVC
{

    /// <summary>
    /// DI container registration for asp.net mvc that have shared mocked items
    /// </summary>
    [TestClass]
    public class DIContainerSharedMock : IDependencyInject
    {

        /// <summary>
        /// Configure the DI container for shared mocked items
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //http response is a shared item

            //now let's register the actual html helper we are going to mock
            DIContainer.Register<MockHttpResponse>();
        }

    }

}
