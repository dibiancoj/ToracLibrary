using System;
using System.Collections.Generic;
using System.Text;

namespace ToracLibrary.Core.ICSAppointments
{
    internal class NewYorkTimeZoneFactory : BaseTimeZoneFactory
    {
        internal override string TimeZoneDateOutput => "America/New_York";

        internal override string TimeZoneDefinitionOutput => NewYorkTimeZoneDefintion;

        internal const string NewYorkTimeZoneDefintion = @"BEGIN:VTIMEZONE
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
END:VTIMEZONE";
    }
}
