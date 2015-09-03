using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.SqlBuilder;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP.Logger
{

    /// <summary>
    /// Entity Framework Basic Sql Logger
    /// </summary>
    public class EFSqlLogger : DatabaseLogFormatter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Context">Context</param>
        /// <param name="WriteAction">Action to write to the log</param>
        public EFSqlLogger(DbContext Context, Action<string> WriteAction)
            : base(Context, WriteAction)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Do you want to log each time a connection is opened
        /// </summary>
        public bool LogConnectionOpened { get; set; }

        #endregion

        #region Overrides

        //LogCommand – Override this to change how commands are logged before they are executed. By default LogCommand calls LogParameter for each parameter; you may choose to do the same in your override or handle parameters differently instead.
        //LogResult – Override this to change how the outcome from executing a command is logged.
        //LogParameter – Override this to change the formatting and content of parameter logging.

        //you can also get access to the sql timing by using (internal to database log formatter)
        //this.Stopwatch (grab the stop watch in Executed (override that  method to grab the executed total time)

        /// <summary>
        /// Override the command so we can replace the parameter variable. with the actual value instead of the parameter name
        /// </summary>
        /// <typeparam name="TResult">Type Of Result</typeparam>
        /// <param name="Command">Command that was ran</param>
        /// <param name="InterceptionContext">Access To The Objects For The Command That was ran</param>
        public override void LogCommand<TResult>(DbCommand Command, DbCommandInterceptionContext<TResult> InterceptionContext)
        {
            //we want to replace all the @variable names with the actual value in the log. so go grab the updated sql and write that to the log
            Write(UpdateSqlWithParameterValues(Command));
        }

        /// <summary>
        /// Connection Is Opened
        /// </summary>
        /// <param name="Connection">Connection</param>
        /// <param name="InterceptionContext">Interception Context</param>
        public override void Opened(DbConnection Connection, DbConnectionInterceptionContext InterceptionContext)
        {
            //if we want to write to this log then do it (this way it excludes in the log that the connection was opened at...)
            if (LogConnectionOpened)
            {
                //call the base. we might have to change this one day but for now its good.
                base.Opened(Connection, InterceptionContext);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Loops through all the parameters and replaces the parameter name with the parameter value
        /// </summary>
        /// <param name="Command">Db command that was ran</param>
        /// <returns>Updated sql with all the parameters so you can copy and paste straight into management studio and have it run</returns>
        private static string UpdateSqlWithParameterValues(DbCommand Command)
        {
            //grab the sql from the command text
            StringBuilder SqlToReturn = new StringBuilder(Command.CommandText);

            //loop through the parameters and replace the value with the actual value in the sql
            foreach (DbParameter thisParameter in Command.Parameters)
            {
                //grab the parameter name with the @
                string thisParameterName = string.Format("@{0}", thisParameter.ParameterName);

                //holds the format which we need for anything that needs a quote
                string thisReplaceFormat;

                //is this something that we need a quote on?
                if (SharedSqlHelpers.DataTypeNeedsQuoteInSql(thisParameter.DbType))
                {
                    //it needs a quote
                    thisReplaceFormat = "'{0}'";
                }
                else
                {
                    //doesnt need a quote
                    thisReplaceFormat = "{0}";
                }

                //go replace the parameter
                SqlToReturn = SqlToReturn.Replace(thisParameterName, string.Format(thisReplaceFormat, (thisParameter.Value == null ? null : thisParameter.Value.ToString())));
            }

            //return the sql
            return SqlToReturn.ToString();
        }

        #endregion

    }
}
