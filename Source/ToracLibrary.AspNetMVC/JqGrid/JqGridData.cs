using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Paging;
using static ToracLibrary.Core.ExtensionMethods.IOrderedQueryableExtensions.IOrderedQueryableExtensionMethods;

namespace ToracLibrary.AspNet.AspNetMVC.JqGrid
{

    /*
    In order to not have readonly properties that point to real properties, i use newtonsoft Jsonproperty to serialize to a specific property name.
    Newtonsoft is the mandatory json serializer for jqgrid. If you can't use JqGrid you will need something that maps TotalPages = "total", etc.
    */

    /// <summary>
    /// Holds the data structure the JqGrid needs when using Json data type
    /// </summary>
    /// <typeparam name="T">Type of data used for each row</typeparam>
    /// <remarks>Don't change Json property names, these names are expected (lowercase and names)</remarks>
    public class JqGridData<T>
    {

        #region Properties

        //*** Note these property names and case can't change. This is what JqGrid is expecting (the json property - property names that is)

        /// <summary>
        /// Total Pages Of The Query
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Current Page That We Are On
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int CurrentPageId { get; set; }

        /// <summary>
        /// Total number of records for the query
        /// </summary>
        [JsonProperty(PropertyName = "records")]
        public int TotalNumberOfRecordsInDataSet { get; set; }

        /// <summary>
        /// Array that contains the actual data
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public IEnumerable<T> RowsOfData { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Takes the full data source of the grid and returns the data for that specific page. (manually implemented paging or server side paging)
        /// </summary>
        /// <typeparam name="TRow">Data Type Of Each Row</typeparam>
        /// <param name="GridDataSource">Full Data Source For The Grid</param>
        /// <param name="PageIndex">The Page You Currently Are On (1 is the first page)</param>
        /// <param name="HowManyPerPage">How Many Records Per Page</param>
        /// <returns>Grid Data With Just The Current Page</returns>
        public static JqGridData<TRow> BuildJqGridData<TRow, TSortByPropertyType>(IEnumerable<TRow> GridDataSource, Expression<Func<TRow, TSortByPropertyType>> SortPropertySelector, int PageIndex, int HowManyPerPage)
        {
            //this is good if you have the entire data source in session or what not.
            //if you have big data source you would want to sort and page in the database then you would need to set the properties specifically

            //return grid data
            var ReturnGridData = new JqGridData<TRow>();

            //set the number of records
            ReturnGridData.TotalNumberOfRecordsInDataSet = GridDataSource.Count();

            //set what page we are on
            ReturnGridData.CurrentPageId = PageIndex;

            //set the total number of pages
            ReturnGridData.TotalPages = DataSetPaging.CalculateTotalPages(ReturnGridData.TotalNumberOfRecordsInDataSet, HowManyPerPage);

            //let's set the data now for just this page
            ReturnGridData.RowsOfData = GridDataSource.AsQueryable().OrderBy(SortPropertySelector).PaginateResults(PageIndex, HowManyPerPage).ToArray();

            //return the data
            return ReturnGridData;
        }

        #endregion


    }

}
