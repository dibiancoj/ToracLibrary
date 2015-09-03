using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Mathematical.Growth
{

    /// <summary>
    /// Calculate the growth for a data set
    /// </summary>
    public static class GrowthCalculation
    {

        #region Formula Documentation

        //Excluding the handles for 0's and stuff like that. The basic formula is the following
        //(Current Number - Previous Number) / Previous Number

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the growth between 2 numbers.
        /// </summary>
        /// <param name="PreviousNumber">Previous Time Period Number</param>
        /// <param name="CurrentNumber">Current Time Period Number</param>
        /// <returns>Growth Percentage Between The 2 Numbers</returns>
        public static decimal CalculateGrowth(decimal PreviousNumber, decimal CurrentNumber)
        {
            //if the previous number is 0 and the current is 0 then return 0
            if (PreviousNumber == 0 && CurrentNumber == 0)
            {
                return 0;
            }

            //if the previous number is > 0 and the current number is 0 then return -100% 
            if (PreviousNumber > 0 && CurrentNumber == 0)
            {
                return -1;
            }

            //is the previous number 0 and the current number is greater than 0...then return 100%
            if (PreviousNumber == 0 && CurrentNumber > 0)
            {
                return 1;
            }

            //if the previous is less than 0 and the current is 0 then return 0
            //if (PreviousNumber < 0 && CurrentNumber == 0)
            //    return 0;

            //if we get here we might need to use the absolute value for the previous number. I don't want to overwrite the parameter value so i will declare a variable and set its value to the parameter passed in
            decimal PreviousDenominatorNumber = PreviousNumber;

            //if the previous is less than 0 and the current is greater than 0 then return the formula
            if (PreviousNumber < 0 && CurrentNumber >= 0)
            {
                //if we get here we want to use the formula with the previous number absolute value...
                PreviousDenominatorNumber = Math.Abs(PreviousNumber);
            }

            //if we get to here then run the formula
            return ((CurrentNumber - PreviousNumber) / PreviousDenominatorNumber);
        }

        #endregion

    }

}
