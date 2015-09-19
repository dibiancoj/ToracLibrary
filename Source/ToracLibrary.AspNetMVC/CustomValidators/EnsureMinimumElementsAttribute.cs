using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.AspNet.AspNetMVC.CustomModelBinders.CustomValidators
{

    /// <summary>
    /// In asp.net when you have a list, you want to ensure you have x amount of items at the minimum in your list.
    /// </summary>
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MinimumNumberOfElementsAllowedToSet">The minimum number of elements. If false, validation will fail</param>
        public EnsureMinimumElementsAttribute(int MinimumNumberOfElementsAllowedToSet)
        {
            MinimumNumberOfElementsAllowed = MinimumNumberOfElementsAllowedToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The minimum number of elements. If false, validation will fail
        /// </summary>
        public int MinimumNumberOfElementsAllowed { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override method to test if this object is valid and passes validation
        /// </summary>
        /// <param name="value">value to check</param>
        /// <returns>Does it pass validation? Is the model valid</returns>
        public override bool IsValid(object value)
        {
            //is the value null and we want more then 0 elements, fail it
            if (value == null && MinimumNumberOfElementsAllowed > 0)
            {
                return false;
            }

            //try to case this to an ilist so we can grab the count
            var CastedToIListTry = value as IList;

            //were we able to cast this?
            if (CastedToIListTry != null)
            {
                //we have it in a list, just return the check
                return IsValidHelperMethod(CastedToIListTry.Count, MinimumNumberOfElementsAllowed);
            }

            //let's try to cast this to ienumerable
            var CastedToIEnumerableTry = value as IEnumerable;

            //could we cast this?
            if (CastedToIEnumerableTry == null)
            {
                //throw an exception
                throw new InvalidCastException("Not Able To Cast Property To IEnumerable In EnsureMinimumElementsAttribute. Type Is = " + value.GetType().Name);
            }

            //let's try to count these item and compare it
            return IsValidHelperMethod(CastedToIListTry.Count(), MinimumNumberOfElementsAllowed);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Common helper method to compare the same logic in all scenarios
        /// </summary>
        /// <param name="CountInCollection">Number of items in the collection found</param>
        /// <param name="MinimumNumberOfElementsAllowedToValidate">The minimum number of elements. If false, validation will fail</param>
        /// <returns>Does it pass validation? Is the model valid</returns>
        private static bool IsValidHelperMethod(int CountInCollection, int MinimumNumberOfElementsAllowedToValidate)
        {
            //return the result
            return CountInCollection >= MinimumNumberOfElementsAllowedToValidate;
        }

        #endregion

    }

}
