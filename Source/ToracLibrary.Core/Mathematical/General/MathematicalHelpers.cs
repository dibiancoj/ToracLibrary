using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Mathematical
{

    /// <summary>
    /// General math helpers
    /// </summary>
    public static class MathematicalHelpers
    {

        /// <summary>
        /// Converts each item in the array to part of a number. ie: [0] = 22, [1] = 5, [2] = 6 ==> Result 2256
        /// </summary>
        /// <param name="NumbersToConvert">numbers to convert</param>
        /// <returns>The number of each segment to a number</returns>
        public static int ArrayOfNumbersToNumber(IEnumerable<int> NumbersToConvert)
        {
            //holds the running tally
            var RunningTally = 0;

            //what we multiple with
            var MultiplyValue = 1;

            //loop through the numbers backwards
            foreach (var Number in NumbersToConvert.Reverse())
            {
                //multiply and add it
                RunningTally += Number * MultiplyValue;

                //now multiple by 10
                MultiplyValue *= 10;
            }

            //return the result now
            return RunningTally;
        }

    }

}
