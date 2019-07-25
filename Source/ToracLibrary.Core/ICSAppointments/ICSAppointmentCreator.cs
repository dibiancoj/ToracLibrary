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
        /// Mime type for the ics file
        /// </summary>
        public const string ICSMimeType = "text/calendar";

        /// <summary>
        /// Format to use when outputting a date
        /// </summary>
        /// <remarks>Unit test uses reflection to grab these fields so we can validate the data</remarks>
        private const string FormatSpecificDateTime = "yyyyMMddTHHmmss";

        #endregion

        #region Enum

        public enum IcsTimeZoneEnum
        {
            NewYork
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an .ics file which creates an event in outlook, google calendar, etc.
        /// </summary>
        /// <param name="icsTimeZone">Time zone of appointment</param>
        /// <param name="startDateTimeOfAppointment">Start date or time of appointment</param>
        /// <param name="endDateTimeOfAppointment">End date or time of appointment</param>
        /// <param name="summaryOfAppointment">Summary description of the appointment</param>
        /// <param name="locationOfAppointment">Location of the appointment</param>
        /// <param name="bodyOfReminder">The body text of the reminder</param>
        /// <returns>A String. Either call  System.IO.File.WriteAllText("test.ics", result) to write it to disk. Or Encoding.ASCII.GetBytes(result) to get it into a byte array for download</returns>
        public static string CreateICSAppointment(IcsTimeZoneEnum icsTimeZone,
                                                  DateTime startDateTimeOfAppointment,
                                                  DateTime endDateTimeOfAppointment,
                                                  string summaryOfAppointment,
                                                  string locationOfAppointment,
                                                  string bodyOfReminder)
        {
            /*Syntax should be something like this
             *BEGIN:VCALENDAR
             *VERSION:2.0
             *PRODID:-//hacksw/handcal//NONSGML v1.0//EN
             * {{the time zone definition data}}
             *BEGIN:VEVENT
             *DTSTART;TZID=America/New_York:20140606T180000
             *DTEND:;TZID=America/New_York:20140606T180000
             *SUMMARY: bla bla
             *LOCATION: New York
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

            //grab the time zone factory
            var timeZoneFactoryToUse = BaseTimeZoneFactory.CreateTimeZone(icsTimeZone);

            //we will use a string builder to write everything
            var icsWriter = new StringBuilder();

            //there are basically 5 fields tht we need to fill in...start date, end date, summary, location, and the body.

            //let's add the first couple of lines that are static and won't change
            icsWriter.AppendLine("BEGIN:VCALENDAR");
            icsWriter.AppendLine("VERSION:2.0");
            icsWriter.AppendLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");

            //add the time zone definition
            icsWriter.AppendLine(timeZoneFactoryToUse.TimeZoneDefinitionOutput);

            icsWriter.AppendLine("BEGIN:VEVENT");

            //We basically need: 
            //"DTSTART:Date:TheFormattedDateNow"
            //"DTEND:Date:TheFormattedDateNow"

            //add the start date time value in the format we need
            icsWriter.Append($"DTSTART;TZID={timeZoneFactoryToUse.TimeZoneDateOutput}:{GetFormattedDateTime(startDateTimeOfAppointment)}").Append(Environment.NewLine);

            //add the end date time value in the format we need
            icsWriter.Append($"DTEND;TZID={timeZoneFactoryToUse.TimeZoneDateOutput}:{GetFormattedDateTime(endDateTimeOfAppointment)}").Append(Environment.NewLine);

            //add the summary
            icsWriter.Append($"SUMMARY:{summaryOfAppointment}").Append(Environment.NewLine);

            //add the location
            icsWriter.Append($"LOCATION:{locationOfAppointment}").Append(Environment.NewLine);

            //add the description
            icsWriter.Append($"DESCRIPTION:{bodyOfReminder}").Append(Environment.NewLine);

            //add the closing brackets
            icsWriter.AppendLine("END:VEVENT");

            //add the end calendar
            icsWriter.AppendLine("END:VCALENDAR");

            //let's go return the results
            return icsWriter.ToString();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Return the formatted time that an ics file expects
        /// </summary>
        /// <param name="dateToBuild">Date to build up</param>
        /// <returns>the formatted time in a string</returns>
        private static string GetFormattedDateTime(DateTime dateToBuild)
        {
            //just make sure if number is 1 digit, then we make it ie 09.

            //return it in the format they want. Don't use z at the end because we want it local time
            return dateToBuild.ToString(FormatSpecificDateTime);
        }

        #endregion

    }

}
