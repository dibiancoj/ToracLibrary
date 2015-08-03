using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        #region Parameters

        /// <summary>
        /// Initalizes the list which holds the parameters being passed into the database
        /// </summary>
        /// <remarks>It runs checks when you try to add or clear parameters to make sure the list is valid</remarks>
        void InitializeParameters();

        /// <summary>
        ///  Clears all the parameters in the list of parameters
        /// </summary>
        /// <remarks>Raises an error if the list of objects is not valid</remarks>
        void ClearParameters();

        /// <summary>
        /// Adds a SQL parameter to the list of parameters being passed into the stored procedure or SQL
        /// </summary>
        /// <typeparam name="T">The parameter's type</typeparam>
        /// <param name="ParameterName">Parameter Name. You can use the @ symbol or not. Doesn't really matter.</param>
        /// <param name="ParameterType">Parameter Type. What type of variable is this.</param>
        /// <param name="ParameterValue">The value of the parameter. Is an object so it can take whatever you pass in</param>
        /// <remarks>Will raise an error if you haven't called InitializeParameters before calling this method</remarks>
        void AddParameter<T>(string ParameterName, DbType ParameterType, T ParameterValue);

        #endregion

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

        #region DataSets

        /// <summary>
        /// Get A Data Set.
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <returns>Dataset</returns>
        DataSet GetDataSet(string SqlToRun, CommandType CommandTypeToRun);

        /// <summary>
        /// Get A Data Set With The DataSet Name Being Set By Parameter You Passed In
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="DataSetName">Name Of The DataSet You Wish To Call It</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSet(string SqlToRun, CommandType CommandTypeToRun, string DataSetName);

        #endregion

        #region DataTable

        /// <summary>
        /// Get A Data Table
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <returns>Datatable</returns>
        DataTable GetDataTable(string SqlToRun, CommandType CommandTypeToRun);

        /// <summary>
        /// Get A Data Table
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="TableName">Name Of The DataSet You Wish To Call It</param>
        /// <returns>Datatable</returns>
        DataTable GetDataTable(string SqlToRun, CommandType CommandTypeToRun, string TableName);

        #endregion

        #region Execute Non-Query

        /// <summary>
        /// Execute A Query That Does Not Return Any Records
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <returns>Boolan on if the command was successful</returns>
        bool ExecuteNonQuery(string SqlToRun, CommandType CommandTypeToRun);

        #endregion

        #region Data Reader's

        /// <summary>
        /// Retrieve A Data Reader.
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="myCommandBehavior">Command Behavior To The DataReader.</param>
        /// <returns>A Data Reader</returns>
        /// <remarks>Be Sure To Close The Connection After You Are Done With The Reader. (Don't Have To If You Passed In CommandBehavior.CloseConnection)</remarks>
        DbDataReader GetDataReader(string SqlToRun, CommandType CommandTypeToRun, CommandBehavior myCommandBehavior);

        #endregion

        #region Scalar Values

        /// <summary>
        /// Get A Single Field Query
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <returns>Your result in an object. Be sure to cast it otherwise it will be late bound!</returns>
        /// <example>Select Count(*)</example>
        System.Object GetScalar(string SqlToRun, CommandType CommandTypeToRun);

        /// <summary>
        /// Get A Typed Single Field Query
        /// </summary>
        /// <typeparam name="T">Return Type That You Expect Back</typeparam>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <returns>Your result that is typed because of the type of T you passed in</returns>
        /// <example>Select Count(*)</example>
        T GetScalar<T>(string SqlToRun, CommandType CommandTypeToRun);

        #endregion

        #endregion

    }

}
