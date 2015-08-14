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
    /// Allows you to combine 2 different linq expression parameters (same data type passed into func) and merge it into 1 set of parameters
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    [LinqToObjectsCompatible]
    [EntityFrameworkCompatible]
    public class ExpressionParameterRemapper : ExpressionVisitor
    {

        #region Documentation

        //example. i have 2
        //Expression<Func<T, int>> IndividualPropertySelector, 
        //Expression<Func<T, int>> GroupPropertySelector

        //they both are the same T...different instances, but same data type. So we remap T to point to the same instance of T. Then we can combine the 2 expressions

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FirstExpressionParameterToSet">First Expression Parameters</param>
        /// <param name="SecondExpressionParametersToSet">Second Expression Parameters</param>
        public ExpressionParameterRemapper(IReadOnlyList<ParameterExpression> FirstExpressionParameterToSet, IReadOnlyList<ParameterExpression> SecondExpressionParametersToSet)
        {
            //make sure we have parameters for the first expression
            if (FirstExpressionParameterToSet == null)
            {
                throw new ArgumentNullException("FirstExpressionParameter");
            }

            //make sure we have parameters for the second expression
            if (SecondExpressionParametersToSet == null)
            {
                throw new ArgumentNullException("SecondExpressionParameters");
            }

            //make sure we have the same amount of parmaters
            if (FirstExpressionParameterToSet.Count != SecondExpressionParametersToSet.Count)
            {
                throw new InvalidOperationException("Parameter lengths must match");
            }

            //we are all good...let's set the variables
            FirstExpressionParameter = FirstExpressionParameterToSet;
            SecondExpressionParameters = SecondExpressionParametersToSet;
        }

        #endregion

        #region Properties And Variables

        /// <summary>
        /// Holds the parameters for the first expression
        /// </summary>
        private IReadOnlyList<ParameterExpression> FirstExpressionParameter { get; }

        /// <summary>
        /// Holds the parameters for the second expression
        /// </summary>
        private IReadOnlyList<ParameterExpression> SecondExpressionParameters { get; }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Override To Find The Correct Parameter. For The Second Expression We Set The Parameters To The First Expression So That Both Expressions Use The Same Parameters (Doesn't Matter Of Data Type, We Need The Same Object Instance For Expression Tree's)
        /// </summary>
        /// <param name="Node">Node To Check</param>
        /// <returns>Expression</returns>
        protected override Expression VisitParameter(ParameterExpression Node)
        {
            //we are visiting the 2nd expression....so we need to try to find the parameter for the first expression
            //basically we just need to find where this node (2nd expression parameter) is located (index wise) in the first expression

            //let's loop through the second expression's parameters...we will try to match it to this node
            for (int i = 0; i < SecondExpressionParameters.Count; i++)
            {
                //does this node match the node parameter passed in?
                if (SecondExpressionParameters[i] == Node)
                {
                    //we have a match, set the node to the first expression value
                    Node = FirstExpressionParameter[i];

                    //let's exit the for loop because we already have a match
                    break;
                }
            }

            //let's go visit the rest of the parameters
            return base.VisitParameter(Node);
        }

        #endregion

    }

}
