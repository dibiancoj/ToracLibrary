using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ToracLibrary.UnitTest.Framework
{

    /// <summary>
    /// Any generic method we may need
    /// </summary>
    internal static class FrameworkHelperMethods
    {

        /// <summary>
        /// Helper method to grab the field for a specified name
        /// </summary>
        /// <param name="ClassType">Class which has the type we need to retrieve</param>
        /// <param name="FieldName">field name to retrieve</param>
        /// <returns>format string to use</returns>
        internal static string GetPrivateFieldValue(Type ClassType, string FieldName)
        {
            return ClassType.GetField(FieldName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null).ToString();
        }

    }

}
