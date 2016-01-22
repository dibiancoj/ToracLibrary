using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.CharacterMapping
{

    /// <summary>
    /// Holds static list for alphabet and numeric characters
    /// </summary>
    public static class Characters
    {

        /// <summary>
        /// Returns the number characters in the english language
        /// </summary>
        /// <returns>all the numbers in the english language in an iterator</returns>
        public static IEnumerable<int> AllNumberCharactersLazy()
        {
            //loop through the numbers and yield them
            for (int i = 0; i < 10; i++)
            {
                //return i
                yield return i;
            }
        }

        /// <summary>
        /// Returns the alphabet characters in the english language
        /// </summary>
        /// <returns>all the alphabet characters in the english language in an iterator</returns>
        /// <remarks>Call .ToUpper() if you want uppercase</remarks>
        public static IEnumerable<char> AllAlphaBetCharactersLazy()
        {
            //loop through each character
            foreach (var Character in "abcdefghijklmnopqrstuvwxyz")
            {
                //return the character
                yield return Character;
            }
        }

    }

}
