using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Holds the parameter we use to invoke a method dynamically. Used when we have overloads to find the correct method to run
    /// </summary>
    public class InvokeDynamicallyParameter
    {

        #region Contructor

        /// <summary>
        /// Constructor To Create An Invoke Parameter
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        /// <param name="ParameterTypeToSet">Parameter Type</param>
        /// <remarks>Class is immutable. Gets called from external developers need to validate parameter type is not null</remarks>
        public InvokeDynamicallyParameter(object ParameterValueToSet, Type ParameterTypeToSet)
        {
            //set the value
            ParameterValue = ParameterValueToSet;

            //make sure the parameter type is not null
            if (ParameterTypeToSet == null)
            {
                throw new Exception("Parameter Type Can't Be Null");
            }

            //set the type
            ParameterType = ParameterTypeToSet;
        }

        /// <summary>
        /// Internal Constructor so we can only set the parameter value within the library (ie invoke class)
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        internal InvokeDynamicallyParameter(object ParameterValueToSet)
        {
            //just set the value
            ParameterValue = ParameterValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the parameter value
        /// </summary>
        public object ParameterValue { get; }

        /// <summary>
        /// Holds the parameter type
        /// </summary>
        public Type ParameterType { get; }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Factory Method To Create An Invoke Parameter
        /// </summary>
        /// <param name="ParameterValue">Parameter Value</param>
        /// <param name="ParameterType">Parameter Type</param>
        /// <returns>The Created Invoke Parameter</returns>
        public static InvokeDynamicallyParameter Create(object ParameterValue, Type ParameterType)
        {
            //go create the object and return it
            return new InvokeDynamicallyParameter(ParameterValue, ParameterType);
        }

        #endregion

        #region Interal Static Methods

        /// <summary>
        /// Get the list of types for each of the parameters.
        /// </summary>
        /// <param name="Parameters">Parameter List To Grab The Types From</param>
        /// <returns>list of types lazy loaded. Call ToArray() to execute right away</returns>
        internal static IEnumerable<Type> ParameterTypesSelect(IEnumerable<InvokeDynamicallyParameter> Parameters)
        {
            //go grab the parameter types (need to filter out the nulls because if they are using the overload
            //that doesn't call a method that is overloaded then the type will be null
            foreach (var ParameterToSelect in Parameters)
            {
                //if the parameter type is not null then return the type
                if (ParameterToSelect.ParameterType != null)
                {
                    //return the paramter type
                    yield return ParameterToSelect.ParameterType;
                }
            }
        }

        /// <summary>
        /// Get the list of values for each of the parameters.
        /// </summary>
        /// <param name="Parameters">Parameter List To Grab The Types From</param>
        /// <returns>list of values lazy loaded. Call ToArray() to execute right away</returns>
        internal static IEnumerable<object> ParameterValuesSelect(IEnumerable<InvokeDynamicallyParameter> Parameters)
        {
            //loop through the parameters and just return the actual value
            foreach (var ParameterToSelect in Parameters)
            {
                //just return the value
                yield return ParameterToSelect.ParameterValue;
            }
        }

        #endregion

    }

}
