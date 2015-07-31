using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Caching.BaseClass
{

    /// <summary>
    /// Holds the methods in Cache Base that we want untyped
    /// </summary>
    public static class CacheBase
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
            foreach (KeyValuePair<string, object> thisItemInCache in MemoryCache.Default)
            {
                //use yield result so we don't have to throw this guy in a list before returning it
                yield return thisItemInCache;
            }
        }

    }

}
