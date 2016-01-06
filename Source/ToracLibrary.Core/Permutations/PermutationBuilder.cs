using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Permutations
{

    /// <summary>
    /// Builds the list of possible permutations for a give set of characters for the given length
    /// </summary>
    public static class PermutationBuilder
    {

        #region Public Methods

        /// <summary>
        /// Builds the list of possible permutations for a give set of characters for the given length
        /// </summary>
        /// <param name="CharactersToPermutate">Characters that will permutate (or numbers if T is a number)</param>
        /// <param name="LengthToPermutate">The length of each row will permutate too.</param>
        /// <param name="ItemsAreExclusive">Are items exclusive? Meaning once they are used in the combo, they can't be used in the next items. Example: "abc". Can it be "aaa"? Or once a is used, it can't be used again.</param>
        /// <typeparam name="T">Type of items to permutate. Characters or strings</typeparam>
        /// <returns>An array with all the combinations inside</returns>
        public static IEnumerable<PermutationBuilderResult<T>> BuildPermutationListLazy<T>(IEnumerable<T> CharactersToPermutate, int LengthToPermutate, bool ItemsAreExclusive)
        {
            //loop through all the permutations
            foreach (var Permutations in PermuteLazy<T>(CharactersToPermutate, LengthToPermutate, ItemsAreExclusive))
            {
                //return this list now
                yield return new PermutationBuilderResult<T>(Permutations.PermutationItems);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an enumeration of enumerators, one for each permutation of the input.
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="ListToPermute">List to permute </param>
        /// <param name="LengthOfPermute">Length of the word to go to</param>
        /// <param name="ItemsAreExclusive">Are items exclusive? Meaning once they are used in the combo, they can't be used in the next items. Example: "abc". Can it be "aaa"? Or once a is used, it can't be used again.</param>
        /// <returns>An array with all the combinations inside</returns>
        private static IEnumerable<PermutationBuilderResult<T>> PermuteLazy<T>(IEnumerable<T> ListToPermute, int LengthOfPermute, bool ItemsAreExclusive)
        {
            //do we have 0 length to go to?
            if (LengthOfPermute == 0)
            {
                //just return the 0 based index so we can short circuit the rescursive function
                yield return new PermutationBuilderResult<T>(Array.Empty<T>());
            }
            else
            {
                //starting element index
                int StartingElementIndex = 0;

                //loop through the elements
                foreach (T StartingElement in ListToPermute)
                {
                    //grab the remaining items
                    IEnumerable<T> RemainingItems;

                    //are the items exclusive?
                    if (ItemsAreExclusive)
                    {
                        //grab everything but this item that is at the specified index
                        RemainingItems = ListToPermute.Where((x, i) => i != StartingElementIndex);
                    }
                    else
                    {
                        //just use the list of items
                        RemainingItems = ListToPermute;
                    }

                    //loop through the next set recursively
                    foreach (var PermutationOfRemainder in PermuteLazy(RemainingItems, LengthOfPermute - 1, ItemsAreExclusive))
                    {
                        //go start from the previous call and keep looping
                        yield return new PermutationBuilderResult<T>(new T[] { StartingElement }.Concat(PermutationOfRemainder.PermutationItems).ToArray());
                    }

                    //increase the tally
                    StartingElementIndex += 1;
                }
            }
        }

        #endregion

    }

}
