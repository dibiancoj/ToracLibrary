using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Paging.BuildPagerText.Keywords
{

    /// <summary>
    /// The number of the record that is at the end on that page
    /// </summary>
    internal class ToRecordNumberPagerKeyword : IPagerKeyword
    {

        #region Static Properties

        /// <summary>
        /// Calling code can use this variable instead of hard coding string
        /// </summary>
        private const string KeyWordTag = "[[ToRecordNumber]]";

        #endregion

        #region Interface Items

        /// <summary>
        /// Keyword to replace and look for
        /// </summary>
        public string KeyWord
        {
            get
            {
                return KeyWordTag;
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
            //get the max number of records
            int ToRecordTemp = (CurrentPageYouAreOn * HowManyRecordsPerPage);

            //is there not enough on the last page?
            if (ToRecordTemp > HowManyTotalRecordsInDataSet)
            {
                //there aren't enough records..so reset the variable
                ToRecordTemp = HowManyTotalRecordsInDataSet;
            }

            //return the final variable
            return ToRecordTemp.ToString();
        }

        #endregion

    }

}
