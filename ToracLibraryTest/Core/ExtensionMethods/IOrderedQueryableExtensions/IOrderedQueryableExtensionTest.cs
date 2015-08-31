using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IOrderedQueryable Extension Methods
    /// </summary>
    [TestClass]
    public class IOrderedQueryableExtensionTest
    {

        /// <summary>
        /// Unit test for pagination in linq to objects
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IOrderedQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void PaginateForLinqToObjectsTest1()
        {
            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(100).ToArray();

            //grab the paged data
            var PagedData = DummyCreatedList.AsQueryable().OrderBy(x => x.Id).PaginateResults(2, 10).ToArray();

            //go check the results
            Assert.AreEqual(10, PagedData.Length);
            Assert.AreEqual(10, PagedData[0].Id);
            Assert.AreEqual(11, PagedData[1].Id);
            Assert.AreEqual(12, PagedData[2].Id);
        }

        /// <summary>
        /// Unit test for pagination in ef
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IOrderedQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void PaginateForEFTest1()
        {
            //add 100 records now
            DataProviderSetupTearDown.AddRows(25, true);

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //grab the paged data
                var PagedData = DP.Fetch<Ref_Test>(false).OrderBy(x => x.Id).PaginateResults(2, 10).ToArray();

                //go check the results
                Assert.AreEqual(10, PagedData.Length);
                Assert.AreEqual(11, PagedData[0].Id);
                Assert.AreEqual(12, PagedData[1].Id);
                Assert.AreEqual(13, PagedData[2].Id);
            }
        }

    }

}