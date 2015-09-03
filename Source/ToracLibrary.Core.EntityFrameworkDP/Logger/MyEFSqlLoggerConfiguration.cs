using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP.Logger
{

    //***** in order to set the EFSqlLogger to be active for the given context you will need to add the following attribute on the context
    //[DbConfigurationType(typeof(MyEFSqlLoggerConfiguration))]
    //public class MyEfContext : DbContext
    //{
    //}

    /// <summary>
    /// Creates the db configuration for a db context so the EFSqlLogger will run when an log is attached to the context
    /// </summary>
    public class MyEFSqlLoggerConfiguration : DbConfiguration
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MyEFSqlLoggerConfiguration()
        {
            //go create the logger (EFSqlLogger)
            SetDatabaseLogFormatter((context, writeAction) => new EFSqlLogger(context, writeAction));
        }

        #endregion

    }

}
