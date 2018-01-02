using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions
{

    /// <summary>
    /// Extension Methods For IEnumerable
    /// </summary>
    public static class IEnumerableExtensionMethods
    {

        #region Untyped IEnumerable

        #region Count

        /// <summary>
        /// Counts the number of items in the collection when it can't be typed into IEnumerableOfT
        /// </summary>
        /// <param name="CollectionToCount">Collection To Count</param>
        /// <returns>Number Of Items In The Collection</returns>
        public static int Count(this IEnumerable CollectionToCount)
        {
            //let's try to cast it first so we don't have to loop through the collection. This will help with performance on large collections
            if (CollectionToCount is ICollection TryToCastAsCollection)
            {
                //it's some sort of collection...let's return it because its a property
                return TryToCastAsCollection.Count;
            }

            //we weren't able to cast it...let's go the long way and loop through it

            //grab the enumerator
            IEnumerator CollectionEnumerator = CollectionToCount.GetEnumerator();

            //holds the tally of how many items we have
            int CountOfItems = 0;

            //keep looping until we reach the end
            while (CollectionEnumerator.MoveNext())
            {
                //increase the tally
                CountOfItems++;
            }

            //return the count
            return CountOfItems;
        }

        #endregion

        #endregion

        #region Typed IEnumerable

        #region Any With Null Check

        /// <summary>
        /// Checks To See If The Collection Has Any Items (Same Idea As The Any Extension Method Of Off IEnumerable). Only Difference Is, If The Collection Is Null It Returns False Instead Of Throwing An Error
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable</typeparam>
        /// <param name="Collection">Collection To Check Against</param>
        /// <returns>If The Item Has Any Records</returns>
        public static bool AnyWithNullCheck<T>(this IEnumerable<T> Collection)
        {
            //use the overload passing in null
            return Collection.AnyWithNullCheck(null);
        }

        /// <summary>
        /// Checks To See If The Collection Has Any Items (Same Idea As The Any Extension Method Of Off IEnumerable). Only Difference Is, If The Collection Is Null It Returns False Instead Of Throwing An Error
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable</typeparam>
        /// <param name="Collection">Collection To Check Against</param>
        /// <param name="Predicate">Predicate To Query The Collection With A Where Before We Determine If Any Exist</param>
        /// <returns>If The Item Has Any Records</returns>
        public static bool AnyWithNullCheck<T>(this IEnumerable<T> Collection, Func<T, bool> Predicate)
        {
            //if it's null then return false
            if (Collection == null)
            {
                return false;
            }

            //use the any method with the overload
            return Predicate == null ?
                    Collection.Any() :
                    Collection.Any(Predicate);
        }

        #endregion

        #region Coalesce

        /// <summary>
        /// Finds the first item that is not the default value.
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable</typeparam>
        /// <param name="Collection">Collection To Check Against</param>
        /// <returns>first item that is not the default value. If no items are returned then the default value of T will be returned.</returns>
        public static T Coalesce<T>(this IEnumerable<T> Collection)
        {
            //if you don't have a list you would just do item1 ?? item2 ?? item3 ?? item4...
            //using the Equals so we can handle structs instead of just checking for nulls
            return Collection.FirstOrDefault(x => !Equals(x, default(T)));
        }

        #endregion

        #region Empty If Null

        /// <summary>
        /// Returns an empty ienumerable if the item is null. This way you don't need to check for a null value. ie: a foreach loop
        /// </summary>
        /// <typeparam name="T">Type of Enumerable</typeparam>
        /// <param name="EnumerableToCheck">Enumerable to check and return the value off of</param>
        /// <returns>The original IEnumerable (if not null). Or an empty enumerable with 0 elements if the enumerable passed is null</returns>
        /// <remarks>Please note this is slower then checking for null. Multiple reasons including the foreach still allocated the enumerator 'GetEnumerator'</remarks>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> EnumerableToCheck)
        {
            //if null return the empty T
            return EnumerableToCheck ?? Enumerable.Empty<T>();
        }

        #endregion

        #region First Index Of Element

        /// <summary>
        /// Gets The First Index Of The Items That Match From The Predicate
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable</typeparam>
        /// <param name="Collection">Collection To Check Against</param>
        /// <param name="Predicate">Predicate To Search For The Element In The Collection</param>
        /// <returns>Last Index. Returns Null If Nothing Is Found</returns>
        public static int? FirstIndexOfElement<T>(this IEnumerable<T> Collection, Func<T, bool> Predicate)
        {
            //holds the tally so we can keep track of what index we are up to
            int IndexTally = 0;

            //let's loop through the collection now
            foreach (T ItemToCheck in Collection)
            {
                //let's test if the predicate returns true
                if (Predicate(ItemToCheck))
                {
                    //we found the item so just return the index
                    return IndexTally;
                }

                //let's increase the tally before we go to the next item
                IndexTally++;
            }

            //never found a match, return null
            return null;
        }

        #endregion

        #region Last Index Of Element

        /// <summary>
        /// Gets The Last Index Of The Items That Match From The Predicate. This is used when you have duplicate values and you want to get the last index in the array of anything that evaluates to true from the predicate.
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable</typeparam>
        /// <param name="Collection">Collection To Check Against</param>
        /// <param name="Predicate">Predicate To Search For The Element In The Collection</param>
        /// <returns>Last Index. Returns Null If Nothing Is Found</returns>
        public static int? LastIndexOfElement<T>(this IEnumerable<T> Collection, Func<T, bool> Predicate)
        {
            //let's check if this is ICollection so we can just use the index and reverse the array so we don't have to loop through the entire collection. (we can exit the first item found)
            var CollectionCastAttempt = Collection as IList<T>;

            //is it a collection with indexers?
            if (CollectionCastAttempt == null)
            {
                //we either need to call .Count() and then loop through it backwards subtracting the index we are up to - Count(). I have no idea if this is faster or just pushing it to an array and then using the logic below.
                //this would only really  matter if the array is huge
                CollectionCastAttempt = Collection.ToArray();
            }

            //if we get here, then we have an IList...so we can just run it off of the array index
            for (int i = CollectionCastAttempt.Count - 1; i >= 0; i--)
            {
                //let's test if the predicate returns true
                if (Predicate(CollectionCastAttempt[i]))
                {
                    //this is true, return the index now
                    return i;
                }
            }

            //never found an item, so return null
            return null;
        }

        #endregion

        #region For Each (On IEnumerable)

        /// <summary>
        /// For Each Extension Method. Runs The Action Method For Each Element In The List
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable. Don't Need To Pass In</typeparam>
        /// <param name="CollectionToProcess">This IEnumerable To Run The Action On</param>
        /// <param name="MethodToRunOnEachElement">Method to run on each element.</param>
        /// <remarks>.Net Framework Has For Each Only On List (Ext. Method). This is for anything of IEnumerable. there is a generic constraint because if you pass in a list of string or int's, it won't change it because its a value type</remarks>
        public static void ForEach<T>(this IEnumerable<T> CollectionToProcess, Action<T> MethodToRunOnEachElement) where T : class
        {
            //there is a generic constraint because if you pass in a list of int's, it won't change it because its a value type

            //example on how to call this
            //lst.ForEach(x => x.Id = -1); (lst is IEnumerable of a model class)

            //loop through each element and invoke the item
            foreach (T ElementToProcess in CollectionToProcess)
            {
                //let's go invoke the element
                MethodToRunOnEachElement.Invoke(ElementToProcess);
            }
        }

        #endregion

        #region Distinct By Property Selector

        /// <summary>
        /// Runs a distinct by a property that is passed in for a given object using linq. Then returns the object.
        /// </summary>
        /// <typeparam name="TSource">Type of the record to query</typeparam>
        /// <typeparam name="TKey">Property selector data type</typeparam>
        /// <param name="DataSource">Data source to look in</param>
        /// <param name="PropertySelector">Property selector to run the distinct on</param>
        /// <returns>yield returns the objects that have a distinct value by the property selector</returns>
        public static IEnumerable<TSource> DistinctByLazy<TSource, TKey>(this IEnumerable<TSource> DataSource, Func<TSource, TKey> PropertySelector)
        {
            //holds the values that have been found already
            var PropertyValuesFound = new HashSet<TKey>();

            //loop through each of the records
            foreach (TSource RecordToCheck in DataSource)
            {
                //did we store this guy yet? (if true then we haven't and the add was successful)
                if (PropertyValuesFound.Add(PropertySelector(RecordToCheck)))
                {
                    //it was added...so return the element
                    yield return RecordToCheck;
                }
            }
        }

        #endregion

        #region Chunk List

        /// <summary>
        /// Will take the contents of the list and chunk it up into groups. This is used if you have a large number of items, and a method needs to handle it in a smaller number of items. @Html.Raw was choking on a large list, so we will pass a smaller amount of items to the method
        /// </summary>
        /// <typeparam name="T">Type Of The IEnumerable. Don't Need To Pass In</typeparam>
        /// <param name="CollectionToChunk">Collection to chunk up</param>
        /// <param name="MaxNumberOfItemsInBucket">The maximum number of elements to put in a bucket.</param>
        /// <returns>chunked up items</returns>
        public static IEnumerable<IEnumerable<T>> ChunkUpListItemsLazy<T>(this IEnumerable<T> CollectionToChunk, int MaxNumberOfItemsInBucket)
        {
            //ensure we pass in more then 0 items
            if (MaxNumberOfItemsInBucket < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxNumberOfItemsInBucket), "Parameter Must Be >= 1");
            }

            //the current group we are inserting into
            var CurrentGroup = new List<T>();

            //let's loop through the elements
            foreach (var ItemToProcess in CollectionToChunk)
            {
                //if we have the number of items in the collection, then add another group
                if (CurrentGroup.Count == MaxNumberOfItemsInBucket)
                {
                    //return the current group
                    yield return CurrentGroup;

                    //let's add another group
                    CurrentGroup = new List<T>();
                }

                //add this item to the group
                CurrentGroup.Add(ItemToProcess);
            }

            //yield return the current group if we have less then the max
            if (CurrentGroup.Any())
            {
                yield return CurrentGroup;
            }
        }

        #endregion

        #region Prepend and Append Single Item With Enumerable

        /// <summary>
        /// Adds a single item at the first element in the list
        /// </summary>
        /// <typeparam name="T">Type of the record and list</typeparam>
        /// <param name="IEnumerableToPrependInto">IEnumerable to prepend into</param>
        /// <param name="ItemToAddToList">Item to prepend to the list.</param>
        /// <returns>The single item at the first element position followed by the rest of the collection elements</returns>
        public static IEnumerable<T> PrependItemLazy<T>(this IEnumerable<T> IEnumerableToPrependInto, T ItemToAddToList)
        {
            //output the single item first
            yield return ItemToAddToList;

            //now add the rest of the list
            foreach (T ItemInList in IEnumerableToPrependInto)
            {
                //return this item
                yield return ItemInList;
            }
        }

        /// <summary>
        /// Adds a single item at the end of the list. The single item passed in would be at the end of the collection
        /// </summary>
        /// <typeparam name="T">Type of the record and list</typeparam>
        /// <param name="IEnumerableToAppendInto">IEnumerable to append into</param>
        /// <param name="ItemToAddToList">Item to append to the end of the list.</param>
        /// <returns>Collection passed in with the item at the last element in the list.</returns>
        public static IEnumerable<T> AppendItemLazy<T>(this IEnumerable<T> IEnumerableToAppendInto, T ItemToAddToList)
        {
            //add all the collection items first
            foreach (T ItemInList in IEnumerableToAppendInto)
            {
                //return this item
                yield return ItemInList;
            }

            //output the single item last
            yield return ItemToAddToList;

        }

        #endregion

        #endregion

    }

}
