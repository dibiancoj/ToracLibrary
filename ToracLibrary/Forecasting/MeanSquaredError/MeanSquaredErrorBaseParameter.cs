using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Forecasting.MeanSquaredError
{

    /// <summary>
    /// Holds the item to calculate the mean squared error.
    /// </summary>
    /// <remarks>This class is immutable.</remarks>
    public class MeanSquaredErrorBaseParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CurrentPeriodValueToSet">Field For The Data's Current Period Value</param>
        /// <param name="ForecastedValueToSet">Field For The Data's Forecasted Value</param>
        protected MeanSquaredErrorBaseParameter(double CurrentPeriodValueToSet, double? ForecastedValueToSet)
        {
            //set the current period value
            CurrentPeriodValue = CurrentPeriodValueToSet;

            //set the forecasted value
            ForecastedValue = ForecastedValueToSet;
        }

        #endregion

        #region Read Only Properties

        /// <summary>
        /// Field For The Data's Current Period Value 
        /// </summary>
        public double CurrentPeriodValue { get; }

        /// <summary>
        /// Field For The Data's Forecasted Value
        /// </summary>
        public double? ForecastedValue { get; }

        #endregion

    }

}
