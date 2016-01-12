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
        public PermutationBuilderResult(IEnumerable<T> ItemsThatMakeUpTheResult)
        {
            PermutationItems = ItemsThatMakeUpTheResult;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the items that make up the different permutation. ie. if you get back "A", "B", "C"...the word would be "ABC". Leaving it like this for numbers to see what you want to do with the numbers individually.
        /// </summary>
        public IEnumerable<T> PermutationItems { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Combines all the permutation items that make up this result. It just does a string builder concat and returns it. One right after the other. If you have numbers and you need something you will need to implement that yourself
        /// </summary>
        /// <returns>All the combined permutation items that make up this result</returns>
        public string PermutationItemsTogether()
        {
            //start the string builder
            var Result = new StringBuilder();

            //loop through all the items
            foreach (var ItemToAdd in PermutationItems)
            {
                //add the item
                Result.Append(ItemToAdd);
            }

            //return the result
            return Result.ToString();
        }

        #endregion

    }

}
