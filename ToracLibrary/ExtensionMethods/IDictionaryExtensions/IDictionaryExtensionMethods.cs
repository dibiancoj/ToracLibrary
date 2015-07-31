﻿using System;
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

    }

}
