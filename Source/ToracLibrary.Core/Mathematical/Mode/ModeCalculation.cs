using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.Mathematical.Mode
{

    /// <summary>
    /// Retrieves the most used values in the set
    /// </summary>
    public static class ModeCalculation
    {

        #region Public Overload Methods

        /// <summary>
        /// Int16 Overload For Mode Calcuation
        /// </summary>
        /// <param name="Source">Source Of Int16's</param>
        /// <returns>ModeResult</returns>
        public static ModeResult<Int16> CalculateMode(IEnumerable<Int16> Source)
        {
            return ModeCalculationHelper(Source);
        }

        /// <summary>
        /// Int Overload For Mode Calcuation
        /// </summary>
        /// <param name="Source">Source Of Int's</param>
        /// <returns>ModeResult</returns>
        public static ModeResult<Int32> CalculateMode(IEnumerable<Int32> Source)
        {
            return ModeCalculationHelper(Source);
        }

        /// <summary>
        /// Int64 Overload For Mode Calcuation
        /// </summary>
        /// <param name="Source">Source Of Int 64's</param>
        /// <returns>ModeResult</returns>
        public static ModeResult<Int64> CalculateMode(IEnumerable<Int64> Source)
        {
            return ModeCalculationHelper(Source);
        }

        /// <summary>
        /// double Overload For Mode Calcuation
        /// </summary>
        /// <param name="Source">Source Of doubles</param>
        /// <returns>ModeResult</returns>
        public static ModeResult<double> CalculateMode(IEnumerable<double> Source)
        {
            return ModeCalculationHelper(Source);
        }

        /// <summary>
        /// decimal Overload For Mode Calcuation
        /// </summary>
        /// <param name="Source">Source Of decimals</param>
        /// <returns>ModeResult</returns>
        public static ModeResult<decimal> CalculateMode(IEnumerable<decimal> Source)
        {
            return ModeCalculationHelper(Source);
        }

        #endregion

        #region Private Helper Methods

        /*This is the linq implementation. It was slower with larger datasets (and more memory)
            //grab the grouped data
            var GroupedData = DataSet.GroupBy(x => x).Select(x => new
            {
                Number = x.Key,
                CountOfTimesUsed = x.Count()
            }).ToArray();

            //now grab the max count
            int MaxTimesUsed = GroupedData.Max(x => x.CountOfTimesUsed);

            //now return everything which is used x amount of times
            return new ModeResult<T>(MaxTimesUsed, GroupedData.Where(x => x.CountOfTimesUsed == MaxTimesUsed).Select(x => x.Number));
         */

        /// <summary>
        /// Calculate the Mode of list of numbers
        /// </summary>
        /// <param name="Source">Data Source To Calculate</param>
        /// <returns>ModeResult Value</returns>
        private static ModeResult<T> ModeCalculationHelper<T>(IEnumerable<T> Source) where T : struct
        {
            //manually using a dictionary was faster and uses less memory then using a groupby in linq. since this is a helper method i would rather make it as faster as possible.
            //The linq method (above code comment) It was slower with larger datasets (and more memory). Even though it was 5 lines

            //basically we just want to find which number is used the most. Complication is if we have 2 numbers that have been used the most.

            //first make sure the source has atleast 1 value
            if (!Source.AnyWithNullCheck())
            {
                //throw an error because there are no number
                throw new ArgumentNullException("Source", "Can't compute mean for an empty set.");
            }

            //declare a dictionary which will hold the tally for the numbers
            var TallyContainer = new Dictionary<T, int>();

            //Holds the max number found
            int MaxNumberUsedRunningTally = 1;

            //loop through all the numbers now
            foreach (var NumberToProcess in Source)
            {
                //declare the number we want to try to get from the dictionary. This holds the count of how many times it's been used
                int CountOfTimesUsedFromDictionary;

                //try to grab this number for the dictionary (which the numbers we have already processed)
                if (TallyContainer.TryGetValue(NumberToProcess, out CountOfTimesUsedFromDictionary))
                {
                    //we can't update the dictionary by setting the CountOfTimesUsedFromDictionary because int is a struct. So increment the local variable
                    CountOfTimesUsedFromDictionary++;

                    //set the dictionary now
                    TallyContainer[NumberToProcess] = CountOfTimesUsedFromDictionary;

                    //if it's more then the max number we currently have, then set the max
                    if (CountOfTimesUsedFromDictionary > MaxNumberUsedRunningTally)
                    {
                        //its the most times used, so set the max number
                        MaxNumberUsedRunningTally = CountOfTimesUsedFromDictionary;
                    }
                }
                else
                {
                    //we never found it in the dictionary...so just add it
                    TallyContainer.Add(NumberToProcess, 1);
                }
            }

            //now return everything which is used x amount of times
            return new ModeResult<T>(MaxNumberUsedRunningTally, TallyContainer.Where(x => x.Value == MaxNumberUsedRunningTally).Select(x => x.Key));
        }

        #endregion

    }

}
