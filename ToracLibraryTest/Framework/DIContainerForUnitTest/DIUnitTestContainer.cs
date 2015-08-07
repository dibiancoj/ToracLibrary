using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ReflectionDynamic;

namespace ToracLibraryTest.Framework
{

    /// <summary>
    /// DI Unit Test Container
    /// </summary>
    /// <remarks>Needs the TestClass attribute so the AssemblyInitialize gets called</remarks>
    [TestClass]
    public static class DIUnitTestContainer
    {

        #region Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        /// <remarks>Uses AssemblyInitialize to ensure this runs before any tests. We need a setter because this isn't a static constructor</remarks>
        [AssemblyInitialize()]
        public static void ConfigureDIContainer(TestContext context)
        {
            //create the new di container
            DIContainer = new UnityContainer();

            //let's go build up the sql data provider
            ConfigureDIContainer(DIContainer);
        }

        #endregion

        #region Static Properties

        /// <summary> 
        /// Declare a di container so we can build whatever we need (need the setter because we aren't setting the variable in the static constructor - we need assembly initalize)
        /// </summary>
        public static UnityContainer DIContainer { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Builds up the DI container by grabbing everything that implements IDependencyInject
        /// </summary>
        /// <param name="ContainerToBuildUp">DI Container To Add Too</param>
        private static void ConfigureDIContainer(UnityContainer ContainerToBuildUp)
        {
            //grab each of the class types that implement IDependencyInject. Then loop through each of the implementations, and call the confiure DI method
            foreach (var ClassImplementation in ImplementingClasses.RetrieveImplementingClassesLazy<IDependencyInject>())
            {
                //create the instance of that class
                var ImplementationInstance = (IDependencyInject)Activator.CreateInstance(ClassImplementation);

                //now call the method that creates the setup configuration for the DI container
                ImplementationInstance.ConfigureDIContainer(ContainerToBuildUp);
            }
        }

        #endregion

    }

}
