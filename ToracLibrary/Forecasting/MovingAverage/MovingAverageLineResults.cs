using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Forecasting.MeanSquaredError;

namespace ToracLibrary.Core.Forecasting.MovingAverage
{

    /// <summary>
    /// Holds The Results For Each Line Item (Actual Data, Line By Line)
    /// </summary>
    /// <remarks>Class is property immutable</remarks>
    public class MovingAverageLineResults : MeanSquaredErrorBaseParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CurrentPeriodValueToSet">The Current Period Value</param>
        /// <param name="MovingAveragePlotResultToSet">MovingAveragePlotResult</param>
        public MovingAverageLineResults(double CurrentPeriodValueToSet, double? MovingAveragePlotResultToSet)
            : base(CurrentPeriodValueToSet, MovingAveragePlotResultToSet)
        {
        }

        #endregion

    }

}
