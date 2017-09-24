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

        public static ExcelFluentBuilder BlankBuilder()
        {
            return new ExcelFluentBuilder(new Mock<IExcelEPPlusCreator>().Object);
        }

        public static ExcelFluentWorkSheetBuilder<ExcelEPPlusDataRow> CompleteDefaultConfig(Mock<IExcelEPPlusCreator> CreatorMock, bool MakeHeaderBold = true, bool AddAutoFilter = true, bool AutoFitColumns = true)
        {
            return new ExcelFluentBuilder(CreatorMock.Object)
                            .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
                            .AddHeader(1, MakeHeaderBold, AddAutoFilter, AutoFitColumns)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column1, "Column 1", x => x.Id)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column2, "Column 2", x => x.Text)
                            .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column3Date, "Column 3", x => x.CreatedDate, Formatter.ExcelBuilderFormatters.Date, 50);
        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// make sure if we don't specify a column configuration that it throws an error
        /// </summary>        
        [Fact]
        public void NoColumnMappingShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BlankBuilder()
                                                                .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
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
                                                                .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
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
                                                                .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
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
                                                                .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
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
        /// Ensure a header of not bold doesn't create a bold header
        /// </summary>        
        [Fact]
        public void HeaderIsNotBold()
        {
            var MockEPPlusCreator = new Mock<ExcelEPPlusCreator>() { CallBase = true }.As<IExcelEPPlusCreator>();

            CompleteDefaultConfig(MockEPPlusCreator, MakeHeaderBold: false)
                           .Build(ExcelEPPlusDataRow.BuildTestData());

            MockEPPlusCreator.Verify(x => x.MakeRangeBold(It.IsAny<ExcelRange>()), Times.Never);
        }

        /// <summary>
        /// Ensure a header doesn't add an auto filter
        /// </summary>        
        [Fact]
        public void HeaderDoesntAddAutoFilter()
        {
            var MockEPPlusCreator = new Mock<ExcelEPPlusCreator>() { CallBase = true }.As<IExcelEPPlusCreator>();

            CompleteDefaultConfig(MockEPPlusCreator, AddAutoFilter: false)
                           .Build(ExcelEPPlusDataRow.BuildTestData());

            MockEPPlusCreator.Verify(x => x.AddAutoFilter(It.IsAny<ExcelRange>()), Times.Never);
        }

        /// <summary>
        /// make sure if we get the correct configuration after the fluent build up
        /// </summary>        
        [Fact]
        public void ConfigIsCorrect()
        {
            var Config = BlankBuilder()
                            .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
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

            CompleteDefaultConfig(MockEPPlusCreator)
                            .Build(ExcelEPPlusDataRow.BuildTestData());

            //verify add worksheet is called once
            MockEPPlusCreator.Verify(x => x.AddWorkSheet(WorksheetNameToUse), Times.Once);

            //verify select worksheet is called once
            MockEPPlusCreator.Verify(x => x.WorkSheetSelect(WorksheetNameToUse), Times.Once);

            //verify we made the header bold
            MockEPPlusCreator.Verify(x => x.MakeRangeBold(It.IsAny<ExcelRange>()), Times.Once);

            //verify we added the auto filter
            MockEPPlusCreator.Verify(x => x.AddAutoFilter(It.IsAny<ExcelRange>()), Times.Once);

            //verify the auto fit
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

        /// <summary>
        /// Build renders correctly with multiple worksheets
        /// </summary>        
        [Fact]
        public void BuildRendersCorrectlyWithMultipleWorksheets()
        {
            var MockEPPlusCreator = new Mock<ExcelEPPlusCreator>() { CallBase = true }.As<IExcelEPPlusCreator>();

            const string SecondWorksheetNameToUse = "WorksheetNumber2";

            //2nd spreadsheet data
            var SecondWorkSheetDataSet = new List<Tuple<int, string>>
            {
                new Tuple<int, string>(1,"1"),
                new Tuple<int, string>(2,"2"),
            };

            var Config = new ExcelFluentBuilder(MockEPPlusCreator.Object)
                                //build spreadsheet 1 (we are duplicating the config code here so we can see the full syntax and ensure we like it)
                                .AddWorkSheet<ExcelEPPlusDataRow>(WorksheetNameToUse)
                                    .AddHeader(1, true, true, true)
                                    .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column1, "Column 1", x => x.Id)
                                    .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column2, "Column 2", x => x.Text)
                                    .AddColumnConfiguration((int)EPPlusUnitTestColumns.Column3Date, "Column 3", x => x.CreatedDate, Formatter.ExcelBuilderFormatters.Date, 50)
                                .Build(ExcelEPPlusDataRow.BuildTestData())

                                //add the next spreadsheet
                                .AddWorkSheet<Tuple<int, string>>(SecondWorksheetNameToUse)
                                    .AddHeader(1, false, false, false)
                                    .AddColumnConfiguration(1, "Column A", x => x.Item1)
                                    .AddColumnConfiguration(2, "Column B", x => x.Item2)
                                .Build(SecondWorkSheetDataSet);

            //grab both worksheets so we can check which one gets rendered to
            var WorkSheet1 = Config.ExcelCreator.WorkSheetSelect(WorksheetNameToUse);
            var WorkSheet2 = Config.ExcelCreator.WorkSheetSelect(SecondWorksheetNameToUse);

            //verify add worksheet is called once
            MockEPPlusCreator.Verify(x => x.AddWorkSheet(WorksheetNameToUse), Times.Once);
            MockEPPlusCreator.Verify(x => x.AddWorkSheet(SecondWorksheetNameToUse), Times.Once);

            //verify select worksheet is called once (we call it twice so we call it above to get the instance)
            MockEPPlusCreator.Verify(x => x.WorkSheetSelect(WorksheetNameToUse), Times.Exactly(2));
            MockEPPlusCreator.Verify(x => x.WorkSheetSelect(SecondWorksheetNameToUse), Times.Exactly(2));

            //verify we made the header bold
            MockEPPlusCreator.Verify(x => x.MakeRangeBold(It.IsAny<ExcelRange>()), Times.Once);

            //verify we added the auto filter
            MockEPPlusCreator.Verify(x => x.AddAutoFilter(It.IsAny<ExcelRange>()), Times.Once);

            //verify the auto fit
            MockEPPlusCreator.Verify(x => x.AutoFitColumnsInASpreadSheet(It.Is<ExcelWorksheet>(y => y == WorkSheet1)), Times.Once);

            //write out the headers
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 1, 1, "Column 1"), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 2, 1, "Column 2"), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 3, 1, "Column 3"), Times.Once);

            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet2), 1, 1, "Column A"), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet2), 2, 1, "Column B"), Times.Once);

            var DataSet = ExcelEPPlusDataRow.BuildTestData().ToList();

            //check the body now
            //1st record
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 1, 2, DataSet[0].Id), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 2, 2, DataSet[0].Text), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 3, 2, DataSet[0].CreatedDate), Times.Once);

            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 1, 3, DataSet[1].Id), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 2, 3, DataSet[1].Text), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet1), 3, 3, DataSet[1].CreatedDate), Times.Once);

            //make sure only 2 rows were written (body of data)
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet2), 1, 4, It.IsAny<object>()), Times.Never);

            //handle the 2nd worksheet
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet2), 1, 2, SecondWorkSheetDataSet[0].Item1), Times.Once);
            MockEPPlusCreator.Verify(x => x.WriteToCell(It.Is<ExcelWorksheet>(y => y == WorkSheet2), 2, 2, SecondWorkSheetDataSet[0].Item2), Times.Once);
        }


        #endregion

    }

}
