using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC
{

    /// <summary>
    /// DI container registration for asp.net mvc that have shared mocked items
    /// </summary>
    [TestClass]
    public class AspNetDIContainerSharedMock : IDependencyInject
    {

        #region Constants

        /// <summary>
        /// Holds the factory name for the simple mocked items
        /// </summary>
        internal const string AspNetMockFactoryName = "AspMock";

        #endregion

        /// <summary>
        /// Configure the DI container for shared mocked items
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //now let's register a simple mock http response
            DIContainer.Register<MockHttpResponse>()
                .WithFactoryName(AspNetMockFactoryName);

            //add a mocked request (simple mock request)
            DIContainer.Register<MockHttpRequest>()
                .WithFactoryName(AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockHttpRequest(null, null, null, null, null, null));

            //add a mocked session state
            DIContainer.Register<MockHttpSessionState>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockHttpSessionState(null));

            //add the identity
            DIContainer.Register<MockIdentity>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetMockFactoryName)
                .WithConstructorParameters(new PrimitiveCtorParameter("TestUser"));

            //add the mocked principal
            DIContainer.Register<MockPrincipal>(ToracDIContainer.DIContainerScope.Singleton)
                .WithFactoryName(AspNetMockFactoryName)
                .WithConstructorImplementation((di) => new MockPrincipal(di.Resolve<MockIdentity>(), new string[] { "Role1", "Role2" }));
        }

    }

}
