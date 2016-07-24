using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToracLibrary.Core.DataProviders.SqlBuilder;
using Xunit;

namespace ToracLibrary.UnitTest.Core.DataProviders.SqlBuilder
{

    /// <summary>
    /// Unit test to test shared sql
    /// </summary>
    public class SharedSqlHelpersTest
    {

        [Fact]
        public void DataTypeNeedsQuoteInSqlDbDataTest1()
        {
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiString));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.AnsiStringFixedLength));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.String));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.StringFixedLength));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Boolean));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Date));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTime2));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.DateTimeOffset));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Guid));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Xml));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Binary));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Byte));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Currency));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Decimal));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Double));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int16));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int32));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Int64));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.SByte));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.Single));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt16));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt32));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.UInt64));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(DbType.VarNumeric));
        }

        [Fact]
        public void DataTypeNeedsQuoteInSqlCSharpTypeTest1()
        {
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(string)));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime)));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(DateTime?)));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool)));
            Assert.True(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(bool?)));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16?)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int16)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32?)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int32)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64?)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Int64)));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(double?)));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(Single?)));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(float?)));

            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal)));
            Assert.False(SharedSqlHelpers.DataTypeNeedsQuoteInSql(typeof(decimal?)));
        }

    }

}