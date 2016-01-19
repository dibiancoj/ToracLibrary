using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Mathematical.Age
{

    /// <summary>
    /// Calculations for age
    /// </summary>
    public static class AgeCalculation
    {

        /// <summary>
        /// Calculate a persons age
        /// </summary>
        /// <param name="DateOfBirth">Person's date of birth. We will calculate age from this date</param>
        /// <returns>What is the current age of the person</returns>
        public static int CalculateAge(DateTime DateOfBirth)
        {
            //grab the date today
            var Today = DateTime.Today;

            //grab the date of birth date
            var WorkingDateOfBirth = DateOfBirth.Date;

            //subtract the 2 years
            int AgeInYears = Today.Year - WorkingDateOfBirth.Year;

            //if today is less then the current year, then subtract 1 year because it isn't there birth date yet
            if (Today < WorkingDateOfBirth.AddYears(AgeInYears))
            {
                //subtract 1 year
                AgeInYears--;
            }

            //return the age
            return AgeInYears;
        }

    }

}
