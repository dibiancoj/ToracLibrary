using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.Mathematical.Median
{

    /// <summary>
    /// Calculate the median for a data set
    /// </summary>
    public static class MedianCalculation
    {

        /// <summary>
        /// Calculate the median of list of numbers
        /// </summary>
        /// <param name="DataSource">Data Source To Calculate</param>
        /// <returns>Median Value</returns>
        public static double CalculateMedian(IEnumerable<double> DataSource)
        {
            //first make sure the source has atleast 1 value
            if (!DataSource.AnyWithNullCheck())
            {
                //throw an error because there are no number
                throw new ArgumentNullException("Can't compute the median for an empty set.");
            }

            //1. Sort the list
            var SortedDataSourceList = DataSource.OrderBy(x => x).ToArray();

            //2. grab the index (this is the count / 2)
            int Index = SortedDataSourceList.Length / 2;

            //3. check to see if the sorted index is odd or even (if odd we just return the odd value) (if even a little bit harder)
            if (SortedDataSourceList.Length % 2 == 0)
            {
                // 4a. Even number of items. We need grab the 2 elements near the index. Once we have that we divide by 2
                return (SortedDataSourceList[Index] + SortedDataSourceList[Index - 1]) / 2;
            }

            // 4b. Odd number of items. Just return that element at that index
            return SortedDataSourceList[Index];
        }

    }

}
