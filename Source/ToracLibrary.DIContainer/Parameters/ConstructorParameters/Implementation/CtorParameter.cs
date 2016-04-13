using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Parameters.ConstructorParameters
{

    /// <summary>
    /// Allow the user to pass in any type as a parameter
    /// </summary>
    /// <typeparam name="T">Type of the parameter to pass in</typeparam>
    /// <remarks>Class is immutable</remarks>
    public class CtorParameter<T> : IConstructorParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public CtorParameter(T ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the parameter value that we will use to pass into the constructor
        /// </summary>
        private T ParameterValue { get; }

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
            return ParameterValue;
        }

        #endregion

    }

}
