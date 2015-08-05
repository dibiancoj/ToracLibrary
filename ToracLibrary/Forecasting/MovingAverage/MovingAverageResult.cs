using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Forecasting.MovingAverage
{

    /// <summary>
    /// Holds The Return Results For The Moving Average Calculation. Shows Move Of A Overall Result. Use The LineResults For Results Of Each Individual Line
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class MovingAverageResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="NumberOfPeriodsToSet">Number of periods. Used Just For Reference When You Pass The Results Back</param>
        /// <param name="LineResultsToSet">Hold each of the line data in a list</param>
        /// <param name="ForecastedValueToSet">Hold the forecasted value for the 1+ the last period</param>
        /// <param name="MeanSquareErrorToSet">Holds the MSE. (Mean Square Error) - Use the lowest value (Compare Different Number Of Periods And Take The One With The Lowest MSE Value)</param>
        public MovingAverageResult(int NumberOfPeriodsToSet, IList<MovingAverageLineResults> LineResultsToSet, double? ForecastedValueToSet, double? MeanSquareErrorToSet)
        {
            //Set the number of periods...Used Just For Reference When You Pass The Results Back
            NumberOfPeriods = NumberOfPeriodsToSet;

            //set the line results
            LineResults = LineResultsToSet;

            //set the forecasted values
            ForecastedValue = ForecastedValueToSet;

            //set the mean squared error
            MeanSquaredError = MeanSquareErrorToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of periods. Used Just For Reference When You Pass The Results Back
        /// </summary>
        public int NumberOfPeriods { get; }

        /// <summary>
        /// Hold each of the line data in a list
        /// </summary>
        public IList<MovingAverageLineResults> LineResults { get; }

        /// <summary>
        /// Hold the forecasted value for the 1+ the last period
        /// </summary>
        public double? ForecastedValue { get; }

        /// <summary>
        /// Holds the MSE. (Mean Square Error) - Use the lowest value (Compare Different Number Of Periods And Take The One With The Lowest MSE Value)
        /// </summary>
        public double? MeanSquaredError { get; }

        #endregion

    }

}
