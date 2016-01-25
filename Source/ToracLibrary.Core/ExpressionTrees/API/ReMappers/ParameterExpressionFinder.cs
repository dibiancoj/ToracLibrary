using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExpressionTrees.API.ReMappers
{

    /// <summary>
    /// Find's the parameter expression for the given expression
    /// </summary>
    public class ParameterExpressionFinder : ExpressionVisitor
    {

        #region Documentation

        //this is mainly designed to find a specific nodes parameter.
        //protected override Expression VisitMemberInit(MemberInitExpression node)
        //{
        //    go find my parameter (the x in x => x.Id)
        //    var findMyParameter = new ParameterExpressionFinder();

        //    go run the visitor
        //    findMyParameter.Visit(node);

        //    now i can retrieve my parameter expression
        //    var paramExpression = findMyParameter.ParameterExpressionsThatWeFound;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the parameter expression that we found
        /// </summary>
        public ParameterExpression ParameterExpressionsThatWeFound;

        #endregion

        #region Method Overrides

        /// <summary>
        /// Override the visitor for a parameter expression
        /// </summary>
        /// <param name="Node">Visit parameter expression</param>
        /// <returns>The same node. Use ParameterExpressionsThatWeFound after you call this method to grab the parameter expression</returns>
        protected override Expression VisitParameter(ParameterExpression Node)
        {
            //this is the ParameterExpression...grab the node and set the property so we can retrieve this later
            ParameterExpressionsThatWeFound = Node;

            //keep going down the tree
            return base.VisitParameter(Node);
        }

        #endregion

    }

}
