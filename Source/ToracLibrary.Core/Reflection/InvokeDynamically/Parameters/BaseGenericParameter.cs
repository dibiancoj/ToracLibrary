using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke.Parameters
{

    /// <summary>
    /// Generic parameter so we can pass everything in 1 list
    /// </summary>
    public abstract class BaseGenericParameter
    {

        /// <summary>
        /// Is there a generic parameter
        /// </summary>
        public abstract bool IsGenericType { get; }

        /// <summary>
        /// Do the values passed in match to ensure the method has the same parameters
        /// </summary>
        /// <param name="MethodWeAreValidating">Method we are validating</param>
        /// <param name="MethodParameterToMatch">Parameter in the method that we want to match with</param>
        /// <returns>true if they match</returns>
        public abstract bool ParametersMatch(MethodInfo MethodWeAreValidating, ParameterInfo MethodParameterToMatch);

    }

}
