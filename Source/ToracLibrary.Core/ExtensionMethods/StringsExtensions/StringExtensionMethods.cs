using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToracLibrary.Core.ExtensionMethods.StringExtensions
{

    /// <summary>
    /// Extension Methods For String
    /// </summary>
    public static class StringExtensionMethods
    {

        #region Contains

        /// <summary>
        /// Extension Method For A String To See If It Contains A String And Let The User Use String Comparison. NOTE: **** In dot net core this a native extension method for this ****
        /// </summary>
        /// <param name="StringToLookForValueIn">String Variable</param>
        /// <param name="ValueToCheckTheStringFor">Value to check inside the string</param>
        /// <param name="WhichComparison">Which comparison to use</param>
        /// <returns>Boolean if it contains that value</returns>
        /// <remarks>stringToCheckIn.Contains("ValueToCheckForInSide (stringToCheckIn)", StringComparison.OrdinalIgnoreCase);</remarks>
        public static bool Contains(this string StringToLookForValueIn, string ValueToCheckTheStringFor, StringComparison WhichComparison)
        {
            //Index Of -1 => Not Found
            //Index Of 0  => If str is empty or null

            //just return the result now because we have a legit string
            return StringToLookForValueIn.IndexOf(ValueToCheckTheStringFor, WhichComparison) >= 0;
        }

        /// <summary>
        /// Extension Method For A String To See If It Contains A String And Let The User Use String Comparison
        /// </summary>
        /// <param name="StringsToLookThrough">String List To Look For The Value In</param>
        /// <param name="ValueToCheckTheStringFor">Value to check inside the string</param>
        /// <param name="WhichComparison">Which comparison to use</param>
        /// <returns>Boolean if it contains that value</returns>
        /// <remarks>stringToCheckIn.Contains("ValueToCheckForInSide (stringToCheckIn)", StringComparison.OrdinalIgnoreCase);</remarks>
        public static bool Contains(this IEnumerable<string> StringsToLookThrough, string ValueToCheckTheStringFor, StringComparison WhichComparison)
        {
            //we could use an Any() call in linq but it was a tad bit slower. Since this is a hot path in most of my stuff going to leave it in the foreach loop
            foreach (string StringToTest in StringsToLookThrough)
            {
                //use the singlar method so we have code reuse
                if (StringToTest.Contains(ValueToCheckTheStringFor, WhichComparison))
                {
                    //we found a match, so return true
                    return true;
                }
            }

            //can't find the item
            return false;
        }

        #endregion

        #region String Is Null Or Empty - Instance

        /// <summary>
        /// Returns true if this string is neither null or empty
        /// </summary>
        /// <param name="StringToValidate">String to validate</param>
        /// <returns>Is the string has a value</returns>
        /// <remarks>Easier to write and flows more naturally then string.IsNullOrEmpty</remarks>
        public static bool HasValue(this string StringToValidate)
        {
            return !string.IsNullOrEmpty(StringToValidate);
        }

        /// <summary>
        /// Returns true if this string is either null or empty
        /// </summary>
        /// <param name="StringToValidate">String to validate</param>
        /// <returns>Is the string is either null or empty</returns>
        /// <remarks>Easier to write and flows more naturally then string.IsNullOrEmpty</remarks>
        public static bool IsNullOrEmpty(this string StringToValidate)
        {
            return string.IsNullOrEmpty(StringToValidate);
        }

        #endregion

        #region Format Phone Numbers

        /// <summary>
        /// Format a string to a USA Phone Number
        /// </summary>
        /// <param name="PhoneNumber">Phone Number To Format - Needs to be 10 characters otherwise will just return the number</param>
        /// <returns>Outputted with ( and -</returns>
        /// <remarks>Needs to use this instead of string.format because it can't handle a leading 0</remarks>
        public static string ToUSAPhoneNumber(this string PhoneNumber)
        {
            //make sure the phone is not null Or the length is 10 characters
            if (PhoneNumber.IsNullOrEmpty() || PhoneNumber.Length != 10 || !PhoneNumber.All(char.IsDigit))
            {
                //not 10 digits, just return whatever was passed in
                return PhoneNumber;
            }

            //we have the number formatted exactly with everything, return it
            return new StringBuilder()

                    //set the area code
                    .Append("(")
                    .Append(PhoneNumber.Substring(0, 3))
                    .Append(") ")

                    //now lets set the first 3 digits of the regular #
                    .Append(PhoneNumber.Substring(3, 3))

                    //add the dash
                    .Append("-")

                    //add the last 4
                    .Append(PhoneNumber.Substring(6, 4))

                    //return the formatted string
                    .ToString();
        }

        #endregion

        #region Format Zip Code

        /// <summary>
        /// Format a string to a USA Zip Code
        /// </summary>
        /// <param name="ZipCode">Zip Code - 9 characters</param>
        /// <returns>Outputted with - then the rest of the 4 extension zip #'s</returns>
        /// <remarks>Needs to use this instead of string.format because it can't handle a leading 0</remarks>
        public static string ToUSAZipCode(this string ZipCode)
        {
            //if the zip code is null then just return it right away
            if (ZipCode.IsNullOrEmpty())
            {
                //zip code is null / blank...just return the string that was passed in
                return ZipCode;
            }

            if (ZipCode.Length == 5)
            {
                //if its just the 5 character version, then return just the item passed in
                return ZipCode;
            }

            if (ZipCode.Length != 9)
            {
                //if the length is not 9 then return it...we need 9 characters
                return ZipCode;
            }

            //we have 9 characters, create the instance of the string builder becauase we need it (init the capacity to reduce memory just a tag)
            return new StringBuilder()

                //Add the first 5 characters
                .Append(ZipCode.Substring(0, 5))

                //add the dash
                .Append("-")

                //return the last 4 digits
                .Append(ZipCode.Substring(5, 4))

                //return the formatted zip code
                .ToString();
        }

        #endregion

        #region Validate E-mail Address

        /// <summary>
        /// Extension Of A String To Determine If This Is In A Valid Email Format
        /// </summary>
        /// <param name="EmailAddressToValidate">Email address to validate</param>
        /// <returns>Boolean | True = Is Valid Email Format, False = It Is Not</returns>
        public static bool IsValidEmailAddress(this string EmailAddressToValidate)
        {
            //the if statement version is much faster then mvc's [EmailAddress]. They all pass the same tests and it's simpler. (I profiled the 2 versions in benchmark dot net. Mine was faster by 30%)

            //this is the mvc regex version incase we ever want to use it. For now i'm sticking with what i have
            //var result = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.Compiled).Match(email to test);

            //needs to be not null
            if (EmailAddressToValidate.IsNullOrEmpty())
            {
                return false;
            }

            //let's throw the last index of the dot into a variable so we can re-use it
            int LastIndexOfDot = EmailAddressToValidate.LastIndexOf(".");

            // Needs a . (we could use a indexOf instead of last index, but we need last index anyway...so grab that value, check it against that and then re-use the variable)
            //this way we don't have to grab the index / last index of . multiple times
            if (LastIndexOfDot <= 0)
            {
                return false;
            }

            //make sure there is something after the . (last index) 
            //last index is a 0 based index...length is a 1 based index (putting this here incase it fails we don't need to go find the @ symbol - little faster)
            if (LastIndexOfDot == (EmailAddressToValidate.Length - 1))
            {
                return false;
            }

            //can't contain any spaces
            if (EmailAddressToValidate.Contains(" "))
            {
                return false;
            }

            //grab the index of @ so we can re-use it
            int IndexOfAtSymbol = EmailAddressToValidate.IndexOf("@");

            // Needs atleast an @ symbol (0 = string is empty or first character. -1 means it's not found)
            if (IndexOfAtSymbol <= 0)
            {
                return false;
            }

            // Needs the @ symbol before the .
            if (IndexOfAtSymbol > LastIndexOfDot)
            {
                return false;
            }

            //we made it past all the validation...it's a valid email..return true
            return true;
        }

        #endregion

        #region Replace Case In Sensitive

        /// <summary>
        /// Provides an extension method for string to run a replace with a case in-sensitive search
        /// </summary>
        /// <param name="TextToSearchIn">Text Variable To Search In</param>
        /// <param name="TextToSearchFor">Text To Search For. Phrase You Are Searching For</param>
        /// <param name="TextToReplaceWith">Text To Replace With.</param>
        /// <returns>String With The Removed Text. Or It Not Found Then The Orig Text - TextToSearchIn</returns>
        public static string ReplaceCaseInSensitiveString(this string TextToSearchIn, string TextToSearchFor, string TextToReplaceWith)
        {
            //go run the regular expression that is not case sensitive...then replace that text
            return new Regex(TextToSearchFor, RegexOptions.IgnoreCase).Replace(TextToSearchIn, TextToReplaceWith);
        }

        #endregion

        #region Right

        /// <summary>
        /// Grab x number of characters to the right of this string
        /// </summary>
        /// <param name="StringToGrabRightCharacters">String To Retrieve The Data For</param>
        /// <param name="NumberOfCharacters">Number Of Right Characters To Grab</param>
        /// <returns>Right Value String</returns>
        public static string Right(this string StringToGrabRightCharacters, int NumberOfCharacters)
        {
            //make sure the string you passed is not null and you want more then 0 characters
            if (StringToGrabRightCharacters.IsNullOrEmpty())
            {
                //string to check is null...just return whatever is passed in
                return StringToGrabRightCharacters;
            }

            if (NumberOfCharacters <= 0)
            {
                //raise an error if they pass in less than 0 characters to the right
                throw new ArgumentNullException("Number Of Characters Can't Be Less Than 0 In Right Extension Method");
            }

            //holds the start index to grab from the right
            //grab the start index...the length of the string - the number of characters. If the string isn't long enough then start Index will be a negative number
            int StartIndex = StringToGrabRightCharacters.Length - NumberOfCharacters;

            //check to see if we have a negative start index (more characters to grab then in StringToGrabRightCharacters)
            if (StartIndex > 0)
            {
                //start index is ok...we have enough characters
                return StringToGrabRightCharacters.Substring(StartIndex, NumberOfCharacters);
            }

            //Number of characters is too large for the length of StringToGrabRightCharacters, just return the string
            return StringToGrabRightCharacters;
        }

        #endregion

        #region Trim With Null Check

        /// <summary>
        /// Extension Method To Trim A String. Will return string.empty if the string is null. Regular Trim will bomb out
        /// </summary>
        /// <param name="StringToTrim">String To Trim</param>
        /// <returns>Trimmed String Value. If null an empty string will be returned</returns>
        public static string TrimHandleNull(this string StringToTrim)
        {
            //use the overload
            return TrimHandleNull(StringToTrim, string.Empty);
        }

        /// <summary>
        /// Extension Method To Trim A String. It will handle a string and return ReturnValueIfNull (parameter passed in). Regular Trim will bomb out
        /// </summary>
        /// <param name="StringToTrim">String To Trim</param>
        /// <param name="ReturnValueIfNull">The value to be returned if the string to trim is null</param>
        /// <returns>Trimmed String Value. If null ReturnValueIfNull will be returned</returns>
        public static string TrimHandleNull(this string StringToTrim, string ReturnValueIfNull)
        {
            //if the string is null return whatever the ReturnValueIfNull (which is passed in from the parameter)
            if (StringToTrim.IsNullOrEmpty())
            {
                //it's null, return the value passed in
                return ReturnValueIfNull;
            }

            //trim the string and return it
            return StringToTrim.Trim();
        }

        #endregion

        #region String To Byte Array - Ascii

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="StringToConvertToByteArray">String to convert</param>
        /// <returns>byte array</returns>
        /// <remarks>Used in the past, if i have a string which is sql, i convert it to a file (byte array), then send it for download on the web server without saving it to a temporary file</remarks>
        public static byte[] ToAsciiByteArray(this string StringToConvertToByteArray)
        {
            //convert it to ascii and return the bytes
            return Encoding.ASCII.GetBytes(StringToConvertToByteArray);
        }

        #endregion

        #region String To Byte Array (Utf-8)

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="StringToConvertToByteArray">String to convert</param>
        /// <returns>byte array</returns>
        /// <remarks>Used in the past, if i have a string which is sql, i convert it to a file (byte array), then send it for download on the web server without saving it to a temporary file</remarks>
        public static byte[] ToUtf8ByteArray(this string StringToConvertToByteArray)
        {
            //convert it to ascii and return the bytes
            return Encoding.UTF8.GetBytes(StringToConvertToByteArray);
        }

        #endregion

        #region Index Of All Items

        /// <summary>
        /// Looks through the string for the specific string to look for. Will return the index of for each item found
        /// </summary>
        /// <param name="StringToLookThrough">The string to look through for the specific characters</param>
        /// <param name="StringValueToLookFor">String value to find in the StringToLookThrough</param>
        /// <returns>List of all the index of the StringValueToLookFor.</returns>
        public static IEnumerable<int> IndexesOfAllLazy(this string StringToLookThrough, string StringValueToLookFor)
        {
            //make sure the string value is not null
            if (StringValueToLookFor.IsNullOrEmpty())
            {
                throw new ArgumentException("The string value is null", "StringValueToLookFor");
            }

            //return the local function
            return Iterator();

            //declare the iterator to run through the results
            IEnumerable<int> Iterator()
            {
                //working index that we found the item with
                int? WorkingIndex = null;

                //loop through the string until we are done
                while (WorkingIndex >= 0 || !WorkingIndex.HasValue)
                {
                    //if this is the first element search at 0
                    if (!WorkingIndex.HasValue)
                    {
                        //Set it to 0
                        WorkingIndex = 0;
                    }

                    //grab the index of for this value
                    WorkingIndex = StringToLookThrough.IndexOf(StringValueToLookFor, WorkingIndex.Value + 1);

                    //if we have a match the return it
                    if (WorkingIndex > 0)
                    {
                        //return this record
                        yield return WorkingIndex.Value;
                    }
                }
            }
        }

        #endregion

        #region Lazy String Split

        /// <summary>
        /// Splits a string in a lazy fashion. This way you don't need to allocate the string array if there will be a large number of elements. Saves on memory if you only need the last element or a specific element and not the entire dataset
        /// </summary>
        /// <param name="StringToSplit">String to split</param>
        /// <param name="Seperator">Character to split on</param>
        /// <returns>Iterator of strings that are split</returns>
        /// <remarks>Leaving this just a char parameter. If i have to peek at characters then i need to build a string using a string builder and parsing strings and this method looses it's performance value.</remarks>
        public static IEnumerable<string> SplitLazy(this string StringToSplit, char Seperator)
        {
            //method is good if you are splitting for a large amount of items. Or if you want to grab the first item where the first 2 characters are "yz". Then you can just run a FirstOrDefault and not allocated every single string[] 

            //start of the phrase
            int StartPhraseIndex = 0;

            //loop through all the characters
            for (int i = 0; i < StringToSplit.Length; i++)
            {
                //is this the character we are looking for?
                if (StringToSplit[i] == Seperator)
                {
                    //this is the character we are looking for...return everything from the working character to this character we are on
                    yield return StringToSplit.Substring(StartPhraseIndex, i - StartPhraseIndex);

                    //now we want to increment the start of the next phrase  (we increment by 1 so we can skip over the delimiter. i hasn't incremented yet
                    StartPhraseIndex = i + 1;
                }
            }

            //always return the last string "column0,column1,column2,column3,column4"
            //we need to return 4.
            yield return StringToSplit.Substring(StartPhraseIndex, StringToSplit.Length - StartPhraseIndex);
        }

        #endregion

        #region Surround With

        /// <summary>
        /// Surround a string with quotes
        /// </summary>
        /// <param name="StringToQuote">String to put quotes around</param>
        /// <returns>String with the value at the beg and end of it</returns>
        public static string SurroundWithQuotes(this string StringToQuote)
        {
            //use the overload and pass in quotes
            return StringToQuote.SurroundWith("\"");
        }

        /// <summary>
        /// Surround a string with a specific character at the front and back
        /// </summary>
        /// <param name="StringToQuote">String to put characters around it. Front and Back of the string</param>
        /// <param name="StringToAddAtBegAndEnd">String value to add at the beg and end of the passed in string</param>
        /// <returns>String with the value at the beg and end of it</returns>
        public static string SurroundWith(this string StringToQuote, string StringToAddAtBegAndEnd)
        {
            //benchmarkdot net showed that a string builder or ${}... was slower and used more memory then just appending 3 items. 
            //i assume since its only 3 string that is the reason
            //string builder to build it up
            return StringToAddAtBegAndEnd + StringToQuote + StringToAddAtBegAndEnd;
        }

        #endregion

        #region To Stream

        /// <summary>
        /// Write a string into a stream
        /// </summary>
        /// <param name="StringToWriteIntoAStream">String to write into a stream</param>
        /// <returns>Stream. Be Sure to Dispose of it</returns>
        public static MemoryStream ToStream(this string StringToWriteIntoAStream)
        {
            //can't dispose of anything otherwise you won't be able to read it...The calling method needs to make sure they dispose of the stream

            //create the memory stream
            var MemoryStreamToUse = new MemoryStream();

            //create the writer
            var WriterToUse = new StreamWriter(MemoryStreamToUse);

            //write the string data
            WriterToUse.Write(StringToWriteIntoAStream);

            //flush it out
            WriterToUse.Flush();

            //set the position to the beg of the stream
            MemoryStreamToUse.Position = 0;

            //go run the test method
            return MemoryStreamToUse;
        }

        #endregion

        #region Base 64 Encoding

        /// <summary>
        /// Convert a string to a base 64 encoded string. Can be used for basic authentication
        /// </summary>
        /// <param name="StringToEncode">string to convert to base 64 encoded</param>
        /// <returns>Encoded base 64 string</returns>
        public static string ToBase64Encode(this string StringToEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(StringToEncode));
        }

        /// <summary>
        /// Convert a base 64 encoded string back to a regular string value
        /// </summary>
        /// <param name="StringToDecode">string to decode from base 64 encoded</param>
        /// <returns>Encoded base 64 string</returns>
        public static string ToBase64Decode(this string StringToEncode)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(StringToEncode));
        }

        #endregion

        #region Remove Spaces

        /// <summary>
        /// Remove all spaces for the given string
        /// </summary>
        /// <param name="StringToRemoveSpacesFrom">String to remove all spaces from</param>
        /// <returns>String with reomved spaces</returns>
        public static string RemoveSpaces(this string StringToRemoveSpacesFrom)
        {
            return Regex.Replace(StringToRemoveSpacesFrom, @"\s+", string.Empty);
        }

        #endregion

    }

}
