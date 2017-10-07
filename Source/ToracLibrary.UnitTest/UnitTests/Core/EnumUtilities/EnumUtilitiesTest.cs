using System;
using ToracLibrary.Core.EnumUtilities;
using System.Linq;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to
    /// </summary>
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
        public enum TestEnum : int
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
        [Fact]
        public void EnumGetValuesTest1()
        {
            //grab the values
            var EnumValuesToTest = EnumUtility.GetValuesLazy<TestEnum>().ToArray();

            //make sure we have the number of enum values
            Assert.Equal(4, EnumValuesToTest.Length);

            //make sure we have the 3 enum values now
            Assert.Contains(EnumValuesToTest, x => x == TestEnum.City);
            Assert.Contains(EnumValuesToTest, x => x == TestEnum.State);
            Assert.Contains(EnumValuesToTest, x => x == TestEnum.Country);
            Assert.Contains(EnumValuesToTest, x => x == TestEnum.Planet);
        }

        #endregion

        #region Try Parse

        /// <summary>
        /// Try Parse To Nullable
        /// </summary>
        [InlineData("State", TestEnum.State)]
        [Theory]
        public void TryParseToNullableTest1(string ValueToTest, TestEnum ShouldBeEnumValue)
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>(ValueToTest);

            //make sure we have a value
            Assert.True(ResultOfParse.HasValue);

            //go test the value now
            Assert.Equal(ShouldBeEnumValue, ResultOfParse.Value);
        }

        /// <summary>
        /// Try Parse To Nullable. This will be a negative test that it can't parse
        /// </summary>
        [Fact]
        public void TryParseToNullableTest2()
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>("State123");

            //make sure we have a value
            Assert.False(ResultOfParse.HasValue);
        }

        /// <summary>
        /// Try Parse To Nullable. Ensure case insensitive works
        /// </summary>
        [InlineData(TestEnum.State)]
        [Theory]
        public void TryParseToNullableTest3(TestEnum ShouldBeEnumValue)
        {
            //try to parse state
            var ResultOfParse = EnumUtility.TryParseToNullable<TestEnum>("state", true);

            //make sure we have a value
            Assert.True(ResultOfParse.HasValue);

            //go test the value now
            Assert.Equal(ShouldBeEnumValue, ResultOfParse.Value);
        }

        #endregion

        #region Custom Attribute

        /// <summary>
        /// Test getting a custom attribute off of an enum
        /// </summary>
        [Fact]
        public void GetCustomAttributeFromEnumTest1()
        {
            //check the custom attribute values
            Assert.Equal(CityDescriptionAttributeText, EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.City).Description);
            Assert.Equal(CountryDescriptionAttributeText, EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.Country).Description);

            //make sure this value is null
            Assert.Null(EnumUtility.CustomAttributeGet<DescriptionTextAttribute>(TestEnum.State));
        }

        /// <summary>
        /// Test if the custom attribute exists on an enum value
        /// </summary>
        [Fact]
        public void CustomAttributeExistsFromEnumTest1()
        {
            //check the custom attribute is defined
            Assert.True(EnumUtility.HasCustomAttributeDefined<DescriptionTextAttribute>(TestEnum.City));
            Assert.True(EnumUtility.HasCustomAttributeDefined<DescriptionTextAttribute>(TestEnum.Country));

            //make sure the custom attribute is not defined on this enum
            Assert.False(EnumUtility.HasCustomAttributeDefined<DescriptionTextAttribute>(TestEnum.State));
            Assert.False(EnumUtility.HasCustomAttributeDefined<DescriptionTextAttribute>(TestEnum.Planet));
        }

        #endregion

        #region Bit Mask

        /// <summary>
        /// Test the bit mask functionality
        /// </summary>
        [Fact]
        public void BitMaskTest1()
        {
            //start enum value
            var WorkingBitMaskValue = TestEnum.City;

            //check what values we have in the bit mask
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //now add state
            WorkingBitMaskValue = EnumUtility.BitMaskAddItem(WorkingBitMaskValue, TestEnum.State);

            //make sure we have the correct values
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //now add Country
            WorkingBitMaskValue = EnumUtility.BitMaskAddItem(WorkingBitMaskValue, TestEnum.Country);

            //make sure we have the correct values
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //let's try the multiple add items
            var MultipleRangeAdd = EnumUtility.BitMaskAddItem(TestEnum.City, TestEnum.Country, TestEnum.State);

            //make sure we have the correct values
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.City));
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.State));
            Assert.True(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Country));
            Assert.False(EnumUtility.BitMaskContainsValue(WorkingBitMaskValue, TestEnum.Planet));

            //let's test the multiple contains
            Assert.False(EnumUtility.BitMaskContainsValue(MultipleRangeAdd, TestEnum.Planet));
            Assert.True(EnumUtility.BitMaskContainsValue(MultipleRangeAdd, TestEnum.State));
        }

        #endregion

    }

}