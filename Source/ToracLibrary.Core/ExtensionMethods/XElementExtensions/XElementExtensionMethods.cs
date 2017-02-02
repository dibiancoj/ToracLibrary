using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.ExtensionMethods.XElementExtensions
{

    /// <summary>
    /// Extension Methods For XElements
    /// </summary>
    public static class XElementExtensionMethods
    {

        #region Element Querying With A Namespace

        #region Public Methods

        /// <summary>
        /// Query an element with a namespace
        /// </summary>
        /// <param name="ElementToQuery">Element to query</param>
        /// <param name="NamespaceToUse">Namespace to use</param>
        /// <param name="NameToQuery">Name to query</param>
        /// <returns>Element found</returns>
        public static XElement Element(this XElement ElementToQuery, XNamespace NamespaceToUse, string NameToQuery)
        {
            //return the element with the namespace
            return ElementToQuery.Element(NamespaceToUse + NameToQuery);
        }

        /// <summary>
        /// Query an element with a namespace and return the elements
        /// </summary>
        /// <param name="ElementToQuery">Element to query</param>
        /// <param name="NamespaceToUse">Namespace to use</param>
        /// <param name="NameToQuery">Name to query</param>
        /// <returns>Elements found</returns>
        public static IEnumerable<XElement> Elements(this XElement ElementToQuery, XNamespace NamespaceToUse, string NameToQuery)
        {
            //return the element with the namespace
            return ElementToQuery.Elements(NamespaceToUse + NameToQuery);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Query string value to query with a namespace
        /// </summary>
        /// <param name="NamespaceToUse">Namespace to use</param>
        /// <param name="NameToQuery">Name to query</param>
        /// <returns>Name to use when we query</returns>
        private static XName NameToQueryWithNamespace(XNamespace NamespaceToUse, string NameToQuery)
        {
            //just combine them
            return NamespaceToUse + NameToQuery;
        }

        #endregion

        #endregion

        /// <summary>
        /// Removes blank element's where there is no value
        /// </summary>
        /// <param name="XElementToRemoveBlanksFrom">XElement To Remove Blanks From</param>
        public static void RemoveBlankElements(this XElement XElementToRemoveBlanksFrom)
        {
            //i used this when i can't control the xml and i need to serialize it. 
            //so the xml is
            /*<root>
             * <item>jason</item>
             * <itemdate></itemdate>
             * </root>
             */

            //xml serialization can't handle nullable types. if the nil=true is there you don't need this. If it isn't there and you try to deserialize an item that is a blank string it will fail into a nullable type datetime?, bool?, decimal?, etc.
            //let's loop through all the descendants and where the value is null, remove it
            XElementToRemoveBlanksFrom.Descendants().Where(x => x.Value.IsNullOrEmpty()).Remove();
        }

    }

}
