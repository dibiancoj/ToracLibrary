using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;
using ToracLibrary.UnitTest.Framework;

namespace ToracLibrary.UnitTestUnitsTest.AspNet.AspNetMVC
{

    /// <summary>
    /// DI container registration for asp.net mvc that have shared mocked items
    /// </summary>
    public class AspNetDIContainerSharedMock
    {

        #region Constants

        /// <summary>
        /// Holds the factory name for the simple mocked items
        /// </summary>
        internal const string AspNetMockFactoryName = "AspMock";

        #endregion

    }

}
