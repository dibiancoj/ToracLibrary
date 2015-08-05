using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Forecasting.LinearRegression
{

    /// <summary>
    /// Allows us to seperate sub class and still have it inside the LinearRegressionModel
    /// </summary>
    public partial class LinearRegressionCalculator
    {

        /// <summary>
        /// Class Is Used To Provide An Object To Help With The Calculations In Linear Regression Class
        /// </summary>
        /// <remarks>Class is immutable</remarks>
        private class LinearRegressionLineTallyResult
        {

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="CountToSet">Number of plots</param>
            /// <param name="SumXToSet">Sum of X</param>
            /// <param name="SumYToSet">Sum of Y</param>
            /// <param name="SumXYToSet">Sum of X*Y</param>
            /// <param name="SumXXToSet">Sum of X*X</param>
            /// <param name="SumXSquaredToSet">Sum of X Squared</param>
            /// <param name="SumYSquaredToSet">Sum of Y Sqaured</param>
            /// <remarks>Class Is Immutable</remarks>
            public LinearRegressionLineTallyResult(int CountToSet, double SumXToSet, double SumYToSet, double SumXYToSet, double SumXXToSet, double SumXSquaredToSet, double SumYSquaredToSet)
            {
                Count = CountToSet;
                SumX = SumXToSet;
                SumY = SumYToSet;
                SumXY = SumXYToSet;
                SumXX = SumXXToSet;
                SumXSquared = SumXSquaredToSet;
                SumYSquared = SumYSquaredToSet;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Holds The Current Count Of Items Passed In
            /// </summary>
            public int Count { get; }

            /// <summary>
            /// Holds The Sum Of The X Column
            /// </summary>
            public double SumX { get; }

            /// <summary>
            /// Holds The Sum Of The Y Column
            /// </summary>
            public double SumY { get; }

            /// <summary>
            /// Holds The Sum Of The XY Column
            /// </summary>
            public double SumXY { get; }

            /// <summary>
            /// Holds The Sum Of The XX Column
            /// </summary>
            public double SumXX { get; }

            /// <summary>
            /// Holds the Sum Of The X Squared Column
            /// </summary>
            public double SumXSquared { get; }

            /// <summary>
            /// Holds The Sum Of The Y Squared Column
            /// </summary>
            public double SumYSquared { get; }

            #endregion

        }

    }

}
