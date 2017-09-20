using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ToracLibrary.ExcelEPPlus.Builder;
using ToracLibrary.ExcelEPPlus;
using Moq;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;
using ToracLibrary.ExcelEPPlus.Builder.Formatters;
using OfficeOpenXml;

namespace ToracLibrary.UnitTest.UnitTests.ExcelEPPlus
{

    /// <summary>
    /// Unit tests for EP Plus - mainly the fluent api
    /// </summary>
    public class ExcelEPPlusTest
    {

        #region Framework

        public enum EPPlusUnitTestColumns
        {
            [ExcelColumnHeader("Column 1")]
            Column1 = 1,

            [ExcelColumnHeader("Column 2")]
            Column2 = 2,

            [ExcelColumnHeader("Column 3")]
            Column3Date = 3
        }

        public class ExcelEPPlusDataRow
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public DateTime CreatedDate { get; set; }

            public static IEnumerable<ExcelEPPlusDataRow> BuildTestData()
            {
                yield return new ExcelEPPlusDataRow { Id = 1, Text = "Test1", CreatedDate = DateTime.Now };
                yield return new ExcelEPPlusDataRow { Id = 2, Text = "Test2", CreatedDate = DateTime.Now };
            }
        }

        public static ExcelFluentWorksheetBuilder<EPPlusUnitTestColumns, ExcelEPPlusDataRow> BlankBuilder()
        {
            return new ExcelFluentWorksheetBuilder<EPPlusUnitTestColumns, ExcelEPPlusDataRow>(new Mock<IExcelEPPlusCreator>().Object);
        }

        public static ExcelFluentWorksheetBuilder<EPPlusUnitTestColumns, ExcelEPPlusDataRow> CompleteDefaultConfig(Mock<IExcelEPPlusCreator> CreatorMock, bool AutoFitColumns = true)
        {
            return new ExcelFluentWorksheetBuilder<EPPlusUnitTestColumns, ExcelEPPlusDataRow>(CreatorMock.Object)
                            .AddWorkSheet("Jason")
                            .AddHeader(1, true, true, AutoFitColumns)
                            .AddDataMapping(EPPlusUnitTestColumns.Column1, x => x.Id)
                            .AddDataMapping(EPPlusUnitTestColumns.Column2, x => x.Text)
                            .AddDataMapping(EPPlusUnitTestColumns.Column3Date, x => x.CreatedDate)
                            .AddColumnFormatter(EPPlusUnitTestColumns.Column3Date, Formatter.ExcelBuilderFormatters.Date)
                            .AddColumnWidth(EPPlusUnitTestColumns.Column3Date, 50);
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// make sure if we don't specify a worksheet name that it throws an error
        /// </summary>        
        [Fact]
        public void NoWorkSheetNameShouldThrow()
        {
            Assert.Throws<NullReferenceException>(() => BlankBuilder()
                                                        .Build(ExcelEPPlusDataRow.BuildTestData()));
        }

        /// <summary>
        /// make sure if we don't specify a column configuration that it throws an error
        /// </summary>        
        [Fact]
        public void NoColumnMappingShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BlankBuilder()
                                                                .AddWorkSheet("Jason")
                                                                .AddHeader(1, true, true, true)
                                                                .Build(ExcelEPPlusDataRow.BuildTestData()));
        }

        /// <summary>
        /// make sure a no header calls throws an error
        /// </summary>        
        [Fact]
        public void NoHeaderSetShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => BlankBuilder()
                                                                .AddWorkSheet("Jason")
                                                                .AddDataMapping(EPPlusUnitTestColumns.Column1, x => x.Id)
                                                                .AddDataMapping(EPPlusUnitTestColumns.Column2, x => x.Text)
                                                                .Build(null));
        }

        /// <summary>
        /// make sure if we pass in a null data set that it throws an error
        /// </summary>        
        [Fact]
        public void NoDataSetShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => BlankBuilder()
                                                                .AddWorkSheet("Jason")
                                                                .AddDataMapping(EPPlusUnitTestColumns.Column1, x => x.Id)
                                                                .AddDataMapping(EPPlusUnitTestColumns.Column2, x => x.Text)
                                                                .Build(null));
        }

        /// <summary>
        /// Dont call autofit when config says not to
        /// </summary>        
        [Fact]
        public void DontCallAutoFit()
        {
            var MockEPPlusCreator = new Mock<ExcelEPPlusCreator>() { CallBase = true }.As<IExcelEPPlusCreator>();

            MockEPPlusCreator.Setup(x => x.AutoFitColumnsInASpreadSheet(It.IsAny<ExcelWorksheet>()));

            CompleteDefaultConfig(MockEPPlusCreator, AutoFitColumns: false)
                .Build(ExcelEPPlusDataRow.BuildTestData());

            MockEPPlusCreator.Verify(x => x.AutoFitColumnsInASpreadSheet(It.IsAny<ExcelWorksheet>()), Times.Never);
        }

        /// <summary>
        /// make sure if we get the correct configuration after the fluent build up
        /// </summary>        
        [Fact]
        public void ConfigIsCorrect()
        {
            var Config = BlankBuilder()
                            .AddWorkSheet("Jason")
                            .AddHeader(1, true, true, true)
                            .AddDataMapping(EPPlusUnitTestColumns.Column1, x => x.Id)
                            .AddDataMapping(EPPlusUnitTestColumns.Column2, x => x.Text)
                            .AddDataMapping(EPPlusUnitTestColumns.Column3Date, x => x.CreatedDate)
                            .AddColumnFormatter(EPPlusUnitTestColumns.Column3Date, Formatter.ExcelBuilderFormatters.Date)
                            .AddColumnWidth(EPPlusUnitTestColumns.Column3Date, 50);

            Assert.Equal("Jason", Config.WorkSheetName);

            //header config
            Assert.Equal(1, Config.HeaderConfiguration.RowIndexToWriteInto);
            Assert.True(Config.HeaderConfiguration.MakeBold);
            Assert.True(Config.HeaderConfiguration.AutoFitTheColumns);
            Assert.True(Config.HeaderConfiguration.AddAutoFilter);

            //column configuration
            Assert.Equal(EPPlusUnitTestColumns.Column3Date, Config.ColumnConfiguration[EPPlusUnitTestColumns.Column3Date].ColumnValue);
            Assert.Equal(50, Config.ColumnConfiguration[EPPlusUnitTestColumns.Column3Date].ColumnWidth);
            Assert.Equal(Formatter.ExcelBuilderFormatters.Date, Config.ColumnConfiguration[EPPlusUnitTestColumns.Column3Date].Formatter);
            Assert.NotNull(Config.ColumnConfiguration[EPPlusUnitTestColumns.Column3Date].DataMapper);
        }

        /// <summary>
        /// Build renders correctly
        /// </summary>        
        [Fact]
        public void BuildRendersCorrectly()
        {
            var MockEPPlusCreator = new Mock<ExcelEPPlusCreator>() { CallBase = true }.As<IExcelEPPlusCreator>();

            MockEPPlusCreator.Setup(x => x.AutoFitColumnsInASpreadSheet(It.IsAny<ExcelWorksheet>()));

            CompleteDefaultConfig(MockEPPlusCreator)
                            .Build(ExcelEPPlusDataRow.BuildTestData());

            Assert.True(true);
            MockEPPlusCreator.Verify(x => x.AutoFitColumnsInASpreadSheet(It.IsAny<ExcelWorksheet>()), Times.Once);
        }

        #endregion

    }

}
