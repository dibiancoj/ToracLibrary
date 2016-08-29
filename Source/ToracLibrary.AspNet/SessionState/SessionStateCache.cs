using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.SessionState
{

    /// <summary>
    /// Use session state like a cache
    /// </summary>
    public class SessionStateCache
    {

        #region Public Methods

        ///<summary>
        /// Get the data from session or reload it from the data source with no cache expiration.
        /// </summary>
        /// <typeparam name="T">Type of the record stored in cache</typeparam>
        /// <param name="SessionKey">session key to retrieve it by</param>
        /// <param name="ReloadDataFromSource">if not found in session, how do we reload it</param>
        /// <returns>Item from either session or the data data source</returns>
        public static T GetFromSessionCache<T>(string SessionKey, Func<T> ReloadDataFromSource) where T : class
        {
            //use the overload
            return GetFromSessionCache<T>(SessionKey, ReloadDataFromSource, null);
        }

        /// <summary>
        /// Get the data from session or reload it from the data source with a cache expiration
        /// </summary>
        /// <typeparam name="T">Type of the record stored in cache</typeparam>
        /// <param name="SessionKey">session key to retrieve it by</param>
        /// <param name="ReloadDataFromSource">if not found in session, how do we reload it</param>
        /// <param name="CacheExpirationInSeconds">How long before the cache expires in seconds</param>
        /// <returns>Item from either session or the data data source</returns>
        public static T GetFromSessionCache<T>(string SessionKey, Func<T> ReloadDataFromSource, int? CacheExpirationInSeconds) where T : class
        {
            //try to get it from session
            var TryToGetFromSession = System.Web.HttpContext.Current.Session[SessionKey] as Tuple<DateTime?, T>;

            //do we have it in session?
            if (TryToGetFromSession == null || TryToGetFromSession.Item1 < DateTime.Now)
            {
                //don't have it in session. grab from source
                TryToGetFromSession = new Tuple<DateTime?, T>(CalculateExpirationFromSeconds(CacheExpirationInSeconds), ReloadDataFromSource());

                //stick it in session
                System.Web.HttpContext.Current.Session[SessionKey] = TryToGetFromSession;
            }

            //return just whatever the object they are looking for
            return TryToGetFromSession.Item2;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calculate the expiration date with a value in seconds
        /// </summary>
        /// <param name="CacheExpirationInSeconds">How long before the cache expires in seconds</param>
        /// <returns>The expiration in date time</returns>
        private static DateTime? CalculateExpirationFromSeconds(int? CacheExpirationInSeconds)
        {
            //do we have a value?
            if (!CacheExpirationInSeconds.HasValue)
            {
                //no value...just return null
                return null;
            }

            //add x amount of seconds
            return DateTime.Now.AddSeconds(CacheExpirationInSeconds.Value);
        }

        #endregion

    }

}
