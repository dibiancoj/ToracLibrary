using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Paging.BuildPagerText.Keywords
{

    /// <summary>
    /// The number of the record that is starting on that page
    /// </summary>
    public class FromRecordNumberPagerKeyword : IPagerKeyword
    {

        /// <summary>
        /// Keyword to replace and look for
        /// </summary>
        public string KeyWord
        {
            get
            {
                return "{FromRecordNumber}";
            }
        }

        /// <summary>
        /// Replacement Value To Set
        /// </summary>
        /// <param name="HowManyTotalRecordsInDataSet">Total number of records in the data set</param>
        /// <param name="HowManyRecordsPerPage">How many records per page</param>
        /// <param name="CurrentPageYouAreOn">Current page you are on</param>
        /// <returns>replace value</returns>
        public string ReplacementValue(int HowManyTotalRecordsInDataSet, int HowManyRecordsPerPage, int CurrentPageYouAreOn)
        {
            //on the first page?
            if (CurrentPageYouAreOn == 1)
            {
                return 1.ToString();
            }

            return (((CurrentPageYouAreOn - 1) * HowManyRecordsPerPage) + 1).ToString();
        }
    }

}
