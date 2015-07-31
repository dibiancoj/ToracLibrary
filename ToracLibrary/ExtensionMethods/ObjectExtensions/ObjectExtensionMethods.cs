using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.ExtensionMethods.ObjectExtensions
{

    /// <summary>
    /// Extension Methods For Objects
    /// </summary>
    public static class ObjectExtensionMethods
    {

        #region Single Object To List

        /// <summary>
        /// Pushes a single object to IEnumerable of that object type (T)
        /// </summary>
        /// <typeparam name="T">Type Of The Item Passed In</typeparam>
        /// <param name="ItemToPutInArray">Item To Push Into The IEnumerable</param>
        /// <returns>IEnumerable Of That Object Type, With The Item In The IEnumerable</returns>
        public static IEnumerable<T> ToIEnumerable<T>(this T ItemToPutInArray)
        {
            //to call this method
            //ClaimTeamToSave.ToIEnumerable();

            //calling the ToIList To Share The Functionality And Not Duplicate Code
            return ItemToPutInArray.ToIList();
        }

        /// <summary>
        /// Pushes a single object to a list of that object type (T) and returns IList Of T
        /// </summary>
        /// <typeparam name="T">Type Of The Item Passed In</typeparam>
        /// <param name="ItemToPutInArray">Item To Push Into The IEnumerable</param>
        /// <returns>IList Of That Object Type, With The Item In The IList</returns>
        public static IList<T> ToIList<T>(this T ItemToPutInArray)
        {
            //to call this method
            //ClaimTeamToSave.ToIList();

            //because it's an extension method of T...it's basically an object type because this will show up for every type when the namespace is imported

            //return the new array with the item in it
            return new List<T> { ItemToPutInArray };
        }

        #endregion

    }

}
