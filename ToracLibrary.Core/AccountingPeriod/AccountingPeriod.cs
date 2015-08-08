using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.AccountingPeriods.Exceptions;

namespace ToracLibrary.Core.AccountingPeriods
{

    /// <summary>
    /// Holds a common class to build, use, and manipulate accounting periods. Accounting periods are YearMonth int's.
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class AccountingPeriod
    {

        #region Constructor

        /// <summary>
        /// Constructor using a seperate month and year variables
        /// </summary>
        /// <param name="MonthToSet">Month To Set.ie 1 = January, 2 = Feb.</param>
        /// <param name="YearToSet">Year To Set</param>
        public AccountingPeriod(int MonthToSet, int YearToSet)
        {
            //go validate that the month is legit (will throw an error if it fails)
            ValidateMonth(MonthToSet);

            //set the variables
            Month = MonthToSet;
            Year = YearToSet;
        }

        /// <summary>
        /// Constructor using a YearMonth format to set the model from
        /// </summary>
        /// <param name="AccountingPeriodToSet"></param>
        public AccountingPeriod(int AccountingPeriodToSet)
        {
            //go validate the accounting period
            ValidateAccountingPeriod(AccountingPeriodToSet);

            //we are all good set the variables (first push the int to a string
            var PeriodInStringFormat = AccountingPeriodToSet.ToString();

            //grab the month now and set it
            Month = GetMonthFromAccountingPeriod(PeriodInStringFormat);

            //grab the year now and set it
            Year = GetYearFromAccountingPeriod(PeriodInStringFormat);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Month - ie 1 = January, 2 = Feb.
        /// </summary>
        public int Month { get; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; }

        #endregion

        #region Validation

        /// <summary>
        /// Validates that the accounting period in an integer is legal
        /// </summary>
        /// <param name="AccountingPeriod">Accounting Period To Validate. Will Throw An Error If It Doesn't Match The Criteria</param>
        private static void ValidateAccountingPeriod(int AccountingPeriod)
        {
            //throw the accounting period to a string so we don't have to cast it twice
            string AccountingPeriodToString = AccountingPeriod.ToString(CultureInfo.InvariantCulture);

            //make sure it's 6 characters
            if (AccountingPeriodToString.Length != 6)
            {
                //not the correct length
                throw new AccountingPeriodYearOutOfRangeException(AccountingPeriod);
            }

            //go validate that the month number is legit (we can't call period breakdown otherwise we will get a circular reference)
            ValidateMonth(GetMonthFromAccountingPeriod(AccountingPeriodToString));
        }

        /// <summary>
        /// Validates that the accounting period month passed in, is a valid number
        /// </summary>
        /// <param name="MonthToTest">Month To Test</param>
        private static void ValidateMonth(int MonthToTest)
        {
            //make sure the month are within the expected range
            if (MonthToTest < 1)
            {
                //our month is less than Jan throw an error
                throw new AccountingPeriodMonthOutOfRangeException(MonthToTest);
            }

            //make sure we aren't past December
            if (MonthToTest > 12)
            {
                //our month is greater than Dec throw an error
                throw new AccountingPeriodMonthOutOfRangeException(MonthToTest);
            }
        }

        #endregion

        #region Building An Accounting Period

        /// <summary>
        /// Gets the year from an accounting period
        /// </summary>
        /// <param name="AccountingPeriodString">Accounting Period.ToString()</param>
        /// <returns>Year Part Of The Accounting Period String</returns>
        private static int GetYearFromAccountingPeriod(string AccountingPeriodString)
        {
            //grab the sub string then build the int and return it
            return Convert.ToInt32(AccountingPeriodString.Substring(0, 4));
        }

        /// <summary>
        /// Gets the month from an accounting period
        /// </summary>
        /// <param name="AccountingPeriodString">Accounting Period</param>
        /// <returns>Month Part Of The Accounting Period String</returns>
        private static int GetMonthFromAccountingPeriod(string AccountingPeriodString)
        {
            //grab the sub string then build the int and return it
            return Convert.ToInt32(AccountingPeriodString.Substring(4, 2));
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts an accounting period to a date time object. Will set it to the first day of that month
        /// </summary>
        /// <param name="AccountingPeriod">Accounting Period To Convert</param>
        /// <returns>Date Time Object</returns>
        public static DateTime PeriodToDateTime(int AccountingPeriod)
        {
            //first validate the accounting period
            ValidateAccountingPeriod(AccountingPeriod);

            //go get the break down of this accounting period
            var Breakdown = new AccountingPeriod(AccountingPeriod);

            //go convert it and return it
            return new DateTime(Breakdown.Year, Breakdown.Month, 1);
        }

        /// <summary>
        /// Converts a date to an accounting period object
        /// </summary>
        /// <param name="DateToConvert">Date to convert</param>
        /// <returns>Accounting Period</returns>
        public static int DateTimeToPeriod(DateTime DateToConvert)
        {
            return new AccountingPeriod(DateToConvert.Month, DateToConvert.Year).ToAccountingPeriod();
        }

        /// <summary>
        /// Pushes the current model to an integer accounting period
        /// </summary>
        /// <returns>accounting period in an integer format</returns>
        public int ToAccountingPeriod()
        {
            //create a new string builder (only going to use 6 characters)
            var sb = new StringBuilder(6);

            //we need to basically send it into the string builder as
            //YYYYmm

            //always add the year
            sb.Append(Year);

            //if we have a 1 digit month then we need to add a 0
            if (Month < 10)
            {
                //we have a 1 digit month, so we need to a month
                sb.Append("0");
            }

            //always add the month now
            sb.Append(Month);

            //push the string builder to a string, convert it to an int, then return it
            return Convert.ToInt32(sb.ToString());
        }

        #endregion

        #region Adding - Subtracting Periods

        /// <summary>
        /// Increments a period. Can add or subtract however many periods that is passed in
        /// </summary>
        /// <param name="AccountingPeriod">Accounting period to add too</param>
        /// <param name="HowManyPeriodsToAdd">How many periods to add. You can pass in -1 to subtract 1 period (will handle that)</param>
        /// <returns>New Period</returns>
        public static int IncrementPeriod(int AccountingPeriod, int HowManyPeriodsToAdd)
        {
            //first validate that the accounting period is legit (will throw an error if it fails)
            ValidateAccountingPeriod(AccountingPeriod);

            //let's split this out first
            var SplitOutPeriod = new AccountingPeriod(AccountingPeriod);

            //are we adding or subtracting
            if (HowManyPeriodsToAdd > 0)
            {
                //use the add period method
                return AddPeriod(SplitOutPeriod, HowManyPeriodsToAdd).ToAccountingPeriod();
            }

            //are we subtracting periods?
            if (HowManyPeriodsToAdd < 0)
            {
                //we are subtracting
                return SubtractPeriod(SplitOutPeriod, HowManyPeriodsToAdd).ToAccountingPeriod();
            }

            //HowManyPeriodsToAdd = 0...just return whatever was passed in
            return AccountingPeriod;
        }

        #region Calculations

        /// <summary>
        /// Increments the accounting period by HowManyPeriodsToAdd periods.
        /// </summary>
        /// <param name="AccountingPeriodToAddTo">Accounting period to add to</param>
        /// <param name="HowManyPeriodsToAdd">How many periods to add. You can pass in -1 to subtract 1 period (will handle that)</param>
        /// <returns>New Period</returns>
        private static AccountingPeriod AddPeriod(AccountingPeriod AccountingPeriodToAddTo, int HowManyPeriodsToAdd)
        {
            //if we are are not adding then throw an error
            if (HowManyPeriodsToAdd <= 0)
            {
                //throw an error because we aren't adding a period
                throw new ArgumentOutOfRangeException("Can't Use Add Period When You Want To Subtract A Period");
            }

            //since Accounting Period is immutable let's create a working month and a working year
            var WorkingMonth = AccountingPeriodToAddTo.Month;

            //grab the year now
            var WorkingYear = AccountingPeriodToAddTo.Year;

            //we want to loop through for each period we want to add
            for (int i = 0; i < HowManyPeriodsToAdd; i++)
            {
                //are we up to december?
                if (WorkingMonth == 12)
                {
                    //we need to go to the next year
                    WorkingYear += 1;

                    //set the month to january
                    WorkingMonth = 1;
                }
                else
                {
                    //we just need to increment the month (because we aren't in Dec)
                    WorkingMonth += 1;
                }
            }

            //we just need to increment the month
            return new AccountingPeriod(WorkingMonth, WorkingYear);
        }

        /// <summary>
        /// subtracts the accounting period by HowManyPeriodsToAdd periods.
        /// </summary>
        /// <param name="AccountingPeriodToSubtractTo">Accounting period to subtract to</param>
        /// <param name="HowManyPeriodsToSubtract">How many periods to subtract. You can pass in -1 to subtract 1 period (will handle that)</param>
        /// <returns>New Period</returns>
        private static AccountingPeriod SubtractPeriod(AccountingPeriod AccountingPeriodToSubtractTo, int HowManyPeriodsToSubtract)
        {
            //if we are are not adding then throw an error
            if (HowManyPeriodsToSubtract >= 0)
            {
                //throw an error because we aren't adding a period
                throw new ArgumentOutOfRangeException("Can't Use Subtract Period When You Want To Add A Period");
            }

            //since Accounting Period is immutable let's create a working month and a working year
            var WorkingMonth = AccountingPeriodToSubtractTo.Month;

            //grab the year now
            var WorkingYear = AccountingPeriodToSubtractTo.Year;

            //we want to loop through for each period we want to add
            for (int i = 0; i > HowManyPeriodsToSubtract; i--)
            {
                //are we up to january?
                if (WorkingMonth == 1)
                {
                    //we need to go to the previous year
                    WorkingYear -= 1;

                    //set the month to december
                    WorkingMonth = 12;
                }
                else
                {
                    //we just need to subtract the month (because we aren't in january)
                    WorkingMonth -= 1;
                }
            }

            //we just need to increment the month
            return new AccountingPeriod(WorkingMonth, WorkingYear);
        }

        #endregion

        #endregion

    }

}
