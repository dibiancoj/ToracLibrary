using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.SqlBuilder
{
    /// <summary>
    /// Common class which helps to build sql that can be shared between data providers.
    /// </summary>
    public static class SharedSqlHelpers
    {

        #region Auto Seed Sql in SharedSqlHelpers For ColumnIsAutoSeedSql

        /// <summary>
        /// Builds the sql for Sql Server which determines if the field has identity on. Ie. primary key has auto seed on.
        /// </summary>
        /// <param name="TableName">Table Name Where The Column Exists.</param>
        /// <param name="ColumnName">Column Name To Check If Auto Seed Is On</param>
        /// <returns>Sql to execute on a data provider. The actual result if the column has auto seed on. Should be null if the table or column name was never found</returns>
        public static string ColumnIsAutoSeedSql(string TableName, string ColumnName)
        {
            //build the sql and return it
            return string.Format($"SELECT C.is_identity FROM sys.objects O INNER JOIN sys.columns C ON O.object_id = C.object_id WHERE O.type='U' AND O.name = '{TableName}' AND C.name = '{ColumnName}';");
        }

        #endregion

        #region Data Type Needs Quote

        /// <summary>
        /// Checks the data type passed in and returns boolean if this data type needs a quote when you are in sql
        /// </summary>
        /// <param name="PropertyDataType">Property data type to check when converting to sql</param>
        /// <returns>Does it need a quote in sql</returns>
        /// <remarks>This overload uses type.</remarks>
        public static bool DataTypeNeedsQuoteInSql(Type PropertyDataType)
        {
            //check the data type and return it
            return PropertyDataType == typeof(string) ||
                   PropertyDataType == typeof(DateTime) || PropertyDataType == typeof(DateTime?) ||
                   PropertyDataType == typeof(bool) || PropertyDataType == typeof(bool?);
        }

        /// <summary>
        /// Checks the data type passed in and returns boolean if this data type needs a quote when you are in sql
        /// </summary>
        /// <param name="PropertyDataType">Property data type to check when converting to sql</param>
        /// <returns>Does it need a quote in sql</returns>
        /// <remarks>This overload uses System.Data.DbType.</remarks>
        public static bool DataTypeNeedsQuoteInSql(DbType PropertyDataType)
        {
            //check the data type and return it
            return PropertyDataType == DbType.String ||
                            PropertyDataType == DbType.StringFixedLength ||
                            PropertyDataType == DbType.Date ||
                            PropertyDataType == DbType.DateTime ||
                            PropertyDataType == DbType.DateTime2 ||
                            PropertyDataType == DbType.DateTimeOffset ||
                            PropertyDataType == DbType.Boolean ||
                            PropertyDataType == DbType.Date ||
                            PropertyDataType == DbType.DateTime ||
                            PropertyDataType == DbType.DateTime2 ||
                            PropertyDataType == DbType.DateTimeOffset ||
                            PropertyDataType == DbType.Guid ||
                            PropertyDataType == DbType.Xml ||
                            PropertyDataType == DbType.AnsiString ||
                            PropertyDataType == DbType.AnsiStringFixedLength;
        }

        #endregion

    }

}
