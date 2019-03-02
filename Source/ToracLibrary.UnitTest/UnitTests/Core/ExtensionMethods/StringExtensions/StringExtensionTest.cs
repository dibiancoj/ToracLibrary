using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using Xunit;
using static ToracLibrary.Core.ExtensionMethods.StringExtensions.StringExtensionMethods;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to String Extension Methods
    /// </summary>
    public class StringExtensionTest
    {

        #region Contains

        #region Contains Off Of String

        /// <summary>
        /// Unit test string contains with string comparison (true result)
        /// </summary>
        [InlineData("Jason", "JASON")]
        [InlineData("JASON", "JASON")]
        [InlineData("jason2", "JASON")]
        [Theory]
        public void StringContainsTrueTest1(string ValueToTest, string ContainsValueToTest)
        {
            Assert.True(ValueToTest.Contains(ContainsValueToTest, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Unit test string contains with string comparison (false result)
        /// </summary>
        [InlineData("test 123", "JASON")]
        [InlineData("123", "Bla123")]
        [Theory]
        public void StringContainsFalseTest1(string ValueToTest, string ContainsValueToTest)
        {
            Assert.False(ValueToTest.Contains(ContainsValueToTest, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Contains Off Of IEnumerable

        /// <summary>
        /// Unit test ienumerable of string contains with string comparison
        /// </summary>
        [Fact]
        public void IEnumerableStringContainsTest1()
        {
            //create a dummy string enumerator to test
            var ListToContainsWith = new string[] { "jason1", "jason2", "jason3" };

            //make sure we can find some values
            Assert.True(ListToContainsWith.Contains("JASON", StringComparison.OrdinalIgnoreCase));
            Assert.False(ListToContainsWith.Contains("JASONABC", StringComparison.OrdinalIgnoreCase));

            //check ordinal now
            Assert.False(ListToContainsWith.Contains("JASON", StringComparison.Ordinal));
            Assert.True(ListToContainsWith.Contains("jason1", StringComparison.Ordinal));
        }

        #endregion

        #endregion

        #region String Is Null Or Empty - Instance

        /// <summary>
        /// Test if a string has a value in a string instance extension method (true result)
        /// </summary>
        [InlineData("Test")]
        [InlineData("123")]
        [InlineData("12345")]
        [Theory]
        public void HasValueTrueResultTest1(string ValueToTest)
        {
            Assert.True(ValueToTest.HasValue());
        }

        /// <summary>
        /// Test if a string has a value in a string instance extension method (false result)
        /// </summary>
        [InlineData("")]
        [InlineData((string)null)]
        [Theory]
        public void HasValueFalseResultTest1(string ValueToTest)
        {
            Assert.False(ValueToTest.HasValue());
        }

        /// <summary>
        /// Test is a string is null or empty in a string instance extension method (not null result)
        /// </summary>
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("123 456")]
        [InlineData("a")]
        [Theory]
        public void NullOrEmptyNotNullTestTest1(string ValueToTest)
        {
            Assert.False(ValueToTest.IsNullOrEmpty());
        }

        /// <summary>
        /// Test is a string is null or empty in a string instance extension method  (null result)
        /// </summary>
        [InlineData("")]
        [InlineData((string)null)]
        [Theory]
        public void NullOrEmptyNullTestTest1(string ValueToTest)
        {
            Assert.True(ValueToTest.IsNullOrEmpty());
        }

        #endregion

        #region Format USA Phone Number

        /// <summary>
        /// Unit test for formatting a string to a usa phone number
        /// </summary>
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("9145552235", "(914) 555-2235")]
        [InlineData("914552", "914552")]
        [InlineData("914555)235", "914555)235")]
        [InlineData("(914) 555-7777", "(914) 555-7777")]
        [InlineData("(914)5557777", "(914) 555-7777")]
        [Theory]
        public void FormatUSAPhoneNumberTest1(string ValueToTest, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, ValueToTest.ToUSAPhoneNumber());
        }

        #endregion

        #region Format USA Zip Code

        /// <summary>
        /// Unit test for formatting a string to a usa zip code
        /// </summary>
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("10583", "10583")]
        [InlineData("1058322", "1058322")]
        [InlineData("105832233", "10583-2233")]
        [InlineData("12345678-", "12345678-")] //not valid should return whatever we passed in
        [Theory]
        public void FormatUSAZipCodeTest1(string ValueToTest, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, ValueToTest.ToUSAZipCode());
        }

        #endregion

        #region Is Valid Email

        /// <summary>
        /// Test is a string is a valid email address (this test should be for invalid email address)
        /// </summary>
        [InlineData("jason")]
        [InlineData("jason.com@")]
        [InlineData("jason@")]
        [InlineData("j ason@aol.com")]
        [InlineData("jason@com.")]
        [InlineData("@jasoncom.")]
        [Theory]
        public void IsValidEmailTestNegativeResultTest1(string EmailValueToTest)
        {
            //should be invalid email adddress
            Assert.False(EmailValueToTest.IsValidEmailAddress());
        }

        /// <summary>
        /// Test is a string is a valid email address (this test should be for correct email address)
        /// </summary>
        [InlineData("jason@aol.com")]
        [InlineData("JoeJ@gmail.com")]
        [InlineData("jason@my.torac.com")] //sub domain test
        [Theory]
        public void IsValidEmailTestPostiveResultTest1(string EmailValueToTest)
        {
            //should be valid email adddress
            Assert.True(EmailValueToTest.IsValidEmailAddress());
        }

        #endregion

        #region Replace Case Sensitive

        /// <summary>
        /// Test the replacement of characters ignoring case
        /// </summary>
        [InlineData("jason", "ASON", "j")]
        [InlineData("jason", "yay", "jason")]
        [Theory]
        public void StringReplaceCaseSensitiveTest1(string TestString, string TextToReplace, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, TestString.ReplaceCaseInSensitiveString(TextToReplace, string.Empty));
        }

        #endregion

        #region String Right

        /// <summary>
        /// Test the string right function
        /// </summary>
        [Fact]
        public void StringRightTest1()
        {
            //test string
            const string TestString = "jason";

            //check the values
            Assert.Equal("on", TestString.Right(2));
            Assert.Equal("ason", TestString.Right(4));
        }

        #endregion

        #region Trim Handle Null

        /// <summary>
        /// Check trim with null check with no replacement value
        /// </summary>
        [Fact]
        public void TrimHandleNullNoReplacementTest1()
        {
            //create our constants so we can test
            const string String1Test = " Jason Test 1 ";
            const string String2Test = "Jason Test 2";
            const string String3Test = "Jason Test 3 ";
            const string StringThatNull = null;

            //go run our test
            Assert.Equal(String1Test.Trim(), String1Test.TrimHandleNull());
            Assert.Equal(String2Test.Trim(), String2Test.TrimHandleNull());
            Assert.Equal(String3Test.Trim(), String3Test.TrimHandleNull());
            Assert.Equal(string.Empty, StringThatNull.TrimHandleNull());
            Assert.Equal(string.Empty, string.Empty.TrimHandleNull());
        }

        /// <summary>
        /// Check trim with null check with with replacement value
        /// </summary>
        [Fact]
        public void TrimHandleNullWithReplacementTest2()
        {
            //create our constants so we can test
            const string String1Test = " Jason Test 1 ";
            const string String2Test = "Jason Test 2";
            const string String3Test = "Jason Test 3 ";
            const string StringThatNull = null;

            //replacement text
            const string NullReplacement = "This Is Null";

            //go run our test
            Assert.Equal(String1Test.Trim(), String1Test.TrimHandleNull(NullReplacement));
            Assert.Equal(String2Test.Trim(), String2Test.TrimHandleNull(NullReplacement));
            Assert.Equal(String3Test.Trim(), String3Test.TrimHandleNull(NullReplacement));
            Assert.Equal(NullReplacement, string.Empty.TrimHandleNull(NullReplacement));
            Assert.Equal(NullReplacement, StringThatNull.TrimHandleNull(NullReplacement));
        }

        #endregion

        #region String To Byte Array - Ascii

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [Fact]
        public void StringToByteArrayAsciiTest1()
        {
            //loop through the elements to test using the helper method for this
            Assert.Equal(new byte[] { 106, 97, 115, 111, 110 }, "jason".ToAsciiByteArray());
        }

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [Fact]
        public void StringToByteArrayAsciiTest2()
        {
            //loop through the elements to test using the helper method for this
            Assert.Equal(new byte[] { 106, 97, 115, 111, 110, 50 }, "jason2".ToAsciiByteArray());
        }

        #endregion

        #region String To Byte Array - Utf-8

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [Fact]
        public void StringToByteArrayUtf8Test1()
        {
            //loop through the elements to test using the helper method for this
            Assert.Equal(new byte[] { 106, 97, 115, 111, 110 }, "jason".ToUtf8ByteArray());
        }

        /// <summary>
        /// Test to make sure a string to a byte array is correct
        /// </summary>
        [Fact]
        public void StringToByteArrayUtf8Test2()
        {
            //loop through the elements to test using the helper method for this
            Assert.Equal(new byte[] { 106, 97, 115, 111, 110, 50 }, "jason2".ToUtf8ByteArray());
        }

        #endregion

        #region Indexes Of All Lazy

        /// <summary>
        /// Test to make sure the index of all returns the correct value
        /// </summary>
        [Fact]
        public void IndexesOfAllLazyTest1()
        {
            //test string to look through
            string TestLookThroughString = "<html><img src='relative/test.jpg' /><img src='www.google.com' /></html>";

            //go run the method
            var Results = TestLookThroughString.IndexesOfAllLazy("src='", StringComparison.OrdinalIgnoreCase);

            //test this value
            Assert.Equal(2, Results.Count());

            //test the indexes
            Assert.Contains(Results, x => x == 11);
            Assert.Contains(Results, x => x == 42);
        }

        [Fact]
        public void IndexesOfAllLazyTestWithNoMatches()
        {
            //test string to look through
            string TestLookThroughString = "Test 123";

            //go run the method
            Assert.Empty(TestLookThroughString.IndexesOfAllLazy("Fact", StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region String Split Lazy

        /// <summary>
        /// Test to make sure the string split lazy works
        /// </summary>
        [Fact]
        public void StringSplitLazyTest1()
        {
            //test string to look through
            string StringToTest = "column0,column1,column2,column3,column4";

            //delimiter to test with
            const char Delimiter = ',';

            //we are going to use string.split to check our results against
            string[] StringSplitExpected = StringToTest.Split(Delimiter);

            //go run the method
            var Results = StringToTest.SplitLazy(Delimiter).ToArray();

            //test to make sure we have the correct count
            Assert.Equal(StringSplitExpected.Length, Results.Length);

            //loop through each of the elements returned
            for (int i = 0; i < StringSplitExpected.Length; i++)
            {
                //test the value
                Assert.Equal(StringSplitExpected[i], Results[i]);
            }
        }

        #endregion

        #region Surround With

        /// <summary>
        /// Test to make sure the SurroundWith works
        /// </summary>
        [InlineData("Jason", "?Jason?", "?")]
        [InlineData("Test", "!Test!", "!")]
        [Theory]
        public void SurroundWithTest1(string ValueToTest, string ShouldBeValue, string ValueToSurroundWith)
        {
            Assert.Equal(ShouldBeValue, ValueToTest.SurroundWith(ValueToSurroundWith));
        }

        /// <summary>
        /// Test to make sure the SurroundWithQuotes works
        /// </summary>
        [InlineData("Jason", @"""Jason""")]
        [InlineData("Test", @"""Test""")]
        [Theory]
        public void SurroundWithQuotesTest1(string ValueToTest, string ShouldBeValue)
        {
            Assert.Equal(ShouldBeValue, ValueToTest.SurroundWithQuotes());
        }

        #endregion

        #region To Base 64 Decode

        /// <summary>
        /// Test to make sure the Base 64 Encoding Works works
        /// </summary>
        [InlineData("Test12345hts")]
        [InlineData("456vsdfidsajf sdfds")]
        [Theory]
        public void Base64Encoded(string ValueToTest)
        {
            Assert.Equal(ValueToTest, ValueToTest.ToBase64Encode().ToBase64Decode());
        }

        #endregion

        #region Remove Spaces

        /// <summary>
        /// Test to make sure the spaces are removed
        /// </summary>
        [InlineData("Test123", "Test123")]
        [InlineData("Test 1 2 3 4 5", "Test12345")]
        [InlineData("Test 1 2 345", "Test12345")]
        [Theory]
        public void RemoveSpaces(string ValueToTest, string ExpectedValue)
        {
            Assert.Equal(ExpectedValue, ValueToTest.RemoveSpaces());
        }

        #endregion

        #region Digits

        [InlineData("", 0)]
        [InlineData("abc", 0)]
        [InlineData("1abc", 1)]
        [InlineData("1abc2", 2)]
        [InlineData("(914)-552-2205", 10)]
        [InlineData("123456789", 9)]
        [Theory]
        public void HowManyDigitsInString(string stringToTest, int expectedInstancesOfADigit)
        {
            Assert.Equal(expectedInstancesOfADigit, stringToTest.NumberOfDigitsInTheString());
        }

        [InlineData("", "")]
        [InlineData("abc", "")]
        [InlineData("1abc", "1")]
        [InlineData("1abc2", "12")]
        [InlineData("(914)-552-2205", "9145522205")]
        [InlineData("123456789", "123456789")]
        [Theory]
        public void PullDigitsFromString(string StringToTest, string ExpectedResult)
        {
            Assert.Equal(ExpectedResult, StringToTest.PullDigitsFromString());
        }

        #endregion

    }

}