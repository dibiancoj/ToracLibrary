using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Excel
{

    /// <summary>
    /// Holds Any Excel Based Tools. Used When Creating Workbooks And Worksheets Programmically
    /// </summary>
    /// <remarks>Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class ExcelUtilities
    {

        /// <summary>
        /// Converts The Column Such As A Or AA Into The Column Index
        /// </summary>
        /// <param name="ColumnLetter">Column Letter</param>
        /// <returns>Column Index</returns>
        /// <remarks>Hasn't been throughly tested</remarks>
        public static int ColumnLetterToColumnIndex(string ColumnLetter)
        {
            //Start with a base of 0
            int WorkingNumber = 0;

            //start with a power of 1
            int PowerToUse = 1;

            //let's loop through the length until we have what we need
            for (int i = ColumnLetter.Length - 1; i >= 0; i--)
            {
                //add the column letter to the power
                WorkingNumber += (ColumnLetter[i] - 'A' + 1) * PowerToUse;

                //now multiply by 26
                PowerToUse *= 26;
            }

            //return the number now
            return WorkingNumber;
        }

        /// <summary>
        /// Convert A Column Number To Its Alpha Bet Letter. Used Mainly When Using For Loops. You Convert The Number Into The Column Letter When Setting The Range Value With A AlphaBet Character
        /// </summary>
        /// <param name="ColumnNumber">Int - Column Number</param>
        /// <returns>String - Alpha Bet Character Which Is The Equivalant To The Numeric Column Number Passed In</returns>
        public static string ColumnIndexToColumnLetter(int ColumnNumber)
        {
            //Holds the Alpha Base Value. Alpha Characters Start At 64
            const int AlphaBase = 64;

            //Holds The Secondary Base Value. 
            const int SecondBase = 26;

            // If 1-26, then this is an easy conversion. Its 1 letter.
            if (ColumnNumber < 27)
            {
                //return the character associated with the key...Add the base because alpha characters start at 64
                return Convert.ToString(Convert.ToChar((ColumnNumber + AlphaBase)));
            }

            //Now we have to account for AA-ZZ

            //Set the first letter. Column number / base figure
            int FirstLetter = (ColumnNumber / SecondBase);

            //Set the second letter. (% = Mod [remainder after /])
            int SecondLetter = (ColumnNumber % SecondBase);

            //If the remainder is 0
            if (SecondLetter == 0)
            {
                //If the remainder is 0 then it is Z, which should be 26
                SecondLetter = SecondBase;

                //you need to subtract 1 from the initial letter otherwise your lettering will be the next letter in the alphabet
                FirstLetter--;
            }

            //return the value...first letter then second letter. Conver the values to a string before returning it...Need to convert both of the letter
            return Convert.ToString(Convert.ToChar((FirstLetter + AlphaBase))) +
                   Convert.ToString(Convert.ToChar((SecondLetter + AlphaBase)));
        }

    }
}
