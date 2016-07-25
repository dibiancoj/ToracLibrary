using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.Framework;
using ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IOrderedQueryable Extension Methods
    /// </summary>
    public class IOrderedQueryableExtensionTest
    {

        /// <summary>
        /// Unit test for pagination in linq to objects
        /// </summary>
        [Fact]
        public void PaginateForLinqToObjectsTest1()
        {
            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(100).ToArray();

            //grab the paged data
            var PagedData = DummyCreatedList.AsQueryable().OrderBy(x => x.Id).PaginateResults(2, 10).ToArray();

            //go check the results
            Assert.Equal(10, PagedData.Length);
            Assert.Equal(10, PagedData[0].Id);
            Assert.Equal(11, PagedData[1].Id);
            Assert.Equal(12, PagedData[2].Id);
        }

        /// <summary>
        /// Unit test for pagination in ef
        /// </summary>
        [Fact]
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
                Assert.Equal(10, PagedData.Length);
                Assert.Equal(11, PagedData[0].Id);
                Assert.Equal(12, PagedData[1].Id);
                Assert.Equal(13, PagedData[2].Id);
            }
        }

    }

}