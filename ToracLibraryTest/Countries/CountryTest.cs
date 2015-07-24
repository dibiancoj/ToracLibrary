using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Countries;

namespace ToracLibraryTest.CountryTest
{

    /// <summary>
    /// Unit test to test the country listing
    /// </summary>
    [TestClass]
    public class CountryTest
    {

        #region Main Tests

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