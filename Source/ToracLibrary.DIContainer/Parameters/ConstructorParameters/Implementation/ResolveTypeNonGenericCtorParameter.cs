using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Parameters.ConstructorParameters
{

    /// <summary>
    /// Resolve constructor parameter implementation. Used when you want to resolve a constructor parameter from the container when you don't have it in a generic
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class ResolveTypeNonGenericCtorParameter : IConstructorParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeToResolveToSet">Type to resolve</param>
        public ResolveTypeNonGenericCtorParameter(Type TypeToResolveToSet)
        {
            TypeToResolve = TypeToResolveToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Type to resolve
        /// </summary>
        private Type TypeToResolve { get; }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Gets the parameter value for the given constructor parameter implementation
        /// </summary>
        /// <param name="Container">The container which we are currently using to resolve items</param>
        /// <returns>The parameter value</returns>
        public object GetParameterValue(ToracDIContainer Container)
        {
            //just return whatever we have saved
            return Container.Resolve(TypeToResolve);
        }

        #endregion

    }

}
