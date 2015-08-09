using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;

namespace ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Unit test for entity framework
    /// </summary>
    [TestClass]
    public class EntityFrameworkTest
    {

        #region Untyped Methods

        [TestCategory("Core.DataProviders.EntityFramework")]
        [TestCategory("EntityFramework")]
        [TestCategory("Core")]
        [TestMethod]
        public void BuildConnectionStringTest1()
        {
            //the value we want to check for
            const string ValueToCheckFor = "metadata=res://*/;provider=System.Data.SqlClient;provider connection string=\"Data Source=ServerName123;Initial Catalog=Db123;Integrated Security=True\"";

            //go run the method and check the results
            Assert.AreEqual(ValueToCheckFor, EFUnTypedDP.BuildConnectionString("ServerName123", "Db123"));
        }

        #endregion

    }

}
