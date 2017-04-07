using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExtensionMethods.IDictionaryExtensions
{

    /// <summary>
    /// Extension Methods For IDictionary
    /// </summary>
    public static class IDictionaryExtensionMethods
    {

        //I was going to add AddOrUpdate but you can just use the dictionary init to do that. ie: MyDictionary["Key"] = AddOrUpdateObject

        /// <summary>
        /// Tries to add an item to the dictionary if it doesn't exist already. If it exists, it will just return false
        /// </summary>
        /// <typeparam name="TKey">Type Of The Key Of The Dictionary</typeparam>
        /// <typeparam name="TValue">Type Of The Value Of The Dictionary</typeparam>
        /// <param name="DictionaryToUse">Dictionary to try to add the item too</param>
        /// <param name="KeyToCheck">Key to check if the item exists</param>
        /// <param name="ValueToAdd">Value to add if it the key doesn't already exist in the dictionary</param>
        /// <returns>boolean. True if the item was not found and was addd (works like the hashset.add). False if the value was found and it was not added to the dictionary</returns>
        public static bool TryAdd<TValue, TKey>(this IDictionary<TKey, TValue> DictionaryToUse, TKey KeyToCheck, TValue ValueToAdd)
        {
            //this method is just shorthand and will save some typing

            //let's try to get it now
            if (DictionaryToUse.ContainsKey(KeyToCheck))
            {
                //value exists..so at this point, just return false because we aren't adding it to the dictionary
                return false;
            }

            //the value doesn't exist in the dictionary, so add it
            DictionaryToUse.Add(KeyToCheck, ValueToAdd);

            //return the positive result
            return true;
        }

        /// <summary>
        /// Try to get the item in the dictionary. This cleans up TryGetValue syntax which makes you specify an out parameter. This will return a TValue that will be null if it's not in the dictionary
        /// </summary>
        /// <typeparam name="TKey">Type Of The Key Of The Dictionary</typeparam>
        /// <typeparam name="TValue">Type Of The Value Of The Dictionary</typeparam>
        /// <param name="DictionaryToUse">Dictionary to try to add the item too</param>
        /// <param name="KeyToCheck">Key to try to get from the dictionary</param>
        /// <returns>TValue. default(TValue) if not found. You can compare result.Equals(default(TValue)</returns>
        public static TValue TryGet<TValue, TKey>(this IDictionary<TKey, TValue> DictionaryToUse, TKey KeyToTryToRetrieve)
        {
            //go try to find the object in the dictionary
            if (DictionaryToUse.TryGetValue(KeyToTryToRetrieve, out var ValueToTryToFetch))
            {
                //we found the item in the dictionary, return it
                return ValueToTryToFetch;
            }

            //we never found it...just return null
            return default(TValue);
        }

        /// <summary>
        /// try to get the value from the dictionary first. if not found then go create it. Useful for cache like dictionary. Currently its in the concurrent dictionary
        /// </summary>
        /// <typeparam name="TKey">Type Of The Key Of The Dictionary</typeparam>
        /// <typeparam name="TValue">Type Of The Value Of The Dictionary</typeparam>
        /// <param name="DictionaryToUse">Dictionary to try to get the item or add the item</param>
        /// <param name="KeyToTryToRetrieve">Key to try to retrieve with</param>
        /// <param name="ValueCreator">func that creates the value if not found. This value will be inserted into the dictionary after it is created, then returned</param>
        /// <returns>TValue. Either from dictionary lookup or from the value creator</returns>
        public static TValue GetOrAdd<TValue, TKey>(this IDictionary<TKey, TValue> DictionaryToUse, TKey KeyToTryToRetrieve, Func<TValue> ValueCreator)
        {
            //go try to find the object in the dictionary
            if (DictionaryToUse.TryGetValue(KeyToTryToRetrieve, out var ValueToTryToFetch))
            {
                //we found the item in the dictionary, return it
                return ValueToTryToFetch;
            }

            //we don't have it...so go create it
            TValue CreatedValue = ValueCreator();

            //put it in the dictionary
            DictionaryToUse.Add(KeyToTryToRetrieve, CreatedValue);

            //return the value now
            return CreatedValue;
        }

    }

}
