using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.EnumUtilities;
using System.Linq;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to
    /// </summary>
    [TestClass]
    public class EnumUtilityTest
    {

        #region Framework

        #region Constants

        /// <summary>
        /// Text used for the description attribute
        /// </summary>
        private const string CityDescriptionAttributeText = "City Text";

        /// <summary>
        /// Text used for the description attribute
        /// </summary>
        private const string CountryDescriptionAttributeText = "Country Text";

        #endregion

        #region Test Enums

        /// <summary>
        /// Test Enum
        /// </summary>
        [Flags]
        private enum TestEnum : int
        {

            [DescriptionText(CityDescriptionAttributeText)]
            City = 0,

            /// <summary>
            /// No description text to make sure the custom attribute test returns null
            /// </summary>
            State = 1,

            [DescriptionText(CountryDescriptionAttributeText)]
            Country = 2,

            Planet = 4
        }

        #endregion

        #region Test Attribute

        private class DescriptionTextAttribute : Attribute
        {
            public DescriptionTextAttribute(string DescriptionText)
            {
                Description = DescriptionText;
            }

            public string Description { get; }
        }

        #endregion

        #endregion

        #region Get Values

        /// <summary>
        /// Enum get values
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void EnumGetValuesTest1()
        {
            //grab the values
            var EnumValuesToTest = EnumUtility.GetValuesLazy<TestEnum>().ToArray();

            //make sure we have the number of enum values
            Assert.AreEqual(4, EnumValuesToTest.Length);

            //make sure we have the 3 enum values now
            Assert.IsTrue(EnumValuesToTest.Any(x => x == TestEnum.City));
            Assert.IsTrue(EnumValuesToTest.Any(x => x == TestEnum.State));
            Assert.IsTrue(EnumValuesToTest.Any(x => x == TestEnum.Country));
            Assert.IsTrue(EnumValuesToTest.Any(x => x == TestEnum.Planet));
        }

        #endregion

        #region Try Parse

        /// <summary>
        /// Try Parse To Nullable
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void TryParseToNullableTest1()
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>("State");

            //make sure we have a value
            Assert.IsTrue(ResultOfParse.HasValue);

            //go test the value now
            Assert.AreEqual(TestEnum.State, ResultOfParse.Value);
        }

        /// <summary>
        /// Try Parse To Nullable. This will be a negative test that it can't parse
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void TryParseToNullableTest2()
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>("State123");

            //make sure we have a value
            Assert.IsFalse(ResultOfParse.HasValue);
        }

        /// <summary>
        /// Try Parse To Nullable. Ensure case insensitive works
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void TryParseToNullableTest3()
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>("state", true);

            //make sure we have a value
            Assert.IsTrue(ResultOfParse.HasValue);

            //go test the value now
            Assert.AreEqual(TestEnum.State, ResultOfParse.Value);
        }

        #endregion

        #region Custom Attribute

        /// <summary>
        /// Test getting a custom attribute off of an enum
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void GetCustomAttributeFromEnumTest1()
        {
            //check the custom attribute values
            Assert.AreEqual(CityDescriptionAttributeText, EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.City).Description);
            Assert.AreEqual(CountryDescriptionAttributeText, EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.Country).Description);

            //make sure this value is null
            Assert.IsNull(EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.State));
        }

        #endregion

        #region Bit Mask

        /// <summary>
        /// Test the bit mask functionality
        /// </summary>
        [TestCategory("Core.EnumUtilities")]
        [TestCategory("Core")]
        [TestMethod]
        public void BitMaskTest1()
        {
            //start enum value
            var WorkingBitMaskValue = TestEnum.City;

            //check what values we have in the bit mask
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //now add state
            WorkingBitMaskValue = EnumUtility.BitMaskAddItem(WorkingBitMaskValue, TestEnum.State);

            //make sure we have the correct values
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //now add Country
            WorkingBitMaskValue = EnumUtility.BitMaskAddItem(WorkingBitMaskValue, TestEnum.Country);

            //make sure we have the correct values
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //let's try the multiple add items
            var MultipleRangeAdd = EnumUtility.BitMaskAddItem(TestEnum.City, TestEnum.Country, TestEnum.State);

            //make sure we have the correct values
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //let's test the multiple contains
            Assert.IsFalse(EnumUtility.BitMaskContainsValue(MultipleRangeAdd, TestEnum.Planet));
            Assert.IsTrue(EnumUtility.BitMaskContainsValue(MultipleRangeAdd, TestEnum.State));
        }

        #endregion

    }

}