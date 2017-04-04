using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke.Parameters
{

    /// <summary>
    /// Helper class to allow multiple values to be stored in an array. Class contains information if the parameter to a method is a generic type
    /// </summary>
    public class GenericTypeParameter : BaseGenericParameter
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
        public override bool IsGenericType { get; }

        /// <summary>
        /// What is the parameter data type
        /// </summary>
        public Type ParameterType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Do the values passed in match to ensure the method has the same parameters
        /// </summary>
        /// <param name="MethodWeAreValidating">Method we are trying to validate</param>
        /// <param name="MethodParameterToMatch">Parameter in the method that we want to match with</param>
        /// <returns>true if they match</returns>
        public override bool ParametersMatch(MethodInfo MethodWeAreValidating, ParameterInfo MethodParameterToMatch)
        {
            //is this a generic parameter?
            if (IsGenericType)
            {
                //if the parameter types don't match then this method doesn't match
                if (MethodParameterToMatch.ParameterType.GetGenericTypeDefinition() != ParameterType)
                {
                    return false;
                }
            }
            else if (MethodParameterToMatch.ParameterType != ParameterType)
            {
                //regular parameter doesn't match
                return false;
            }

            //they match
            return true;
        }

        #endregion

    }

}
