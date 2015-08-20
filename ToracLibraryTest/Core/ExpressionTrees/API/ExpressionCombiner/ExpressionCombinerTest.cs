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
    /// Unit test for expression tree combiner tests
    /// </summary>
    [TestClass]
    public class ExpressionCombinerTest
    {

        #region Combine Expression

        /// <summary>
        /// combine 2 expressions and run a where (linq to objects)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void CombineExpressionTreesLinqToObjectsTest1()
        {
            //which id's to fetch
            const int FirstExpressionIdToFetch = 1;
            const int SecondExpressionIdToFetch = 2;

            //let's create 2 expressions
            Expression<Func<DummyObject, bool>> Expression1 = x => x.Id == FirstExpressionIdToFetch;

            //create the 2nd expression
            Expression<Func<DummyObject, bool>> Expression2 = x => x.Id == SecondExpressionIdToFetch;

            //let's combine them now
            var OrStatement = ExpressionCombiner.CombineExpressions(Expression1, ExpressionCombiner.CombineType.OrElse, Expression2);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(10).AsQueryable().Where(OrStatement).ToArray();

            //we should have 2 records
            Assert.AreEqual(2, ResultOfQuery.Length);

            //check the id's to make sure we have the id's we want
            Assert.IsTrue(ResultOfQuery.Any(x => x.Id == FirstExpressionIdToFetch));
            Assert.IsTrue(ResultOfQuery.Any(x => x.Id == SecondExpressionIdToFetch));
        }

        /// <summary>
        /// combine 2 expressions and run a where (ef)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void CombineExpressionTreesEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                const int FirstExpressionIdToFetch = 1;
                const int SecondExpressionIdToFetch = 2;

                //let's create 2 expressions
                Expression<Func<Ref_Test, bool>> Expression1 = x => x.Id == FirstExpressionIdToFetch;

                //create the 2nd expression
                Expression<Func<Ref_Test, bool>> Expression2 = x => x.Id == SecondExpressionIdToFetch;

                //let's combine them now
                var OrStatement = ExpressionCombiner.CombineExpressions(Expression1, ExpressionCombiner.CombineType.OrElse, Expression2);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(OrStatement).ToArray();

                //we should have 2 records
                Assert.AreEqual(2, ResultOfQuery.Length);

                //check the id's to make sure we have the id's we want
                Assert.IsTrue(ResultOfQuery.Any(x => x.Id == FirstExpressionIdToFetch));
                Assert.IsTrue(ResultOfQuery.Any(x => x.Id == SecondExpressionIdToFetch));
            }
        }

        #endregion

        #region Not Expression

        /// <summary>
        /// use a not on an expression (linq to objects)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void NotLinqToObjectsTest1()
        {
            //which id's to fetch
            const int IdToFetch = 1;

            //how many records to build
            const int HowManyRecordsToBuild = 10;

            //let's combine them now
            var NotExpression = ExpressionCombiner.Not<DummyObject>(x => x.Id == IdToFetch);

            //let's run the linq to objects query
            var ResultOfQuery = DummyObject.CreateDummyListLazy(HowManyRecordsToBuild).AsQueryable().Where(NotExpression).ToArray();

            //we should have 2 records
            Assert.AreEqual(HowManyRecordsToBuild - 1, ResultOfQuery.Length);

            //make sure we don't have the id we wanted to exclude
            Assert.IsFalse(ResultOfQuery.Any(x => x.Id == IdToFetch));
        }

        /// <summary>
        /// use a not on an expression (ef)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void NotEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                const int IdToFetch = 1;

                //how many records to build
                const int HowManyRecordsToBuild = 10;

                //let's combine them now
                var NotExpression = ExpressionCombiner.Not<Ref_Test>(x => x.Id == IdToFetch);

                //let's run the ef query
                var ResultOfQuery = DP.Fetch<Ref_Test>(false).Where(NotExpression).ToArray();

                //we should have 2 records
                Assert.AreEqual(HowManyRecordsToBuild - 1, ResultOfQuery.Length);

                //make sure we don't have the id we wanted to exclude
                Assert.IsFalse(ResultOfQuery.Any(x => x.Id == IdToFetch));
            }
        }

        #endregion

    }

}
