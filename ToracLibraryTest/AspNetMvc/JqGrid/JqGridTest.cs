using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNetMVC.JqGrid;
using ToracLibrary.AspNetMVC.JqGrid.InlineFilters;
using ToracLibrary.AspNetMVC.UnitTestMocking;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.UnitsTest.AspNetMVC
{

    /// <summary>
    /// Unit test for a the JqGrid column modals
    /// </summary>
    [TestClass]
    public class JqGridTest
    {

        #region Unit Tests

        #region Grid Model

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JqGridJsonPropertyNamesTest1()
        {
            //we are going to borrow the JsonNet result to test the properties of the jqgrid
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonActionResultTest.JsonNetActionControllerTest>(JsonActionResultTest.JsonActionResultFactoryName);

            //let's go build our data source
            var GridDataSource = DummyObject.CreateDummyListLazy(3).ToArray();

            //let's build the test JqGridData
            var GridData = JqGridData<DummyObject>.BuildJqGridData(GridDataSource, x => x.Id, 1, 10);

            //let's go execute the action result
            TestController.SerializeToJsonNet(GridData).ExecuteResult(TestController.ControllerContext);

            //let's check the result now
            Assert.AreEqual("{\"total\":1,\"page\":1,\"records\":3,\"rows\":[{\"Id\":0,\"Description\":\"Test_0\"},{\"Id\":1,\"Description\":\"Test_1\"},{\"Id\":2,\"Description\":\"Test_2\"}]}",
                ((MockHttpResponse)TestController.Response).HtmlOutput.ToString());
        }

        #endregion

        #region Grid Inline Filters

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JqGridInlineFilterQueryBuilderAndStatementTest1()
        {
            DataProviderSetupTearDown.AddRows(true);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //id to use
                int IdToQuery = 2;
                string DescriptionToQuery = "1";

                //build the base query
                var SqlQuery = DP.Fetch<Ref_Test>(false).AsQueryable();

                //filters to use
                var FilterOnId = new JqGridInlineFilter { ColumnName = nameof(Ref_Test.Id), UserEnteredValue = IdToQuery.ToString() };
                var FilterOnDescription = new JqGridInlineFilter { ColumnName = nameof(Ref_Test.Description), UserEnteredValue = DescriptionToQuery };

                //let's go grab the filters to use
                var FilterQuery = JqGridInlineFilterQueryBuilder.BuildInlineFilterQuery<Ref_Test>(new JqGridInlineFilters { Filters = new JqGridInlineFilter[] { FilterOnId, FilterOnDescription }, Operation = JqGridInlineFilters.JqGridOperationType.AND }, false);

                //now combine the filter query with the base query
                var QueryResults = SqlQuery.Where(FilterQuery).ToArray();

                //now test it
                Assert.AreEqual(1, QueryResults.Length);

                //make sure we have the id = 2
                Assert.AreEqual(IdToQuery, QueryResults.First().Id);

                //make sure we have the description
                Assert.AreEqual(DescriptionToQuery, QueryResults.First().Description);
            }
        }

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JqGridInlineFilterQueryBuilderOrStatementTest1()
        {
            DataProviderSetupTearDown.AddRows(true);

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //id to use
                int IdToQuery = 2;
                string DescriptionToQuery = "4";

                //build the base query
                var SqlQuery = DP.Fetch<Ref_Test>(false).AsQueryable();

                //filters to use
                var FilterOnId = new JqGridInlineFilter { ColumnName = nameof(Ref_Test.Id), UserEnteredValue = IdToQuery.ToString() };
                var FilterOnDescription = new JqGridInlineFilter { ColumnName = nameof(Ref_Test.Description), UserEnteredValue = DescriptionToQuery };

                //let's go grab the filters to use
                var FilterQuery = JqGridInlineFilterQueryBuilder.BuildInlineFilterQuery<Ref_Test>(new JqGridInlineFilters { Filters = new JqGridInlineFilter[] { FilterOnId, FilterOnDescription }, Operation = JqGridInlineFilters.JqGridOperationType.OR }, false);

                //now combine the filter query with the base query
                var QueryResults = SqlQuery.Where(FilterQuery).OrderBy(x => x.Id).ToArray();

                //now test it
                Assert.AreEqual(2, QueryResults.Length);

                //make sure we have the id = 2 for the first record
                Assert.AreEqual(IdToQuery, QueryResults[0].Id);

                //check the 2nd record
                Assert.AreEqual(DescriptionToQuery, QueryResults[1].Description);
            }
        }

        #endregion

        #endregion

    }

}
