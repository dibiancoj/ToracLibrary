using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.SessionState.Cache
{

    /// <summary>
    /// Holds the internal model that goes into session which contains the necessary data to store the object in session with an expiration
    /// </summary>
    /// <typeparam name="T">Type of the object stored in the session cache</typeparam>
    internal class SessionStateCacheModel<T> where T : class
    {

        #region Constructor

        /// <summary>
        /// Constructor. Overload where you actually set an expiration
        /// </summary>
        /// <param name="CacheExpirationToSet"> Holds the expiration DateTime to set</param>
        /// <param name="CachedObjectToSet">Holds the object that is cached</param>
        public SessionStateCacheModel(DateTime? CacheExpirationToSet, T CachedObjectToSet)
        {
            //set the properties
            CacheExpiration = CacheExpirationToSet;
            CachedObject = CachedObjectToSet;
        }

        /// <summary>
        /// Constructor. Overload when you don't want an expiration
        /// </summary>
        /// <param name="CachedObjectToSet">Holds the object that is cached</param>
        public SessionStateCacheModel(T CachedObjectToSet)
            : this((int?)null, CachedObjectToSet)
        {
        }


        /// <summary>
        /// Constructor. Overload where you actually set an expiration
        /// </summary>
        /// <param name="CacheExpirationInSecondsToSet"> Holds the expiration in seconds until the cache session object is invalidated</param>
        /// <param name="CachedObjectToSet">Holds the object that is cached</param>
        public SessionStateCacheModel(int? CacheExpirationInSecondsToSet, T CachedObjectToSet)
        {
            //set the properties
            CacheExpiration = CalculateExpirationFromSeconds(CacheExpirationInSecondsToSet.Value);
            CachedObject = CachedObjectToSet;
        }

        #endregion

        #region Immutable Properties

        /// <summary>
        /// Holds the expiration in seconds until the cache session object is invalidated
        /// </summary>
        public DateTime? CacheExpiration { get; }

        /// <summary>
        /// Holds the object that is cached
        /// </summary>
        public T CachedObject { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Is this cache expired
        /// </summary>
        /// <returns>Result if cache is expired</returns>
        public bool CacheIsExpired()
        {
            //if we have a value...and the value is less then right now
            return CacheExpiration.HasValue && CacheExpiration.Value < DateTime.Now;
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
