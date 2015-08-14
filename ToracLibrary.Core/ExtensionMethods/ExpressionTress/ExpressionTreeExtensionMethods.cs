using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToracLibrary.Core.ExpressionTrees.API.ReMappers;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExtensionMethods.ExpressionTreeExtensions
{

    /// <summary>
    /// Extension Methods For Expression Trees
    /// </summary>
    public static class ExpressionTreeExtensionMethods
    {

        /// <summary>
        /// Merges the member initialization list of two lambda expressions into one.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base mapping expression. TBaseDest is  typically a super class of TExtendedDest</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the extended mapping expression.</typeparam>
        /// <param name="BaseExpression">The base mapping expression, containing a member  initialization expression.</param>
        /// <param name="MergeExpression">The extended mapping expression to be merged into the base member initialization expression.</param>
        /// <param name="MergePosition">Where do you want to merge the second expression. Before Or After The Base (First Expression)</param>
        /// <returns>Resulting expression, after the merged select expression has been applied.</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<TSource, TExtendedDest>> Merge<TSource, TBaseDest, TExtendedDest>(this Expression<Func<TSource, TBaseDest>> BaseExpression, Expression<Func<TSource, TExtendedDest>> MergeExpression, ExpressionReMapperShared.ExpressionMemberInitMergerPosition MergePosition)
        {
            //Use an expression visitor to perform the merge of the select expressions.
            var ExpressionToMerge = new ExpressionMemberInitMerger<TSource, TBaseDest, TExtendedDest>(BaseExpression, MergePosition);

            //now build a new expression and return it
            return (Expression<Func<TSource, TExtendedDest>>)ExpressionToMerge.Visit(MergeExpression);
        }

        /// <summary>
        /// Merges a sub property which is an object of a member initialization list of two lambda expressions into one.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base mapping expression. TBaseDest is typically a super class of TExtendedDest</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the extended mapping expression.</typeparam>
        /// <param name="BaseExpression">The base mapping expression, containing a member initialization expression.</param>
        /// <param name="MergeExpression">The extended mapping expression to be merged into the base member initialization expression.</param>
        /// <param name="PropertyNameOfSubClass">Holds the property name of the sub class off of the base class. So we will use reflection to grab this property name off of the base object</param>
        /// <param name="MergeSubObjectPosition">Where do you want to merge the second expression. Before Or After The Base (First Expression)</param>
        /// <returns>Resulting expression, after the merged select expression has been applied.</returns>
        /// <remarks>See full documentation example in the method</remarks>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<TSource, TBaseDest>> MergeSubObject<TSource, TBaseDest, TExtendedDest>(this Expression<Func<TSource, TBaseDest>> BaseExpression, Expression<Func<TSource, TExtendedDest>> MergeExpression, string PropertyNameOfSubClass, ExpressionReMapperShared.ExpressionMemberInitMergerPosition MergeSubObjectPosition)
        {
            //Use an expression visitor to perform the merge of the select expressions.
            var ExpressionToMerge = new ExpressionMemberInitSubPropertyObjectMerger<TSource, TBaseDest>(BaseExpression, PropertyNameOfSubClass, MergeSubObjectPosition);

            //go visit the expression (don't absorb the result, we don't need, we will grab the result from the final result property)
            ExpressionToMerge.Visit(MergeExpression);

            //use the final result property and return it
            return ExpressionToMerge.FinalResult;
        }

    }

}
