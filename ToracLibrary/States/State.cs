using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToracLibrary.Xml.Schema;

namespace ToracLibrary.States
{
    /// <summary>
    /// Retrieve all the state
    /// </summary>
    public class State
    {

        #region Public Methods

        /// <summary>
        /// Get a listing of states along (Key Value Pair)
        /// </summary>
        /// <returns>IImmutableDictionary of string - string</returns>
        /// <remarks>Is validated before returning data. It will raise any errors if there were errors found</remarks>
        public static IImmutableDictionary<string, string> UnitedStatesStateListing()
        {
            //load the xml
            XDocument xDoc = XDocument.Parse(Properties.Resources.States);

            //validate the xml first, make sure its in the proper format. It will raise an exception and return false if it fails
            if (XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(xDoc, XMLSchemaValidation.LoadSchemaFromText(Properties.Resources.StatesSchema, null)))
            {
                //no schema errors...continue to query the xml..first create the dictionary
                var ReturnObject = new Dictionary<string, string>();

                //Loop Through The XML To Load The Dictionary
                foreach (XElement StateToAdd in xDoc.Element("States").Elements("State"))
                {
                    //add the state value
                    ReturnObject.Add(StateToAdd.Attribute("id").Value, StateToAdd.Attribute("txt").Value);
                }

                //return the dictionary
                return ReturnObject.ToImmutableDictionary();
            }

            //if we get here then we have some sort of error
            throw new Exception("UnitedStatesStateListing Failed");
        }

        /// <summary>
        /// Get a listing of provinces (Canada) (Key Value Pair)
        /// </summary>
        /// <returns>IImmutableDictionary of string - string</returns>
        /// <remarks>Is validated before returning data. It will raise any errors if there were errors found</remarks>
        public static IImmutableDictionary<string, string> CanadaProvincesListing()
        {
            //load the xml to the xDoc
            XDocument xDoc = XDocument.Parse(Properties.Resources.Provinces);

            //validate the xml first, make sure its in the proper format. It will raise an exception and return false if it fails
            if (XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(xDoc, XMLSchemaValidation.LoadSchemaFromText(Properties.Resources.ProvincesSchema, null)))
            {
                //no schema errors...continue to query the xml..first create the dictionary
                var ReturnObject = new Dictionary<string, string>();

                //Loop Through The XML To Load The Dictionary
                foreach (XElement ProvinceToAdd in xDoc.Element("Provinces").Elements("Province"))
                {
                    //add the province value
                    ReturnObject.Add(ProvinceToAdd.Attribute("id").Value, ProvinceToAdd.Attribute("txt").Value);
                }

                //return the dictionary
                return ReturnObject.ToImmutableDictionary();
            }

            //if we get here then we have some sort of error
            throw new Exception("ProvincesListing Failed");
        }

        #endregion

    }

}
