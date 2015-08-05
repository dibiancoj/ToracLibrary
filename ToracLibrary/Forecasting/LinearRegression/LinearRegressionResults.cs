using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Forecasting.LinearRegression
{

    /// <summary>
    /// Class Used To Return Data From The Linear Regression Calculations
    /// </summary>
    /// <remarks>Properties Are Privately Immutable So The Calling Method Can't Change The Data</remarks>
    public class LinearRegressionResults
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MSlopeToSet">Slope</param>
        /// <param name="YInterceptBToSet">Y Intercept</param>
        /// <param name="RToSet">R</param>
        /// <param name="RSquaredToSet">R Squared</param>
        public LinearRegressionResults(double MSlopeToSet, double YInterceptBToSet, double RToSet, double RSquaredToSet)
        {
            MSlope = MSlopeToSet;
            YInterceptB = YInterceptBToSet;
            R = RToSet;
            RSquared = RSquaredToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds The Slope
        /// </summary>
        public double MSlope { get; }

        /// <summary>
        /// Holds The Y Intercept
        /// </summary>
        public double YInterceptB { get; }

        /// <summary>
        /// Holds R
        /// </summary>
        public double R { get; }

        /// <summary>
        /// Holds R Squared
        /// </summary>
        public double RSquared { get; }

        #endregion

    }

}
