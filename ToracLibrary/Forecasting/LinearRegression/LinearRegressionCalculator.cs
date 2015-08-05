using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Forecasting.LinearRegression
{

    /// <summary>
    /// Calculate Linear Regression
    /// </summary>
    public static partial class LinearRegressionCalculator
    {

        #region Documentation

        /*
              'Examples used for testing
              'x	  y	    xy	   x2	    y2
              '--------------------------------
              '1	2.6   2.6     1	        6.8
              '2.3  2.8   6.4     5.3	    7.8
              '3.1  3.1   9.6     9.6	    9.6
              '4.8  4.7   22.6    23.0	    22.1
              '5.6  5.1   28.6	  31.4	    26.0
              '6.3  5.3   33.4	  39.7	    28.1
              '-------------------------------------
              '                Sums                '
              '-------------------------------------
              '23.1 23.6  103.16  109.99	100.4

              '0.5842	Slope (m)
              '1.6842	Y Intercept (b)
              '0.9488	Rsquared (r2)
              '0.9741	Correlation R
              '533.6	X Squared
              '556.96	Y Squared

              'Y = mX + b
              'Trend Lines
              '2.2684	
              '3.0278	
              '3.4952	
              '4.4883	
              '4.9557	
              '5.3646	
        */

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate Linear Regression
        /// </summary>
        /// <param name="X">X Points | IEnumerable Of Double</param>
        /// <param name="Y">Y Points | IEnumerable Of Double</param>
        /// <returns>LinearRegressionResults</returns>
        public static LinearRegressionResults CalculateLinearRegression(IEnumerable<double> X, IEnumerable<double> Y)
        {
            //let's push the IEnumerable to arrays so we can validate the count and then pass it into Tally Up Figures (need something where we can use the index)
            IList<double> XValues = X.ToArray();
            IList<double> YValues = Y.ToArray();

            //Validate to make sure they are the same size
            if (XValues.Count != YValues.Count)
            {
                throw new ArgumentOutOfRangeException("The X And Y Data Points Are Not The Same Length. Please Verify Your Data Points");
            }

            //Tally Results. Used For When We Loop Through Each Line And Return The Results To The Main Method
            LinearRegressionLineTallyResult Tally = TallyUpFigures(XValues, YValues);

            //Calculate (R) - Correlation
            double R = CalculateCorrelation(Tally.Count, Tally.SumXY, Tally.SumX, Tally.SumY, Tally.SumXSquared, Tally.SumYSquared);

            //R Squared (R2)
            double RSquared = CalculateRSquared(R);

            //Calculate Slope (M)
            double MSlope = CalculateSlope(Tally.Count, Tally.SumXY, Tally.SumX, Tally.SumY, Tally.SumXX);

            //Calculate Intercept (B)
            double YInterceptB = CalculateYIntercept(Tally.Count, Tally.SumY, Tally.SumX, MSlope);

            //Return The Results
            return new LinearRegressionResults(MSlope, YInterceptB, R, RSquared);
        }

        /// <summary>
        /// Determine How Many Interval Periods Until You Reach Your Forecast Goal
        /// </summary>
        /// <param name="ForecastGoal">What Value You Want To Reach And Determine How Many Intervals It Will Take</param>
        /// <param name="X">X Points | IEnumerable Of Double</param>
        /// <param name="Y">Y Points | IEnumerable Of Double</param>
        /// <returns>Double | # Of Intervals</returns>
        public static double ForecastNumberOfPeriods(double ForecastGoal, IEnumerable<double> X, IEnumerable<double> Y)
        {
            /*
            Formula y=mx+b
            y = forecast value to (the y value)
            m = slope
            x = what we are solving for (x intercept)
            b = y intercept
    
            y = 160
            m = 10
            b = 10
        
            160=10x+10
            Subtract the 10 at the end
            150=10x
            Divide by the 10 to get the x by itself
            5=x
            */

            //Get The Results Stats First
            LinearRegressionResults Results = CalculateLinearRegression(X, Y);

            //Y = mX + B (solving for x)
            //We know Y (forecast goal) and we know B (y intercept)...so subtract the 2...
            double Tally = (ForecastGoal - Results.YInterceptB);

            //we also know slope...
            //divide Slope(m) To Get X By ItSelf
            Tally /= Results.MSlope;

            //Return the results because we have X by itself
            return Tally;
        }

        #endregion

        #region Private Calc Helpers

        #region Tally Results

        /// <summary>
        /// Tally Up Each Line To Calculate The Sums
        /// </summary>
        /// <param name="X">X Array Of Items</param>
        /// <param name="Y">Y Array Of Items</param>
        /// <returns>TallyResultsClass</returns>
        private static LinearRegressionLineTallyResult TallyUpFigures(IList<double> X, IList<double> Y)
        {
            //holds the sum of x
            double SumX = 0;

            //holds the sum of y
            double SumY = 0;

            //holds the sum of (x * y)
            double SumXY = 0;

            //holds the sum of (x * x)
            double SumXX = 0;

            //holds the x squared
            double SumXSquared = 0;

            //holds the y squared
            double SumYSquared = 0;

            //Loop through all the items and calculate each row
            for (int i = 0; i < X.Count; i++)
            {
                //Set the holder variables
                double HolderX = X[i];
                double HolderY = Y[i];

                //Calculate The Sum X Squared (and increase it)
                SumXSquared += Math.Pow(HolderX, 2);
                SumYSquared += Math.Pow(HolderY, 2);

                //Add to the sums
                SumX += HolderX;
                SumY += HolderY;
                SumXY += HolderX * HolderY;
                SumXX += (HolderX * HolderX);
            };

            //Return the class for the results
            return new LinearRegressionLineTallyResult(X.Count, SumX, SumY, SumXY, SumXX, SumXSquared, SumYSquared);
        }

        #endregion

        #region Calculation Stats

        /// <summary>
        /// Calculate Correlation (R)
        /// </summary>
        /// <param name="NumberOfItemsInXArray">Number Of Items In The X Array</param>
        /// <param name="SumXY">Sum Of (X*Y) For Each Line</param>
        /// <param name="SumX">Sum Of X For Each Line</param>
        /// <param name="SumY">Sum Of Y For Each Line</param>
        /// <param name="SumXSquared">Sum Of (X2) For Each Line</param>
        /// <param name="SumYSquared">Sum Of (Y2) For Each Line</param>
        /// <returns>Double</returns>
        private static double CalculateCorrelation(int NumberOfItemsInXArray, double SumXY, double SumX, double SumY, double SumXSquared, double SumYSquared)
        {
            return (NumberOfItemsInXArray * SumXY - SumX * SumY) / Math.Sqrt((NumberOfItemsInXArray * SumXSquared - Math.Pow(SumX, 2)) * (NumberOfItemsInXArray * SumYSquared - Math.Pow(SumY, 2)));
        }

        /// <summary>
        /// Calculate R Squared
        /// </summary>
        /// <param name="R">Result Of CalculateCorrelation</param>
        /// <returns>Double</returns>
        private static double CalculateRSquared(double R)
        {
            return Math.Pow(R, 2);
        }

        /// <summary>
        /// Calculate The Slope
        /// </summary>
        /// <param name="NumberOfItemsInXArray">Number Of Items In The X Array</param>
        /// <param name="SumXY">Sum Of (X*Y) For Each Line</param>
        /// <param name="SumX">Sum Of X For Each Line</param>
        /// <param name="SumY">Sum Of Y For Each Line</param>
        /// <param name="SumXX">Sum Of (X*X) For Each Line</param>
        /// <returns>Double</returns>
        private static double CalculateSlope(int NumberOfItemsInXArray, double SumXY, double SumX, double SumY, double SumXX)
        {
            return ((NumberOfItemsInXArray * SumXY) - (SumX * SumY)) / ((NumberOfItemsInXArray * SumXX) - (SumX * SumX));
        }

        /// <summary>
        /// Calculate The Y Intercept
        /// </summary>
        /// <param name="NumberOfItemsInXArray">Number Of Items In The X Array</param>
        /// <param name="SumY">Sum Of Y For Each Line</param>
        /// <param name="SumX">Sum Of X For Each Line</param>
        /// <param name="SlopeM">Result Of CalculateSlope</param>
        /// <returns>Double</returns>
        private static double CalculateYIntercept(int NumberOfItemsInXArray, double SumY, double SumX, double SlopeM)
        {
            return ((SumY - (SlopeM * SumX)) / NumberOfItemsInXArray);
        }

        #endregion

        #endregion

    }

}
