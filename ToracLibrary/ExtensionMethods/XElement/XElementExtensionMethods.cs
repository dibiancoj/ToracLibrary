using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToracLibrary.ExtensionMethods.XElementExtensions
{

    /// <summary>
    /// Extension Methods For XElements
    /// </summary>
    public static class XElementExtensionMethods
    {

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
            XElementToRemoveBlanksFrom.Descendants().Where(x => string.IsNullOrEmpty(x.Value)).Remove();
        }

    }

}
