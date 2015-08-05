using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibraryTest.Framework;
using ToracLibraryTest.UnitsTest.Caching;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using static ToracLibrary.Core.ReflectionDynamic.ImplementingClasses;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit tests for reflection based functionality
    /// </summary>
    [TestClass]
    public class ReflectionTest
    {

        #region Test Derived Classes

        /// <summary>
        /// This is the derived class which is built off of the base
        /// </summary>
        internal class DeriveReflectionClass : BaseDeriveReflectionClass
        {
        }

        /// <summary>
        /// base class which we will fetch by using this type. It should return anything that uses this class as a base
        /// </summary>
        internal class BaseDeriveReflectionClass
        {
        }

        #endregion

        #region Implementing Classes

        /// <summary>
        /// Test Implementing classes. Even though the DI uses this functionality, I'm still going to give it the test it deserves
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void ImplementingInterfacesFromClass()
        {
            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(IDependencyInject)).ToArray();

            //we should currently have 2 (could change)
            Assert.AreEqual(3, ImplementationResults.Length);

            //check it's the sql server data provider
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(SqlDataProviderTest)));

            //make sure its the in memory caching now
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(InMemoryCacheTest)));

            //make sure sql cache dep is in there
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(SqlCacheDependencyTest)));
        }

        /// <summary>
        /// Test Derived Classes
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeriveClassFromBaseClass()
        {
            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(BaseDeriveReflectionClass)).ToArray();

            //we should currently have (class is above)
            Assert.AreEqual(1, ImplementationResults.Length);

            //check it's the sql server data provider
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(DeriveReflectionClass)));
        }

        #endregion

    }
}
