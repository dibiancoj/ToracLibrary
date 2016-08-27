using System;
using ToracLibrary.Core.AccountingPeriods;
using ToracLibrary.Core.AccountingPeriods.Exceptions;
using ToracLibrary.Graphics;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit tests for graphics. Whatever we can test
    /// </summary>
    public class GraphicsTest
    {

        #region Main Tests

        /// <summary>
        /// Test to make sure we can find the mime type
        /// </summary>
        [InlineData("image/jpeg")] //CompressImage uses this
        [InlineData("image/tiff")] //TiffCombiner uses this
        [Theory]
        public void GetEncoderTest1(string MimeTypeToFind)
        {
            //grab the result
            var Result = GraphicsCommonUtilities.GetEncoderInfo(MimeTypeToFind);

            //just make sure we can find the mime type
            Assert.NotNull(Result);

            //just make sure we get that mime type back
            Assert.Equal(MimeTypeToFind, Result.MimeType);
        }

        /// <summary>
        /// Test to make sure a not found items throws an error
        /// </summary>
        [InlineData("ShouldBeNotFound")]
        [Theory]
        public void GetEncoderTest2(string MimeTypeToFind)
        {
            //should throw an error
            Assert.Throws<InvalidOperationException>(() => GraphicsCommonUtilities.GetEncoderInfo(MimeTypeToFind));
        }

        #endregion

    }

}