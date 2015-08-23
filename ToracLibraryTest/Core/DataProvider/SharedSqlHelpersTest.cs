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
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiString));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiStringFixedLength));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.String));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.StringFixedLength));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Boolean));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Guid));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Xml));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Binary));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Byte));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Currency));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Decimal));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Double));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int16));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int32));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int64));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.SByte));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Single));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt16));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt32));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt64));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.VarNumeric));
        }

        [TestCategory("Core.DataProviders.SqlBuilder")]
        [TestCategory("Core")]
        [TestMethod]
        public void DataTypeNeedsQuoteInSqlCSharpTypeTest1()
        {
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(string)));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime)));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime?)));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool)));
            Assert.IsTrue(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool?)));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16?)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32?)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64?)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64)));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double?)));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single?)));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float?)));

            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal)));
            Assert.IsFalse(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal?)));
        }
    }

}