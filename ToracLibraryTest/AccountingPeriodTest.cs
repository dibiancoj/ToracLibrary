using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.AccountingPeriods;
using ToracLibrary.AccountingPeriods.Exceptions;

namespace ToracLibraryTest.AccountingPeriodsTest
{

    [TestClass]
    public class AccountingPeriodTest
    {

        #region Constants

        const int Test1 = 201412;
        const int Test2 = 201301;
        const int Test3 = 201306;

        #endregion

        #region Main Tests

        /// <summary>
        /// Test builds an accounting period object and verifies its correct
        /// </summary>
        [TestMethod]
        public void BuildAccountingPeriodObjectIntConstructorTest1()
        {
            //first test
            var result1 = new AccountingPeriod(Test1);

            Assert.AreEqual(12, result1.Month);
            Assert.AreEqual(2014, result1.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result1.ToAccountingPeriod(), Test1);

            //second test
            var result2 = new AccountingPeriod(Test2);

            Assert.AreEqual(1, result2.Month);
            Assert.AreEqual(2013, result2.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result2.ToAccountingPeriod(), Test2);

            //third test
            var result3 = new AccountingPeriod(Test3);

            Assert.AreEqual(6, result3.Month);
            Assert.AreEqual(2013, result3.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result3.ToAccountingPeriod(), Test3);
        }

        /// <summary>
        /// Test builds an accounting period object and verifies its correct
        /// </summary>
        [TestMethod]
        public void BuildAccountingPeriodObjectSeperateMonthYearConstructorTest2()
        {
            //first test
            var result1 = new AccountingPeriod(12, 2014);

            Assert.AreEqual(12, result1.Month);
            Assert.AreEqual(2014, result1.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result1.ToAccountingPeriod(), Test1);

            //second test
            var result2 = new AccountingPeriod(01, 2013);

            Assert.AreEqual(1, result2.Month);
            Assert.AreEqual(2013, result2.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result2.ToAccountingPeriod(), Test2);

            //third test
            var result3 = new AccountingPeriod(06, 2013);

            Assert.AreEqual(6, result3.Month);
            Assert.AreEqual(2013, result3.Year);

            //make sure the ToAccountingPeriod Works
            Assert.AreEqual(result3.ToAccountingPeriod(), Test3);

        }

        /// <summary>
        /// Test how we add periods
        /// </summary>
        [TestMethod]
        public void IncrementPeriodTest1()
        {
            //add 1 month to december
            Assert.AreEqual(201501, AccountingPeriod.IncrementPeriod(201412, 1));

            //add a month to july to make sure that works
            Assert.AreEqual(201408, AccountingPeriod.IncrementPeriod(201407, 1));

            //add a month to january to make sure that works
            Assert.AreEqual(201402, AccountingPeriod.IncrementPeriod(201401, 1));

            //let's add multiple periods now
            Assert.AreEqual(201403, AccountingPeriod.IncrementPeriod(201401, 2));

            //let's add multiple periods where we want to go to the next year
            Assert.AreEqual(201501, AccountingPeriod.IncrementPeriod(201401, 12));

            //let's add multiple periods where we want to go to the next year
            Assert.AreEqual(201503, AccountingPeriod.IncrementPeriod(201412, 3));
        }

        /// <summary>
        /// Test how we subtract periods
        /// </summary>
        [TestMethod]
        public void DecreasePeriodTest1()
        {
            //subtract 1 month where there shouldn't be a year change
            Assert.AreEqual(201507, AccountingPeriod.IncrementPeriod(201508, -1));

            //subtract 1 month where there should be a year change
            Assert.AreEqual(201412, AccountingPeriod.IncrementPeriod(201501, -1));

            //subtract 1 month where we are in Dec
            Assert.AreEqual(201411, AccountingPeriod.IncrementPeriod(201412, -1));

            //let's let multiple periods now
            Assert.AreEqual(201312, AccountingPeriod.IncrementPeriod(201412, -12));

            //let's let multiple periods now
            Assert.AreEqual(201406, AccountingPeriod.IncrementPeriod(201408, -2));
        }

        /// <summary>
        /// Test how we convert a period to a date
        /// </summary>
        [TestMethod]
        public void ConvertPeriodToDateTest1()
        {
            //just test the date
            Assert.AreEqual(new DateTime(2014, 12, 1), AccountingPeriod.PeriodToDateTime(201412));

            //test a few more
            Assert.AreEqual(new DateTime(2014, 11, 1), AccountingPeriod.PeriodToDateTime(201411));

            //test a few more
            Assert.AreEqual(new DateTime(2013, 11, 1), AccountingPeriod.PeriodToDateTime(201311));
        }

        /// <summary>
        /// /Test a date to a period
        /// </summary>
        [TestMethod]
        public void ConvertDateToPeriodTest1()
        {
            //just test the date
            Assert.AreEqual(201412, AccountingPeriod.DateTimeToPeriod(new DateTime(2014, 12, 1)));

            //test a few more
            Assert.AreEqual(201411, AccountingPeriod.DateTimeToPeriod(new DateTime(2014, 11, 1)));

            //test a few more
            Assert.AreEqual(201311, AccountingPeriod.DateTimeToPeriod(new DateTime(2013, 11, 1)));
        }

        #endregion

        #region Validation Tests

        [TestMethod]
        [ExpectedException(typeof(AccountingPeriodYearOutOfRangeException))]
        public void ValidateAccountingPeriodOutOfRangeExceptionTest1()
        {
            new AccountingPeriod(20140000);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountingPeriodMonthOutOfRangeException))]
        public void ValidateAccountingPeriodOutOfRangeExceptionTest2()
        {
            new AccountingPeriod(201400);
        }

        [TestMethod]
        [ExpectedException(typeof(AccountingPeriodMonthOutOfRangeException))]
        public void ValidateAccountingPeriodOutOfMonthExceptionTest3()
        {
            new AccountingPeriod(201413);
        }

        #endregion

    }

}