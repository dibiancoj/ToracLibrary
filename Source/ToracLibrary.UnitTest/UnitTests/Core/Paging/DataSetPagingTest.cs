using System;
using ToracLibrary.Core.Paging;
using ToracLibrary.Core.Paging.BuildPagerText.Keywords;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to page data sets
    /// </summary>
    public class DataSetPagingTest
    {

        #region Unit Tests

        #region Paging

        /// <summary>
        /// Test dataset paging. How many pages for a given dataset
        /// </summary>
        [InlineData(5, 5, 1)]
        [InlineData(6, 3, 2)]
        [InlineData(6, 4, 2)]
        [InlineData(1, 4, 1)]
        [Theory]
        public void CalculateTheNumberOfPagesTest1(int TotalNumberOfRecords, int RecordsPerPage, int ShouldBeNumberOfPages)
        {
            Assert.Equal(ShouldBeNumberOfPages, DataSetPaging.CalculateTotalPages(TotalNumberOfRecords, RecordsPerPage));
        }

        #endregion

        #region Pager Text

        /// <summary>
        /// Calculate the pager text. Initial Simple Test
        /// </summary>
        /// <param name="FormatToUse"></param>
        [InlineData(5, 10, 1, "Record [[FromRecordNumber]] Of [[ToRecordNumber]]. Page [[CurrentPage]] Of [[TotalPages]]. TotalRecords = [[TotalRecordCount]]")]
        [Theory]
        public void CalculatePagerTextTest1(int TotalNumberOfRecords, int RecordsPerPage, int CurrentPageId, string FormatToUse)
        {
            Assert.Equal("Record 1 Of 5. Page 1 Of 1. TotalRecords = 5", PagerText.BuildPagerText(TotalNumberOfRecords, RecordsPerPage, CurrentPageId, FormatToUse));
        }

        /// <summary>
        /// Calculate the pager text. Test the last record on a page
        /// </summary>
        /// <param name="FormatToUse"></param>
        [InlineData(10, 10, 1, "Record [[FromRecordNumber]] Of [[ToRecordNumber]]. Page [[CurrentPage]] Of [[TotalPages]]. TotalRecords = [[TotalRecordCount]]")]
        [Theory]
        public void CalculatePagerTextTest2(int TotalNumberOfRecords, int RecordsPerPage, int CurrentPageId, string FormatToUse)
        {
            Assert.Equal("Record 1 Of 10. Page 1 Of 1. TotalRecords = 10", PagerText.BuildPagerText(TotalNumberOfRecords, RecordsPerPage, CurrentPageId, FormatToUse));
        }

        /// <summary>
        /// Calculate the pager text. Test the first record on the first page - when you have multiple pages
        /// </summary>
        /// <param name="FormatToUse"></param>
        [InlineData(11, 10, 1, "Record [[FromRecordNumber]] Of [[ToRecordNumber]]. Page [[CurrentPage]] Of [[TotalPages]]. TotalRecords = [[TotalRecordCount]]")]
        [Theory]
        public void CalculatePagerTextTest3(int TotalNumberOfRecords, int RecordsPerPage, int CurrentPageId, string FormatToUse)
        {
            Assert.Equal("Record 1 Of 10. Page 1 Of 2. TotalRecords = 11", PagerText.BuildPagerText(TotalNumberOfRecords, RecordsPerPage, CurrentPageId, FormatToUse));
        }

        /// <summary>
        /// Calculate the pager text. Test the first record on the second page - when you have multiple pages
        /// </summary>
        /// <param name="FormatToUse"></param>
        [InlineData(11, 10, 2, "Record [[FromRecordNumber]] Of [[ToRecordNumber]]. Page [[CurrentPage]] Of [[TotalPages]]. TotalRecords = [[TotalRecordCount]]")]
        [Theory]
        public void CalculatePagerTextTest4(int TotalNumberOfRecords, int RecordsPerPage, int CurrentPageId, string FormatToUse)
        {
            Assert.Equal("Record 11 Of 11. Page 2 Of 2. TotalRecords = 11", PagerText.BuildPagerText(TotalNumberOfRecords, RecordsPerPage, CurrentPageId, FormatToUse));
        }

        /// <summary>
        /// Calculate the pager text. Test 2 full pages
        /// </summary>
        /// <param name="FormatToUse"></param>
        [InlineData(20, 10, 2, "Record [[FromRecordNumber]] Of [[ToRecordNumber]]. Page [[CurrentPage]] Of [[TotalPages]]. TotalRecords = [[TotalRecordCount]]")]
        [Theory]
        public void CalculatePagerTextTest5(int TotalNumberOfRecords, int RecordsPerPage, int CurrentPageId, string FormatToUse)
        {
            Assert.Equal("Record 11 Of 20. Page 2 Of 2. TotalRecords = 20", PagerText.BuildPagerText(TotalNumberOfRecords, RecordsPerPage, CurrentPageId, FormatToUse));
        }

        #endregion

        #endregion

    }

}