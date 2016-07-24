using System;
using ToracLibrary.Core.AccountingPeriods;
using ToracLibrary.Core.AccountingPeriods.Exceptions;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    public class AccountingPeriodTest
    {

        #region Main Tests

        /// <summary>
        /// Test builds an accounting period object and verifies its correct
        /// </summary>
        [InlineData(201412, 12, 2014)]
        [InlineData(201301, 01, 2013)]
        [InlineData(201306, 06, 2013)]
        [Theory]
        public void BuildAccountingPeriodObjectIntConstructorTest1(int AccountingPeriodToTest, int ShouldBeMonth, int ShouldBeYear)
        {
            //go run the method and get the results
            var Result = new AccountingPeriod(AccountingPeriodToTest);

            //test the month and year
            Assert.Equal(ShouldBeMonth, Result.Month);
            Assert.Equal(ShouldBeYear, Result.Year);

            //make sure the ToAccountingPeriod Works
            Assert.Equal(Result.ToAccountingPeriod(), AccountingPeriodToTest);
        }

        /// <summary>
        /// Test builds an accounting period object and verifies its correct
        /// </summary>
        [InlineData(12, 2014, 201412)]
        [InlineData(01, 2013, 201301)]
        [InlineData(06, 2013, 201306)]
        [Theory]
        public void BuildAccountingPeriodObjectSeperateMonthYearConstructorTest2(int MonthToTest, int YearToTest, int ShouldBeAccountingPeriod)
        {
            //grab the result
            var Result = new AccountingPeriod(MonthToTest, YearToTest);

            //test the month and year
            Assert.Equal(MonthToTest, Result.Month);
            Assert.Equal(YearToTest, Result.Year);

            //make sure the ToAccountingPeriod Works
            Assert.Equal(Result.ToAccountingPeriod(), ShouldBeAccountingPeriod);
        }

        /// <summary>
        /// Test how we add periods
        /// </summary>
        [InlineData(201412, 1, 201501)]
        [InlineData(201407, 1, 201408)]
        [InlineData(201401, 1, 201402)]
        [InlineData(201401, 2, 201403)]
        [InlineData(201401, 12, 201501)]
        [InlineData(201412, 3, 201503)]
        [Theory]
        public void IncrementPeriodTest1(int AccountingPeriodToTest, int IncrementBy, int ShouldBeAccoutingPeriod)
        {
            //run the test and make sure everything equals out
            Assert.Equal(ShouldBeAccoutingPeriod, AccountingPeriod.IncrementPeriod(AccountingPeriodToTest, IncrementBy));
        }

        /// <summary>
        /// Test how we subtract periods
        /// </summary>
        [InlineData(201508, -1, 201507)]
        [InlineData(201501, -1, 201412)]
        [InlineData(201412, -1, 201411)]
        [InlineData(201412, -12, 201312)]
        [InlineData(201408, -2, 201406)]
        [Theory]
        public void DecreasePeriodTest1(int AccountingPeriodToTest, int IncrementBy, int ShouldBeAccoutingPeriod)
        {
            //run the test and make sure everything equals out
            Assert.Equal(ShouldBeAccoutingPeriod, AccountingPeriod.IncrementPeriod(AccountingPeriodToTest, IncrementBy));
        }

        /// <summary>
        /// Test how we convert a period to a date
        /// </summary>
        [InlineData(201412, 12, 2014)]
        [Theory]
        public void ConvertPeriodToDateTest1(int AccountingPeriodToTest, int ShouldBeMonthDate, int ShouldBeYearDate)
        {
            //make sure the date equals what is being returned
            Assert.Equal(new DateTime(ShouldBeYearDate, ShouldBeMonthDate, 1), AccountingPeriod.PeriodToDateTime(AccountingPeriodToTest));
        }

        /// <summary>
        /// Test a date to a period
        /// </summary>
        [InlineData(12, 2014, 201412)]
        [InlineData(11, 2014, 201411)]
        [InlineData(11, 2013, 201311)]
        [Theory]
        public void ConvertDateToPeriodTest1(int TestMonthDate, int TestYearDate, int ShouldBeAccountingPeriod)
        {
            //make sure we get back the correct accounting period
            Assert.Equal(ShouldBeAccountingPeriod, AccountingPeriod.DateTimeToPeriod(new DateTime(TestYearDate, TestMonthDate, 1)));
        }

        #endregion

        #region Validation Tests

        /// <summary>
        /// Make sure we get the year out of range exception for a parameter that has a bad year
        /// </summary>
        [InlineData(20140000)]
        [Theory]
        public void ValidateAccountingPeriodOutOfRangeExceptionTest1(int AccountingPeriodToTest)
        {
            Assert.Throws<AccountingPeriodYearOutOfRangeException>(() => new AccountingPeriod(AccountingPeriodToTest));
        }

        /// <summary>
        /// Make sure we get the month out of range exception for a parameter that has a bad year
        /// </summary>
        [InlineData(201400)]
        [InlineData(201413)]
        [Theory]
        public void ValidateAccountingPeriodOutOfRangeExceptionTest2(int AccountingPeriodToTest)
        {
            Assert.Throws<AccountingPeriodMonthOutOfRangeException>(() => new AccountingPeriod(AccountingPeriodToTest));
        }

        #endregion

    }

}