using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.ADO
{

    /// <summary>
    /// Interface Which Contains All ADO.Net Level Data Providers
    /// </summary>
    public interface IDataProvider : IDisposable
    {

        #region Supporting Methods

        /// <summary>
        /// Check if we can connect to the database
        /// </summary>
        /// <returns>boolean value. True if you can connect...false if you can't</returns>
        /// <remarks>Used More For Troubleshooting</remarks>
        bool CanConnectToDatabase();

        /// <summary>
        /// Close the current connection to the database. 
        /// </summary>
        /// <remarks>Must be used when you grab a datareader. You will need to close the connection before it gets disposed</remarks>
        void CloseConn();

        #endregion

        #region Main Data Calls

        /// <summary>
        /// Query and return a dataset from the database
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="DataSetName">Name Of The DataSet You Wish To Call It</param>
        /// <param name="CommandTimeout">Command Time Out (in seconds)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSet(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, int? CommandTimeout = null, string DataSetName = "ds");

        /// <summary>
        /// Retrieve a Data Table from the database
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="TableName">Name Of The DataSet You Wish To Call It</param>
        /// <param name="CommandTimeOut">Command Time Out - Null is the default value</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Datatable</returns>
        DataTable GetDataTable(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, string TableName = "dt", int? CommandTimeOut = null);

        /// <summary>
        /// Execute A Query That Does Not Return Any Records
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Boolan on if the command was successful</returns>
        bool ExecuteNonQuery(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null);

        /// <summary>
        /// Retrieve A Data Reader.
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="myCommandBehavior">Command Behavior To The DataReader.</param>
        /// <returns>A Data Reader</returns>
        /// <remarks>Be Sure To Close The Connection After You Are Done With The Reader. (Don't Have To If You Passed In CommandBehavior.CloseConnection)</remarks>
        DbDataReader GetDataReader(string SqlToRun, CommandType CommandTypeToRun, CommandBehavior CommandBehaviorToUse, IEnumerable<SqlParameter> QueryParameters = null);

        /// <summary>
        /// Get A Single Field Query
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Your result in an object. Be sure to cast it otherwise it will be late bound!</returns>
        /// <example>Select Count(*)</example>
        object GetScalar(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null);

        /// <summary>
        /// Get A Typed Single Field Query
        /// </summary>
        /// <typeparam name="T">Return Type That You Expect Back</typeparam>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Your result that is typed because of the type of T you passed in</returns>
        /// <example>Select Count(*)</example>
        T GetScalar<T>(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null);

        #endregion

    }

}
