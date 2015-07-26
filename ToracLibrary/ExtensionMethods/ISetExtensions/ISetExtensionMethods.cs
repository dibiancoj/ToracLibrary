using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.ExtensionMethods.ISetExtensions
{

    /// <summary>
    /// Extension Methods For ISet (HashSets)
    /// </summary>
    public static class ISetExtensionMethods
    {

        #region Add Range Lazy

        /// <summary>
        /// Add Range Lazy Result
        /// </summary>
        /// <typeparam name="T">Type of the hashset</typeparam>
        /// <remarks>Class is immutable</remarks>
        public class AddRangeLazyResult<T>
        {

            #region Constructor

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="ItemAttemptedToBeAddedToHashSet">The attempted item that we tried to add to the hashset</param>
            /// <param name="WasItAddedToTheHashSet">Was it successful in being added to the hashset</param>
            public AddRangeLazyResult(T ItemAttemptedToBeAddedToHashSet, bool WasItAddedToTheHashSet)
            {
                AttemptedItemToBeAdded = ItemAttemptedToBeAddedToHashSet;
                SuccesfullyAddedToHashSet = WasItAddedToTheHashSet;
            }

            #endregion

            #region Readonly Properties

            /// <summary>
            /// The attempted item that we tried to add to the hashset
            /// </summary>
            public T AttemptedItemToBeAdded { get; }

            /// <summary>
            /// Was it successful in being added to the hashset
            /// </summary>
            public bool SuccesfullyAddedToHashSet { get; }

            #endregion

        }

        /// <summary>
        /// Add multiple items to a hashset at once. You can use UnionWith but it doesn't flow naturally because you could have items that were not inserted. HashSet return's false
        /// </summary>
        /// <typeparam name="T">Working HashSet</typeparam>
        /// <param name="WorkingHashSet">Hashset to add the items too</param>
        /// <param name="ItemsToAdd">Items To Add</param>
        /// <returns>IEnumerable Of Tuple Of T And Bool. T is the item trying to be added. The boolean is if it was added to the hashset. Uses yield return</returns>
        public static IEnumerable<AddRangeLazyResult<T>> AddRangeLazy<T>(this ISet<T> WorkingHashSet, IEnumerable<T> ItemsToAdd)
        {
            //make sure we have items first
            if (ItemsToAdd.AnyWithNullCheck())
            {
                //now just loop through and add them
                foreach (T ItemToAdd in ItemsToAdd)
                {
                    //add the item to the hashset and return the result
                    yield return new AddRangeLazyResult<T>(ItemToAdd, WorkingHashSet.Add(ItemToAdd));
                }
            }
        }

        #endregion

    }

}
