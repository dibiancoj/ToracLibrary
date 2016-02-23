using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes
{

    /// <summary>
    /// Output for strings
    /// </summary>
    internal class IntPrimitiveTypeOutput : BasePrimitiveTypeOutput
    {

        #region Interface Properties

        /// <summary>
        /// The type that we are using to build this output
        /// </summary>
        internal override Type TypeToOutput
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Does the value in json need quotes?
        /// </summary>
        internal override bool NeedsQuotesAroundValue
        {
            get
            {
                return false;
            }
        }

        #endregion

    }

}
