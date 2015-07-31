using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace ToracLibrary.Xml.XSLT
{

    /// <summary>
    /// Helper class to  handle xml transformations using XSLT
    /// </summary>
    public static class XSLTTransformation
    {

        /******XSLT Example******
         * <xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
          <xsl:template match="/">
            <Customers>
              <xsl:for-each select="Persons/Person">
                <Customer>
                  <xsl:value-of select="FirstName"/>
                </Customer>
              </xsl:for-each>
            </Customers>
          </xsl:template>
        </xsl:stylesheet>
        */


        /******XML Example******
        * <Persons xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                <Person>
                    <FirstName id="1">First Name 1</FirstName>
                    <LastName>Last Name 1</LastName>
                </Person>
                <Person>
                    <FirstName id="2">First Name 2</FirstName>
                    <LastName>Last Name 2</LastName>
                </Person>
           </Persons>
        */

        #region Public Overloads Methods

        /// <summary>
        /// Transform the document from file path (xslt is saved to file and xml)
        /// </summary>
        /// <param name="XSLTFilePath">XSLT File Path</param>
        /// <param name="XMLFilePath">XML File Path</param>
        /// <returns>XElement</returns>
        public static XElement TransformToXML(string XSLTFilePath, string XMLFilePath)
        {
            //first make sure we can find both files

            //first check on the xslt file path
            if (!File.Exists(XSLTFilePath))
            {
                //can't find the file...throw an error
                throw new FileNotFoundException("Can't Find XSLT File Path At: " + XSLTFilePath);
            }

            //check for the xml file path
            if (!File.Exists(XMLFilePath))
            {
                //can't find the file...throw an error
                throw new FileNotFoundException("Can't Find XML File Path At: " + XMLFilePath);
            }

            //transform and return the xml
            return XElement.Parse(Transform(XElement.Load(XSLTFilePath), XElement.Load(XMLFilePath)));
        }

        /// <summary>
        /// Transform the xml using the xslt style sheet
        /// </summary>
        /// <param name="XSLTStyleSheet">XSLT Style Sheet</param>
        /// <param name="XMLToTransforms">XML To Transform</param>
        /// <returns>Transformed XML Data</returns>
        public static XElement TransformToXML(XElement XSLTStyleSheet, XElement XMLToTransforms)
        {
            //use the helper method
            return XElement.Parse(Transform(XSLTStyleSheet, XMLToTransforms));
        }

        #region Transform To String

        /// <summary>
        /// Transform the xml using the xslt style sheet and return a string (transformed data)
        /// </summary>
        /// <param name="XSLTStyleSheet">XSLT Style Sheet</param>
        /// <param name="XMLToTransforms">XML To Transform</param>
        /// <returns>Transformed XML Data</returns>
        public static string TransformToString(XElement XSLTStyleSheet, XElement XMLToTransforms)
        {
            //use the helper method
            return Transform(XSLTStyleSheet, XMLToTransforms);
        }

        /// <summary>
        /// Transform the xml using the xslt style sheet and return a string (transformed data)
        /// </summary>
        /// <param name="XSLTFilePath">XSLT Style Sheet</param>
        /// <param name="XMLFilePath">XML To Transform</param>
        /// <returns>Transformed XML Data</returns>
        public static string TransformToString(string XSLTFilePath, string XMLFilePath)
        {
            //first check on the xslt file path
            if (!File.Exists(XSLTFilePath))
            {
                //can't find the file...throw an error
                throw new FileNotFoundException("Can't Find XSLT File Path At: " + XSLTFilePath);
            }

            //check for the xml file path
            if (!File.Exists(XMLFilePath))
            {
                //can't find the file...throw an error
                throw new FileNotFoundException("Can't Find XML File Path At: " + XMLFilePath);
            }

            //transform and return the xml
            return Transform(XElement.Load(XSLTFilePath), XElement.Load(XMLFilePath));
        }

        #endregion

        #endregion

        #region Private Method

        /// <summary>
        /// Transform the xml using the xslt style sheet - Helper Method With The Base Logic
        /// </summary>
        /// <param name="XSLTStyleSheet">XSLT Style Sheet</param>
        /// <param name="XMLToTransforms">XML To Transform</param>
        /// <returns>String Result</returns>
        private static string Transform(XElement XSLTStyleSheet, XElement XMLToTransforms)
        {
            //xslt tranformation Engine.
            var XSLTEngine = new XslCompiledTransform();

            //load the xslt file
            XSLTEngine.Load(XSLTStyleSheet.CreateReader());

            //create the text writer to write out the string
            using (TextWriter TxtWriter = new StringWriter())
            {
                //run the transformation...it writes to the text writer
                XSLTEngine.Transform(XMLToTransforms.CreateReader(), null, TxtWriter);

                //return the result
                return TxtWriter.ToString();
            }
        }

        #endregion

    }

}
