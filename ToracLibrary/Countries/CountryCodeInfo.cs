using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Countries
{

    /// <summary>
    /// Holds the object for each of the country codes
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class CountryCodeInfo
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CountryIDToSet">Country ID</param>
        /// <param name="ShortCountryNameToSet">Short Country Name</param>
        /// <param name="LongCountryNameToSet">Long Country Name</param>
        /// <param name="ISO2CharCodeToSet">ISO 2 Character Code</param>
        /// <param name="IRS2CharCodeToSet">IRS 2 Character Code</param>
        /// <param name="ISO3CharCodeToSet">ISO 3 Character Code</param>
        /// <param name="ISO3DigitCodeToSet">ISO 3 Digit Code</param>
        public CountryCodeInfo(int CountryIDToSet,
                               string ShortCountryNameToSet,
                               string LongCountryNameToSet,
                               string ISO2CharCodeToSet,
                               string IRS2CharCodeToSet,
                               string ISO3CharCodeToSet,
                               int ISO3DigitCodeToSet)
        {
            //set all the properties
            CountryID = CountryIDToSet;
            ShortCountryName = ShortCountryNameToSet;
            LongCountryName = LongCountryNameToSet;
            ISO2CharCode = ISO2CharCodeToSet;
            IRS2CharCode = IRS2CharCodeToSet;
            ISO3CharCode = ISO3CharCodeToSet;
            ISO3DigitCode = ISO3DigitCodeToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Country ID (Primary Key In Table)
        /// </summary>
        public int CountryID { get; }

        /// <summary>
        /// Short Country Name
        /// </summary>
        public string ShortCountryName { get; }

        /// <summary>
        /// Long Country Name
        /// </summary>
        public string LongCountryName { get; }

        /// <summary>
        /// ISO 2 Character Country Code
        /// </summary>
        public string ISO2CharCode { get; }

        /// <summary>
        /// IRS 2 Character Country Code
        /// </summary>
        public string IRS2CharCode { get; }

        /// <summary>
        /// ISO 3 Character Country Code
        /// </summary>
        public string ISO3CharCode { get; }

        /// <summary>
        /// ISO 3 Digit Country Code
        /// </summary>
        public int ISO3DigitCode { get; }

        #endregion

    }

}
