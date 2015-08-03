using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibraryTest.UnitsTest.Framework
{

    /// <summary>
    /// Put this on any class, and the inject method will get called so it can add its configuration to the di container
    /// </summary>
    public interface IDependencyInject
    {

        /// <summary>
        /// Any class that implements IDependencyInject will get picked up by the DI container and this method will be ran so the calling class can implement whatever it needs
        /// </summary>
        /// <param name="DIContainer">DI Container to add to</param>
        void ConfigureDIContainer(UnityContainer DIContainer);

    }

}
