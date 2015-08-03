using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.ADO;
using ToracLibraryTest.UnitsTest.EntityFramework.DataContext;

namespace ToracLibraryTest.Framework
{

    /// <summary>
    /// DI Unit Test Container
    /// </summary>
    public static class DIUnitTestContainer
    {

        #region Constructor

        /// <summary>
        /// Static Constructor
        /// </summary>
        static DIUnitTestContainer()
        {
            //create the new di container
            DIContainer = new UnityContainer();

            //let's go build up the sql data provider
            BuildSqlAdoNetDiContainer();
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Declare a di container so we can build whatever we need
        /// </summary>
        public static UnityContainer DIContainer { get; }

        #endregion

        #region Build Up The Modules For The Unity Container 

        /// <summary>
        /// Builds up the sql data provider for all the sql server data provider unit tests
        /// </summary>
        private static void BuildSqlAdoNetDiContainer()
        {
            //connection string variable
            string SqlServerConnectionString;

            //grab the connection string from the ef model
            using (var EFDataContext = new EntityFrameworkEntityDP())
            {
                //set the connection string
                SqlServerConnectionString = EFDataContext.Database.Connection.ConnectionString;
            }

            //let's register the di container now
            DIContainer.RegisterType<IDataProvider, SQLDataProvider>(new InjectionConstructor(SqlServerConnectionString));
        }

        #endregion

    }

}
