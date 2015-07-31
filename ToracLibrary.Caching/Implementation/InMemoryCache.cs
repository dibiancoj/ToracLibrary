using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Caching.BaseClass;

namespace ToracLibrary.Caching
{

    //Documentation
    //public static class TestCacheHelper
    //{
    //    public static List<string> Get()
    //    {
    //        return new TestCache<List<string>>().Get();
    //    }
    //}

    //public class TestCache<T> : GenericCache<T>
    //{
    //    public TestCache()
    //    {
    //        CacheKey = "TestCache";
    //    }

    //    protected override string CacheKey { get; set; }

    //    protected override T GetDataFromDataSource()
    //    {
    //        var l = new List<string>();
    //        l.Add("1");
    //        l.Add("2");

    //        return GenericsConversion.ConvertValue<T>(l);
    //    }

    //    public T Get()
    //    {
    //        return GetCacheItem();
    //    }
    //}

    //**** don't use any async methods when getting the data source data. Because you are not aloud to await inside a lock. You will get freezing!!!

    /// <summary>
    /// In Memory Cache
    /// </summary>
    public abstract class InMemoryCache<T> : CacheBase<T>
    {

        #region Constructor

        /// <summary>
        /// Constructor Where The Absolute Expiration Date Is Not Set
        /// </summary>
        /// <param name="KeyForCache">Cache Key To Use For This Cache Item</param>
        protected InMemoryCache(string KeyForCache)
            : base(KeyForCache, null)
        {
        }

        /// <summary>
        /// Constructor Where The Absolute Expiration Date Is Set Based On The thisAbsoluteExpirationSpan Parameter
        /// </summary>
        /// <param name="KeyForCache">Cache Key To Use For This Cache Item</param>
        /// <param name="AbsoluteExpirationSpanOnCache">Holds the max amount of time the item in the cache is valid for (AbsoluteExpiration). It gets calculated from the time its put in the cache plus the timespan</param>
        protected InMemoryCache(string KeyForCache, TimeSpan AbsoluteExpirationSpanOnCache)
            : base(KeyForCache, AbsoluteExpirationSpanOnCache)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an item to the cache (only used when we can't find the item in the cache)
        /// </summary>
        /// <param name="ItemToAddToCache">Item to add to the cache</param>
        protected override void AddItemToCache(T ItemToAddToCache)
        {
            //put it into the cache now (the expiration date is the time now plus the _ExpirationLength timespan)
            MemoryCache.Default.Add(new CacheItem(CacheKey, ItemToAddToCache),
                                    new CacheItemPolicy()
                                    {
                                        AbsoluteExpiration = CalculateAbsoluteExpirationDate(AbsoluteExpirationLength)
                                    });
        }

        #endregion

    }

}
