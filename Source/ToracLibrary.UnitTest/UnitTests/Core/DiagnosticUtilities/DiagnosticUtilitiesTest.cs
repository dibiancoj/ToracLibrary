using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.Core.Diagnostic;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to Diagnostic Utilities methods
    /// </summary>
    public class DiagnosticUtilitiesTest
    {

        #region Months Between 2 Dates

        /// <summary>
        /// Test SpinWaitUntilTimespan
        /// </summary>
        [Fact]
        public void SpinWaitUntilTimespanTest1()
        {
            //grab now
            DateTime Now = DateTime.Now;

            //spin until then
            DiagnosticUtilities.SpinWaitUntilTimespan(new TimeSpan(0, 0, 2));

            //grab the current time
            var TimeNow = DateTime.Now.Subtract(Now).Seconds;

            //give a little bit of a buffer to finish..so check 3 to 3.75 seconds
            Assert.True(TimeNow >= 2 && TimeNow < 2.75);
        }

        #endregion  

    }

}