using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExpressionTrees.API.ReMappers
{

    /// <summary>
    /// Remaps 2 expressions into 1. Used mainly for dynamic select statements. So i have a base select expression. then i can set another one. then merge the 2. so i can keep adding on to the base select statement
    /// </summary>
    /// <remarks>Some properties are immutable in class</remarks>
    [LinqToObjectsCompatible]
    [EntityFrameworkCompatible]
    public class ExpressionMemberInitMerger<TSource, TBaseDest, TExtendedDest> : ExpressionVisitor
    {

        #region Documentation

        //there is an ExpressionTree extension method. Use that as the syntax is easier.
        //remapper does the following:
        //var selector = x => new Test {Id = x.Id};
        //i can now add to Test so push new properties on based on if statements
        //if (1 == 1)
        // selector.Merge(x => new Test { Txt = x.Description}.
        //the result will be new Test {Id = x.Id, Txt = x.Description}

        //var sql = context.Ref_Test.Select(selector)

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BaseExpression">The base expression to merge into the member init list of the extended expression.</param>
        /// <param name="MergePosition">Where do you want to merge the second expression. Before Or After The Base (First Expression)</param>
        public ExpressionMemberInitMerger(Expression<Func<TSource, TBaseDest>> BaseExpression, ExpressionReMapperShared.ExpressionMemberInitMergerPosition MergePosition)
        {
            //let's set the merge position
            WhichMergePosition = MergePosition;

            //grab the expression and cast it to a lambda expression
            LambdaExpression ConvertedLambdaExpression = BaseExpression;

            //convert the body to a member init expression
            BaseInit = (MemberInitExpression)ConvertedLambdaExpression.Body;

            //now grab the parameter (first) and set the property
            BaseParameter = ConvertedLambdaExpression.Parameters;
        }

        #endregion

        #region Variables And Properties

        /// <summary>
        /// Base Member Init Expression
        /// </summary>
        private MemberInitExpression BaseInit { get; }

        /// <summary>
        /// Where do you want to merge the second expression. Before Or After The Base (First Expression)
        /// </summary>
        private ExpressionReMapperShared.ExpressionMemberInitMergerPosition WhichMergePosition { get; }

        /// <summary>
        /// Parameter expression for the expression we are using as our base expression
        /// </summary>
        private IReadOnlyList<ParameterExpression> BaseParameter { get; }

        /// <summary>
        /// Parameter expression for the expression we are merging in
        /// </summary>
        private IReadOnlyList<ParameterExpression> NewParameter { get; set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Pick up the extended expressions range variable.
        /// </summary>
        /// <typeparam name="T">Type Of T To Visit</typeparam>
        /// <param name="Node">Lambda expression node</param>
        /// <returns>Unmodified expression tree</returns>
        protected override Expression VisitLambda<T>(Expression<T> Node)
        {
            //do we have a new parameter already?
            if (NewParameter == null)
            {
                //we don't have any parameters so just set it to the expression we are testing and its node
                NewParameter = Node.Parameters;
            }

            //return the lambda expression
            return base.VisitLambda<T>(Node);
        }

        /// <summary>
        /// Visit the member init expression of the extended expression. Merge the base  expression into it.
        /// </summary>
        /// <param name="Node">Member init expression node.</param>
        /// <returns>Merged member init expression.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression Node)
        {
            //rebind the visitor
            var RebindVisitor = new ExpressionParameterRemapper(NewParameter, BaseParameter);

            //case it into a member init expression
            var ReboundBaseInit = (MemberInitExpression)RebindVisitor.Visit(BaseInit);

            //holds the merged items
            IEnumerable<MemberBinding> MergedInitList;

            //based on the merge position put the bindings before or after
            if (WhichMergePosition == ExpressionReMapperShared.ExpressionMemberInitMergerPosition.Before)
            {
                //before base, so insert the node items first
                MergedInitList = Node.Bindings.Concat(ReboundBaseInit.Bindings);
            }
            else
            {
                //after base, so insert the node items first
                MergedInitList = ReboundBaseInit.Bindings.Concat(Node.Bindings);
            }

            //now create a new expression and return it
            return Expression.MemberInit(Expression.New(typeof(TExtendedDest)), MergedInitList);
        }

        #endregion

    }

}
