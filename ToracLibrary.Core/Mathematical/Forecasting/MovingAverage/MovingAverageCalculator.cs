using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Forecasting.MeanSquaredError;

namespace ToracLibrary.Core.Mathematical.Forecasting.MovingAverage
{

    /// <summary>
    /// Calculate Forecasting Values By Using The Moving Average Methodology 
    /// </summary>
    /// <remarks>Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class MovingAverageCalculator
    {

        /*      ******************Example**************************
         *      Data Points = 10,20,30,40,50,60,70,71.25,11,50
         *      Number Of Periods = 4
         * 
         *      Data Point  |   Forecasted Value |  Standard Error
         *      10          |   N/A              |  N/A
         *      20          |   N/A              |  N/A
         *      30          |   N/A              |  N/A
         *      40          |   25               |  N/A
         *      50          |   35               |  N/A
         *      60          |   45               |  N/A
         *      70          |   55               |  15
         *      71.25       |   62.8125          |  13.65825
         *      11          |   53.0625          |  23.9293
         *      50          |   50.5625          |  22.72533
         *      ***************************************************
         *      
         *      Results Class Will Hold LineResults Which Contains All The Line Data Result. 
         *      The Main MovingAverageResult Will Hold Overall Information
         */

        #region Main Public Call

        #region Find Best Period (Best MSE)

        /// <summary>
        /// Get The Best Fit Model Based On The Different Number Of Periods You Want To Test.
        /// </summary>
        /// <param name="NumberOfPeriodsToTest">IEnumerable Of Int - Number Of Periods To Check</param>
        /// <param name="YValues">Y Coordinates</param>
        /// <returns>MovingAverageResult - Best Fit Model. (Lowest MSE)</returns>
        /// <remarks>Used to find the best model. For instance is it more accurate to forecast using a 4 month moving average or a 5 month moving average. Which time period to test best represents your data</remarks>
        public static MovingAverageResult CalculateBestFitModel(IEnumerable<int> NumberOfPeriodsToTest, IList<double> YValues)
        {
            //Return the lowest model....so we loop through all the values. Then only use items where we have an MSE value. Order by that and grab the first value
            return CalculateMovingAverageResultsForDataSetLazy(NumberOfPeriodsToTest, YValues).Where(x => x.MeanSquaredError.HasValue).OrderBy(x => x.MeanSquaredError.Value).FirstOrDefault();
        }

        #endregion

        #region Single Time Period

        /// <summary>
        /// Calculate Moving Average
        /// </summary>
        /// <param name="NumberOfPeriods">How Many Intervals To Average Together</param>
        /// <param name="YValues">Y Coordinates</param>
        /// <returns>MovingAverageResult</returns>
        public static MovingAverageResult CalculateMovingAverage(int NumberOfPeriods, IList<double> YValues)
        {
            //holds the list of line items
            var LineItemsResult = new List<MovingAverageLineResults>();

            //Loop through each of the Y Coordinates Passed In
            for (int i = 0; i < YValues.Count; i++)
            {
                //If we are up to a value less then the moving period then the return value is nothing
                if (i < (NumberOfPeriods))
                {
                    //we don't have enough periods...set the value to a null object
                    LineItemsResult.Add(new MovingAverageLineResults(YValues[i], null));
                }
                else
                {
                    //We Have Enough Period Data to calculate the forecast value

                    //we have enough periods so we add it to the return value
                    LineItemsResult.Add(new MovingAverageLineResults(YValues[i], CalculateMovingAverageTally(i, NumberOfPeriods, YValues)));
                }
            }

            //**Note**
            //Could run the CalculateMSE and CalculateForecastValue in different tasks...It was faster to run then without tasks... Tested with 1000 data points
            //**End Of Note**

            //Calculate the MSE
            double? MeanSquaredErrorValue = MeanSquaredErrorCalculation.CalculateMeanSqauredError(LineItemsResult, NumberOfPeriods);

            //Set the forecasted value
            double? ForecastedValue = CalculateForecastedValue(LineItemsResult, NumberOfPeriods);

            //Return all the moving averages we have calculated
            return new MovingAverageResult(NumberOfPeriods, LineItemsResult, ForecastedValue, MeanSquaredErrorValue);
        }

        #endregion

        #endregion

        #region Help Methods

        /// <summary>
        /// Calculate the moving average result for a given data set. Mainly created for the iterator since we will only every consume data that has a MeanSquareError value. We don't need every calculation
        /// </summary>
        /// <param name="NumberOfPeriodsToTest">IEnumerable Of Int - Number Of Periods To Check</param>
        /// <param name="YValues">Y Coordinates</param>
        /// <returns>Iterator of IEnumerable of Moving Average Results.</returns>
        private static IEnumerable<MovingAverageResult> CalculateMovingAverageResultsForDataSetLazy(IEnumerable<int> NumberOfPeriodsToTest, IList<double> YValues)
        {
            //loop through the number of periods to test and go calculate the line item value
            foreach (int NumberOfPeriods in NumberOfPeriodsToTest)
            {
                //go calculate the line item and return it
                yield return CalculateMovingAverage(NumberOfPeriods, YValues);
            }
        }

        /// <summary>
        /// Calculate The Moving Average For CalculateMovingAverage
        /// </summary>
        /// <param name="ElementIndex">What Element Line Are We Up To In The Loop</param>
        /// <param name="NumberOfPeriods">How Many Intervals To Average Together</param>
        /// <param name="Y_Values">Y Coordinates The Developer Has Passed In</param>
        /// <returns>Double</returns>
        private static double CalculateMovingAverageTally(int ElementIndex, int NumberOfPeriods, IList<double> Y_Values)
        {
            //Holds the Tally Of The Sum For This Moving Average Tally
            double SumOfPeriod = 0;

            //Start Looping At The Current Element (i)...Loop until we are at the current value - Number Of Periods...Stepping -1
            for (int x = (ElementIndex - 1); x > ((ElementIndex - 1) - NumberOfPeriods); x--)
            {
                //Sum Up The Period
                SumOfPeriod += Y_Values[x];
            }

            //If the total is less then 0 then set it to 0. Else Average The Sum / Number Of Periods
            if (SumOfPeriod <= 0)
            {
                //return 0
                return 0;
            }

            //Return the sum by the # of periods
            return (SumOfPeriod / NumberOfPeriods);
        }

        /// <summary>
        /// Calculate The Forecasted Value. The Number Of Periods + 1
        /// </summary>
        /// <param name="LineItemsTally">The Tally Of Results For Each Line We Have Built Up. Need IList Instead of IEnumerable becase we need the index based list</param>
        /// <param name="NumberOfPeriods">Number Of periods To Use</param>
        /// <returns>Double - Forecasted Value</returns>
        private static double? CalculateForecastedValue(IList<MovingAverageLineResults> LineItemsTally, int NumberOfPeriods)
        {
            //Holds the tally 
            double TallyHolder = 0;

            //If we have the number of line items then the number of periods then return null
            if (LineItemsTally.Count < NumberOfPeriods)
            {
                return null;
            }

            //loop through the items. Start at the end and subtract the number of periods
            for (int x = (LineItemsTally.Count - 1); x > ((LineItemsTally.Count - 1) - NumberOfPeriods); x--)
            {
                //Add the current periods value
                TallyHolder += (LineItemsTally[x].CurrentPeriodValue);
            }

            //get the average with all the number of periods with forecasted data
            return TallyHolder / NumberOfPeriods;
        }

        #endregion

    }

}
