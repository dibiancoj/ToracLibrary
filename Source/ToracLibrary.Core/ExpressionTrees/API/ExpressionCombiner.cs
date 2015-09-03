using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees.API.ReMappers;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExpressionTrees.API
{

    /// <summary>
    /// Helps combine expressions using AndAlso, OrElse, Etc.
    /// </summary>
    public static class ExpressionCombiner
    {

        #region Enum

        /// <summary>
        /// Combines the 2 expressions by this method
        /// </summary>
        public enum CombineType : int
        {

            /// <summary>
            /// Combine with an AndAlso
            /// </summary>
            AndAlso = 1,

            /// <summary>
            /// Combine with an OrElse
            /// </summary>
            OrElse = 2

        }

        #endregion

        #region Combiner

        /// <summary>
        /// Combines Expression Into 1 Expression
        /// </summary>
        /// <param name="FirstExpression">First Expression</param>
        /// <param name="SecondExpression">Second Expression</param>
        /// <param name="WhichCombineType">Combine Type</param>
        /// <typeparam name="T">Type Of Record To Query</typeparam>
        /// <returns>Expression To Run</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> FirstExpression, CombineType WhichCombineType, Expression<Func<T, bool>> SecondExpression)
        {
            //go remap the second expression
            var NewSecondExpression = new ExpressionParameterRemapper(FirstExpression.Parameters, SecondExpression.Parameters).VisitAndConvert(SecondExpression.Body, WhichCombineType.ToString());

            //combine type to build
            BinaryExpression CombineExpression;

            //based on the combine type run a different expression
            if (WhichCombineType == CombineType.AndAlso)
            {
                //use the and also
                CombineExpression = Expression.AndAlso(FirstExpression.Body, NewSecondExpression);
            }
            else
            {
                //use the or else
                CombineExpression = Expression.OrElse(FirstExpression.Body, NewSecondExpression);
            }

            //now combine them
            return Expression.Lambda<Func<T, bool>>(CombineExpression, FirstExpression.Parameters);
        }

        /// <summary>
        /// Takes an expression and returns a not in front of it.
        /// </summary>
        /// <typeparam name="T">Type of the record</typeparam>
        /// <param name="ExpressionToPutANotInFrontOf">expression to add the not too</param>
        /// <returns>Expression Of Func</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> ExpressionToPutANotInFrontOf)
        {
            //go put the not in front and return it
            return Expression.Lambda<Func<T, bool>>(Expression.Not(ExpressionToPutANotInFrontOf.Body), ExpressionToPutANotInFrontOf.Parameters);
        }

        #endregion

    }

}
