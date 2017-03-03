﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.DataProviders.ADO
{

    /// <summary>
    /// The Dataprovider class gives you a generic object which allows you to retrieve and perform queries on a SQL Server database
    /// </summary>
    /// <remarks>Sample Connection String ==> "Data Source=192.168.100.242; Failover Partner=192.168.100.243; Network Library=DBMSSOCN; Initial Catalog=Radman; User ID=radman9_read_write; Password=myPassword; Application Name=web1.wmgsuite.wmginc.com"</remarks>
    public class SQLDataProvider : IDataProvider, IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor...Need To Pass In The Connection String
        /// </summary>
        /// <param name="ConnectionString">Connection String For The SQL Server</param>
        public SQLDataProvider(string ConnectionString)
        {
            //Check to make sure the connection string is not blank
            if (ConnectionString.IsNullOrEmpty())
            {
                throw new ArgumentNullException("You Must Pass In A Connection String That Is Not Blank Or Null");
            }

            //Else create the connection object and assign the connection string
            ConnSql = new SqlConnection(ConnectionString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// My SQL Connection
        /// </summary>
        /// <remarks>Variable is Immutable</remarks>
        private SqlConnection ConnSql { get; }

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// Builds a basic SQL parameter to use in a call when you use a parameterized query
        /// </summary>
        /// <typeparam name="T">The parameter's type</typeparam>
        /// <param name="ParameterName">Parameter Name. You can use the @ symbol or not. Doesn't really matter.</param>
        /// <param name="ParameterType">Parameter Type. What type of variable is this.</param>
        /// <param name="ParameterValue">The value of the parameter. Is an object so it can take whatever you pass in</param>
        /// <returns>SqlParameter to use in the query</returns>
        /// <remarks>Will raise an error if you haven't called InitializeParameters before calling this method</remarks>
        public SqlParameter AddParameter<T>(string ParameterName, DbType ParameterType, T ParameterValue)
        {
            //User Already Initialized The Parameter List...Now Add It To The List
            return new SqlParameter
            {
                ParameterName = ParameterName,
                DbType = ParameterType,
                Value = ParameterValue
            };
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get All The SQL Server Instances On Your Network
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>No Parameters Are Available</remarks>
        public static DataTable GetSQLServerInstancesFromNetwork()
        {
            //Simple call. Just return the already defined method
            return System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
        }

        /// <summary>
        /// If your not sure of your connection string you can run this and gather a default connection string
        /// </summary>
        /// <param name="ServerAddress">Server Address. Takes Either IP Address Or DNS Name</param>
        /// <param name="DatabaseName">Database Name</param>
        /// <param name="UserName">User Name To Connect To Database</param>
        /// <param name="Password">User Password To Connect To Database</param>
        /// <param name="IsSQLExpress">Is It A Sql Express Instance</param>
        /// <returns>String</returns>
        /// <remarks>This method isn't for every single connection string example. There are too many to code for. This should just help them get them up and running for simple examples</remarks>
        public static string BuildMyConnectionString(string ServerAddress, string DatabaseName, string UserName, string Password, bool IsSQLExpress)
        {
            //Lets Validate All My Parameters First Just To Make Sure
            if (ServerAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("You Must Pass In A Server Address If You Are Trying To Build Your Connection String");
            }

            if (DatabaseName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("You Must Pass In A Database Name If You Are Trying To Build Your Connection String");
            }

            if (UserName.IsNullOrEmpty())
            {
                throw new ArgumentNullException("You Must Pass In A User Name If You Are Trying To Build Your Connection String");
            }

            if (Password.IsNullOrEmpty())
            {
                throw new ArgumentNullException("You Must Pass In A User Password If You Are Trying To Build Your Connection String");
            }
            //End Of Validation

            //We Pass Validation...Lets Create The String Builder Object
            var ConnectionStringToReturn = new StringBuilder();

            ConnectionStringToReturn.Append("Server=");
            ConnectionStringToReturn.Append(ServerAddress);

            //If its a sql express server then add it to the string builder
            if (IsSQLExpress)
            {
                ConnectionStringToReturn.Append("\\SQLExpress");
            }

            ConnectionStringToReturn.Append(";Database=");
            ConnectionStringToReturn.Append(DatabaseName);
            ConnectionStringToReturn.Append(";Uid=");
            ConnectionStringToReturn.Append(UserName);
            ConnectionStringToReturn.Append(";Pwd=");
            ConnectionStringToReturn.Append(Password);
            ConnectionStringToReturn.Append(";");

            //return the results
            return ConnectionStringToReturn.ToString();
        }

        #endregion

        #region Main Data Calls

        #region DataSets

        /// <summary>
        /// Query and return a dataset from the database
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="DataSetName">Name Of The DataSet You Wish To Call It</param>
        /// <param name="CommandTimeout">Command Time Out (in seconds)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, int? CommandTimeout = null, string DataSetName = "ds")
        {
            try
            {
                using (var DataSeToFill = new DataSet(DataSetName))
                {
                    using (var DataAdapter = new SqlDataAdapter())
                    {
                        using (var SqlCommandToRun = new SqlCommand(SqlToRun, ConnSql))
                        {
                            //set the command type
                            SqlCommandToRun.CommandType = CommandTypeToRun;

                            //if the command timeout has a value then set it
                            if (CommandTimeout.HasValue)
                            {
                                SqlCommandToRun.CommandTimeout = CommandTimeout.Value;
                            }

                            //add the parameters
                            if (QueryParameters.AnyWithNullCheck())
                            {
                                SqlCommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                            }

                            //set the select command 
                            DataAdapter.SelectCommand = SqlCommandToRun;

                            //fill the dataset
                            DataAdapter.Fill(DataSeToFill);

                            //return the dataset
                            return DataSeToFill;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Close the connection on error. It will close if the method completes successfully
                CloseConn();
            }
        }

        #endregion

        #region DataTable

        /// <summary>
        /// Retrieve a Data Table from the database
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="TableName">Name Of The DataSet You Wish To Call It</param>
        /// <param name="CommandTimeOut">Command Time Out - Null is the default value</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Datatable</returns>
        public DataTable GetDataTable(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, string TableName = "dt", int? CommandTimeOut = null)
        {
            try
            {
                using (var DataTableToFill = new DataTable(TableName))
                {
                    using (var DataAdapter = new SqlDataAdapter())
                    {
                        using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                        {
                            //set the command type
                            CommandToRun.CommandType = CommandTypeToRun;

                            //if the command time out is not null then set it
                            if (CommandTimeOut.HasValue)
                            {
                                CommandToRun.CommandTimeout = CommandTimeOut.Value;
                            }

                            //add the parameters if we have any
                            if (QueryParameters.AnyWithNullCheck())
                            {
                                CommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                            }

                            //set the select command
                            DataAdapter.SelectCommand = CommandToRun;

                            //fill the datatable
                            DataAdapter.Fill(DataTableToFill);

                            //return the datatable
                            return DataTableToFill;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Close the connection on error. It will close if the method completes successfully
                CloseConn();
            }
        }

        #endregion

        #region Execute Non-Query

        /// <summary>
        /// Execute A Query That Does Not Return Any Records
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Boolan on if the command was successful</returns>
        public bool ExecuteNonQuery(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null)
        {
            try
            {
                using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                {
                    //set the command type
                    CommandToRun.CommandType = CommandTypeToRun;

                    //add the parameters if there are any
                    if (QueryParameters.AnyWithNullCheck())
                    {
                        CommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                    }

                    //open the connection
                    CommandToRun.Connection.Open();

                    //run the query
                    CommandToRun.ExecuteNonQuery();

                    //let the user know it was successful
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //close the connection in all scenario's
                CloseConn();
            }
        }

        #endregion

        #region Reader's

        #region Data Reader's

        /// <summary>
        /// Retrieve A Data Reader.
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="CommandBehaviorToUse">Command Behavior To The DataReader.</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>A Data Reader</returns>
        /// <remarks>Be Sure To Close The Connection After You Are Done With The Reader. (Don't Have To If You Passed In CommandBehavior.CloseConnection)</remarks>
        public DbDataReader GetDataReader(string SqlToRun, CommandType CommandTypeToRun, CommandBehavior CommandBehaviorToUse, IEnumerable<SqlParameter> QueryParameters = null)
        {
            try
            {
                using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                {
                    //set the command type
                    CommandToRun.CommandType = CommandTypeToRun;

                    //add the parameters
                    if (QueryParameters.AnyWithNullCheck())
                    {
                        CommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                    }

                    //if the connection is closed then open the connection
                    if (CommandToRun.Connection.State == ConnectionState.Closed)
                    {
                        CommandToRun.Connection.Open();
                    }

                    //return the data reader
                    return CommandToRun.ExecuteReader(CommandBehaviorToUse);
                }
            }
            catch (Exception)
            {
                //Close the connection on error (need's to be here because we are passing back the reader which needs to be open)
                CloseConn();

                //throw the error now
                throw;
            }
        }

        #endregion

        #region XML Reader

        /// <summary>
        /// Get an xml reader from the database
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="CommandTimeout">Command Time Out (in seconds)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>XML Data Reader</returns>
        public XmlReader GetXMLDataReader(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, int? CommandTimeout = null)
        {
            try
            {
                //create the sql command
                using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                {
                    //Set the command type
                    CommandToRun.CommandType = CommandTypeToRun;

                    //if there is a command timeout then set it here
                    if (CommandTimeout.HasValue)
                    {
                        //Set the command timeout
                        CommandToRun.CommandTimeout = CommandTimeout.Value;
                    }

                    //Add the parameters
                    if (QueryParameters.AnyWithNullCheck())
                    {
                        CommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                    }

                    //if the connection is closed then oepn it
                    if (CommandToRun.Connection.State == ConnectionState.Closed)
                    {
                        CommandToRun.Connection.Open();
                    }

                    //return the xml reader
                    return CommandToRun.ExecuteXmlReader();
                }
            }
            catch (Exception)
            {
                //Close the connection on error (need's to be here because we are passing back the reader which needs to be open)
                CloseConn();

                //throw the error now
                throw;
            }
        }

        #endregion

        #endregion

        #region XML Specific Return Objects

        /// <summary>
        /// Returns An XElement Node For The Command Passed In
        /// </summary>
        /// <param name="SqlToRun">Sql To Run - Stored Procedure Name If Using Stored Procedure</param>
        /// <param name="CommandTypeToRun">Command Type</param>
        /// <param name="CommandTimeout">Command Timeout If Needed. In Seconds</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>XElement - System.Xml.Linq</returns>
        /// <remarks>Behind the scenes it uses an XML Reader</remarks>
        public XElement GetXMLData(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null, int? CommandTimeout = null)
        {
            using (XmlReader XmlReaderToLoad = GetXMLDataReader(SqlToRun, CommandTypeToRun, QueryParameters))
            {
                //use the helper method to grab the data
                return XElement.Load(XmlReaderToLoad);
            }
        }

        #endregion

        #region Scalar Values

        /// <summary>
        /// Get A Single Field Query
        /// </summary>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Your result in an object. Be sure to cast it otherwise it will be late bound!</returns>
        /// <example>Select Count(*)</example>
        public object GetScalar(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null)
        {
            try
            {
                using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                {
                    //set the command type
                    CommandToRun.CommandType = CommandTypeToRun;

                    //add the parameters
                    if (QueryParameters.AnyWithNullCheck())
                    {
                        CommandToRun.Parameters.AddRange(QueryParameters.ToArray());
                    }

                    //Open the connection if its not open already
                    if (CommandToRun.Connection.State != ConnectionState.Open)
                    {
                        CommandToRun.Connection.Open();
                    }

                    //return the field
                    return CommandToRun.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //close the connection in all scenario's
                CloseConn();
            }
        }

        /// <summary>
        /// Get A Typed Single Field Query
        /// </summary>
        /// <typeparam name="T">Return Type That You Expect Back</typeparam>
        /// <param name="SqlToRun">SQL To Run Or The Stored Procedure Name</param>
        /// <param name="CommandTypeToRun">Command Type. Is it a stored procedure or sql (text)</param>
        /// <param name="QueryParameters">SqlParameter to use in the query</param>
        /// <returns>Your result that is typed because of the type of T you passed in</returns>
        /// <example>Select Count(*)</example>
        public T GetScalar<T>(string SqlToRun, CommandType CommandTypeToRun, IEnumerable<SqlParameter> QueryParameters = null)
        {
            //use the overload and cast it
            return (T)GetScalar(SqlToRun, CommandTypeToRun, QueryParameters);
        }

        #endregion

        #region Bulk Copy

        #region Bulk Insert (Regular)

        #region Data Table Based

        #region Public Methods

        /// <summary>
        /// Run A Bulk Insert On A Table
        /// </summary>
        /// <param name="DataTableSchema">Schema for the table we are writing too</param>
        /// <param name="DataTableToLoad">Datatable which contains the records for the bulk insert</param>
        /// <param name="CopyOptions">Copy options for the command</param>
        /// <param name="BatchSize">Batch size. How many records do you want to insert at a time</param>
        /// <returns>Boolean on if the bulk insert was successful</returns>
        /// <remarks>Run In A Transaction.</remarks>
        public bool BulkInsert(string DataTableSchema, DataTable DataTableToLoad, SqlBulkCopyOptions CopyOptions, Int32 BatchSize)
        {
            //run the helper method
            return BulkInsertHelper(DataTableSchema, DataTableToLoad, CopyOptions, BatchSize, null);
        }

        /// <summary>
        /// Run A Bulk Insert On A Table
        /// </summary>
        /// <param name="DataTableSchema">Schema for the table we are writing too</param>
        /// <param name="DataTableToLoad">Datatable which contains the records for the bulk insert</param>
        /// <param name="CopyOptions">Copy options for the command</param>
        /// <param name="BatchSize">Batch size. How many records do you want to insert at a time</param>
        /// <param name="CommandTimeout">Command Time Out (in seconds) Use 0 For unlimited command timeout</param>
        /// <returns>Boolean on if the bulk insert was successful</returns>
        /// <remarks>Run In A Transaction.</remarks>
        public bool BulkInsert(string DataTableSchema, DataTable DataTableToLoad, SqlBulkCopyOptions CopyOptions, Int32 BatchSize, int CommandTimeout)
        {
            //run the helper method
            return BulkInsertHelper(DataTableSchema, DataTableToLoad, CopyOptions, BatchSize, CommandTimeout);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Bulk Insert For Data Table Helper Method
        /// </summary>
        /// <param name="DataTableSchema">Schema for the table we are writing too</param>
        /// <param name="DataTableToLoad">Data table to run the bulk insert on</param>
        /// <param name="CopyOptions">Copy Options</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <param name="CommandTimeout">Command Timeout</param>
        /// <returns>Result</returns>
        private bool BulkInsertHelper(string DataTableSchema, DataTable DataTableToLoad, SqlBulkCopyOptions CopyOptions, Int32 BatchSize, Nullable<int> CommandTimeout)
        {
            //Declare the transaction as null because it will be set when we start the connection
            SqlTransaction TransactionToUse = null;

            try
            {
                //open the connection
                ConnSql.Open();

                //set the transaction
                TransactionToUse = ConnSql.BeginTransaction();

                //declare the sql bulk copy and its properties
                using (var SqlToRunBulk = new SqlBulkCopy(ConnSql, CopyOptions, TransactionToUse))
                {
                    //set the time out (0 is no timeout)
                    if (CommandTimeout.HasValue)
                    {
                        SqlToRunBulk.BulkCopyTimeout = CommandTimeout.Value;
                    }

                    //batch size
                    SqlToRunBulk.BatchSize = BatchSize;

                    //set destination table
                    SqlToRunBulk.DestinationTableName = string.Format("{0}.{1}", DataTableSchema, DataTableToLoad.TableName);

                    //write the data to the server
                    SqlToRunBulk.WriteToServer(DataTableToLoad.CreateDataReader());
                }

                //bulk copy was successful. Commit the database
                TransactionToUse.Commit();

                //return true because it was successful
                return true;
            }
            catch (Exception)
            {
                //bulk insert failed, rollback the transaction and throw the error
                TransactionToUse.Rollback();

                //throw the error now
                throw;
            }
            finally
            {
                //make sure the connection is closed
                CloseConn();

                //make sure the transaction is disposed
                if (TransactionToUse != null)
                {
                    TransactionToUse.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #region Data Set Based

        #region Public Methods

        /// <summary>
        /// Run A Bulk Insert On Multiple Tables
        /// </summary>
        /// <param name="ds">Dataset which contains the records for the bulk insert</param>
        /// <param name="CopyOptions">Copy options for the command</param>
        /// <param name="BatchSize">Batch size. How many records do you want to insert at a time</param>
        /// <returns>Boolean on if the bulk insert was successful</returns>
        /// <remarks>Run In A Transaction.</remarks>
        public bool BulkInsert(DataSet ds, SqlBulkCopyOptions CopyOptions, Int32 BatchSize)
        {
            //return the helper method
            return BulkInsertHelper(ds, CopyOptions, BatchSize, null);
        }

        /// <summary>
        /// Run A Bulk Insert On Multiple Tables
        /// </summary>
        /// <param name="ds">Dataset which contains the records for the bulk insert</param>
        /// <param name="CopyOptions">Copy options for the command</param>
        /// <param name="BatchSize">Batch size. How many records do you want to insert at a time</param>
        /// <param name="CommandTimeout">Command Time Out (in seconds) - Use 0 For unlimited command timeout</param>
        /// <returns>Boolean on if the bulk insert was successful</returns>
        /// <remarks>Run In A Transaction.</remarks>
        public bool BulkInsert(DataSet ds, SqlBulkCopyOptions CopyOptions, Int32 BatchSize, int CommandTimeout)
        {
            //return the helper method
            return BulkInsertHelper(ds, CopyOptions, BatchSize, CommandTimeout);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Bulk Insert For A Data Set Helper Method
        /// </summary>
        /// <param name="DataSetToLoad">Dataset To Insert</param>
        /// <param name="CopyOptions">Copy Options</param>
        /// <param name="BatchSize">Batch Size</param>
        /// <param name="CommandTimeout">Command Timeout In Seconds</param>
        /// <returns>Result</returns>
        private bool BulkInsertHelper(DataSet DataSetToLoad, SqlBulkCopyOptions CopyOptions, Int32 BatchSize, Nullable<int> CommandTimeout)
        {
            //Declare the transaction as null because it will be set when we start the connection
            SqlTransaction Transaction = null;

            try
            {
                //open the connection
                ConnSql.Open();

                //set the transaction
                Transaction = ConnSql.BeginTransaction();

                //declare the sql bulk copy and its properties
                using (var SqlToRunBulk = new SqlBulkCopy(ConnSql, CopyOptions, Transaction))
                {
                    //Set the command timeout
                    if (CommandTimeout.HasValue)
                    {
                        SqlToRunBulk.BulkCopyTimeout = CommandTimeout.Value;
                    }

                    //set the batch size for the bulk copy
                    SqlToRunBulk.BatchSize = BatchSize;

                    //loop through all the datatables in the dataset's. Do a bulk insert for each datatable in the dataset
                    foreach (DataTable DataTableToLoad in DataSetToLoad.Tables)
                    {
                        //set the destination time out
                        SqlToRunBulk.DestinationTableName = DataTableToLoad.TableName;

                        //write the data to the server
                        SqlToRunBulk.WriteToServer(DataTableToLoad.CreateDataReader());
                    }
                }

                //bulk insert completed successfully. Commit the transaction
                Transaction.Commit();

                //return true
                return true;
            }
            catch (Exception)
            {
                //bulk insert failed. Roll back the transaction and throw the error
                Transaction.Rollback();

                //throw the error
                throw;
            }
            finally
            {
                //make sure the connection is closed
                CloseConn();

                //make sure the transaction is disposed
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Sql SqlDependency

        /// <summary>
        /// Builds Sql Dependency Object
        /// </summary>
        /// <param name="SqlToRun">Sql To Run</param>
        /// <param name="OnChangeEventHandlerDelegate">OnChangeEventHandler For SqlDependency Object To Attach Too</param>
        public void BuildSqlDependency(string SqlToRun, OnChangeEventHandler OnChangeEventHandlerDelegate)
        {
            //to pass in delegate
            //DP.BuildSqlDependency(SqlToRun, DependencyOnChange);
            //private void DependencyOnChange(object sender, SqlNotificationEventArgs e)
            //{   
            //}
            try
            {
                //open the connection 
                ConnSql.Open();

                //create the command that will check to see if it's been updated
                using (var CommandToRun = new SqlCommand(SqlToRun, ConnSql))
                {
                    //add the dependency object
                    SqlDependency SqlCacheDependency = new SqlDependency(CommandToRun);

                    //add the event handler
                    SqlCacheDependency.OnChange += OnChangeEventHandlerDelegate;

                    //go execute the reader
                    CommandToRun.ExecuteReader().Close();

                    //don't use ConnSql.Close()....otherwise the event will never get raised because the connection is disposed.
                    //after the event is triggered the connection will get disposed
                }
            }
            catch (Exception)
            {
                //make sure the connection is closed on error
                CloseConn();

                //throw the error now
                throw;
            }
        }

        #endregion

        #endregion

        #region Supporting Methods

        /// <summary>
        /// Check if we can connect to the database
        /// </summary>
        /// <returns>boolean value. True if you can connect...false if you can't</returns>
        /// <remarks>Used More For Troubleshooting</remarks>
        public bool CanConnectToDatabase()
        {
            try
            {
                //Open then close the connection. if it makes it to the end of the method its successful. Otherwise if it errors out then return false
                ConnSql.Open();
                CloseConn();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Close the current connection to the database. 
        /// </summary>
        /// <remarks>Must be used when you grab a datareader. You will need to close the connection before it gets disposed</remarks>
        public void CloseConn()
        {
            try
            {
                //close the connection.
                ConnSql.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    ConnSql.Close();
                    ConnSql.Dispose();
                }
            }
            Disposed = true;
        }

        #endregion

    }

}
