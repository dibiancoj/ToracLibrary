using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExpressionTrees.API.ReMappers
{

    /// <summary>
    /// Holds standard expression remapper objects. Mainly untyped items
    /// </summary>
    public class ExpressionReMapperShared
    {

        #region Enum

        /// <summary>
        /// Where do you want to merge the bindings. 
        /// </summary>
        public enum ExpressionMemberInitMergerPosition
        {

            /// <summary>
            /// Before
            /// </summary>
            Before = 0,

            /// <summary>
            /// After
            /// </summary>
            After = 1
        }

        #endregion

    }

}
