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
        [Fact]
        public void CalculateTheNumberOfPagesTest1()
        {
            //[5 total records with 5 per page, should be 1 pages]
            Assert.Equal(1, DataSetPaging.CalculateTotalPages(5, 5));

            //[6 total records with 3 per page, should be 2 pages]
            Assert.Equal(2, DataSetPaging.CalculateTotalPages(6, 3));

            //[6 total records with 4 per page, should be 2 pages]
            Assert.Equal(2, DataSetPaging.CalculateTotalPages(6, 4));

            //[1 total records with 4 per page, should be 1 pages]
            Assert.Equal(1, DataSetPaging.CalculateTotalPages(1, 4));
        }

    }

}