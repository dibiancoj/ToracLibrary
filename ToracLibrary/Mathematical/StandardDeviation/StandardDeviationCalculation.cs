using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.Mathematical.StandardDeviation
{

    /// <summary>
    /// Calculate standard deviation
    /// </summary>
    public static class StandardDeviationCalculation
    {

        #region Documentation

        /*
        Step 1: Find the arithmetic mean (or average) of 4 and 8,
        (4 + 8) / 2 = 6.

        Step 2: Find the deviation of each number from the mean,
        4 − 6 = − 2
        8 − 6 = 2.

        Step 3: Square each of the deviations (amplifying larger deviations and making negative values positive),
        ( − 2)2 = 4
        22 = 4.

        Step 4: Sum the obtained squares (as a first step to obtaining an average),
        4 + 4 = 8.

        Step 5: Divide the sum by the number of values, which here is 2 (giving an average),
        8 / 2 = 4.

        Step 6: Take the non-negative square root of the quotient (converting squared units back to regular units),
        sqrt{4}=2.

        So, the standard deviation of the set is 2.
        */

        #endregion

        /// <summary>
        /// Main helper method that actually calculates standard deviation
        /// </summary>
        /// <typeparam name="T">type of the number passed in</typeparam>
        /// <param name="DataSource">Datasource that we use to calculate the standard deviation</param>
        /// <returns>Standard deviation in a double</returns>
        public static double CalculateStandardDeviationHelper(IEnumerable<double> DataSource)
        {
            //validation
            if (!DataSource.AnyWithNullCheck())
            {
                throw new ArgumentNullException("You Must Pass In An IEnumerable With Atleast 1 Element.");
            }
            //end of validation

            //Step 1 - Find The Average
            double RunningTally = 0;

            //holds the count of items
            int CountOfItems = 0;

            //Loop through each guy. We Need to Calculate The Average First
            foreach (var NumberToCalculateInLine in DataSource)
            {
                //Sum up the figures
                RunningTally += NumberToCalculateInLine;

                //Count up how many items we have
                CountOfItems++;
            }

            //Calculate the average (Step 1)
            double AverageOfNumbers = (RunningTally / CountOfItems);

            //Holds the sum of all the deviation differences
            double SumOfAllDeviationDifferences = 0;

            //Step 2 & 3 (Find the deviation of each number from the mean &  Square each of the deviations (amplifying larger deviations and making negative values positive))
            foreach (var NumberToCalculateInLine in DataSource)
            {
                //Step 2 (grab the difference)
                double DeviationOfLineItem = (NumberToCalculateInLine - AverageOfNumbers);

                //Step 3 Square each deviation and Step 4 Add all the together
                SumOfAllDeviationDifferences += Math.Pow(DeviationOfLineItem, 2);
            }

            //Step 5 Divide the sum by the amount of items we have
            double DeviationAverage = (SumOfAllDeviationDifferences / CountOfItems);

            //Step 6 Get the square root and return how many decimal points we want
            return Math.Sqrt(DeviationAverage);
        }

    }

}
