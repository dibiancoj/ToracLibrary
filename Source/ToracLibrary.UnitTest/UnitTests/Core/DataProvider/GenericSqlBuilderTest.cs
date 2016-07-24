using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ToracLibrary.Core.DataProviders.SqlBuilder;
using Xunit;

namespace ToracLibrary.UnitTest.Core.DataProviders.SqlBuilder
{

    /// <summary>
    /// Unit test to test generic building of sql
    /// </summary>
    public class GenericSqlBuilderTest
    {

        #region Framework

        private class TestSqlBuilder
        {

            #region Properties

            public int id { get; set; }
            public string txt { get; set; }
            public bool bl { get; set; }
            public DateTime dt { get; set; }
            public int? idNull { get; set; }
            public int? IdNullFilled { get; set; }
            public bool? boolNull { get; set; }
            public DateTime? dtNull { get; set; }
            public TestSqlBuilder SubObject { get; set; }

            #endregion

            #region Static Methods

            public static TestSqlBuilder BuildTestObject()
            {
                return new TestSqlBuilder
                {
                    id = 1,
                    txt = "txt1",
                    bl = true,
                    dt = new DateTime(1980, 12, 1),
                    idNull = null,
                    IdNullFilled = 10,
                    boolNull = null,
                    dtNull = null,
                    SubObject = null
                };
            }

            #endregion

        }

        #endregion

        #region Unit Test

        [Fact]
        public void GenericSqlBuilderInsertTest1()
        {
            //go grab the insert sql
            var InsertSqlToTest = GenericSqlBuilder.BuildInsertSql(TestSqlBuilder.BuildTestObject(), "dbo", typeof(TestSqlBuilder).Name, "id", false);

            //let's test the results now
            Assert.Equal("INSERT INTO dbo.TestSqlBuilder(id,txt,bl,dt,idNull,IdNullFilled,boolNull,dtNull) VALUES(1,'txt1','True','12/1/1980 12:00:00 AM',Null,10,'Null','Null');", InsertSqlToTest);
        }

        [Fact]
        public void GenericSqlBuilderUpdateTest1()
        {
            //go grab the update sql
            var UpdateSqlToTest = GenericSqlBuilder.BuildUpdateSql(TestSqlBuilder.BuildTestObject(), "dbo", typeof(TestSqlBuilder).Name, "id");

            //let's test the results now
            Assert.Equal("UPDATE dbo.TestSqlBuilder SET txt='txt1',bl='True',dt='12/1/1980 12:00:00 AM',idNull=Null,IdNullFilled=10,boolNull='Null',dtNull='Null' WHERE id = 1;", UpdateSqlToTest);
        }

        #endregion

    }

}