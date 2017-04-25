using System;
using ToracLibrary.Core.Readers;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Test for a buffered string reader
    /// </summary>
    public class BufferedStringReaderTest
    {

        /// <summary>
        /// Ensure that you can't pass in a null string value
        /// </summary>
        [Fact]
        public void BufferedStringNullValueTest1()
        {
            //null value
            Assert.Throws<ArgumentNullException>(() => new BufferedStringReader(null));
        }

        /// <summary>
        /// Ensure if read is the first action that it reads
        /// </summary>
        [Fact]
        public void BufferedStringReadRightAwayTest1()
        {
            //string to test with
            const string TestString = "test";

            //create the reader
            using (var ReaderToUse = new BufferedStringReader(TestString))
            {
                //read the value
                Assert.Equal(TestString[0], (char)ReaderToUse.Read());

                //just run a peak to verify we moved the reader
                Assert.Equal(TestString[1], (char)ReaderToUse.Peek(0));
            }
        }

        /// <summary>
        /// Test the buffered string reader. Simple test with multiple scenarios
        /// </summary>
        [Fact]
        public void BufferedStringReaderTest1()
        {
            //string to test with
            const string TestString = "test";

            //no more characters
            const string NoMoreCharacters = "-1";

            //create the reader
            using (var ReaderToUse = new BufferedStringReader(TestString))
            {
                //peak at the first character (should be "t")
                Assert.Equal(TestString[0], (char)ReaderToUse.Peek(0));

                //look at the next character (should be "e")
                Assert.Equal(TestString[1], (char)ReaderToUse.Peek(1));

                //now peek at the 3rd character (should be "s")
                Assert.Equal(TestString[2], (char)ReaderToUse.Peek(2));

                //go read the first character "t"
                Assert.Equal(TestString[0], (char)ReaderToUse.Read());

                //peek now...since we read t, then it should be "e"
                Assert.Equal(TestString[1], (char)ReaderToUse.Peek(0));

                //peek again after the read. Should be "s" now.
                Assert.Equal(TestString[2], (char)ReaderToUse.Peek(1));

                //now let's read and test the End of File
                ReaderToUse.Read();
                ReaderToUse.Read();
                ReaderToUse.Read();
                ReaderToUse.Read();

                //make sure we return -1 on all commands
                Assert.Equal(NoMoreCharacters, ReaderToUse.Peek(0).ToString());
                Assert.Equal(NoMoreCharacters, ReaderToUse.Peek(0).ToString());

                //test the End of file...when passing in > 0 at the end
                Assert.Equal(NoMoreCharacters, ReaderToUse.Peek(1).ToString());
                Assert.Equal(NoMoreCharacters, ReaderToUse.Peek(1).ToString());

                //test the read method now
                Assert.Equal(NoMoreCharacters, ReaderToUse.Read().ToString());
                Assert.Equal(NoMoreCharacters, ReaderToUse.Read().ToString());
            }
        }

    }

}