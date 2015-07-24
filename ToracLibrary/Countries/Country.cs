using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToracLibrary.Xml.Schema;

namespace ToracLibrary.Countries
{
    /// <summary>
    /// Retrieve all the country codes
    /// </summary>
    public class Country
    {

        #region Public Methods

        /// <summary>
        /// Get a dictionary of countries.
        /// </summary>
        /// <returns>IImmutableDictionary of int (country id) - Country Code Info</returns>
        /// <remarks>Is validated before returning data. It will raise any errors if there were errors found</remarks>
        public static IImmutableDictionary<int, CountryCodeInfo> CountryListing()
        {
            //load the xml document
            var xDoc = XDocument.Parse(Properties.Resources.Countries);

            //**** Notes ****
            // This method is not faster with a parallized for loop. Tested in dual core...Not sure about a quad core.
            //**** End Of Notes ****

            //validate the xml first, make sure its in the proper format. It will raise an exception and return false if it fails
            if (XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(xDoc, XMLSchemaValidation.LoadSchemaFromText(Properties.Resources.CountriesSchema, null)))
            {
                //no schema errors...continue to query the xml..first create the dictionary
                var ReturnObject = new Dictionary<int, CountryCodeInfo>();

                //Loop Through The XML To Load The Dictionary
                foreach (XElement thisNode in xDoc.Element("Countries").Elements("Country"))
                {
                    //create the new country. Using a variable so we can re-use the country id below when we insert it into the dictionary
                    var CountryToAdd = new CountryCodeInfo(Convert.ToInt32(thisNode.Attribute("id").Value),
                                                            thisNode.Attribute("shortname").Value,
                                                            thisNode.Attribute("longname").Value,
                                                            thisNode.Attribute("iso2").Value,
                                                            thisNode.Attribute("irs2").Value,
                                                            thisNode.Attribute("iso3char").Value,
                                                            Convert.ToInt32(thisNode.Attribute("iso3digit").Value));

                    //add the country value
                    ReturnObject.Add(CountryToAdd.CountryID, CountryToAdd);
                }

                //return the dictionary
                return ReturnObject.ToImmutableDictionary();
            }

            //if we get here then we have some sort of error
            throw new Exception("CountryListing Failed");
        }

        #endregion

    }

}
