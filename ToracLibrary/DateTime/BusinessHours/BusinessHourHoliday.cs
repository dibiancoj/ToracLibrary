using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DateTimeHelpers.BusinessHours
{

    /// <summary>
    /// Holds The Records For Each Holiday
    /// </summary>
    /// <remarks>Class Is Immutable</remarks>
    public class BusinessHourHoliday
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WhatIsTheStartDateOfTheHoliday">Start Date Time Of The Holiday.</param>
        /// <param name="WhatIsTheEndDateOfTheHoliday">End Date Time Of The Holiday</param>
        public BusinessHourHoliday(DateTime WhatIsTheStartDateOfTheHoliday, DateTime WhatIsTheEndDateOfTheHoliday)
        {
            //set the variables
            StartDateOfHoliday = WhatIsTheStartDateOfTheHoliday;
            EndDateOfHoliday = WhatIsTheEndDateOfTheHoliday;
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Start Date Of The Holiday
        /// </summary>
        public DateTime StartDateOfHoliday { get; }

        /// <summary>
        /// End Date Of The Holiday
        /// </summary>
        public DateTime EndDateOfHoliday { get; }

        #endregion

    }

}
