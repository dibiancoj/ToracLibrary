using System;
using System.Collections.Generic;
using System.Text;
using static ToracLibrary.Core.ICSAppointments.ICSAppointmentCreator;

namespace ToracLibrary.Core.ICSAppointments
{
    internal abstract class BaseTimeZoneFactory
    {
        internal abstract string TimeZoneDateOutput { get; }
        internal abstract string TimeZoneDefinitionOutput { get; }

        //so we don't keep create a new instance. We don't need state so we will just cache 1 instance and throw it into a static property
        private static BaseTimeZoneFactory CachedNyFactory { get; } = new NewYorkTimeZoneFactory();

        internal static BaseTimeZoneFactory CreateTimeZone(IcsTimeZoneEnum timeZoneId)
        {
            switch (timeZoneId)
            {
                case IcsTimeZoneEnum.NewYork:
                    return CachedNyFactory;

                default:
                    throw new NotImplementedException();
            }
        }

        //incase we ever need it. The other time zone definitions are below. Should be a line break after each line below crlf
        /*  BEGIN:VTIMEZONE
            TZID:America/Los_Angeles
            X-LIC-LOCATION:America/Los_Angeles
            BEGIN:DAYLIGHT
            TZOFFSETFROM:-0800
            TZOFFSETTO:-0700
            TZNAME:PDT
            DTSTART:19700308T020000
            RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=2SU
            END:DAYLIGHT
            BEGIN:STANDARD
            TZOFFSETFROM:-0700
            TZOFFSETTO:-0800
            TZNAME:PST
            DTSTART:19701101T020000
            RRULE:FREQ=YEARLY;BYMONTH=11;BYDAY=1SU
            END:STANDARD
            END:VTIMEZONE

            BEGIN:VTIMEZONE
            TZID:America/Phoenix
            X-LIC-LOCATION:America/Phoenix
            BEGIN:STANDARD
            TZOFFSETFROM:-0700
            TZOFFSETTO:-0700
            TZNAME:MST
            DTSTART:19700101T000000
            END:STANDARD
            END:VTIMEZONE


            BEGIN:VTIMEZONE
            TZID:America/Chicago
            X-LIC-LOCATION:America/Chicago
            BEGIN:DAYLIGHT
            TZOFFSETFROM:-0600
            TZOFFSETTO:-0500
            TZNAME:CDT
            DTSTART:19700308T020000
            RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=2SU
            END:DAYLIGHT
            BEGIN:STANDARD
            TZOFFSETFROM:-0500
            TZOFFSETTO:-0600
            TZNAME:CST
            DTSTART:19701101T020000
            RRULE:FREQ=YEARLY;BYMONTH=11;BYDAY=1SU
            END:STANDARD
            END:VTIMEZONE
        */
    }
}
