using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using ToracLibrary.Xml.Schema;

namespace ToracLibrary.States
{
    /// <summary>
    /// Retrieve all the state
    /// </summary>
    public class State
    {

        #region Public Methods

        #region States

        /// <summary>
        /// Get the state xml resouce
        /// </summary>
        /// <returns>xml file in an xdocument</returns>
        public static XDocument UnitedStatesXmlResource()
        {
            return XDocument.Parse(Properties.Resources.UnitedStatesXml);
        }

        /// <summary>
        /// Get the state xml schema resource to validate with
        /// </summary>
        /// <returns>XmlSchemaSet to validate with</returns>
        public static XmlSchemaSet UnitedStatesXmlSchemaResource()
        {
            return XMLSchemaValidation.LoadSchemaFromText(Properties.Resources.UnitedStatesSchema, null);
        }

        /// <summary>
        /// Get a listing of states along (Key Value Pair)
        /// </summary>
        /// <returns>IImmutableDictionary of string - string</returns>
        /// <remarks>Is validated before returning data. It will raise any errors if there were errors found</remarks>
        public static IImmutableDictionary<string, string> UnitedStatesStateListing()
        {
            //xml is validated in a unit test against the schema. We don't need to keep validating it on each method call. This will make the method faster

            //dictionary to be returned
            var ReturnObject = new Dictionary<string, string>();

            //Loop Through The XML To Load The Dictionary
            foreach (XElement StateToAdd in UnitedStatesXmlResource().Element("States").Elements("State"))
            {
                //add the state value
                ReturnObject.Add(StateToAdd.Attribute("id").Value, StateToAdd.Attribute("txt").Value);
            }

            //return the dictionary
            return ReturnObject.ToImmutableDictionary();
        }

        #endregion

        #region Provinces

        /// <summary>
        /// Get the provinces xml resouce
        /// </summary>
        /// <returns>xml file in an xdocument</returns>
        public static XDocument CanadaProvinceXmlResource()
        {
            return XDocument.Parse(Properties.Resources.CanadaProvinceXml);
        }

        /// <summary>
        /// Get the provinces xml schema resource to validate with
        /// </summary>
        /// <returns>XmlSchemaSet to validate with</returns>
        public static XmlSchemaSet CanadaProvinceXmlSchemaResource()
        {
            return XMLSchemaValidation.LoadSchemaFromText(Properties.Resources.CanadaProvinceSchema, null);
        }

        /// <summary>
        /// Get a listing of provinces (Canada) (Key Value Pair)
        /// </summary>
        /// <returns>IImmutableDictionary of string - string</returns>
        /// <remarks>Is validated before returning data. It will raise any errors if there were errors found</remarks>
        public static IImmutableDictionary<string, string> CanadaProvincesListing()
        {
            //xml is validated in a unit test against the schema. We don't need to keep validating it on each method call. This will make the method faster

            //dictionary to be returned
            var ReturnObject = new Dictionary<string, string>();

            //Loop Through The XML To Load The Dictionary
            foreach (XElement ProvinceToAdd in CanadaProvinceXmlResource().Element("Provinces").Elements("Province"))
            {
                //add the province value
                ReturnObject.Add(ProvinceToAdd.Attribute("id").Value, ProvinceToAdd.Attribute("txt").Value);
            }

            //return the dictionary
            return ReturnObject.ToImmutableDictionary();
        }

        #endregion

        #endregion

    }

}
