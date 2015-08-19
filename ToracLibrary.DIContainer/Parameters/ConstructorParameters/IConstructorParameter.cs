using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Parameters.ConstructorParameters
{

    /// <summary>
    /// Common Interface to bind all the constructor parameter types
    /// </summary>
    public interface IConstructorParameter
    {

        /// <summary>
        /// Gets the parameter value for the given constructor parameter implementation
        /// </summary>
        /// <param name="Container">The container which we are currently using to resolve items</param>
        /// <returns>The parameter value</returns>
        object GetParameterValue(ToracDIContainer Container);

    }

}
