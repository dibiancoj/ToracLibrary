using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Caching
{

    /// <summary>
    /// Common interface so we can use DI for all the caching types
    /// </summary>
    /// <typeparam name="T">Type of the record cached</typeparam>
    public interface ICacheImplementation<T>
    {

        #region Properties

        /// <summary>
        /// What is the key to the cache for this item
        /// </summary>
        string CacheKey { get; }

        /// <summary>
        /// If we can't find the data in the cache, then we need to call this method to get it from the data source
        /// </summary>
        Func<T> FunctionToGetDataFromSource { get; }

        #endregion

        #region Methods

        /// <summary>
        /// get the cached item...if it's not there it goes and grab's the data and puts it in the cache, then returns it
        /// </summary>
        /// <returns>Object</returns>
        /// <remarks>Can be overriden if you need some other functionality</remarks>
        T GetCacheItem();

        /// <summary>
        /// Add an item to the cache (only used when we can't find the item in the cache)
        /// </summary>
        /// <param name="ItemToAddToCache">Item to add to the cache</param>
        /// <remarks>Virtual so sql cache dep can piggy back off of this</remarks>
        void AddItemToCache(T ItemToAddToCache);

        /// <summary>
        /// Removes The Item From The Cache
        /// </summary>
        /// <returns>Result</returns>
        bool RemoveCacheItem();

        /// <summary>
        /// Refresh the cached item. Clear it from the cache, then Grab it from the data source and put it back into cache
        /// </summary>
        /// <returns>The Updated / Refreshed Item</returns>
        T RefreshCacheItem();

        #endregion

    }

}
