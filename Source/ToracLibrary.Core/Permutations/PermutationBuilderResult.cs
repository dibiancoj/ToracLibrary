using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Permutations
{

    /// <summary>
    /// Holds the result of the permutation builder which makes digesting the result object easier
    /// </summary>
    /// <typeparam name="T">The data type we calcuated for</typeparam>
    /// <remarks>Class is immutable</remarks>
    public class PermutationBuilderResult<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// <param name="ItemsThatMakeUpTheResult">Holds the items that make up the different permutation. ie. if you get back "A", "B", "C"...the word would be "ABC". Leaving it like this for numbers to see what you want to do with the numbers individually.</param>
        /// </summary>
        public PermutationBuilderResult(IList<T> ItemsThatMakeUpTheResult)
        {
            PermutationItems = ItemsThatMakeUpTheResult;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the items that make up the different permutation. ie. if you get back "A", "B", "C"...the word would be "ABC". Leaving it like this for numbers to see what you want to do with the numbers individually.
        /// </summary>
        public IList<T> PermutationItems { get; }

        #endregion

    }

}
