using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Mathematical.Forecasting.MeanSquaredError
{
    /// <summary>
    /// Shared Function To Calculate MSE
    /// </summary>
    /// <remarks>Items To Be Passed In Must Implement Navigators.Forecasting.Results.IListItems. Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class MeanSquaredErrorCalculation
    {

        /// <summary>
        /// Holds The Model's MSE. Use The Lowest MSE If You Run The Same Data With Different Number Of Periods
        /// </summary>
        /// <param name="ItemsToCalculate">The Tally Of Results For Each Line We Have Built Up</param>
        /// <param name="MinimumNumberOfPeriodsForValidMSE">Number Of periods To Use If The Count Of myItemsToCalculate Less Then MinimumNumberOfPeriodsForValidMSE Then It Will Return Null</param>
        /// <returns>Double - MSE</returns>
        public static Nullable<double> CalculateMeanSqauredError(IEnumerable<MeanSquaredErrorBaseParameter> ItemsToCalculate, int MinimumNumberOfPeriodsForValidMSE)
        {
            //Hold the return value
            double ReturnValue = 0;

            //hold the number of items we have a forecast value for
            int Count = 0;

            //loop through the periods. Only calculate for periods that have forecasted values
            foreach (var LineItem in ItemsToCalculate)
            {
                //Validate it to make sure it has a result
                if (LineItem.ForecastedValue.HasValue)
                {
                    //Sum up the value for each line that we have a forecasted value for
                    ReturnValue += Math.Pow((LineItem.CurrentPeriodValue - LineItem.ForecastedValue.Value), 2);

                    //Increment the count. Get a count of only items that have forecasted values
                    Count++;
                }
            }

            //do we have 0 points? or we don't have enough points to give a valid MSE then return null
            if (Count == 0 || Count < MinimumNumberOfPeriodsForValidMSE)
            {
                //if we do or we 
                return null;
            }

            //return the summed up value / Count of how many items we have with forecasted values
            return (ReturnValue / Count);
        }

    }

}
