using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ICSAppointments
{

    /// <summary>
    /// Creates and .ics file format which creates an event in outlook, google calendar, etc.
    /// </summary>
    public static class ICSAppointmentCreator
    {

        #region Constants

        /// <summary>
        /// Format to use when we just want a full day.
        /// </summary>
        /// <remarks>Only public for unit testing</remarks>
        public const string FormatDate = "yyyyMMdd";

        /// <summary>
        /// Format to use when we just want a full day.
        /// </summary>
        /// <remarks>Only public for unit testing</remarks>
        public const string FormatSpecificDateTime = "yyyyMMddTHHmmss";

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an .ics file which creates an event in outlook, google calendar, etc.
        /// </summary>
        /// <param name="IsFullDayAppointment">Is this a full day appointment. IE: 9/10/2015 to 9/11/2015. Or will the date time include the hours and minutes</param>
        /// <param name="StartDateTimeOfAppointment">Start date or time of appointment</param>
        /// <param name="EndDateTimeOfAppointment">End date or time of appointment</param>
        /// <param name="SummaryOfAppointment">Summary description of the appointment</param>
        /// <param name="LocationOfAppointment">Location of the appointment</param>
        /// <param name="BodyOfReminder">Body of the reminder. This is the full description</param>
        /// <returns>A String. Either call  System.IO.File.WriteAllText("test.ics", result) to write it to disk. Or Encoding.ASCII.GetBytes(input) to get it into a byte array for download</returns>
        public static string CreateICSAppointment(bool IsFullDayAppointment, DateTime StartDateTimeOfAppointment, DateTime EndDateTimeOfAppointment, string SummaryOfAppointment, string LocationOfAppointment, string BodyOfReminder)
        {
            /*Syntax should be something like this
             *BEGIN:VCALENDAR
             *VERSION:2.0
             *PRODID:-//hacksw/handcal//NONSGML v1.0//EN
             *BEGIN:VEVENT
             *DTSTART: UTC Time  ==> if this is a full date use DTSTART;VALUE=DATE:20150930
             *DTEND: (Same as DTStart)
             *SUMMARY: bla bla
             *LOCATION: New York
             *DESCRIPTION: Bla Bla Bla
             *END:VEVENT
             *END:VCALENDAR
             *
             * For Dates:
             * If for specific date time [using start as an example]:
             *DTSTART:FormattedDateTimeYouWant
             *
             * If for entire day [using start as an example]
             *DTSTART;VALUE=DATE:FormattedDateTimeYouWant
             */

            //For line breaks use "\\n"

            //we will use a string builder to write everything
            var ICSWriter = new StringBuilder();

            //there are basically 5 fields tht we need to fill in...start date, end date, summary, location, and the body.

            //let's add the first couple of lines that are static and won't change
            ICSWriter.AppendLine("BEGIN:VCALENDAR");
            ICSWriter.AppendLine("VERSION:2.0");
            ICSWriter.AppendLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");
            ICSWriter.AppendLine("BEGIN:VEVENT");

            //date lables we are going to use *the date field will start with these labels)
            const string StartDateLabel = "DTSTART";
            const string EndDateLabel = "DTEND";

            //is this a full day?
            if (IsFullDayAppointment)
            {
                //full date format seperator (should go after the label)
                const string FullDateSeperator = ";";

                //full date format
                const string FullDateFormat = "VALUE=DATE:";

                //We basically need: 
                //"DTSTART;Value=Date:TheFormattedDateNow"
                //"DTEND;Value=Date:TheFormattedDateNow"

                //add the start full date appointment
                ICSWriter.Append(StartDateLabel).Append(FullDateSeperator).Append(FullDateFormat).Append(GetFormattedDate(StartDateTimeOfAppointment.Date)).Append(Environment.NewLine);

                //add the end date now
                ICSWriter.Append(EndDateLabel).Append(FullDateSeperator).Append(FullDateFormat).Append(GetFormattedDate(EndDateTimeOfAppointment.Date)).Append(Environment.NewLine);
            }
            else
            {
                //We basically need: 
                //"DTSTART:Date:TheFormattedDateNow"
                //"DTEND:Date:TheFormattedDateNow"

                //date time seperator (should go after the label)
                const string SpecificDateTimeSeperator = ":";

                //add the start date time value in the format we need
                ICSWriter.Append(StartDateLabel).Append(SpecificDateTimeSeperator).Append(GetFormattedDateTime(StartDateTimeOfAppointment)).Append(Environment.NewLine);

                //add the end date time value in the format we need
                ICSWriter.Append(EndDateLabel).Append(SpecificDateTimeSeperator).Append(GetFormattedDateTime(EndDateTimeOfAppointment)).Append(Environment.NewLine);
            }

            //add the summary
            ICSWriter.Append("SUMMARY:").Append(SummaryOfAppointment).Append(Environment.NewLine);

            //add the location
            ICSWriter.Append("LOCATION:").Append(LocationOfAppointment).Append(Environment.NewLine);

            //add the description / body of appointment
            ICSWriter.Append("DESCRIPTION:").Append(BodyOfReminder).Append(Environment.NewLine);

            //add the closing brackets
            ICSWriter.AppendLine("END:VEVENT");

            //add the end calendar
            ICSWriter.AppendLine("END:VCALENDAR");

            //let's go return the results
            return ICSWriter.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// return the formatted date that the ics file expects
        /// </summary>
        /// <param name="DateToBuild">Date to build up</param>
        /// <returns>the formatted time in a string</returns>
        private static string GetFormattedDate(DateTime DateToBuild)
        {
            //add them all together now
            return DateToBuild.ToString(FormatDate);
        }

        /// <summary>
        /// Return the formatted time that an ics file expects
        /// </summary>
        /// <param name="DateToBuild">Date to build up</param>
        /// <returns>the formatted time in a string</returns>
        private static string GetFormattedDateTime(DateTime DateToBuild)
        {
            //just make sure if number is 1 digit, then we make it ie 09.

            //return it in the format they want. Don't use z at the end because we want it local time
            return DateToBuild.ToString(FormatSpecificDateTime);
        }

        #endregion

    }

}
