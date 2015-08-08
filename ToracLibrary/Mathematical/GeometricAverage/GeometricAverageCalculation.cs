using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.Mathematical.GeometricAverage
{

    /// <summary>
    /// Calculate the geometric average for a data set
    /// </summary>
    public static class GeometricAverageCalculation
    {

        /// <summary>
        /// Calculate the geometric average for an ienumerable of double's.
        /// </summary>
        /// <param name="DataSet">An ienumerable of double's</param>
        /// <returns>A double</returns>
        /// <remarks>your ienumerable can't contain a 0 in any of the values.
        ///    Test Examples {1,2,3,4,5)
        ///    Formula = ((1) * (2) * (3) * (4) * (5)) ^ [1 / # of numbers (5 in this example)] ==> Should equal 2.605171085 (GeoMean Formula In Excel - Verify)
        /// </remarks>
        public static double CalculateGeometricAverage(IEnumerable<double> DataSet)
        {
            //validation that we have some number
            if (!DataSet.AnyWithNullCheck())
            {
                throw new NullReferenceException("You Must Pass In An IEnumerable With Atleast 1 Element.");
            }

            //Need to init the number cause otherwise it will be always be 0...This way the first number will be itself
            double WorkingFigure = 1;

            //stores how many items we have
            int CountOfItems = 0;

            foreach (double LineNumberToCalculate in DataSet)
            {
                //Make sure the number is not 0. This is used because the math geo mean formula does not allow 0
                if (LineNumberToCalculate <= 0)
                {
                    throw new InvalidOperationException("All Numbers In The IEnumerable Must Be Greater Than 0.");
                }

                //Set the working figure
                WorkingFigure *= LineNumberToCalculate;

                //Get how many items we have. Increment by 1
                CountOfItems++;
            }

            //return the result
            return Math.Pow(WorkingFigure, (1d / CountOfItems));
        }

    }

}
