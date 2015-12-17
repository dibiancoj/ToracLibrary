using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.URLHelperMethods
{

    /// <summary>
    /// Helpers for anything dealing with url's
    /// </summary>
    public static class URLHelpers
    {

        /// <summary>
        /// Appends the passed in query strings to the url passed in.
        /// </summary>
        /// <param name="QueryStringsToAppend">Query strings to add to the url</param>
        /// <param name="UrlToModify">URL to add the query strings into</param>
        /// <returns>updated url</returns>
        public static string AppendQueryStringToUrl(IEnumerable<KeyValuePair<string, string>> QueryStringsToAppend, string UrlToModify)
        {
            //let's build the url. Throw it into a uri builder
            var URIToBuild = new UriBuilder(UrlToModify);

            //parse the query strings
            var UrlQuery = System.Web.HttpUtility.ParseQueryString(URIToBuild.Query);

            //loop through each query string to add
            foreach (var QueryStringToAdd in QueryStringsToAppend)
            {
                //do we already have this query string in the url builder?
                if (UrlQuery[QueryStringToAdd.Key] == null)
                {
                    //add the query string to the builder
                    UrlQuery[QueryStringToAdd.Key] = QueryStringToAdd.Value;
                }
            }

            //set the query
            URIToBuild.Query = UrlQuery.ToString();

            //return the url now
            return URIToBuild.ToString();
        }

    }

}
