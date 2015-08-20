using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExpressionTrees.API;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for expression tree builder tests
    /// </summary>
    [TestClass]
    public class ExpressionBuilderTest
    {

        #region Expression Builder

        /// <summary>
        /// build a parameter expression for linq to objects
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
            Assert.AreEqual(1, ResultOfQuery.Length);

            //check the id's to make sure we have the id's we want
            Assert.IsTrue(ResultOfQuery.Any(x => x.Id == IdToFetch));
        }

        /// <summary>
        ///  build a parameter expression for ef
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
                Assert.AreEqual(1, ResultOfQuery.Length);

                //check the id's to make sure we have the id's we want
                Assert.IsTrue(ResultOfQuery.Any(x => x.Id == IdToFetch));
            }
        }

        #endregion

        #region IEnumerable Contains

        /// <summary>
        /// build a dynamic ienumerable.contains for linq to objects
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
            Assert.AreEqual(IdsToFetch.Length, ResultOfQuery.Length);

            //loop through the id's we want to fetch and make sure they exist
            foreach (int IdToCheck in IdsToFetch)
            {
                //check the id's to make sure we have the id's we want
                Assert.IsTrue(ResultOfQuery.Any(x => x.Id == IdToCheck));
            }
        }

        /// <summary>
        /// build a dynamic ienumerable.contains for ef
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
                Assert.AreEqual(IdsToFetch.Length, ResultOfQuery.Length);

                //loop through the id's we want to fetch and make sure they exist
                foreach (int IdToCheck in IdsToFetch)
                {
                    //check the id's to make sure we have the id's we want
                    Assert.IsTrue(ResultOfQuery.Any(x => x.Id == IdToCheck));
                }
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// build a dynamic select for linq to objects
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
            Assert.AreEqual(1, ResultOfQuery.Length);

            //make sure the result is the id and it matches the id to fetch
            Assert.IsTrue(ResultOfQuery.Any(x => x == IdToFetch));
        }

        /// <summary>
        /// build a dynamic select for for ef
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
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
                Assert.AreEqual(1, ResultOfQuery.Length);

                //make sure the result is the id and it matches the id to fetch
                Assert.IsTrue(ResultOfQuery.Any(x => x == IdToFetch));
            }
        }

        #endregion

    }

}
