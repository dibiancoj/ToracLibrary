using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNet.AspNetMVC.HtmlHelpers
{

    /// <summary>
    ///  Gives you the ability to create a custom attribute helper
    /// </summary>
    public static class MvcHtmlHelpers
    {

        /// <summary>
        /// Checks the evaluation. If it's met it will return the Output To Display If Predicate Is Met. Otherwise will return null so we don't output the value
        /// </summary>
        /// <typeparam name="TModel">Model Type Of The View</typeparam>
        /// <param name="Helper">Helper Extension Method Object</param>
        /// <param name="OutputAttributePredicate">Value if we should output the attribute</param>
        /// <param name="OutputToDisplayIfPredicateIsMet">Output to return if we meet the predicate. Will return null if the predicate is not met</param>
        /// <returns>Null if predicate is not met. Otherwise will return OutputToDisplayIfPredicateIsMet</returns>
        public static string CustomOutputAttribute<TModel>(this HtmlHelper<TModel> Helper, bool OutputAttributePredicate, string OutputToDisplayIfPredicateIsMet)
        {
            //call this in a razor view using (after importing the namespace to this class)
            //@Html.CustomOutputAttribute(x.Model =="bla bla", "data-id=5");

            //did we meet the predicate
            if (OutputAttributePredicate)
            {
                //we met the predicate, return the display
                return OutputToDisplayIfPredicateIsMet;
            }

            //we didn't meet the predicate, so return null
            return null;
        }

    }

}
