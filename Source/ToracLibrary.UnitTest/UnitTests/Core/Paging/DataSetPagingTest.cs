using System;
using ToracLibrary.Core.Paging;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to page data sets
    /// </summary>
    public class DataSetPagingTest
    {

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

    }

}