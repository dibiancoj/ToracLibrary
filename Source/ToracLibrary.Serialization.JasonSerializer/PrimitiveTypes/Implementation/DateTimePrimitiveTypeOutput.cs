using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes
{

    /* need to finish this....need to call ToString() when setting the string builder append value....*/ 

    /// <summary>
    /// Output for DateTime
    /// </summary>
    internal class DateTimePrimitiveTypeOutput : BasePrimitiveTypeOutput
    {

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

        #endregion

    }

}
