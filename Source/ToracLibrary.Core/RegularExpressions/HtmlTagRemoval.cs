using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToracLibrary.Core.RegularExpressions
{

    /// <summary>
    /// Removes Html Tags From A String.
    /// </summary>
    public static class HtmlTagRemoval
    {

        #region Public Static Variables

        /// <summary>
        /// Compiled Regular Expression To Grab Html Tags In A String. Compile Regex Is Used For Performance. Made Public To Allow Other Methods To Use It If You Want A Different Implementation
        /// </summary>
        private static readonly Regex HtmlCompiledRegExStatement = new Regex("<.*?>", RegexOptions.Compiled);

        #endregion

        #region Public Methods

        /// <summary>
        /// Remove HTML From String With Compiled Regex. Will Replace The Html Tags With String.Empty. Use Overload If You Want To Replace It With A Custom Value You Pass In
        /// </summary>
        /// <param name="StringToStrip">String To Strip</param>
        /// <returns>String Passed In Minus The Html Tags</returns>
        public static string StripHtmlTags(string StringToStrip)
        {
            return HtmlCompiledRegExStatement.Replace(StringToStrip, string.Empty);
        }

        /// <summary>
        /// Remove HTML From String With Compiled Regex And Replace It With The Replace Value You Passed In
        /// </summary>
        /// <param name="StringToStrip">String To Strip</param>
        /// <param name="ReplaceValue">The Value You Want To Replace The Tags With</param>
        /// <returns>String Passed In Minus The Html Tags</returns>
        public static string StripHtmlTags(string StringToStrip, string ReplaceValue)
        {
            return HtmlCompiledRegExStatement.Replace(StringToStrip, ReplaceValue);
        }

        #endregion

    }

}
