using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExpressionTrees.API;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.Framework;
using ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for expression tree builder tests
    /// </summary>
    public class ExpressionBuilderTest
    {

        #region Expression Builder

        /// <summary>
        /// build a parameter expression for linq to objects
        /// </summary>
        [Fact]
        public void ExpressionTreesBuildStatementLinqToObjectsTest1()
        {
            //which id's to fetch
            const int IdToFetch = 1;

            //let's go build the parameter
            var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<DummyObject>(x => x.Id);

            //let's go build the expression
            var ExpressionThatWasBuilt = ExpressionBuilder.BuildStatement<DummyObject, int>(Parameter, ExpressionBuilder.DynamicUtilitiesEquations.Equal, IdToFetch);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(10).AsQueryable().Where(ExpressionThatWasBuilt).ToArray();

            //we should have 1 records
            Assert.Equal(1, ResultOfQuery.Length);

            //check the id's to make sure we have the id's we want
            Assert.True(ResultOfQuery.Any(x => x.Id == IdToFetch));
        }

        /// <summary>
        /// build a parameter expression for linq to objects
        /// </summary>
        [Fact]
        public void ExpressionTreesBuildStatementLinqToObjectsUnTypedTest1()
        {
            //which id's to fetch
            const int IdToFetch = 1;

            //let's go build the parameter
            var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<DummyObject>(x => x.Id);

            //let's go build the expression
            var ExpressionThatWasBuilt = ExpressionBuilder.BuildStatement<DummyObject>(Parameter, ExpressionBuilder.DynamicUtilitiesEquations.Equal, IdToFetch);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(10).AsQueryable().Where(ExpressionThatWasBuilt).ToArray();

            //we should have 1 records
            Assert.Equal(1, ResultOfQuery.Length);

            //check the id's to make sure we have the id's we want
            Assert.True(ResultOfQuery.Any(x => x.Id == IdToFetch));
        }

        /// <summary>
        ///  build a parameter expression for ef
        /// </summary>
        [Fact]
        public void ExpressionTreesBuildStatementEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                const int IdToFetch = 1;

                //let's go build the parameter
                var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<Ref_Test>(x => x.Id);

                //let's go build the expression
                var ExpressionThatWasBuilt = ExpressionBuilder.BuildStatement<Ref_Test, int>(Parameter, ExpressionBuilder.DynamicUtilitiesEquations.Equal, IdToFetch);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(ExpressionThatWasBuilt).ToArray();

                //we should have 1 records
                Assert.Equal(1, ResultOfQuery.Length);

                //check the id's to make sure we have the id's we want
                Assert.True(ResultOfQuery.Any(x => x.Id == IdToFetch));
            }
        }

        /// <summary>
        ///  build a parameter expression for ef
        /// </summary>
        [Fact]
        public void ExpressionTreesBuildStatementEntityFrameworkUnTypedTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                const int IdToFetch = 1;

                //let's go build the parameter
                var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<Ref_Test>(x => x.Id);

                //let's go build the expression
                var ExpressionThatWasBuilt = ExpressionBuilder.BuildStatement<Ref_Test>(Parameter, ExpressionBuilder.DynamicUtilitiesEquations.Equal, IdToFetch);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(ExpressionThatWasBuilt).ToArray();

                //we should have 1 records
                Assert.Equal(1, ResultOfQuery.Length);

                //check the id's to make sure we have the id's we want
                Assert.True(ResultOfQuery.Any(x => x.Id == IdToFetch));
            }
        }

        #endregion

        #region IEnumerable Contains

        /// <summary>
        /// build a dynamic ienumerable.contains for linq to objects
        /// </summary>
        [Fact]
        public void IEnumerableContainsForLinqToObjectsTest1()
        {
            //which id's to fetch
            int[] IdsToFetch = { 1, 2 };

            //let's go build the parameter
            var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<DummyObject>(x => x.Id);

            //let's go build the expression
            var ExpressionThatWasBuilt = ExpressionBuilder.BuildIEnumerableContains<DummyObject, int>(IdsToFetch, Parameter);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(10).AsQueryable().Where(ExpressionThatWasBuilt).ToArray();

            //we should have the 2 records
            Assert.Equal(IdsToFetch.Length, ResultOfQuery.Length);

            //loop through the id's we want to fetch and make sure they exist
            foreach (int IdToCheck in IdsToFetch)
            {
                //check the id's to make sure we have the id's we want
                Assert.True(ResultOfQuery.Any(x => x.Id == IdToCheck));
            }
        }

        /// <summary>
        /// build a dynamic ienumerable.contains for ef
        /// </summary>
        [Fact]
        public void IEnumerableContainsForEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                int[] IdsToFetch = { 1, 2 };

                //let's go build the parameter
                var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<Ref_Test>(x => x.Id);

                //let's go build the expression
                var ExpressionThatWasBuilt = ExpressionBuilder.BuildIEnumerableContains<Ref_Test, int>(IdsToFetch, Parameter);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(ExpressionThatWasBuilt).ToArray();

                //we should have the 2 records
                Assert.Equal(IdsToFetch.Length, ResultOfQuery.Length);

                //loop through the id's we want to fetch and make sure they exist
                foreach (int IdToCheck in IdsToFetch)
                {
                    //check the id's to make sure we have the id's we want
                    Assert.True(ResultOfQuery.Any(x => x.Id == IdToCheck));
                }
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// build a dynamic select for linq to objects
        /// </summary>
        [Fact]
        public void SelectForLinqToObjectsTest1()
        {
            //which id to fetch
            const int IdToFetch = 1;

            //let's go build the parameter
            var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<DummyObject>(x => x.Id);

            //let's go build the expression
            var ExpressionThatWasBuilt = ExpressionBuilder.Select<DummyObject, int>(Parameter);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(10).AsQueryable().Where(x => x.Id == IdToFetch).Select(ExpressionThatWasBuilt).ToArray();

            //we should have the 1 records
            Assert.Equal(1, ResultOfQuery.Length);

            //make sure the result is the id and it matches the id to fetch
            Assert.True(ResultOfQuery.Any(x => x == IdToFetch));
        }

        /// <summary>
        /// build a dynamic select for for ef
        /// </summary>
        [Fact]
        public void SelectForEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id to fetch
                const int IdToFetch = 1;

                //let's go build the parameter
                var Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector<Ref_Test>(x => x.Id);

                //let's go build the expression
                var ExpressionThatWasBuilt = ExpressionBuilder.Select<Ref_Test, int>(Parameter);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(x => x.Id == IdToFetch).Select(ExpressionThatWasBuilt).ToArray();

                //we should have the 1 records
                Assert.Equal(1, ResultOfQuery.Length);

                //make sure the result is the id and it matches the id to fetch
                Assert.True(ResultOfQuery.Any(x => x == IdToFetch));
            }
        }

        #endregion

        #region String Contains

        /// <summary>
        /// build a string contains expression for linq to objects
        /// </summary>
        [Fact]
        public void StringContainstForLinqToObjectsTest1()
        {
            //let's build a dummy list
            var DataSource = DummyObject.CreateDummyListLazy(5).ToArray();

            //grab the id we are going to modify
            int IdWeAreSetting = DataSource[2].Id;

            //set the 2nd element description to "jason123"
            DataSource[2].Description = "Jason123";

            //let's go build the expression to run a string contians
            var ExpressionThatWasBuilt = ExpressionBuilder.StringContains<DummyObject>(nameof(DummyObject.Description), "Jason", true, true);

            //let's run the linq to objects query
            var ResultOfQuery = DataSource.AsQueryable().Where(ExpressionThatWasBuilt).ToArray();

            //we should have the 1 records
            Assert.Equal(1, ResultOfQuery.Length);

            //make sure the result is the id and it matches the id to fetch
            Assert.True(ResultOfQuery.Any(x => x.Id == IdWeAreSetting));

            //---------------------
            //now let run a mix cased (should return 0 records)
            Assert.Equal(0, DataSource.AsQueryable().Where(ExpressionBuilder.StringContains<DummyObject>(nameof(DummyObject.Description), "jason", true, true)).Count());

            //---------------------
            //let's go run a mixed case where i don't want to check for case
            Assert.Equal(1, DataSource.AsQueryable().Where(ExpressionBuilder.StringContains<DummyObject>(nameof(DummyObject.Description), "jason", false, true)).Count());
        }

        /// <summary>
        ///  build a string contains expression for ef
        /// </summary>
        [Fact]
        public void StringContainstForEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //grab the dataset
                var DataSet = DP.Fetch<Ref_Test>(true).ToArray();

                //grab the id we are going to modify
                int IdWeAreSetting = DataSet[2].Id;

                //set the 2nd element
                DataSet[2].Description = "Jason123";

                //save the database changes
                DP.SaveChanges();

                //let's go build the expression to run a string contians
                var ExpressionThatWasBuilt = ExpressionBuilder.StringContains<Ref_Test>(nameof(Ref_Test.Description), "Jason", false, false);

                //let's run the linq to objects query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(ExpressionThatWasBuilt).ToArray();

                //we should have the 1 records
                Assert.Equal(1, ResultOfQuery.Length);

                //make sure the result is the id and it matches the id to fetch
                Assert.True(ResultOfQuery.Any(x => x.Id == IdWeAreSetting));

                //ef doesn't allow for case searches...so ignore the string contains overload
            }
        }

        #endregion

    }

}
