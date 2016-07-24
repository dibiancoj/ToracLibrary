
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static ToracLibrary.Core.Exceptions.ExceptionUtilities;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for exceptions utility methods
    /// </summary>
    public class ExceptionUtilityTest
    {

        #region Find Exception Type

        /// <summary>
        /// Test to find a specific exception from an error stack trace
        /// </summary>
        [Fact]
        public void FindExceptionTypeTest1()
        {
            try
            {
                //let's throw an exception and catch it below
                throw new FormatException("Test Format Exception");
            }
            catch (Exception ex)
            {
                //try to grab an argument null exception (should return null)
                Assert.Null(RetrieveExceptionType<ArgumentNullException>(ex));

                //try to grab a format exception (which we can find)
                Assert.NotNull(RetrieveExceptionType<FormatException>(ex));
            }
        }

        /// <summary>
        /// Test to find a specific exception from an error stack trace. This test will bubble up an exception to see if it can traverse the stack trace tree
        /// </summary>
        [Fact]
        public void FindExceptionTypeTest2()
        {
            try
            {
                //throw 2 exceptions
                throw new FormatException("Test Format Exception", new ArgumentNullException("Test Argument Null Exception"));
            }
            catch (Exception ex)
            {
                //we are going to create an exception tree to see if the code can traverse it
                var ExceptionTree = new ArgumentNullException("Test", ex);

                //we should be able to find the orig exception
                Assert.Null(RetrieveExceptionType<InvalidCastException>(ExceptionTree));

                //we should be able to find the format exception
                Assert.NotNull(RetrieveExceptionType<FormatException>(ExceptionTree));

                //this is the exception we just created, to create the tree, we should be able to find this
                Assert.NotNull(RetrieveExceptionType<ArgumentNullException>(ExceptionTree));
            }
        }

        #endregion

        #region Retrieve Exception History

        /// <summary>
        /// Test that we get all the exceptions that were raised in the exception tree. Method should traverse the tree and return every exception
        /// </summary>
        [Fact]
        public void RetrieveExceptionHistoryTest1()
        {
            try
            {
                //try to to convert a string to an int so we raise an error
                throw new ArgumentNullException("ExceptionTest");
            }
            catch (Exception ex)
            {
                //grab the error tree and check to make sure we only have 1 error
                Assert.Equal(1, RetrieveExceptionHistoryLazy(ex).Count());
            }
        }

        /// <summary>
        /// Test that we get all the exceptions that were raised in the exception tree. Method should traverse the tree and return every exception
        /// </summary>
        [Fact]
        public void RetrieveExceptionHistoryTest2()
        {
            try
            {
                //create the exception tree. Should be 2 exception
                throw new ArgumentNullException("Error", new ArgumentOutOfRangeException("Out Of Range"));
            }
            catch (Exception ex)
            {
                //let's make sure we get 2 errors back
                Assert.Equal(2, RetrieveExceptionHistoryLazy(ex).Count());
            }
        }

        #endregion

    }

}
