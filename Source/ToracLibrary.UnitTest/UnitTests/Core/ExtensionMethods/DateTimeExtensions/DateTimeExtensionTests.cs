using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.DateTimeExtensions;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to Date Time Extension Methods
    /// </summary>
    public class DateTimeExtensionTests
    {

        /// <summary>
        /// Unit test for DateTime.Between
        /// </summary>
        [Fact]
        public void DateTimeBetweenTest1()
        {
            Assert.True(new DateTime(2017, 1, 2).IsBetween(new DateTime(2017, 1, 1), new DateTime(2018, 1, 1)));
            Assert.True(new DateTime(2017, 1, 3).IsBetween(new DateTime(2017, 1, 1), new DateTime(2017, 1, 4)));

            Assert.False(new DateTime(2017, 1, 3).IsBetween(new DateTime(2017, 1, 5), new DateTime(2017, 1, 6)));

            Assert.Throws<ArgumentOutOfRangeException>(() => new DateTime(2017, 1, 3).IsBetween(new DateTime(2017, 1, 2), new DateTime(2017, 1, 1)));
        }

    }

}
