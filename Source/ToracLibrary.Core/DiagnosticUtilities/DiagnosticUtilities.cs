using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Diagnostic
{

    /// <summary>
    /// Random stuff used for troubleshooting stuff
    /// </summary>
    public static class DiagnosticUtilities
    {

        /// <summary>
        /// Run a spin and wait until the timespan passed in has expired
        /// </summary>
        /// <param name="TimeSpanToSpinUntil">Time span to spin until. If you pass in 5 seconds then we will spin until that expires</param>
        public static void SpinWaitUntilTimespan(TimeSpan TimeSpanToSpinUntil)
        {
            //figure out the time we need to spin until
            DateTime SpinUntil = DateTime.Now.Add(TimeSpanToSpinUntil);

            //Spin until we are past the spin until variable
            SpinWait.SpinUntil(() => DateTime.Now > SpinUntil);
        }

    }

}
