﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions
{

    /// <summary>
    /// Extension Methods For IOrderedQueryable
    /// </summary>
    public static class IOrderedQueryable
    {

        #region Paginate Results

        /// <summary>
        /// Takes The IOrderedQueryable And Grabs The Current Page We Are On. We use iordered queryable because the data should always be ordered when paged. EF actually pages you order it otherwise it will through an error.
        /// Making the developer pass in an ordered set so we don't get any silent issues because the data isn't ordered
        /// </summary>
        /// <typeparam name="T">Type Of Records That Are Returned</typeparam>
        /// <param name="QueryToModify">Query To Modify</param>
        /// <param name="CurrentPageNumber">What Page Number Are You Currently On</param>
        /// <param name="HowManyRecordsPerPage">How Many Records Per Page</param>
        /// <returns>IQueryable Of T</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IQueryable<T> PaginateResults<T>(this IOrderedQueryable<T> QueryToModify, int CurrentPageNumber, int HowManyRecordsPerPage)
        {
            //how to call this
            //sql = sql.OrderBy(sidx, isAscending).PaginateResults(page, rows);

            //run a quick check to make sure the page number is ok
            if (CurrentPageNumber == 0)
                throw new IndexOutOfRangeException("Current Page Number Can't Be 0. Use 1 For The First Page");

            //go skip however many pages we are past...and take only x amount of records per page
            return QueryToModify.Skip((CurrentPageNumber - 1) * HowManyRecordsPerPage).Take(HowManyRecordsPerPage).AsQueryable();
        }

        #endregion

    }

}