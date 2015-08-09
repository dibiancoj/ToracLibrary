using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToracLibrary.Core.DataProviders.SqlBuilder;

namespace ToracLibraryTest.UnitsTest.Core.DataProviders.SqlBuilder
{

    /// <summary>
    /// Unit test to test shared sql
    /// </summary>
    [TestClass]
    public class SharedSqlHelpersTest
    {

        [TestCategory("Core.DataProviders.SqlBuilder")]
        [TestCategory("Core")]
        [TestMethod]
        public void DataTypeNeedsQuoteInSqlDbDataTest1()
        {
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiString));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiStringFixedLength));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.String));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.StringFixedLength));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Boolean));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Guid));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Xml));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Binary));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Byte));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Currency));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Decimal));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Double));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int16));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int32));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int64));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.SByte));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Single));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt16));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt32));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt64));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.VarNumeric));
        }

        [TestCategory("Core.DataProviders.SqlBuilder")]
        [TestCategory("Core")]
        [TestMethod]
        public void DataTypeNeedsQuoteInSqlCSharpTypeTest1()
        {
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(string)));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime)));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime?)));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool)));
            Assert.AreEqual(true, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool?)));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16?)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32?)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64?)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64)));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double?)));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single?)));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float?)));

            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal)));
            Assert.AreEqual(false, SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal?)));
        }
    }

}