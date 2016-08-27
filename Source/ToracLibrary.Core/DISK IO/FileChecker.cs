using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DiskIO
{

    /// <summary>
    /// Various file checker utilities
    /// </summary>
    public static class FileChecker
    {

        /// <summary>
        /// Determines if a file is an executable by looking at the first 2 bytes. Handles scenario's where the user modifies the file extension. 
        /// </summary>
        /// <param name="StreamToInspect">Stream to inspect to see if its a executable</param>
        /// <returns>Is it an executable</returns>]
        public static bool IsExecutable(Stream StreamToInspect)
        {
            //we only need to look at the first 2 bytes
            var FirstTwoBytes = new byte[2];

            //read the first 2 bytes from the stream
            StreamToInspect.Read(FirstTwoBytes, 0, 2);

            //now convert that to a string and compare it against MZ (which is an executable in windows)
            return Encoding.UTF8.GetString(FirstTwoBytes) == "MZ";
        }

    }

}
