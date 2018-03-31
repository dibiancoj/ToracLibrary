using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Common non instance data providers helpers
    /// </summary>
    public static class EFUnTypedDP
    {

        #region Utilities

        /// <summary>
        /// Builds an entity framework connection string
        /// </summary>
        /// <param name="ServerName">Server Name</param>
        /// <param name="DatabaseName">Database Name</param>
        /// <returns>connection string to use</returns>
        public static string BuildConnectionString(string ServerName, string DatabaseName)
        {
            //Initialize the connection string builder for the
            //underlying provider.
            var Builder = new SqlConnectionStringBuilder
            {
                //Set the properties for the data source.
                DataSource = ServerName,
                InitialCatalog = DatabaseName,
                IntegratedSecurity = true
            };

            // Initialize the EntityConnectionStringBuilder.
            return new EntityConnectionStringBuilder
            {
                //Set the provider name.
                Provider = "System.Data.SqlClient",

                //Set the provider-specific connection string.
                ProviderConnectionString = Builder.ToString(),

                //Set the Metadata location.
                Metadata = "res://*/",

            }.ToString();
        }

        #endregion

    }

}
