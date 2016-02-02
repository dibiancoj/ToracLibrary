using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees;
using ToracLibrary.Core.ExpressionTrees.API.ReMappers;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExtensionMethods.IQueryableExtensions
{

    /// <summary>
    /// Extension Methods For IOrderedQueryable
    /// </summary>
    public static class IQueryableExtensionMethods
    {

        #region Enum

        /// <summary>
        /// Direction to sort the column in
        /// </summary>
        public enum SortDirection
        {

            /// <summary>
            /// Ascending
            /// </summary>
            Ascending = 0,

            /// <summary>
            /// Descending
            /// </summary>
            Descending = 1
        }


        #endregion

        #region Order By

        /// <summary>
        /// Modifies Queryable Of T And Orders It For Entity Framework with the multiple sort properties. Supports sub objects using the Property1.Property2 syntax
        /// </summary>
        /// <typeparam name="T">Type Of The IQueryable</typeparam>
        /// <param name="QueryToSort">Query To Sort</param>
        /// <param name="ColumnsToSortBy">Columns to sort by. Key would be the property name, value is the direction</param>
        /// <returns>IQueryable Of T which is sorted</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> QueryToSort, IDictionary<string, SortDirection> ColumnsToSortBy)
        {
            //do we have any columns specified
            if (ColumnsToSortBy.AnyWithNullCheck())
            {
                //sorted query
                IOrderedQueryable<T> SortedQuery = null;

                //loop through each sort record
                foreach (var SortColumn in ColumnsToSortBy)
                {
                    //do we have a sorted query yet?
                    if (SortedQuery == null)
                    {
                        //asc?
                        SortedQuery = SortColumn.Value == SortDirection.Ascending
                            ? QueryToSort.OrderBy(SortColumn.Key)
                            : QueryToSort.OrderByDescending(SortColumn.Key);
                    }
                    else
                    {
                        //we already have the first sort..use "thenby" now
                        SortedQuery = SortColumn.Value == SortDirection.Ascending
                            ? SortedQuery.ThenBy(SortColumn.Key)
                            : SortedQuery.ThenByDescending(SortColumn.Key);
                    }
                }

                //return the sorted query now
                return SortedQuery;
            }

            //we don't have any columns...return what was passed in
            return QueryToSort;
        }

        /// <summary>
        /// Modifies Queryable Of T And Orders It For Entity Framework. Supports sub objects using the Property1.Property2 syntax
        /// </summary>
        /// <typeparam name="T">Type Of The IQueryable</typeparam>
        /// <param name="QueryToModify">Query To Modifiy</param>
        /// <param name="PropertyNameToSortBy">Property Name To Sort By</param>
        /// <returns>IOrderedQueryable Of T</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> QueryToModify, string PropertyNameToSortBy)
        {
            return ExpressionTreeHelpers.OrderBy(QueryToModify, PropertyNameToSortBy, true, true);
        }

        /// <summary>
        /// Modifies Queryable Of T And Orders It For Entity Framework. Supports sub objects using the Property1.Property2 syntax
        /// </summary>
        /// <typeparam name="T">Type Of The IQueryable</typeparam>
        /// <param name="QueryToModify">Query To Modifiy</param>
        /// <param name="PropertyNameToSortBy">Property Name To Sort By</param>
        /// <returns>IOrderedQueryable Of T</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> QueryToModify, string PropertyNameToSortBy)
        {
            return ExpressionTreeHelpers.OrderBy(QueryToModify, PropertyNameToSortBy, false, true);
        }

        #endregion

        #region Inject Another Binding Into A Select Statement

        /// <summary>
        /// Let's say i have an IQueryable. Essentially a ef query. I want to tack on a field to my select. My select looks like x => new Record { Id = x.Id};. I want to tack on { Txt = x.Txt} inside the new Record call.
        /// </summary>
        /// <typeparam name="T">Queryable Record Type</typeparam>
        /// <param name="QueryToModify">Query to modify</param>
        /// <param name="FieldNameToSetTheValue">The name of the field you are mapping from. So the table.FieldName</param>
        /// <param name="PropertyNameToSetInSelectProjection">The name of the property you want to set</param>
        /// <returns>New Query with the modified binding</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IQueryable<T> AddBindingToSelectInQuery<T>(this IQueryable<T> QueryToModify, string FieldNameToSetTheValue, string PropertyNameToSetInSelectProjection)
        {
            //go build some query to test this out
            //var query = MapTableRecord.Build().AsQueryable().Where(x => x.Id < 3).Select(x => new GridRecord { MapId = x.Id });

            //now we can merge the select by merging the description into the mix so the select ends up being Select(x => new GridRecord { MapId = x.Id, Description = x.DescriptionFrom});
            //var MergeQueryable = BaseQuery.AddBindingToSelectInQuery(nameof(DummyObject.DescriptionFrom), nameof(GridRecord.Description));

            /* There needs to be a select at the end. This is a select merger!!! */
            //this isn't tested with multiple select's either

            //go build the new query ...add the user to the query
            var MergeQueryBuilder = new IQueryableMemberInitMerger<T>(FieldNameToSetTheValue, PropertyNameToSetInSelectProjection);

            //go visit the query so we can rebuild this
            var NewQuery = MergeQueryBuilder.Visit(QueryToModify.Expression);

            //go create the new IQueryable
            return QueryToModify.Provider.CreateQuery(NewQuery) as IQueryable<T>;
        }

        #endregion

    }

}
