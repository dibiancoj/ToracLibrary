using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AccountingPeriods.Exceptions
{

    /// <summary>
    /// Holds the exception when the accounting period does not match the expected format
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class AccountingPeriodYearOutOfRangeException : Exception
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="YearThatFailedValidationToSet">Year Value That Failed Validation</param>
        /// <remarks>Class is immutable</remarks>
        public AccountingPeriodYearOutOfRangeException(int YearThatFailedValidationToSet)
        {
            //set the property
            YearThatFailedValidation = YearThatFailedValidationToSet;
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Year value that failed validation
        /// </summary>
        public int YearThatFailedValidation { get; }

        #endregion

        #region Overload Methods

        /// <summary>
        /// Override the ToString() method for this custom exception
        /// </summary>
        /// <returns>string output to display</returns>
        public override string ToString()
        {
            return "Accounting Period Year Is Not In The Correct Format. Year Should Be YYYY. Year That Passed Validation Is = " + YearThatFailedValidation;
        }

        #endregion

    }

}
