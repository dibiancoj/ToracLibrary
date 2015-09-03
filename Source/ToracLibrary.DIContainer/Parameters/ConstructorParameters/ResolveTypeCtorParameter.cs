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
    /// <typeparam name="T">T is the type we want to resolve</typeparam>
    /// <remarks>Class is immutable</remarks>
    public class ResolveTypeCtorParameter<T> : IConstructorParameter
    {

        #region Interface Methods

        /// <summary>
        /// Gets the parameter value for the given constructor parameter implementation
        /// </summary>
        /// <param name="Container">The container which we are currently using to resolve items</param>
        /// <returns>The parameter value</returns>
        public object GetParameterValue(ToracDIContainer Container)
        {
            //just return whatever we have saved
            return Container.Resolve<T>();
        }

        #endregion

    }

}
