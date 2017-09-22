using Moq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using ToracLibrary.ExcelEPPlus;
using ToracLibrary.ExcelEPPlus.Builder;
using ToracLibrary.ExcelEPPlus.Builder.Attributes;
using ToracLibrary.ExcelEPPlus.Builder.Formatters;
using Xunit;

namespace ToracLibrary.UnitTest.UnitTests.ExcelEPPlus
{

    /// <summary>
    /// Unit tests for EP Plus - mainly the fluent api
    /// </summary>
    public class ExcelEPPlusTest
    {

        #region Constants

        /// <summary>
        /// Spreadsheet name to use it tests
        /// </summary>
        private const string WorksheetNameToUse = "WorkSheet12345";

        #endregion

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
                yield return new ExcelEPPlusDataRow { Id = 1, Text = "Test1", CreatedDate = new DateTime(2017, 1, 1) };
                yield return new ExcelEPPlusDataRow { Id = 2, Text = "Test2", CreatedDate = new DateTime(2017, 1, 2) };
            }
        }

        public static ExcelFluentWorksheetBuilder<ExcelEPPlusDataRow> BlankBuilder()
        {
            return new ExcelFluentWorksheetBuilder<ExcelEPPlusDataRow>(new Mock<IExcelEPPlusCreator>().Object);
        }

        public static ExcelFluentWorksheetBuilder<ExcelEPPlusDataRow> CompleteDefaultConfig(Mock<IExcelEPPlusCreator> CreatorMock, bool AutoFitColumns = true)
        {
            return new ExcelFluentWorksheetBuilder<ExcelEPPlusDataRow>(CreatorMock.Object)
                            .AddWorkSheet(WorksheetNameToUse)
                            .AddHeader(1, true, true, AutoFitColumns)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column1, "Column 1", x => x.Id)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column2, "Column 2", x => x.Text)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column3Date, "Column 3", x => x.CreatedDate, Formatter.ExcelBuilderFormatters.Date, 50);
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
                                                                .AddWorkSheet(WorksheetNameToUse)
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
                                                                .AddWorkSheet(WorksheetNameToUse)
                                                                .AddColumnConfiguration(1, "Column 1", x => x.Id)
                                                                .AddColumnConfiguration(2, "Column 2", x => x.Text)
                                                                .Build(null));
        }

        /// <summary>
        /// make sure if we pass in a null data set that it throws an error
        /// </summary>        
        [Fact]
        public void NoDataSetShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => BlankBuilder()
                                                                .AddWorkSheet(WorksheetNameToUse)
                                                                .AddColumnConfiguration(1, "Column 1", x => x.Id)
                                                                .AddColumnConfiguration(2, "Column 2", x => x.Text)
                                                                .Build(null));
        }

        /// <summary>
        /// make sure if we pass in a null data set that it throws an error
        /// </summary>        
        [Fact]
        public void DuplicateColumnIndexThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BlankBuilder()
                                                                .AddWorkSheet(WorksheetNameToUse)
                                                                .AddColumnConfiguration(1, "Column 1", x => x.Id)
                                                                .AddColumnConfiguration(1, "Column 2", x => x.Text)
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
                            .AddWorkSheet(WorksheetNameToUse)
                            .AddHeader(1, true, true, true)
                            .AddColumnConfiguration(1, "Column 1", x => x.Id)
                            .AddColumnConfiguration(2, "Column 2", x => x.Text)
                            .AddColumnConfiguration(3, "Column 3", x => x.CreatedDate, FormatterToAdd: Formatter.ExcelBuilderFormatters.Date, WidthToSet: 50);

            Assert.Equal(WorksheetNameToUse, Config.WorkSheetName);

            //header config
            Assert.Equal(1, Config.HeaderConfiguration.RowIndexToWriteInto);
            Assert.True(Config.HeaderConfiguration.MakeBold);
            Assert.True(Config.HeaderConfiguration.AutoFitTheColumns);
            Assert.True(Config.HeaderConfiguration.AddAutoFilter);

            //column configuration
            Assert.Equal(50, Config.ColumnConfiguration.First(x => x.ColumnIndex == 3).ColumnWidth);
            Assert.Equal(Formatter.ExcelBuilderFormatters.Date, Config.ColumnConfiguration.First(x => x.ColumnIndex == 3).Formatter);
            Assert.NotNull(Config.ColumnConfiguration.First(x => x.ColumnIndex == 3).DataMapper);
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

            MockEPPlusCreator.Verify(x => x.AutoFitColumnsInASpreadSheet(It.IsAny<ExcelWorksheet>()), Times.Once);

            //write out the headers
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 1, 1, "Column 1"), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 2, 1, "Column 2"), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 3, 1, "Column 3"), Times.Once);

            var DataSet = ExcelEPPlusDataRow.BuildTestData().ToList();

            //check the body now
            //1st record
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 1, 2, DataSet[0].Id), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 2, 2, DataSet[0].Text), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 3, 2, DataSet[0].CreatedDate), Times.Once);

            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 1, 3, DataSet[1].Id), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 2, 3, DataSet[1].Text), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 3, 3, DataSet[1].CreatedDate), Times.Once);

            //make sure only 2 rows were written (body of data)
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.IsAny<ExcelWorksheet>(), 1, 4, It.IsAny<object>()), Times.Never);
        }

        #endregion

    }

}
