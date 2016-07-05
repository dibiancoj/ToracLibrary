using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.Permutations
{

    /// <summary>
    /// Builds the list of possible permutations for a give set of characters for the given length
    /// </summary>
    public static class PermutationBuilder
    {

        #region Public Methods

        #region Total Number Of Permutations

        /// <summary>
        /// Calculates the total number of permutations possible. Overload when you have the list of characters. 
        /// </summary>
        /// <typeparam name="T">Type of items to permutate. Characters or strings</typeparam>
        /// <param name="ListToPermute">Characters that will permutate (or numbers if T is a number)</param>
        /// <param name="LengthToPermutate">The length of each row will permutate too.</param>
        /// <param name="ItemsAreExclusive">Are items exclusive? Meaning once they are used in the combo, they can't be used in the next items. Example: "abc". Can it be "aaa"? Or once a is used, it can't be used again.</param>
        /// <returns>An array with all the combinations inside</returns>
        /// <returns></returns>
        public static Int64 TotalNumberOfPermutationCombinations<T>(IEnumerable<T> ListToPermute, int LengthToPermutate, bool ItemsAreExclusive)
        {
            //use the overload (count() does a cast to icollection for optimizations, we don't need to run the same logic)
            return TotalNumberOfPermutationCombinations(ListToPermute.Count(), LengthToPermutate, ItemsAreExclusive);
        }

        /// <summary>
        /// Calculates the total number of permutations possible. Overload when you know the number of characters you need to permutate
        /// </summary>
        /// <typeparam name="T">Type of items to permutate. Characters or strings</typeparam>
        /// <param name="ListToPermute">Number of characters to permutate</param>
        /// <param name="LengthToPermutate">The length of each row will permutate too.</param>
        /// <param name="ItemsAreExclusive">Are items exclusive? Meaning once they are used in the combo, they can't be used in the next items. Example: "abc". Can it be "aaa"? Or once a is used, it can't be used again.</param>
        /// <returns>An array with all the combinations inside</returns>
        /// <returns></returns>
        public static Int64 TotalNumberOfPermutationCombinations(int NumberOfCharactersToPermutate, int LengthToPermutate, bool ItemsAreExclusive)
        {
            //Running tally
            Int64 RunningTally = 1;

            //running characters to permutate
            int CharacterCountToPermutate = NumberOfCharactersToPermutate;

            //loop through the length we want to permutate
            for (int i = 0; i < LengthToPermutate; i++)
            {
                //multiple by how many characters are left
                RunningTally *= CharacterCountToPermutate;

                //if they are exclusive, remove 1 from the choices of characters we can use
                if (ItemsAreExclusive)
                {
                    //subtract 1 from the available character count
                    CharacterCountToPermutate--;
                }
            }

            //just return the tally now
            return RunningTally;
        }

        #endregion

        #region Permutation Builder

        /// <summary>
        /// Builds the list of possible permutations for a give set of characters for the given length
        /// </summary>
        /// <typeparam name="T">Type of items to permutate. Characters or strings</typeparam>
        /// <param name="ListToPermute">Characters that will permutate (or numbers if T is a number)</param>
        /// <param name="LengthToPermutate">The length of each row will permutate too.</param>
        /// <param name="ItemsAreExclusive">Are items exclusive? Meaning once they are used in the combo, they can't be used in the next items. Example: "abc". Can it be "aaa"? Or once a is used, it can't be used again.</param>
        /// <returns>An array with all the combinations inside</returns>
        public static IEnumerable<PermutationBuilderResult<T>> BuildPermutationListLazy<T>(IEnumerable<T> ListToPermute, int LengthToPermutate, bool ItemsAreExclusive)
        {
            //loop through all the permutations
            foreach (var Permutations in PermuteLazy(ListToPermute, LengthToPermutate, ItemsAreExclusive))
            {
                //return this list now
                yield return new PermutationBuilderResult<T>(Permutations.PermutationItems);
            }
        }

        #endregion

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

                //calculate the length -1 so we don't have to keep calculating it
                int LengthMinus1 = LengthOfPermute - 1;

                //loop through the elements
                foreach (T StartingElement in ListToPermute)
                {
                    //grab the remaining items
                    IEnumerable<T> RemainingItems;

                    //are the items exclusive?
                    if (ItemsAreExclusive)
                    {
                        //grab everything but this item that is at the specified index
                        RemainingItems = ExcludeAtIndexLazy(ListToPermute, StartingElementIndex);
                    }
                    else
                    {
                        //just use the list of items
                        RemainingItems = ListToPermute;
                    }

                    //loop through the next set recursively
                    foreach (var PermutationOfRemainder in PermuteLazy(RemainingItems, LengthMinus1, ItemsAreExclusive))
                    {
                        //go start from the previous call and keep looping (use the iterator so we don't have to allocate a dummy array with 1 element)
                        yield return new PermutationBuilderResult<T>(PermutationOfRemainder.PermutationItems.ConcatItemLazy(StartingElement, false));
                    }

                    //increase the tally
                    StartingElementIndex++;
                }
            }
        }

        /// <summary>
        /// Return all items except the item at the IndexToExclude. Used for perfomance reasons instead of ListToPermute.Where((x, i) => i != StartingElementIndex); This is better on memory
        /// </summary>
        /// <typeparam name="T">Type of the collection</typeparam>
        /// <param name="Collection">Collection to iterate</param>
        /// <param name="IndexToExclude">index of the item to exclude</param>
        /// <returns>ienumerable of T except for the item at the specified index</returns>
        /// <remarks>Used BenchmarkDotNet for performance diag. This is better on memory then linq where</remarks>
        private static IEnumerable<T> ExcludeAtIndexLazy<T>(IEnumerable<T> Collection, int IndexToExclude)
        {
            //this method is essentially ListToPermute.Where((x, i) => i != StartingElementIndex) but uses less memory

            //holds the index that we are up to
            int CurrentIndex = 0;

            //loop through the collection
            foreach (var Item in Collection)
            {
                //make sure its the index we don't want
                if (CurrentIndex != IndexToExclude)
                {
                    //it's not the item...so return this item
                    yield return Item;
                }

                //increment the counter
                CurrentIndex++;
            }
        }

        #endregion

    }

}
