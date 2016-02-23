using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes
{

    /// <summary>
    /// Holds the interface which primitive types much implement
    /// </summary>
    internal abstract class BasePrimitiveTypeOutput
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BasePrimitiveTypeOutput()
        {
            //go build the string builder write method
            StringBuilderWriteMethod = typeof(StringBuilder).GetMethod("Append", new Type[] { TypeToOutput });
        }

        #endregion

        /// <summary>
        /// The type which this implementation is built for
        /// </summary>
        internal abstract Type TypeToOutput { get; }

        /// <summary>
        /// Does it need quotes around the value
        /// </summary>
        /// <returns>if it needs quotes</returns>
        internal abstract bool NeedsQuotesAroundValue { get; }

        /// <summary>
        /// Which string builder write method doed it use
        /// </summary>
        /// <returns>Method Info to use</returns>
        internal MethodInfo StringBuilderWriteMethod { get; }

    }

}
