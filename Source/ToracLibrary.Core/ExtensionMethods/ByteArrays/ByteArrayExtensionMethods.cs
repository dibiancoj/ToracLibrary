using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToracLibrary.Core.ExtensionMethods.ByteArrayExtensions
{

    /// <summary>
    /// Extension Methods For Byte Arrays
    /// </summary>
    public static class ByteArrayExtensionMethods
    {

        #region String To Hexadecimal

        /// <summary>
        /// Converts a byte array which is hashed to a string for encryption
        /// </summary>
        /// <param name="SecurityBytes">bytes to convert</param>
        /// <param name="ToLowerCaseHash">To lower or uppcase hash. Will convert everything to uppercase if false</param>
        /// <returns>string which represents the bytes</returns>
        public static string ToByteArrayToHexadecimalString(this IEnumerable<byte> SecurityBytes, bool ToLowerCaseHash)
        {
            //create a new string builder
            var Builder = new StringBuilder();

            //format to use
            string FormatToUse = ToLowerCaseHash ? "x2" : "X2";

            //loop through the bytes
            foreach (var BytesToWrite in SecurityBytes)
            {
                //append it (x2 pushed to hexidecimal uppercase)
                Builder.Append(BytesToWrite.ToString(FormatToUse));
            }

            //return the string
            return Builder.ToString();
        }

        #endregion

    }

}
