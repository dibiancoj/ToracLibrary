using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Parameters.ConstructorParameters
{

    /// <summary>
    /// Primitive constructor parameter implementation. Used when you want to pass in specific variables to a constructor without using a lambda
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class PrimitiveCtorParameter : IConstructorParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor for a string value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(string ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a boolean value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(bool ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable boolean value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(bool? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for an int16 value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(Int16 ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable int 16 value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(Int16? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for an int value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(int ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable int value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(int? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for an int64 value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(Int64 ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable int64 value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(Int64? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a double value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(double ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable double value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(double? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a decimal value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(decimal ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable decimal value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(decimal? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a date time value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(DateTime ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        /// <summary>
        /// Constructor for a nullable date time value
        /// </summary>
        /// <param name="ParameterValueToSet">Parameter Value</param>
        public PrimitiveCtorParameter(DateTime? ParameterValueToSet)
        {
            ParameterValue = ParameterValueToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the parameter value that we will use to pass into the constructor
        /// </summary>
        private object ParameterValue { get; }

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
