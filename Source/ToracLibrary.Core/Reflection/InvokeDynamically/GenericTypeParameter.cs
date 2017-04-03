using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Helper class to allow multiple values to be stored in an array. Class contains information if the parameter to a method is a generic type
    /// </summary>
    public class GenericTypeParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ParameterTypeToSet">What is the parameter data type</param>
        /// <param name="IsGenericTypeToSet">Is this parameter a generic type</param>
        public GenericTypeParameter(Type ParameterTypeToSet, bool IsGenericTypeToSet)
        {
            ParameterType = ParameterTypeToSet;
            IsGenericType = IsGenericTypeToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is this parameter a generic type
        /// </summary>
        public bool IsGenericType { get; }

        /// <summary>
        /// What is the parameter data type
        /// </summary>
        public Type ParameterType { get; }

        #endregion

    }

}
