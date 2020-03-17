using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ICSAppointments;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for .ics appointment creation
    /// </summary>
    public class ICSAppointmentCreatorTest
    {

        #region Unit Test Methods

        [Fact]
        public void MimeTypeTest()
        {
            Assert.Equal("text/calendar", ICSAppointmentCreator.ICSMimeType);
        }

        /// <summary>
        /// Test the creation of the appointment with specific date times
        /// </summary>
        [Fact]
        public void ICSCreationWithSpecificDateTimesTest1()
        {
            const string expectedResult = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//hacksw/handcal//NONSGML v1.0//EN
BEGIN:VTIMEZONE
TZID:America/New_York
X-LIC-LOCATION:America/New_York
BEGIN:DAYLIGHT
TZOFFSETFROM:-0500
TZOFFSETTO:-0400
TZNAME:EDT
DTSTART:19700308T020000
RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=2SU
END:DAYLIGHT
BEGIN:STANDARD
TZOFFSETFROM:-0400
TZOFFSETTO:-0500
TZNAME:EST
DTSTART:19701101T020000
RRULE:FREQ=YEARLY;BYMONTH=11;BYDAY=1SU
END:STANDARD
END:VTIMEZONE
BEGIN:VEVENT
DTSTART;TZID=America/New_York:20150305T035500
DTEND;TZID=America/New_York:20150310T045800
SUMMARY:Test Summary 123
LOCATION:Location Text 123
DESCRIPTION:BodyOfReminder 123
END:VEVENT
END:VCALENDAR
";

            var result = ICSAppointmentCreator.CreateICSAppointment(ICSAppointmentCreator.IcsTimeZoneEnum.NewYork,
                                                                                 new DateTime(2015, 3, 5, 3, 55, 0),
                                                                                 new DateTime(2015, 3, 10, 4, 58, 0),
                                                                                 "Test Summary 123",
                                                                                 "Location Text 123",
                                                                                 "BodyOfReminder 123",
                                                                                 false);

            Assert.Equal(expectedResult, result);
        }

        /// <summary>
        /// Test the creation of the appointment with a full day appt
        /// </summary>
        [Fact]
        public void ICSCreationWithFullDayAppointmentTest1()
        {
            const string expectedResult = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//hacksw/handcal//NONSGML v1.0//EN
BEGIN:VTIMEZONE
TZID:America/New_York
X-LIC-LOCATION:America/New_York
BEGIN:DAYLIGHT
TZOFFSETFROM:-0500
TZOFFSETTO:-0400
TZNAME:EDT
DTSTART:19700308T020000
RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=2SU
END:DAYLIGHT
BEGIN:STANDARD
TZOFFSETFROM:-0400
TZOFFSETTO:-0500
TZNAME:EST
DTSTART:19701101T020000
RRULE:FREQ=YEARLY;BYMONTH=11;BYDAY=1SU
END:STANDARD
END:VTIMEZONE
BEGIN:VEVENT
DTSTART;VALUE=DATE:20150305
DTEND;VALUE=DATE:20150306
SUMMARY:Test Summary 123
LOCATION:Location Text 123
DESCRIPTION:BodyOfReminder 123
END:VEVENT
END:VCALENDAR
";

            var result = ICSAppointmentCreator.CreateICSAppointment(ICSAppointmentCreator.IcsTimeZoneEnum.NewYork,
                                                                                 new DateTime(2015, 3, 5),
                                                                                 new DateTime(2015, 3, 6),
                                                                                 "Test Summary 123",
                                                                                 "Location Text 123",
                                                                                 "BodyOfReminder 123",
                                                                                 true);

            Assert.Equal(expectedResult, result);
        }

        #endregion

    }

}
