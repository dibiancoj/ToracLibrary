using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.Countries;
using ToracLibrary.Core.Xml.Schema;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test the country listing
    /// </summary>
    [TestClass]
    public class CountryTest
    {

        #region Main Tests

        /// <summary>
        /// Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        /// </summary>
        [TestMethod]
        public void ValidateXmlAgainstSchemaTest1()
        {
            try
            {
                //go run the validation
                Assert.AreEqual(true, XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(Country.CountryXmlResource(), Country.CountryXmlSchemaResource()));
            }
            catch (Exception)
            {
                //failed, fail the unit test now
                Assert.Fail("Country Xml Doesn't Meet Schema Validation");
            }
        }

        /// <summary>
        /// Test builds the country listing and verifies everything is correct and running
        /// </summary>
        [TestMethod]
        public void CountrySelectTest1()
        {
            //grab the country list
            var countryListing = Country.CountryListing();

            //check the total country count
            Assert.AreEqual(244, countryListing.Count);

            //let's just test a random country
            Assert.AreEqual(1, countryListing[1].CountryID);
            Assert.AreEqual("Andorra", countryListing[1].ShortCountryName);
            Assert.AreEqual("The Principality Of Andorra", countryListing[1].LongCountryName);

            Assert.AreEqual("AD", countryListing[1].ISO2CharCode);
            Assert.AreEqual("AN", countryListing[1].IRS2CharCode);
            Assert.AreEqual("AND", countryListing[1].ISO3CharCode);
            Assert.AreEqual(20, countryListing[1].ISO3DigitCode);
        }

        #endregion

    }

}