using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Excel;
using ToracLibrary.Core.ExpressionTrees;
using ToracLibrary.Core.ExpressionTrees.API.ReMappers;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for expression tree remappers
    /// </summary>
    [TestClass]
    public class ReMappersTest
    {

        #region Build New Object

        /// <summary>
        /// build a new object using expression trees (no parameters)
        /// </summary>
        [TestCategory("Core.ExpressionTrees.API.ReMappers")]
        [TestCategory("Core.ExpressionTrees.API")]
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void ExpressionParameterRemapperTest1()
        {
            //which id's to fetch
            const int FirstExpressionIdToFetch = 1;
            const int SecondExpressionIdToFetch = 2;

            //let's create 2 expressions
            Expression<Func<DummyObject, bool>> Expression1 = x => x.Id == FirstExpressionIdToFetch;

            //create the 2nd expression
            Expression<Func<DummyObject, bool>> Expression2 = x => x.Id == SecondExpressionIdToFetch;

            //let's compare the 2 parameters (DummyObject)
            Assert.IsFalse(Expression1.Parameters == Expression2.Parameters);

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

            Assert.Fail("Need to test in EF");
        }

        #endregion

    }

}
