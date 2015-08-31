using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ToracAttributes
{

    /// <summary>
    /// Attribute to let the developer know that this functionality is not testable
    /// </summary>
    /// <remarks>Attribute is immutable</remarks>
    public class MethodIsNotTestableAttribute : Attribute
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WhyIsItNotTestableDescriptionToSet">Why is this method not testable</param>
        public MethodIsNotTestableAttribute(string WhyIsItNotTestableDescriptionToSet)
        {
            DescriptionWhyMethodIsNotTestable = WhyIsItNotTestableDescriptionToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Why is this method not testable
        /// </summary>
        public string DescriptionWhyMethodIsNotTestable { get; }

        #endregion

    }

}
