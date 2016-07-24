using System;
using ToracLibrary.Core.Countries;
using ToracLibrary.Core.Xml.Schema;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to test the country listing
    /// </summary>
    public class CountryTest
    {

        /// <summary>
        /// Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        /// </summary>
        [Fact]
        public void ValidateXmlAgainstSchemaTest1()
        {
            //go run the validation
            Assert.True(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(Country.CountryXmlResource(), Country.CountryXmlSchemaResource()));
        }

        /// <summary>
        /// Test builds the country listing and verifies everything is correct and running
        /// </summary>
        [Fact]
        public void CountrySelectTest1()
        {
            //grab the country list
            var CountryListing = Country.CountryListing();

            //check the total country count
            Assert.Equal(244, CountryListing.Count);

            //let's just test a random country
            Assert.Equal(1, CountryListing[1].CountryID);
            Assert.Equal("Andorra", CountryListing[1].ShortCountryName);
            Assert.Equal("The Principality Of Andorra", CountryListing[1].LongCountryName);

            Assert.Equal("AD", CountryListing[1].ISO2CharCode);
            Assert.Equal("AN", CountryListing[1].IRS2CharCode);
            Assert.Equal("AND", CountryListing[1].ISO3CharCode);
            Assert.Equal(20, CountryListing[1].ISO3DigitCode);
        }

    }

}