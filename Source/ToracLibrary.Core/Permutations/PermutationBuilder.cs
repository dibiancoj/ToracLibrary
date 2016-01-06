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
        /// <typeparam name="T">Type of items to permutate. Characters or strings</typeparam>
        /// <returns>An array with all the combinations inside</returns>
        public static IEnumerable<IEnumerable<T>> BuildPermutationListLazy<T>(IEnumerable<T> CharactersToPermutate, int LengthToPermutate)
        {
            //loop through all the permutations
            foreach (var Permutations in PermuteLazy<T>(CharactersToPermutate, LengthToPermutate))
            {
                //items in this list
                var ItemsInThisIteration = new List<T>();

                //loop through the inner permutations
                foreach (T EachItemInPermutation in Permutations)
                {
                    //return this item
                    ItemsInThisIteration.Add(EachItemInPermutation);
                }

                //return this list now
                yield return ItemsInThisIteration;
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
        /// <returns>enumeration of enumerators, one for each permutation of the input.</returns>
        private static IEnumerable<IEnumerable<T>> PermuteLazy<T>(IEnumerable<T> ListToPermute, int LengthOfPermute)
        {
            //do we have 0 length to go to?
            if (LengthOfPermute == 0)
            {
                //just return the 0 based index
                yield return new T[0];
            }
            else
            {
                //starting element index
                int StartingElementIndex = 0;

                //loop through the elements
                foreach (T startingElement in ListToPermute)
                {
                    //grab the remaining items
                    IEnumerable<T> RemainingItems = AllExcept(ListToPermute, StartingElementIndex);

                    //loop through the next set recursively
                    foreach (IEnumerable<T> PermutationOfRemainder in PermuteLazy(RemainingItems, LengthOfPermute - 1))
                    {
                        //go start from the previous call and keep looping
                        yield return ConcatItems<T>(new T[] { startingElement }, PermutationOfRemainder);
                    }

                    //increase the tally
                    StartingElementIndex += 1;
                }
            }
        }

        /// <summary>
        /// Enumerates over contents of both lists and returns them for all items
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="FirstSet">First Set To Loop Through</param>
        /// <param name="SecondSet">Second Set To Loop Through</param>
        /// <returns>List of items to </returns>
        private static IEnumerable<T> ConcatItems<T>(IEnumerable<T> FirstSet, IEnumerable<T> SecondSet)
        {
            //return the first set
            foreach (T Item in FirstSet)
            {
                yield return Item;
            }

            //return the second set
            foreach (T Item in SecondSet)
            {
                yield return Item;
            }
        }

        /// <summary>
        /// Enumerates over all items in the input, skipping over the item with the specified offset.
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="Input">Items to loop through to add</param>
        /// <param name="IndexToSkip">Index to skip over</param>
        /// <returns></returns>
        private static IEnumerable<T> AllExcept<T>(IEnumerable<T> Input, int IndexToSkip)
        {
            //index to loop through
            int Index = 0;

            //loop through the items to iterate over
            foreach (T Item in Input)
            {
                //do we want to skip this index?
                if (Index != IndexToSkip)
                {
                    //we don't want to skip so return this item
                    yield return Item;
                }

                //increase the tally
                Index += 1;
            }
        }

        #endregion

    }

}
