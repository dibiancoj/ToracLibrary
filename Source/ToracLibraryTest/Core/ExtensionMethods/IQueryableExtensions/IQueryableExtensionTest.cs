using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExtensionMethods.IQueryableExtensions;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;
using static ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions.IOrderedQueryableExtensionMethods;
using static ToracLibrary.Core.ExtensionMethods.IQueryableExtensions.IQueryableExtensionMethods;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IQueryable Extension Methods
    /// </summary>
    [TestClass]
    public class IQueryableExtensionTest
    {

        #region Column Model Sorting

        /// <summary>
        /// Unit test for sorting by the model in linq to objects
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void OrderByWithColumnModelForLinqToObjectsTest1()
        {
            //string to set in the description field
            const string DescriptionToSet = "ABC";

            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(5).ToArray();

            //set the first 2 items description to the same value
            DummyCreatedList[DummyCreatedList.Length - 1].Description = DescriptionToSet;
            DummyCreatedList[DummyCreatedList.Length - 2].Description = DescriptionToSet;

            //create the sort direction
            var SortModel = new Dictionary<string, SortDirection>
            {
                [nameof(DummyObject.Description)] = SortDirection.Ascending,
                [nameof(DummyObject.Id)] = SortDirection.Descending
            };

            //now let's double sort this by description then id
            var SortedDataSet = DummyCreatedList.AsQueryable().OrderBy(SortModel).ToArray();

            //make sure the first guy is desc so 4...the next guy is 3
            Assert.AreEqual(4, SortedDataSet[0].Id);

            //next guy would be 4
            Assert.AreEqual(3, SortedDataSet[1].Id);
        }

        /// <summary>
        /// Unit test for sorting by the model in ef
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void OrderByWithColumnModelForEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TruncateTable();

            //add 5 records now
            DataProviderSetupTearDown.AddRows(5, false);

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.WritableDataProviderName))
            {
                //get the list of records so we can modify them
                var DataSet = DP.Fetch<Ref_Test>(true).ToArray();

                //set the first 2 items description to the same value
                for (int i = 0; i < DataSet.Length; i++)
                {
                    //description to set based on the index we are up to in the loop
                    string DescriptionToUpdate;

                    //based on the index reset the description?
                    if (i == 0 || i == 1)
                    {
                        DescriptionToUpdate = "ABC";
                    }
                    else
                    {
                        DescriptionToUpdate = "ZZZ";
                    }

                    //set the description
                    DataSet[i].Description = DescriptionToUpdate;
                }

                //save the records back to the database
                DP.SaveChanges();

                //create the sort direction
                var SortModel = new Dictionary<string, SortDirection>
                {
                    [nameof(DummyObject.Description)] = SortDirection.Ascending,
                    [nameof(DummyObject.Id)] = SortDirection.Descending
                };

                //now let's double sort this by description then id
                var SortedDataSet = DP.Fetch<Ref_Test>(false).AsQueryable().OrderBy(SortModel).ToArray();

                //make sure the first guy is desc so 4...the next guy is 3
                Assert.AreEqual(2, SortedDataSet[0].Id);

                //next guy would be 4
                Assert.AreEqual(1, SortedDataSet[1].Id);
            }
        }

        #endregion

        #region String Name Sorting

        /// <summary>
        /// Unit test for sorting by the column name in linq to objects
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void OrderByWithColumnNameForLinqToObjectsTest1()
        {
            //string to set in the description field
            const string DescriptionToSet = "ABC";

            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(5).ToArray();

            //set the first 2 items description to the same value
            DummyCreatedList[DummyCreatedList.Length - 1].Description = DescriptionToSet;
            DummyCreatedList[DummyCreatedList.Length - 2].Description = DescriptionToSet;

            //now let's double sort this by description then id
            var SortedDataSet = DummyCreatedList.AsQueryable().OrderBy(nameof(DummyObject.Description)).ThenByDescending(nameof(DummyObject.Id)).ToArray();

            //make sure the first guy is desc so 4...the next guy is 3
            Assert.AreEqual(4, SortedDataSet[0].Id);

            //next guy would be 4
            Assert.AreEqual(3, SortedDataSet[1].Id);
        }

        /// <summary>
        /// Unit test for sorting by the column name in in ef
        /// </summary>
        [TestCategory("Core.ExtensionMethods.IQueryableExtensions")]
        [TestCategory("ExtensionMethods")]
        [TestCategory("Core")]
        [TestMethod]
        public void OrderByWithColumnNameForEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TruncateTable();

            //add 5 records now
            DataProviderSetupTearDown.AddRows(5, false);

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.WritableDataProviderName))
            {
                //get the list of records so we can modify them
                var DataSet = DP.Fetch<Ref_Test>(true).ToArray();

                //set the first 2 items description to the same value
                for (int i = 0; i < DataSet.Length; i++)
                {
                    //description to set based on the index we are up to in the loop
                    string DescriptionToUpdate;

                    //based on the index reset the description?
                    if (i == 0 || i == 1)
                    {
                        DescriptionToUpdate = "ABC";
                    }
                    else
                    {
                        DescriptionToUpdate = "ZZZ";
                    }

                    //set the description
                    DataSet[i].Description = DescriptionToUpdate;
                }

                //save the records back to the database
                DP.SaveChanges();

                //now let's double sort this by description then id
                var SortedDataSet = DP.Fetch<Ref_Test>(false).AsQueryable().OrderBy(nameof(Ref_Test.Description)).ThenByDescending(nameof(Ref_Test.Id)).ToArray();

                //make sure the first guy is desc so 4...the next guy is 3
                Assert.AreEqual(2, SortedDataSet[0].Id);

                //next guy would be 4
                Assert.AreEqual(1, SortedDataSet[1].Id);
            }
        }

        #endregion

    }

}