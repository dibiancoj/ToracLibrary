using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExpressionTrees.API
{

    /// <summary>
    /// Returns multiple results from the Parameter Builder Methods
    /// </summary>
    /// <remarks>Immutable Class</remarks>
    public class ParameterBuilderResults
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MemberExpressionToSet">Member Expression From The Build Up Parameter To Be Used In The Expression That Will Be Built</param>
        /// <param name="ParameterExpressionToSet">Parameters To Use To Set The Expressions Parameter List</param>
        public ParameterBuilderResults(MemberExpression MemberExpressionToSet, IReadOnlyCollection<ParameterExpression> ParameterExpressionToSet)
        {
            PropertyMemberExpression = MemberExpressionToSet;
            ParametersForExpression = ParameterExpressionToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Member Expression From The Build Up Parameter To Be Used In The Expression That Will Be Built
        /// </summary>
        public MemberExpression PropertyMemberExpression { get; }

        /// <summary>
        /// Parameters To Use To Set The Expressions Parameter List
        /// </summary>
        public IReadOnlyCollection<ParameterExpression> ParametersForExpression { get; }

        #endregion

    }

}
