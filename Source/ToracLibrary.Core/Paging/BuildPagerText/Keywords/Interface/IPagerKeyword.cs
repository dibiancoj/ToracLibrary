using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Paging.BuildPagerText.Keywords
{

    /// <summary>
    /// Common interface for all pager keywords
    /// </summary>
    public interface IPagerKeyword
    {

        /// <summary>
        /// Keyword
        /// </summary>
        string KeyWord { get; }

        /// <summary>
        /// Replacement value if the keyword is specified
        /// </summary>
        /// <param name="HowManyTotalRecordsInDataSet">Total number of records</param>
        /// <param name="HowManyRecordsPerPage">How many records per page</param>
        /// <param name="CurrentPageYouAreOn">Current page you are on</param>
        /// <returns>Text value to replace</returns>
        string ReplacementValue(int HowManyTotalRecordsInDataSet, int HowManyRecordsPerPage, int CurrentPageYouAreOn);

    }

}
