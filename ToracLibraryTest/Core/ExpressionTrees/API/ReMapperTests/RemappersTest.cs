using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.Excel;
using ToracLibrary.Core.ExpressionTrees;
using ToracLibrary.Core.ExpressionTrees.API.ReMappers;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for expression tree remappers
    /// </summary>
    [TestClass]
    public class ReMappersTest
    {

        #region Parameter Remapper

        /// <summary>
        /// build a new object using expression trees (no parameters)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API.ReMappers")]
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExpressionParameterRemapperLinqToObjectsTest1()
        {
            //which id's to fetch
            const int FirstExpressionIdToFetch = 1;
            const int SecondExpressionIdToFetch = 2;

            //let's create 2 expressions
            Expression<Func<DummyObject, bool>> Expression1 = x => x.Id == FirstExpressionIdToFetch;

            //create the 2nd expression
            Expression<Func<DummyObject, bool>> Expression2 = x => x.Id == SecondExpressionIdToFetch;

            //we are going to merge the 2nd expression into the first one...the main goal of the expression visitor is to reset the ParameterExpression so x => is the same x
            var SecondExpressionRemappedParameters = new ExpressionParameterRemapper(Expression1.Parameters, Expression2.Parameters).Visit(Expression2.Body);

            //so now we should be able to combine the expressions
            var CombinedExpression = Expression.OrElse(Expression1.Body, SecondExpressionRemappedParameters);

            //i should be able to run this now
            Func<DummyObject, bool> CombinedExpressionInFunc = Expression.Lambda<Func<DummyObject, bool>>(CombinedExpression, Expression1.Parameters).Compile();

            //let's go run the function
            var ResultsOfLinqToObject = DummyObject.CreateDummyListLazy(5).Where(CombinedExpressionInFunc).ToArray();

            //check the results now
            Assert.AreEqual(2, ResultsOfLinqToObject.Length);

            //make sure we have the first item
            Assert.IsTrue(ResultsOfLinqToObject.Any(x => x.Id == FirstExpressionIdToFetch));

            //make sure we have the 2nd item
            Assert.IsTrue(ResultsOfLinqToObject.Any(x => x.Id == SecondExpressionIdToFetch));
        }

        /// <summary>
        /// build a new object using expression trees (no parameters)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API.ReMappers")]
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExpressionParameterRemapperEntityFrameworkTest1()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //which id's to fetch
                const int FirstExpressionIdToFetch = 1;
                const int SecondExpressionIdToFetch = 2;

                //let's create 2 expressions
                Expression<Func<Ref_Test, bool>> Expression1 = x => x.Id == FirstExpressionIdToFetch;

                //create the 2nd expression
                Expression<Func<Ref_Test, bool>> Expression2 = x => x.Id == SecondExpressionIdToFetch;

                //we are going to merge the 2nd expression into the first one...the main goal of the expression visitor is to reset the ParameterExpression so x => is the same x
                var SecondExpressionRemappedParameters = new ExpressionParameterRemapper(Expression1.Parameters, Expression2.Parameters).Visit(Expression2.Body);

                //so now we should be able to combine the expressions
                var CombinedExpression = Expression.OrElse(Expression1.Body, SecondExpressionRemappedParameters);

                //i should be able to run this now
                var CombinedExpressionInFunc = Expression.Lambda<Func<Ref_Test, bool>>(CombinedExpression, Expression1.Parameters);

                //let's go run the function
                var ResultsOfLinqToObject = DP.Fetch<Ref_Test>(false).Where(CombinedExpressionInFunc).ToArray();

                //check the results now
                Assert.AreEqual(2, ResultsOfLinqToObject.Length);

                //make sure we have the first item
                Assert.IsTrue(ResultsOfLinqToObject.Any(x => x.Id == FirstExpressionIdToFetch));

                //make sure we have the 2nd item
                Assert.IsTrue(ResultsOfLinqToObject.Any(x => x.Id == SecondExpressionIdToFetch));
            }
        }

        #endregion

    }

}
