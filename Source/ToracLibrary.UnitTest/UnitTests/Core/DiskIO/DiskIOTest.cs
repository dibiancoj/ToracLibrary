using System;
using System.Linq;
using Xunit;
using System.IO;
using ToracLibrary.Core.DiskIO;
using System.Text;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Disk IO Tests
    /// </summary>
    public class DiskIOTest
    {

        #region Disk IO Unit Tests

        /// <summary>
        /// FileChecker.IsExecutable test (positive test)
        /// </summary>
        [InlineData("MZ")]
        [Theory]
        public void IsExecutablePositiveTest1(string FirstTwoBytesToTest)
        {
            Assert.True(FileChecker.IsExecutable(new MemoryStream(Encoding.ASCII.GetBytes(FirstTwoBytesToTest))));
        }

        /// <summary>
        /// FileChecker.IsExecutable test (negative test)
        /// </summary>
        [InlineData("123")]
        [InlineData("ab")]
        [InlineData("c")]
        [InlineData("ef")]
        [Theory]
        public void IsExecutableNegativeTest1(string FirstTwoBytesToTest)
        {
            Assert.False(FileChecker.IsExecutable(new MemoryStream(Encoding.ASCII.GetBytes(FirstTwoBytesToTest))));
        }

        #endregion

    }

}