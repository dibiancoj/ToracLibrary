﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExtensionMethods.ObjectExtensions
{

    /// <summary>
    /// Extension Methods For Objects
    /// </summary>
    public static class ObjectExtensionMethods
    {

        #region Casting

        /// <summary>
        /// Generic way to run object as...T. This way you don't need parenthesis when you need to cast it. Just a shorter sytnax instead of (myobject as T).Field. 
        /// </summary>
        /// <typeparam name="ToFrom">Object type to convert From</typeparam>
        /// <typeparam name="TTo">Object type to convert to</typeparam>
        /// <param name="ObjectToConvert">Object to convert</param>
        /// <returns>TTo or null if it can't cast</returns>
        public static TTo As<TTo>(this object ObjectToConvert)
            where TTo : class
        {
            return ObjectToConvert as TTo;
        }

        #endregion

        #region Single Object To List

        /// <summary>
        /// Pushes a single object to IEnumerable of that object type (T)
        /// </summary>
        /// <typeparam name="T">Type Of The Item Passed In</typeparam>
        /// <param name="ItemToPutInArray">Item To Push Into The IEnumerable</param>
        /// <returns>IEnumerable Of That Object Type, With The Item In The IEnumerable</returns>
        public static IEnumerable<T> ToIEnumerableLazy<T>(this T ItemToPutInArray)
        {
            //just return this item
            yield return ItemToPutInArray;
        }

        /// <summary>
        /// Pushes a single object to a list of that object type (T) and returns IList Of T
        /// </summary>
        /// <typeparam name="T">Type Of The Item Passed In</typeparam>
        /// <param name="ItemToPutInArray">Item To Push Into The IEnumerable</param>
        /// <returns>IList Of That Object Type, With The Item In The IList</returns>
        public static IList<T> ToIList<T>(this T ItemToPutInArray)
        {
            //instead of calling the overload which will create an iterator for no reason, we will just return a list
            return new List<T> { ItemToPutInArray };
        }

        #endregion

    }

}
