using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes
{

    /// <summary>
    /// Output for DateTime
    /// </summary>
    internal class DateTimePrimitiveTypeOutput : BasePrimitiveTypeOutput
    {

        #region Constructor

        public DateTimePrimitiveTypeOutput()
        {
            ToStringMethod = typeof(DateTime).GetMethods().First(x => x.Name == nameof(DateTime.ToString));
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Holds the tostring method
        /// </summary>
        private MethodInfo ToStringMethod { get; }

        #endregion

        #region Interface Properties

        /// <summary>
        /// The type that we are using to build this output
        /// </summary>
        internal override Type TypeToOutput
        {
            get
            {
                return typeof(DateTime);
            }
        }

        /// <summary>
        /// Does the value in json need quotes?
        /// </summary>
        internal override bool NeedsQuotesAroundValue
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Allow the way we output the property. For Date time we need to call "ToString()" on the date
        /// </summary>
        /// <param name="PropertySelector">property selector</param>
        /// <returns>new expression</returns>
        internal override Expression OutputValue(MemberExpression PropertySelector)
        {
            //go call the ToString() method
            return Expression.Call(PropertySelector, ToStringMethod);
        }

        #endregion

    }

}
