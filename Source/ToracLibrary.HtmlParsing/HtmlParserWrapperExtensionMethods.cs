using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.HtmlParsing
{

    /// <summary>
    /// Extension methods to help with the syntax in the html parser wrapper
    /// </summary>
    public static class HtmlParserWrapperExtensionMethods
    {

        /// <summary>
        /// Does an element have the specified value listed in the class attribute
        /// </summary>
        /// <param name="NodeToLookIn">node to look in</param>
        /// <param name="ClassNameToLookFor">class name to look for in the class attribute</param>
        /// <returns>does it contain that class</returns>
        public static bool ElementHasClassValue(this HtmlNode NodeToLookIn, string ClassNameToLookFor)
        {
            //go try to grab the class attribute
            var ClassCheck = NodeToLookIn.Attributes["class"];

            //did we find it?
            if (ClassCheck == null)
            {
                //we didn't find a class...doesn't have it
                return false;
            }

            //go split it bc you can have multiple classes in an element and return the result
            return ClassCheck.Value.Split(' ').Any(x => string.Equals(x, ClassNameToLookFor, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Add another attribute to the class in the node. Handles multiple classes listed in the class attribute
        /// </summary>
        /// <param name="NodeToAddClassInto">node to add too</param>
        /// <param name="AttributeValue">attribute value to set</param>
        public static void AddClassValueToClass(this HtmlNode NodeToAddClassInto, string AttributeValue)
        {
            //go check if we have that attribute
            var ClassAttribute = NodeToAddClassInto.Attributes["class"];

            //do we have a class attribute already specified?
            if (ClassAttribute == null)
            {
                //we can just use the default method
                NodeToAddClassInto.Attributes.Add("class", AttributeValue);

                //exit the method
                return;
            }

            //we already have a class value...just add a space and the value we want to set.
            ClassAttribute.Value = string.Format("{0} {1}", ClassAttribute.Value, AttributeValue);
        }

        /// <summary>
        /// Set the inner text of an element
        /// </summary>
        /// <param name="NodeToSetTheTextOf">node to set the text of</param>
        /// <param name="TextToReplace">text to set</param>
        public static void SetTextElement(this HtmlNode NodeToSetTheTextOf, string TextToReplace)
        {
            //grab the first child that is a text element
            var InnerTextNode = NodeToSetTheTextOf.ChildNodes.OfType<HtmlTextNode>().FirstOrDefault();

            //did we find a text element?
            if (InnerTextNode != null)
            {
                //we found a text element...reset the text
                InnerTextNode.Text = TextToReplace;
            }
        }

    }

}
