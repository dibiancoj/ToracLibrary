using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Forecasting.LinearRegression;
using ToracLibrary.Core.Mathematical.Forecasting.MovingAverage;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for forecasting functionality
    /// </summary>
    public class ForecastingTest
    {

        #region Linear Regression

        /// <summary>
        /// Test linear regression
        /// </summary>
        [Fact]
        public void LinearRegressionTest1()
        {
            //declare my x data points
            var X = new List<double>();

            //declare my y data points
            var Y = new List<double>();

            X.Add(1);
            X.Add(2.3);
            X.Add(3.1);
            X.Add(4.8);
            X.Add(5.6);
            X.Add(6.3);

            Y.Add(2.6);
            Y.Add(2.8);
            Y.Add(3.1);
            Y.Add(4.7);
            Y.Add(5.1);
            Y.Add(5.3);

            //grab the results
            var LinearRegressionResults = LinearRegressionCalculator.CalculateLinearRegression(X, Y);

            //test the results now
            Assert.Equal(0.58418427926858263, LinearRegressionResults.MSlope);
            Assert.Equal(0.97405605951954111, LinearRegressionResults.R);
            Assert.Equal(0.94878520708673586, LinearRegressionResults.RSquared);
            Assert.Equal(1.6842238581492899, LinearRegressionResults.YInterceptB);

            //go forecast these numbers and test the value
            Assert.Equal(168.29582655826547, LinearRegressionCalculator.ForecastNumberOfPeriods(100, X, Y));
        }

        #endregion

        #region Moving Average
        [Fact]
        public void MovingAverage_Test1()
        {
            //create the data points
            var DataPoints = new double[] { 10, 20, 30, 40, 50, 60, 70, 71.25, 11, 50 };

            //go calculate the moving average
            var MovingAverageResults = MovingAverageCalculator.CalculateMovingAverage(4, DataPoints);

            //find the best fit model
            var BestFitModel = MovingAverageCalculator.CalculateBestFitModel(new int[] { 4, 5, 6 }, DataPoints);

            //evaulate the results of the moving average calc
            Assert.Equal(4, MovingAverageResults.NumberOfPeriods);
            Assert.Equal(805.49609375, MovingAverageResults.MeanSquaredError);
            Assert.Equal(50.5625, MovingAverageResults.ForecastedValue);

            //validate the best fit model (mse)
            Assert.Equal(50.5625, BestFitModel.ForecastedValue);
            Assert.Equal(805.49609375, BestFitModel.MeanSquaredError);
            Assert.Equal(4, BestFitModel.NumberOfPeriods);

            //let's loop through the values now
            for (int i = 0; i < MovingAverageResults.LineResults.Count; i++)
            {
                //grab the line result so we have it in a variable
                var LineResult = MovingAverageResults.LineResults[i];

                if (i == 0)
                {
                    Assert.Null(LineResult.ForecastedValue);
                    Assert.Equal(10, LineResult.CurrentPeriodValue);
                }
                else if (i == 1)
                {
                    Assert.Null(LineResult.ForecastedValue);
                    Assert.Equal(20, LineResult.CurrentPeriodValue);
                }
                else if (i == 2)
                {
                    Assert.Null(LineResult.ForecastedValue);
                    Assert.Equal(30, LineResult.CurrentPeriodValue);
                }
                else if (i == 3)
                {
                    Assert.Null(LineResult.ForecastedValue);
                    Assert.Equal(40, LineResult.CurrentPeriodValue);
                }
                else if (i == 4)
                {
                    Assert.Equal(25, LineResult.ForecastedValue);
                    Assert.Equal(50, LineResult.CurrentPeriodValue);
                }
                else if (i == 5)
                {
                    Assert.Equal(35, LineResult.ForecastedValue);
                    Assert.Equal(60, LineResult.CurrentPeriodValue);
                }
                else if (i == 6)
                {
                    Assert.Equal(45, LineResult.ForecastedValue);
                    Assert.Equal(70, LineResult.CurrentPeriodValue);
                }
                else if (i == 7)
                {
                    Assert.Equal(55, LineResult.ForecastedValue);
                    Assert.Equal(71.25, LineResult.CurrentPeriodValue);
                }
                else if (i == 8)
                {
                    Assert.Equal(62.8125, LineResult.ForecastedValue);
                    Assert.Equal(11, LineResult.CurrentPeriodValue);
                }
                else if (i == 9)
                {
                    Assert.Equal(53.0625, LineResult.ForecastedValue);
                    Assert.Equal(50, LineResult.CurrentPeriodValue);
                }
            }
        }

        #endregion  

    }

}
