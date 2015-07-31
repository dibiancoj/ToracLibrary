using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToracLibrary.RegularExpressions
{

    /// <summary>
    /// Take a string and return only the numbers in that string. ie 123jason945 will return 123945
    /// </summary>
    public static class NumberParser
    {

        #region Public Static Variables

        /// <summary>
        /// Compiled Regular Expression To Grab Only The Numbers In A String. Compile And Made Public So This Reg-Ex Statement Can Be Re-Used
        /// </summary>
        private static readonly Regex NumberOnlyCompiledRegExStatement = new Regex(@"[\d-]", RegexOptions.Compiled);

        #endregion

        #region Public Methods

        /// <summary>
        /// Take a string and return only the numbers in that string. ie 123jason945 will return 123945
        /// </summary>
        /// <param name="StringToParse">String To Parse</param>
        /// <returns>string with only numbers found in the original string</returns>
        public static string ParseStringAndLeaveOnlyNumbers(string StringToParse)
        {
            return NumberOnlyCompiledRegExStatement.Replace(StringToParse, string.Empty);
        }

        /// <summary>
        /// Take a string and replace only the text (any non numeric text) in that string. ie 123jason945 will return 123ReplaceValue945
        /// </summary>
        /// <param name="StringToParse">String To Parse</param>
        /// <param name="ReplaceValue">Text You Want To Replace The Non Numeric Data Found</param>
        /// <returns>string with only numbers found in the original string</returns>
        public static string ParseStringAndLeaveOnlyNumbers(string StringToParse, string ReplaceValue)
        {
            return NumberOnlyCompiledRegExStatement.Replace(StringToParse, ReplaceValue);
        }

        #endregion

    }

}
