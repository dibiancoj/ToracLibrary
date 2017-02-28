using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExtensionMethods.IQueryableExtensions;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.Framework;
using Xunit;
using static ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions.IOrderedQueryableExtensionMethods;
using static ToracLibrary.Core.ExtensionMethods.IQueryableExtensions.IQueryableExtensionMethods;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to IQueryable Extension Methods
    /// </summary>
    [Collection("DatabaseUnitTests")]
    public class IQueryableExtensionTest
    {

        #region Column Model Sorting

        /// <summary>
        /// Unit test for sorting by the model in linq to objects
        /// </summary>
        [Fact]
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
            Assert.Equal(4, SortedDataSet[0].Id);

            //next guy would be 4
            Assert.Equal(3, SortedDataSet[1].Id);
        }

        /// <summary>
        /// Unit test for sorting by the model in ef
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
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
                Assert.Equal(2, SortedDataSet[0].Id);

                //next guy would be 4
                Assert.Equal(1, SortedDataSet[1].Id);
            }
        }

        #endregion

        #region String Name Sorting

        /// <summary>
        /// Unit test for sorting by the column name in linq to objects
        /// </summary>
        [Fact]
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
            Assert.Equal(4, SortedDataSet[0].Id);

            //next guy would be 4
            Assert.Equal(3, SortedDataSet[1].Id);
        }

        /// <summary>
        /// Unit test for sorting by the column name in in ef
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
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
                Assert.Equal(2, SortedDataSet[0].Id);

                //next guy would be 4
                Assert.Equal(1, SortedDataSet[1].Id);
            }
        }

        #endregion

        #region IQueryable Init Merging

        /// <summary>
        /// Test class
        /// </summary>
        private class IQueryableInitMergeTest
        {
            public int Id { get; set; }
            public string NewMappingField { get; set; }
        }

        /// <summary>
        /// Unit test for merging a select statement in iqueryable (linq to objects)
        /// </summary>
        [Fact]
        public void IQueryableMemberInitMergeForLinqToObjectsTest1()
        {
            //create a dummy list
            var DummyCreatedList = DummyObject.CreateDummyListLazy(10).AsQueryable();

            //let's run a select with the same object just setting the id. (going to test it with a where in front to make sure it works with chaining)
            var OnePropertySet = DummyCreatedList.Where(x => x.Id <= 4).Select(x => new IQueryableInitMergeTest { Id = x.Id });

            //now the only thing that should be populated is the id...
            //we want to merge the txt field into the new NewMappingField
            var MergeQueryable = OnePropertySet.AddBindingToSelectInQuery(nameof(DummyObject.Description), nameof(IQueryableInitMergeTest.NewMappingField));

            //now execute the query
            var Results = MergeQueryable.ToArray();

            //make sure we
            RunIQueryableMemberInitTest(Results, "Test_{0}");
        }

        /// <summary>
        /// Unit test for merging a select statement in iqueryable (entity framework)
        /// </summary>
        [Fact(Skip = DisableSpecificUnitTestAreas.DatabaseAvailableForUnitTestFlag)]
        public void IQueryableMemberInitMergeForEntityFrameworkTest1()
        {
            //tear down the table
            DataProviderSetupTearDown.TruncateTable();

            //add 5 records now
            DataProviderSetupTearDown.AddRows(10, false);

            //build the base query
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.WritableDataProviderName))
            {
                //build the base query
                var BaseQuery = DP.Fetch<Ref_Test>(false).AsQueryable().Where(x => x.Id <= 5).Select(x => new IQueryableInitMergeTest { Id = x.Id });

                //we want to merge the txt field into the new NewMappingField
                var MergeQueryable = BaseQuery.AddBindingToSelectInQuery(nameof(DummyObject.Description), nameof(IQueryableInitMergeTest.NewMappingField));

                //now execute the query
                var Results = MergeQueryable.ToArray();

                //make sure we
                RunIQueryableMemberInitTest(Results, "{0}");
            }
        }

        /// <summary>
        /// Runs the actually assertion for both the ef query and the linq to objects
        /// </summary>
        /// <param name="UnitTestResults">Items that were built from the actual result of the method</param>
        /// <param name="TextFormat">Format to use to test the value. Will work with {0} = i</param>
        /// <remarks>Will raise an error if it fails</remarks>
        private static void RunIQueryableMemberInitTest(IEnumerable<IQueryableInitMergeTest> UnitTestResults, string TextFormat)
        {
            //should be 5 items
            Assert.Equal(5, UnitTestResults.Count());

            //counter 
            int i = 0;

            //make sure the new field is mapped
            foreach (var ItemToTest in UnitTestResults)
            {
                //is the new mapping field correct
                Assert.Equal(ItemToTest.NewMappingField, string.Format(TextFormat, i));

                //increase the counter
                i++;
            }
        }

        #endregion

    }

}