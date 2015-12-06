using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Security.Encryption.Shared
{

    /// <summary>
    /// Common security methods
    /// </summary>
    public static class SecurityHelpers
    {

        /// <summary>
        /// Converts a byte array which is hashed to a string for encryption
        /// </summary>
        /// <param name="SecurityBytes">bytes to convert</param>
        /// <returns>string which represents the bytes</returns>
        public static string SecurityByteArrayToString(byte[] SecurityBytes)
        {
            //create a new string builder
            var Builder = new StringBuilder();

            //loop through the bytes
            foreach (var BytesToWrite in SecurityBytes)
            {
                //append it (x2 pushed to hexidecimal uppercase)
                Builder.Append(BytesToWrite.ToString("X2"));
            }

            //return the string
            return Builder.ToString();
        }

    }

}
