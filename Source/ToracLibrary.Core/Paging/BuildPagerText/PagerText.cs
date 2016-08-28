using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Paging.BuildPagerText.Keywords;

namespace ToracLibrary.Core.Paging
{

    /// <summary>
    /// Build pager text
    /// </summary>
    public static class PagerText
    {

        /// <summary>
        /// Holds the list of rules or keywords that are implemented
        /// </summary>
        private static IList<IPagerKeyword> KeyWordImplementation = new List<IPagerKeyword> { new FromRecordNumberPagerKeyword(), new ToRecordNumberPagerKeyword(), new TotalRecordCountPagerKeyword(), new CurrentPageNumberPagerKeyword(), new TotalPagesPagerKeyword() };

        /// <summary>
        /// Build the pager text
        /// </summary>
        /// <param name="HowManyTotalRecordsInDataSet">Total Number Of Records (Not Just This Page But In The Entire RecordSet)</param>
        /// <param name="HowManyRecordsPerPage">How Many Records Per Page</param>
        /// <param name="CurrentPageYouAreOn">CurrentPageYouAreOn</param>
        /// <param name="FormatToUse">Format to use. See method for keywords to use</param>
        /// <returns>Pager Text</returns>
        public static string BuildPagerText(int HowManyTotalRecordsInDataSet, int HowManyRecordsPerPage, int CurrentPageYouAreOn, string FormatToUse)
        {
            //key word you can use
            //FromRecordNumber
            //ToRecordNumber
            //TotalRecordCount
            //CurrentPage
            //TotalPages

            //sample = "Page [[CurrentPage]] of [[TotalPages]]. Record [[FromRecordNumber]] To [[ToRecordNumber]] Of [[TotalRecordCount]]

            //or use the static properties:
            //private string FormatToUse = string.Format($"Record {FromRecordNumberPagerKeyword.KeyWordTag} Of {TotalRecordCountPagerKeyword.KeyWordTag}. Page {CurrentPageNumberPagerKeyword.KeyWordTag} Of {TotalPagesPagerKeyword.KeyWordTag}. TotalRecords = {TotalRecordCountPagerKeyword.KeyWordTag}");

            //return text
            var PagerTextToReturn = new StringBuilder(FormatToUse);

            //loop through the format the person passed in
            foreach (IPagerKeyword KeyWordToCheckFactory in KeyWordImplementation.Where(x => FormatToUse.Contains(x.KeyWord)))
            {
                //we have this keyword...replace it
                PagerTextToReturn.Replace(KeyWordToCheckFactory.KeyWord, KeyWordToCheckFactory.ReplacementValue(HowManyTotalRecordsInDataSet, HowManyRecordsPerPage, CurrentPageYouAreOn));
            }

            //go return the result now
            return PagerTextToReturn.ToString();
        }

    }

}
