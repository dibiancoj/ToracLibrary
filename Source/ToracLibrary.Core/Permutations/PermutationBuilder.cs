using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions.IEnumerableExtensionMethods;

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
            //use the overload
            return TotalNumberOfPermutationCombinations(ListToPermute.CountWithCastAttempt(), LengthToPermutate, ItemsAreExclusive);
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
                        yield return new PermutationBuilderResult<T>(new T[] { StartingElement }.Concat(PermutationOfRemainder.PermutationItems));
                    }

                    //increase the tally
                    StartingElementIndex += 1;
                }
            }
        }

        #endregion

    }

}
