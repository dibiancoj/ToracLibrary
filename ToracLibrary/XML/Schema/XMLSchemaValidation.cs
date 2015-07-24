using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace ToracLibrary.Xml.Schema
{

    /// <summary>
    /// Validate an XML Fragment Against A Schema
    /// </summary>
    public static class XMLSchemaValidation
    {

        #region Public Methods

        #region External Schema's

        /// <summary>
        /// Check An XML File And Validate It Against A Schema (xsd file). Return all errors
        /// </summary>
        /// <param name="XMLToValidate">XML To Validate</param>
        /// <param name="SchemaToUse">Schema To Check With</param>
        /// <returns>IEnumerable-ValidationEventArgs -- Errors Found</returns>
        public static IEnumerable<ValidationEventArgs> ValidateXMLAgainstSchema(XDocument XMLToValidate, XmlSchemaSet SchemaToUse)
        {
            //Parameter Validation
            if (XMLToValidate == null)
            {
                throw new ArgumentNullException("XML To Validate Is Null");
            }

            if (SchemaToUse == null)
            {
                throw new ArgumentNullException("Schema To Use Is Null");
            }
            //End of Parameter Validation

            //Holds the Errors Found While Validating
            var ErrorsFound = new List<ValidationEventArgs>();

            //Start the XML validation (throw each error into a list)
            XMLToValidate.Validate(SchemaToUse, ((object sender, ValidationEventArgs e) => ErrorsFound.Add(e)));

            //Return the errors fround
            return ErrorsFound;
        }

        /// <summary>
        /// Check An XML File And Validate It Against A Schema (xsd file). Will raise the first exception if an exception is found
        /// </summary>
        /// <param name="XMLToValidate">XML To Validate</param>
        /// <param name="SchemaToUse">Schema To Check With</param>
        /// <returns>Result - Returns true if successful</returns>
        public static bool ValidateXMLAgainstSchemaAndRaiseExceptions(XDocument XMLToValidate, XmlSchemaSet SchemaToUse)
        {
            //use the overload to raise any errors
            foreach (var thisError in ValidateXMLAgainstSchema(XMLToValidate, SchemaToUse))
            {
                //raise the exception
                throw thisError.Exception;
            }

            //return the result that it was successful
            return true;
        }

        #endregion

        #region Internal Schema's

        /// <summary>
        /// Check An XML File And Validate It Against A Schema which is inline (in the xml file). Return all errors. Does not return warnings - see reason below in the notes
        /// </summary>
        /// <param name="XMLWithSchemaToValidate">XML With Schema To Validate</param>
        /// <returns>IEnumerable-ValidationEventArgs -- Errors Found</returns>
        public static IEnumerable<ValidationEventArgs> ValidateXMLAgainstInlineSchema(XDocument XMLWithSchemaToValidate)
        {
            //Holds the Errors Found While Validating
            var ErrorsFound = new List<ValidationEventArgs>();

            // Set the validation settings.
            var XmlReaderConfigSettings = new XmlReaderSettings();


            //****Example of an inline schema. It works differently. You need the target namespace then again in the xml sports xmlns:xsdHeadCound
            //    Headcount is free form
            //    Because the inline schema appears as a child element of the root element, the root element cannot be validated when inline schema validation is performed. A validation warning is generated for the root element.
            //    that is why we ignore warnings
            //  <root>
            //      <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"  targetNamespace="xsdHeadCount">
            //          <xs:element name="sport">
            //              <xs:complexType>
            //                  <xs:attribute name="id" type="xs:int" use="required" />
            //                  <xs:attribute name="txt" type="xs:string" use="required" />
            //              </xs:complexType>
            //          </xs:element>
            //      </xs:schema>
            //      <sports xmlns="xsdHeadCount">
            //          <sport id="1" txt="Hockey" />
            //          <sport id="2" txt="Baseball" />
            //      </sports>
            //  </root>

            //set the validation type
            XmlReaderConfigSettings.ValidationType = ValidationType.Schema;

            //set the bitmask of what to check for
            XmlReaderConfigSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;

            //we can set this but we will always get atleast 1 warning because it's inline and it can't check the root node. see note above

            //settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            //create the event handler for errors
            XmlReaderConfigSettings.ValidationEventHandler += ((object sender, ValidationEventArgs e) => ErrorsFound.Add(e));

            // Create the XmlReader object.
            using (XmlReader SchemaXmlReader = XmlReader.Create(XMLWithSchemaToValidate.CreateReader(), XmlReaderConfigSettings))
            {
                // Parse and read the file. 
                while (SchemaXmlReader.Read()) ;
            }

            //return the list of errors
            return ErrorsFound;
        }

        #endregion

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Load an Schema From A File Location
        /// </summary>
        /// <param name="SchemaFileLocation">Schema File Location</param>
        /// <param name="TargetNamespace">Default Namespace - Could be a blank string</param>
        /// <returns>XmlSchemaSet Object</returns>
        public static XmlSchemaSet LoadSchemaSetFromFileLocation(string SchemaFileLocation, string TargetNamespace)
        {
            //Make sure the file is there
            if (!File.Exists(SchemaFileLocation))
            {
                throw new FileNotFoundException("Can't Find Schema File In: " + SchemaFileLocation);
            }

            //Make sure its a .xsd file
            if (!string.Equals(new FileInfo(SchemaFileLocation).Extension, ".xsd", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("Schema File Must Be Of Extension .xsd");
            }

            //*************Done with validation*************
            //Schema To Load
            var SchemaSet = new XmlSchemaSet();

            //Add the file xsd file to the path
            SchemaSet.Add(TargetNamespace, SchemaFileLocation);

            //Compile The Schema
            SchemaSet.Compile();

            //Return the XMLSchemaSet
            return SchemaSet;
        }

        /// <summary>
        /// Load A XML Schema Set From A String
        /// </summary>
        /// <param name="Schema">Schema In A String Format</param>
        /// <param name="TargetNamespace">Default Namespace - Could be a blank string</param>
        /// <returns>XmlSchemaSet Object</returns>
        public static XmlSchemaSet LoadSchemaFromText(string Schema, string TargetNamespace)
        {
            //Schema To Return
            var SchemaToReturn = new XmlSchemaSet();

            //go build the schema reader so we can dispose of it
            using (var SchemaReader = new StringReader(Schema))
            {
                //create the xml reader
                using (var XDocReader = XmlReader.Create(SchemaReader))
                {
                    //Add the Schema   
                    SchemaToReturn.Add(TargetNamespace, XDocReader);
                }
            }

            //Compile The Schema
            SchemaToReturn.Compile();

            //return the schema
            return SchemaToReturn;
        }

        #endregion

    }

}
