using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Exceptions
{

    /// <summary>
    /// utilities to deal with exceptions
    /// </summary>
    public static class ExceptionUtilities
    {

        /// <summary>
        /// Looks through an exception and tries to find the first exception that is the same type as T...as it walks down the line of the exception tree
        /// </summary>
        /// <typeparam name="T">Type Of Exception To Look For</typeparam>
        /// <param name="ExceptionToLookIn">Exception To Look In To Find That Specific Exception Type</param>
        /// <returns>Exception If Found. Null If Not Found</returns>
        public static T RetrieveExceptionType<T>(Exception ExceptionToLookIn) where T : Exception
        {
            //example on how to call this
            //var foundSqlException = ExceptionTypeFinder.RetrieveExceptionType<System.Data.SqlClient.SqlException>(ex);

            //we are going to re-use the RetrieveExceptionHistory which will return all the exceptions in the tree.
            return RetrieveExceptionHistoryLazy(ExceptionToLookIn).OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Go through the exception tree and return a list of all the exceptions starting with the top exception by checking inner exception
        /// </summary>
        /// <param name="ExceptionToLookIn">Exception to traverse</param>
        /// <returns>list of exceptions. Uses yield return to bring back ienumerable.</returns>
        public static IEnumerable<Exception> RetrieveExceptionHistoryLazy(Exception ExceptionToLookIn)
        {
            //let's add the first exception
            yield return ExceptionToLookIn;

            //if we don't have an inner exception then we have nothing to traverse down the tree. so we will return right away
            if (ExceptionToLookIn.InnerException == null)
            {
                //just exit the method
                yield break;
            }

            //throw the exception into a variable
            Exception InnerExceptionHolder = ExceptionToLookIn;

            //let's keep looping until we find it or the inner exception is null
            while (InnerExceptionHolder.InnerException != null)
            {
                //let's set the variable to the inner exception now
                InnerExceptionHolder = InnerExceptionHolder.InnerException;

                //let's add this exception to the list
                yield return InnerExceptionHolder;
            }
        }

    }

}
