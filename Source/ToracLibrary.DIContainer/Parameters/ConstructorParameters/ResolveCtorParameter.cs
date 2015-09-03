using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Parameters.ConstructorParameters
{

    /// <summary>
    /// Resolve constructor parameter implementation. Used when you want to resolve a constructor parameter from the container.  
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class ResolveCtorParameter : IConstructorParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ResolveObjectExpression">Expression which will fetch the object from the container</param>
        public ResolveCtorParameter(Func<ToracDIContainer, object> ResolveObjectExpression)
        {
            //just set the expression
            ResolveExpression = ResolveObjectExpression;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the expression which will fetch the object from the container
        /// </summary>
        public Func<ToracDIContainer, object> ResolveExpression { get; }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Gets the parameter value for the given constructor parameter implementation
        /// </summary>
        /// <param name="Container">Current container to fetch the parameter from</param>
        /// <returns>The parameter value</returns>
        public object GetParameterValue(ToracDIContainer Container)
        {
            //just invoke the expression and return it
            return ResolveExpression(Container);
        }

        #endregion

    }

}
