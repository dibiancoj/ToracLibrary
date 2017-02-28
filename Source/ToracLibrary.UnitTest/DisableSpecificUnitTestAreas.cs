using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.UnitTest
{

    /// <summary>
    /// Class to turn on or off specific unit tests
    /// </summary>
    public static class DisableSpecificUnitTestAreas
    {

        /// <summary>
        /// Holds the reason for not running the redis tests. Flip this to a blank string to run all the tests. This way you don't have to modify each attribute
        /// </summary>
        //internal const string DatabaseAvailableForUnitTestFlag = "DatabaseNotAvailable";
        internal const string DatabaseAvailableForUnitTestFlag = "";

        /// <summary>
        /// Holds the reason for not running the redis tests. Flip this to a blank string to run all the tests. This way you don't have to modify each attribute
        /// </summary>
        internal const string TurnOnOffRedisTestFlag = "RedisServerNotLoaded";
        //internal const string TurnOnOffFlag = "";



    }

}
