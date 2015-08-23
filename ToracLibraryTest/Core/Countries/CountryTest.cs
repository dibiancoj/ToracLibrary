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
        [TestCategory("Core.Countries")]
        [TestCategory("Core.Xml.Schema")]
        [TestCategory("Core")]
        [TestMethod]
        public void ValidateXmlAgainstSchemaTest1()
        {
            try
            {
                //go run the validation
                Assert.IsTrue(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(Country.CountryXmlResource(), Country.CountryXmlSchemaResource()));
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
        [TestCategory("Core.Countries")]
        [TestCategory("Core")]
        [TestMethod]
        public void CountrySelectTest1()
        {
            //grab the country list
            var CountryListing = Country.CountryListing();

            //check the total country count
            Assert.AreEqual(244, CountryListing.Count);

            //let's just test a random country
            Assert.AreEqual(1, CountryListing[1].CountryID);
            Assert.AreEqual("Andorra", CountryListing[1].ShortCountryName);
            Assert.AreEqual("The Principality Of Andorra", CountryListing[1].LongCountryName);

            Assert.AreEqual("AD", CountryListing[1].ISO2CharCode);
            Assert.AreEqual("AN", CountryListing[1].IRS2CharCode);
            Assert.AreEqual("AND", CountryListing[1].ISO3CharCode);
            Assert.AreEqual(20, CountryListing[1].ISO3DigitCode);
        }

        #endregion

    }

}