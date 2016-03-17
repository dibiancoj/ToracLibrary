using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Mathematical.Mode
{

    /// <summary>
    /// Result of the a mode calculation
    /// </summary>
    /// <typeparam name="T">Type of the number. Is it a set of ints, doubles, etc.</typeparam>
    /// <remarks>Class is immutable</remarks>
    public class ModeResult<T> where T : struct
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HowManyTimesTheModeWasUsedToSet">Holds how many times the most used items were used.</param>
        /// <param name="MeanMostUsedItemsInDataSetToSet">The Mean. The most used items in the dataset</param>
        public ModeResult(int HowManyTimesTheModeWasUsedToSet, IEnumerable<T> MeanMostUsedItemsInDataSetToSet)
        {
            //set the variables
            HowManyTimesUsed = HowManyTimesTheModeWasUsedToSet;

            //set the mean
            Mean = new HashSet<T>(MeanMostUsedItemsInDataSetToSet);
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Holds how many times the most used items were used.
        /// </summary>
        public int HowManyTimesUsed { get; }

        /// <summary>
        /// The mean. The numbers which were the most used numbers in the dataset
        /// </summary>
        public ISet<T> Mean { get; }

        #endregion

    }

}
