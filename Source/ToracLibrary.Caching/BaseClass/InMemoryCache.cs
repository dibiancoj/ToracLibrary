using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Caching
{

    /// <summary>
    /// Base Class For Caching. 
    /// </summary>
    /// <typeparam name="T">Type Of Data That Is Stored For This Key</typeparam>
    /// <remarks>Properties are immutable</remarks>
    public class InMemoryCache<T>: ICacheImplementation<T>
    {

        //**** don't use any async methods when getting the data source data. Because you are not aloud to await inside a lock. You will get freezing!!!

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CacheImplementation">The type of cache this base class will use cache the data and implement</param>
        /// <param name="GetFromDataSource">Method to get the data if we can't find it in the cache</param>
        public InMemoryCache(string KeyForCache, Func<T> GetFromDataSource)
            : this(KeyForCache, GetFromDataSource, new TimeSpan?())
        {

        }

        /// <summary>
        /// Overload constructor helper
        /// </summary>
        /// <param name="CacheImplementation">The type of cache this base class will use cache the data and implement</param>
        /// <param name="GetFromDataSource">Method to get the data if we can't find it in the cache</param>
        /// <param name="AbsoluteExpirationSpanOnCache">How long until we expire the cache</param>
        public InMemoryCache(string KeyForCache, Func<T> GetFromDataSource, TimeSpan AbsoluteExpirationSpanOnCache)
               : this(KeyForCache, GetFromDataSource, new TimeSpan?(AbsoluteExpirationSpanOnCache))
        {
        }

        /// <summary>
        /// Overload constructor helper
        /// </summary>
        /// <param name="CacheImplementation">The type of cache this base class will use cache the data and implement</param>
        /// <param name="GetFromDataSource">Method to get the data if we can't find it in the cache</param>
        /// <param name="AbsoluteExpirationSpanOnCache">How long until we expire the cache</param>
        protected InMemoryCache(string KeyForCache, Func<T> GetFromDataSource, TimeSpan? AbsoluteExpirationSpanOnCache)
        {
            //an internal constructor will give a user an error if they try to inherit from CacheBaseDirectly
            //we want a developer to inherit off either Generic Cache Or Sql Cache Dep. and not this class

            //make sure the cache key is not null
            if (string.IsNullOrEmpty(KeyForCache))
            {
                throw new ArgumentNullException("Cache Key Can't Be Null");
            }

            //set the cache key
            CacheKey = KeyForCache;

            //set the function if we can't get the data from the data source
            FunctionToGetDataFromSource = GetFromDataSource;

            //set the expiration
            AbsoluteExpirationLength = AbsoluteExpirationSpanOnCache;

            //create the cache lock
            CacheLock = new object();
        }

        #endregion

        #region Properties

        /// <summary>
        /// What is the key to the cache for this item
        /// </summary>
        public string CacheKey { get; }

        /// <summary>
        /// Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). Default Is Null (No Expiration Date). It gets calculated from the time its put in the cache plus the timespan
        /// </summary>
        public TimeSpan? AbsoluteExpirationLength { get; }

        /// <summary>
        /// If we can't find the data in the cache, then we need to call this method to get it from the data source
        /// </summary>
        public Func<T> FunctionToGetDataFromSource { get; }

        /// <summary>
        /// Lock Mechanism so we don't have any collisions when we have 2 users trying to retrieve the data source if the cache item is null
        /// </summary>
        private object CacheLock { get; }

        #endregion

        #region Methods

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
                        //we didn't find it in the cache...let's grab it from the datasource.
                        TryToGetItemFromCache = FunctionToGetDataFromSource();

                        //anything that is null will fail when you try to put it in...check to make sure it's not null
                        if (TryToGetItemFromCache != null)
                        {
                            //we need to add the item to the cache, each cache type knows how to add it...so add it now to the cache
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
        /// Add an item to the cache (only used when we can't find the item in the cache)
        /// </summary>
        /// <param name="ItemToAddToCache">Item to add to the cache</param>
        /// <remarks>Virtual so sql cache dep can piggy back off of this</remarks>
        public virtual void AddItemToCache(T ItemToAddToCache)
        {
            //put it into the cache now (the expiration date is the time now plus the _ExpirationLength timespan)
            MemoryCache.Default.Add(new CacheItem(CacheKey, ItemToAddToCache),
                                    new CacheItemPolicy()
                                    {
                                        AbsoluteExpiration = InMemoryCache.CalculateAbsoluteExpirationDate(AbsoluteExpirationLength)
                                    });
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

    }

}
