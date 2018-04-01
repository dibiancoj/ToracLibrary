using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ToracLibrary.Caching
{

    /// <summary>
    /// Holds the methods in Cache Base that we want untyped
    /// </summary>
    public static class InMemoryCache
    {

        /// <summary>
        /// Gets all the keys and their object that are in the cache at the present time 
        /// </summary>
        /// <returns>list of keys and the item in the cache. Lazy loads the return list using yield result</returns>
        public static IEnumerable<KeyValuePair<string, object>> GetAllItemsInCacheLazy()
        {
            //* we have it in this method and the untyped class so a person can call this method without having to create something with T.
            //this way an admin screen can get all the entries without having to create an entire implementation. They also have the luxury if they have 
            //an implementation to get it that way too

            //loop through the cache items
            foreach (var ItemInCache in MemoryCache.Default)
            {
                //use yield result so we don't have to throw this guy in a list before returning it
                yield return ItemInCache;
            }
        }

        /// <summary>
        /// Calculates the absolute expiration date for the cache
        /// </summary>
        /// <param name="ExpirationLength">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). Default Is Null (No Expiration Date). It gets calculated from the time its put in the cache plus the timespan</param>
        /// <returns>Expiration date offset to set on the cache</returns>
        /// <remarks>Right now we don't need this to be public. No harm if it does indeed become public</remarks>
        public static DateTimeOffset CalculateAbsoluteExpirationDate(TimeSpan? ExpirationLength)
        {
            //first check to make sure we have expiration length field that is not null
            return ExpirationLength.HasValue ?

                //we have a value...so calculate it from now
                DateTime.Now.Add(ExpirationLength.Value) :

                //we don't have an expiration, return the max value
                ObjectCache.InfiniteAbsoluteExpiration;
        }

    }

}
