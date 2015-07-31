using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Caching.BaseClass
{

    /// <summary>
    /// Base Abstract Class For Caching. Implementation Must Implement All Abstract Members
    /// </summary>
    /// <typeparam name="T">Type Of Data That Is Stored For This Key</typeparam>
    /// <remarks>Properties are immutable</remarks>
    public abstract class CacheBase<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CacheItemKey">Cache key to use for this cache item</param>
        /// <param name="ExpireCacheLength">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). It gets calculated from the time its put in the cache plus the timespan</param>
        protected CacheBase(string CacheItemKey, TimeSpan? ExpireCacheLength)
        {
            //an internal constructor will give a user an error if they try to inherit from CacheBaseDirectly
            //we want a developer to inherit off either Generic Cache Or Sql Cache Dep. and not this class

            //make sure the cache key is not null
            if (string.IsNullOrEmpty(CacheItemKey))
            {
                throw new ArgumentNullException("Cache Key Can't Be Null");
            }

            //set the cache key
            CacheKey = CacheItemKey;

            //set the absolute expiration span
            AbsoluteExpirationLength = ExpireCacheLength;

            //create the cache lock
            CacheLock = new object();
        }

        #endregion

        #region Properties

        /// <summary>
        /// What is the key to the cache for this item
        /// </summary>
        /// <remarks>Just a getter because the variable is immutable</remarks>
        public string CacheKey { get; }

        /// <summary>
        /// Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). Default Is Null (No Expiration Date). It gets calculated from the time its put in the cache plus the timespan
        /// </summary>
        public TimeSpan? AbsoluteExpirationLength { get; }

        /// <summary>
        /// Lock Mechanism so we don't have any collisions when we have 2 users trying to retrieve the data source if the cache item is null
        /// </summary>
        private object CacheLock { get; }

        #endregion

        #region Abstract Members

        /// <summary>
        /// Can't find data or it's expired...let's go to the data source and populate the cache
        /// </summary>
        /// <returns>T</returns>
        /// <remarks>Protected So A User Can't Run It By Mistake. *** Don't run this in async. Or the method that gets the data. We need to use a lock in the cache. So you won't be able to await it. This will cause locking. Just grab the data normally!</remarks>
        protected abstract T GetDataFromDataSource();

        /// <summary>
        /// Add an item to the cache. Enables sql cache dep. and generic cache to share this class
        /// </summary>
        /// <param name="ItemToAddToCache">Item To Add To The Cache</param>
        protected abstract void AddItemToCache(T ItemToAddToCache);

        #endregion

        #region Base Methods

        /// <summary>
        /// get the cached item...if it's not there it goes and grab's the data and puts it in the cache, then returns it
        /// </summary>
        /// <returns>Object</returns>
        /// <remarks>Can be overriden if you need some other functionality</remarks>
        public virtual T GetCacheItem()
        {
            //try to grab the item from the cache (without any locks first)
            //we don't cast it to T...because if you have a cache of structs..Let's say an Int...you can't cast it to T because the value is null.
            object TryToGetItemFromCache = MemoryCache.Default[CacheKey];

            //check to see if the item is null
            if (TryToGetItemFromCache == null)
            {
                //we didn't find it in the cache...
                //put a lock on this call...(if another thread is already running this call, it will wait)
                lock (CacheLock)
                {
                    //since we finally got the lock...
                    //we could have been waiting for another thread to finish getting the data source. So we are gonna try and grab the cache again (double-check locking)
                    //example Thread A Was running this, thread b came in and couldn't get the lock and had to wait until A was done...A finally completed...so B gets to go...by time that happend A already has put it in cache...So b can just grab it from the cache now
                    TryToGetItemFromCache = MemoryCache.Default[CacheKey];

                    //check to see if we have the data source now
                    if (TryToGetItemFromCache == null)
                    {
                        //we didn't find it in the cache...let's grab it from the datasource
                        TryToGetItemFromCache = GetDataFromDataSource();

                        //anything that is null will fail when you try to put it in...check to make sure it's not null
                        if (TryToGetItemFromCache != null)
                        {
                            //we need to add the item to the cache. This method is abstract so any type of caching can support it
                            //example (Generic Cache Add's it to it's cache, SqlCacheDep add's it to his)
                            AddItemToCache((T)TryToGetItemFromCache);
                        }
                    }
                }
            }

            //we have the item (either from cache or from the data source) let's return it now
            //now is when we cast it to T because we have a value...or we can cast the null into a value type
            return (T)TryToGetItemFromCache;
        }

        /// <summary>
        /// Removes The Item From The Cache
        /// </summary>
        /// <returns>Result</returns>
        public virtual bool RemoveCacheItem()
        {
            //remove the item from the cache
            MemoryCache.Default.Remove(CacheKey);

            //return the result
            return true;
        }

        /// <summary>
        /// Refresh the cached item. Clear it from the cache, then Grab it from the data source and put it back into cache
        /// </summary>
        /// <returns>The Updated / Refreshed Item</returns>
        public virtual T RefreshCacheItem()
        {
            //remove the cache item
            RemoveCacheItem();

            //now get get the data and rebuild it
            return GetCacheItem();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets all the keys and their object that are in the cache at the present time 
        /// </summary>
        /// <returns>list of keys and the item in the cache. Lazy loads the return list using yield result</returns>
        public static IEnumerable<KeyValuePair<string, object>> GetAllItemsInCacheLazy()
        {
            //* we have it in this method and the untyped class so a person can call this method without having to create something with T.
            //this way an admin screen can get all the entries without having to create an entire implementation. They also have the luxury if they have 
            //an implementation to get it that way too

            //use the untyped overload
            return CacheBase.GetAllItemsInCacheLazy();
        }

        /// <summary>
        /// Calculates the absolute expiration date for the cache
        /// </summary>
        /// <param name="ExpirationLength">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). Default Is Null (No Expiration Date). It gets calculated from the time its put in the cache plus the timespan</param>
        /// <returns>Expiration date offset to set on the cache</returns>
        /// <remarks>Right now we don't need this to be public. No harm if it does indeed become public</remarks>
        protected static DateTimeOffset CalculateAbsoluteExpirationDate(TimeSpan? ExpirationLength)
        {
            //first check to make sure we have expiration length field that is not null
            if (ExpirationLength.HasValue)
            {
                //we have a value...so calculate it from now
                return DateTime.Now.Add(ExpirationLength.Value);
            }

            //we don't have an expiration, return the max value
            return ObjectCache.InfiniteAbsoluteExpiration;
        }

        #endregion

    }

}
