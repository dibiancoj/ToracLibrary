using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using static ToracLibraryTest.Framework.FrameworkHelperMethods;

namespace ToracLibraryTest.UnitsTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to String Extension Methods
    /// </summary>
    [TestClass]
    public class StringExtensionTest
    {

        #region Contains

        #region Contains Off Of String

        /// <summary>
        /// Unit test string contains with sring comparison
        /// </summary>
        [TestMethod]
        public void StringContainsTest1()
        {
            //just test different combinations
            Assert.AreEqual(true, "Jason".Contains("JASON", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "JASON".Contains("JASON", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(true, "jason2".Contains("JASON", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(false, "test 123".Contains("JASON", StringComparison.OrdinalIgnoreCase));


            Assert.AreEqual(true, "JASON".Contains("JASON", StringComparison.Ordinal));
            Assert.AreEqual(false, "jason2".Contains("JASON", StringComparison.Ordinal));
        }

        #endregion

        #region Contains Off Of IEnumerable

        /// <summary>
        /// Unit test ienumerable of string contains with sring comparison
        /// </summary>
        [TestMethod]
        public void IEnumerableStringContainsTest1()
        {
            //create a dummy string enumerator to test
            IEnumerable<string> ListToContainsWith = new string[] { "jason1", "jason2", "jason3" };

            //make sure we can find some values
            Assert.AreEqual(true, ListToContainsWith.Contains("JASON", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(false, ListToContainsWith.Contains("JASONABC", StringComparison.OrdinalIgnoreCase));

            //check ordinal now
            Assert.AreEqual(false, ListToContainsWith.Contains("JASON", StringComparison.Ordinal));
            Assert.AreEqual(true, ListToContainsWith.Contains("jason1", StringComparison.Ordinal));
        }

        #endregion

        #endregion

        #region Format USA Phone Number

        /// <summary>
        /// Unit test for formatting a string to a usa phone number
        /// </summary>
        [TestMethod]
        public void FormatUSAPhoneNumberTest1()
        {
            //make sure its gives us a valid format when we have 10 characters
            Assert.AreEqual("(914) 555-2235", "9145552235".ToUSAPhoneNumber());

            //if we don't have 10 characters, make sure it gives us back what we passed in
            const string ShouldNotFormat = "914552";

            //make sure we get back whatever we passed in
            Assert.AreEqual(ShouldNotFormat, ShouldNotFormat.ToUSAPhoneNumber());
        }

        #endregion

        #region Format USA Zip Code

        /// <summary>
        /// Unit test for formatting a string to a usa zip code
        /// </summary>
        [TestMethod]
        public void FormatUSAZipCodeTest1()
        {
            //make sure values that don't have 10 characters return whatever was passed in
            Assert.AreEqual(string.Empty, string.Empty.ToUSAZipCode());

            //5 digit zip should return whatever we passed in
            const string FiveDigitZipAttempt = "10583";

            //try the 5 digit now
            Assert.AreEqual(FiveDigitZipAttempt, FiveDigitZipAttempt.ToUSAZipCode());

            //try 7 digit
            const string SevenDigitZipAttemt = "1058322";

            //try the 7 digit now
            Assert.AreEqual(SevenDigitZipAttemt, SevenDigitZipAttemt.ToUSAZipCode());

            //try a legit zip now
            Assert.AreEqual("10583-2233", "105832233".ToUSAZipCode());
        }

        #endregion

        #region Is Valid Email

        /// <summary>
        /// Test is a string is a valid email address
        /// </summary>
        [TestMethod]
        public void IsValidEmailTest1()
        {
            //just check random text

            //try negative emails addresses
            Assert.AreEqual(false, "jason".IsValidEmailAddress());
            Assert.AreEqual(false, "jason.com@".IsValidEmailAddress());
            Assert.AreEqual(false, "jason@".IsValidEmailAddress());
            Assert.AreEqual(false, "j ason@aol.com".IsValidEmailAddress());
            Assert.AreEqual(false, "jason.com@".IsValidEmailAddress());
            Assert.AreEqual(false, "jason@com.".IsValidEmailAddress());
            Assert.AreEqual(false, "@jasoncom.".IsValidEmailAddress());

            //try positive email addresses
            Assert.AreEqual(true, "jason@aol.com".IsValidEmailAddress());
            Assert.AreEqual(true, "JoeJ@gmail.com".IsValidEmailAddress());
        }

        #endregion

        #region Replace Case Sensitive

        /// <summary>
        /// Test the replacement of characters ignoring case
        /// </summary>
        [TestMethod]
        public void StringReplaceCaseSensitiveTest1()
        {
            //test string
            const string TestString = "jason";

            //check the result now
            Assert.AreEqual("j", TestString.ReplaceCaseInSensitiveString("ASON", string.Empty));
            Assert.AreEqual("jason", TestString.ReplaceCaseInSensitiveString("yay", string.Empty));
        }

        #endregion

        #region String Right

        /// <summary>
        /// Test the string right function
        /// </summary>
        [TestMethod]
        public void StringRightTest1()
        {
            //test string
            const string TestString = "jason";

            //check the values
            Assert.AreEqual("on", TestString.Right(2));
            Assert.AreEqual("ason", TestString.Right(4));
        }

        #endregion

        #region Trim Handle Null

        /// <summary>
        /// Check trim with null check with no replacement value
        /// </summary>
        [TestMethod]
        public void TrimHandleNullNoReplacementTest1()
        {
            //create our constants so we can test
            const string String1Test = " Jason Test 1 ";
            const string String2Test = "Jason Test 2";
            const string String3Test = "Jason Test 3 ";
            const string StringThatIsNull = null;

            //go run our test
            Assert.AreEqual(String1Test.Trim(), String1Test.TrimHandleNull());
            Assert.AreEqual(String2Test.Trim(), String2Test.TrimHandleNull());
            Assert.AreEqual(String3Test.Trim(), String3Test.TrimHandleNull());
            Assert.AreEqual(string.Empty, StringThatIsNull.TrimHandleNull());
            Assert.AreEqual(string.Empty, string.Empty.TrimHandleNull());
        }

        /// <summary>
        /// Check trim with null check with with replacement value
        /// </summary>
        [TestMethod]
        public void TrimHandleNullWithReplacementTest2()
        {
            //create our constants so we can test
            const string String1Test = " Jason Test 1 ";
            const string String2Test = "Jason Test 2";
            const string String3Test = "Jason Test 3 ";
            const string StringThatIsNull = null;

            //replacement text
            const string NullReplacement = "This Is Null";

            //go run our test
            Assert.AreEqual(String1Test.Trim(), String1Test.TrimHandleNull(NullReplacement));
            Assert.AreEqual(String2Test.Trim(), String2Test.TrimHandleNull(NullReplacement));
            Assert.AreEqual(String3Test.Trim(), String3Test.TrimHandleNull(NullReplacement));
            Assert.AreEqual(NullReplacement, string.Empty.TrimHandleNull(NullReplacement));
            Assert.AreEqual(NullReplacement, StringThatIsNull.TrimHandleNull(NullReplacement));
        }

        #endregion

        #region String To Byte Array

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [TestMethod]
        public void StringToByteArrayTest1()
        {
            //loop through the elements to test using the helper method for this
            UnitTestArrayElements(new byte[] { 106, 97, 115, 111, 110 }, "jason".ToByteArray());
        }

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [TestMethod]
        public void StringToByteArrayTest2()
        {
            //loop through the elements to test using the helper method for this
            UnitTestArrayElements(new byte[] { 106, 97, 115, 111, 110, 50 }, "jason2".ToByteArray());
        }

        #endregion

    }

}