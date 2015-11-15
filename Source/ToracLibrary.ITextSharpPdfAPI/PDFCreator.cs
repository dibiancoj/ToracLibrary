using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.events;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ITextSharpPdfAPI
{

    //This Abstraction Uses ITextSharp To Create The Pdf. It sort of is like excel where you add cells to a table. Then add the
    //table to the document. 

    /// <summary>
    /// Class Used To Create PDF's Using Code And ITextSharp
    /// </summary>
    /// <remarks>Class has properties that are immutable</remarks>
    public class PDFCreator : IDisposable
    {

        #region Constructor

        /// <summary>
        /// PDF Creator Helper With No Page Base (Don't want to write a page or report (header or footer)
        /// </summary>
        /// <param name="PageSize">Page Size - A4 is a default page size. Use Enum iTextSharp.text.PageSize</param>
        /// <param name="LandscapeMode">Do you want the pdf in landscape</param>
        /// <param name="MarginTop">Top Margin On The Page</param>
        /// <param name="MarginRight">Right Margin On The Page</param>
        /// <param name="MarginBottom">Bottom Margin On The Page</param>
        /// <param name="MarginLeft">Left Margin On The Page</param>
        public PDFCreator(Rectangle PageSize,
                          bool LandscapeMode,
                          float MarginTop,
                          float MarginRight,
                          float MarginBottom,
                          float MarginLeft) : this(PageSize, LandscapeMode, MarginTop, MarginRight, MarginBottom, MarginLeft, null)
        {
        }

        /// <summary>
        /// PDF Creator Helper With No Page Base (Don't want to write a page or report (header or footer)
        /// </summary>
        /// <param name="PageSize">Page Size - A4 is a default page size. Use Enum iTextSharp.text.PageSize</param>
        /// <param name="LandscapeMode">Do you want the pdf in landscape</param>
        /// <param name="MarginTop">Top Margin On The Page</param>
        /// <param name="MarginRight">Right Margin On The Page</param>
        /// <param name="MarginBottom">Bottom Margin On The Page</param>
        /// <param name="MarginLeft">Left Margin On The Page</param>
        /// <param name="PageEventHandler">A Page Event Class That Will Raise Any Events You Need</param>
        public PDFCreator(Rectangle PageSize,
                          bool LandscapeMode,
                          float MarginTop,
                          float MarginRight,
                          float MarginBottom,
                          float MarginLeft,
                          PageEventsBase PageEventHandler)
        {
            //create the instance of the ITextSharpDocument
            Doc = new Document(PageSize, MarginLeft, MarginRight, MarginTop, MarginBottom);

            //let's build the memory stream now
            Ms = new MemoryStream();

            //let's create the new writer and attach the document
            Writer = PdfWriter.GetInstance(Doc, Ms);

            //add the page events to my custom class
            if (PageEventHandler != null)
            {
                Writer.PageEvent = PageEventHandler;
            }

            //if you want the pdf in landscape now, then rotate it
            if (LandscapeMode)
            {
                Doc.SetPageSize(PageSize.Rotate());
            }

            //let's open the document so the developer can do whatever they wan't with it
            Doc.Open();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Document To Build
        /// </summary>
        public Document Doc { get; }

        /// <summary>
        /// Memory Stream To Write Too
        /// </summary>
        protected MemoryStream Ms { get; }

        /// <summary>
        /// PDF Writer
        /// </summary>
        public PdfWriter Writer { get; }

        #region Dispose Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool disposed { get; set; }

        #endregion

        #endregion

        #region Public Enums

        /// <summary>
        /// Holds the map from enum to int which is used in itextsharp (Otherwise You can just use Element.ALIGN_LEFT)
        /// </summary>
        public enum HorizontalAlignmentEnum : int
        {
            /// <summary>
            /// Align Left
            /// </summary>
            Left = 0,

            /// <summary>
            /// Align Middel
            /// </summary>
            Middle = 1,

            /// <summary>
            /// Align Right
            /// </summary>
            Right = 2
        }

        #endregion

        #region Public Methods

        #region Saving Pdf

        /// <summary>
        /// After you are done building the Pdf. Method will return the Pdf to a byte array
        /// </summary>
        /// <returns>Byte Array</returns>
        public byte[] ToPdfStream()
        {
            //close the document and return the memory stream
            Doc.Close();

            //return the memory stream now
            return Ms.ToArray();
        }

        /// <summary>
        ///  After you are done building the Pdf. Method will return save the file and return a result
        /// </summary>
        /// <param name="FilePathAndName">File Path And Name To Save Too</param>
        /// <returns>result of save operation</returns>
        public bool ToPdfFile(string FilePathAndName)
        {
            //create the file stream to return
            using (FileStream FileStreamToUse = new FileStream(FilePathAndName, FileMode.Create))
            {
                //let's grab the byte array so we can save it
                byte[] PdfContents = ToPdfStream();

                //let's go ahead and save the byte array to a file
                FileStreamToUse.Write(PdfContents, 0, PdfContents.Length);

                //close the file stream
                FileStreamToUse.Close();
            }

            //return the result that it completed successfully
            return true;
        }

        #endregion

        #region Add New Pages

        /// <summary>
        /// Add a new page or skip the rest of the page you are on, and go to the next page.
        /// </summary>
        public void AddNewPage()
        {
            //add a new page...or move to the next page
            Doc.NewPage();
        }

        #endregion

        #region Table Helper Methods

        /// <summary>
        /// Color a tables row using zebra striping
        /// </summary>
        /// <param name="TableToColor">Table To Color</param>
        /// <param name="EvenRowColor">Even Row Color</param>
        /// <param name="OddRowColor">Odd Row Color</param>
        /// <remarks>BaseColor Has Several Overloads. Color, RGB, and predefined colors</remarks>
        public void ColorTableRows(PdfPTable TableToColor, BaseColor EvenRowColor, BaseColor OddRowColor)
        {
            //***note about BaseColor class
            //BaseColor Has Several Overloads. Color, RGB, and predefined colors

            //holds the row index
            int RowIndex = 1;

            //loop through each of the table rows
            foreach (var RowToColor in TableToColor.Rows)
            {
                //make sure we are past the header rows
                if (RowIndex > TableToColor.HeaderRows)
                {
                    //holds the color to set the row
                    BaseColor BgColor;

                    //if we are on an even row then set the background color
                    if (RowIndex % 2 == 0)
                    {
                        BgColor = EvenRowColor;
                    }
                    else
                    {
                        BgColor = OddRowColor;
                    }

                    //let's loop through all the cells now
                    foreach (var CellToColor in RowToColor.GetCells())
                    {
                        //cell is null if it has a span
                        if (CellToColor != null)
                        {
                            //set the background color to gray
                            CellToColor.BackgroundColor = BgColor;
                        }
                    }
                }

                //increase the row
                RowIndex++;
            }
        }

        /// <summary>
        /// Make this cell editable when you are in adobe acrobat.
        /// </summary>
        /// <param name="CellToAddTextFieldOn">Cell to add the writable text field too</param>
        /// <param name="FieldNameToUse">Field name to give the text field</param>
        public void MyCellWritableTextField(PdfPCell CellToAddTextFieldOn, string FieldNameToUse)
        {
            //this is for acrobat, editable field
            var textField = new TextField(Writer, new Rectangle(0, 0, 0, 0), FieldNameToUse);
            //{
            //    FontSize = 12
            //};

            //add the cell event so we get the adobe fill in
            CellToAddTextFieldOn.CellEvent = new FieldPositioningEvents(Writer, textField.GetTextField());
        }

        #endregion

        #region Form Editing

        /// <summary>
        /// When you open a pdf and you have form fields, this will have "show highlight fields" on by default. This will give the form the purple highlight marks
        /// </summary>
        public void HighlightFormsOnOpenByDefault()
        {
            //create this action and return it
            Writer.AddJavaScript(PdfAction.JavaScript("app.runtimeHighlight = true;", Writer));
        }

        #endregion

        #endregion

        #region Public Static Methods

        #region Make Phrase - Or Text

        /// <summary>
        /// Make a text string phrase
        /// </summary>
        /// <param name="TextToOutput">text to output</param>
        /// <returns>Phrase</returns>
        public static Phrase MakePhrase(string TextToOutput)
        {
            return MakePhrase(TextToOutput, null, null, null);
        }

        /// <summary>
        /// Make a text string phrase
        /// </summary>
        /// <param name="TextToOutput">text to output</param>
        /// <param name="MakeBold">Do you want to make the text bold?</param>
        /// <returns>Phrase</returns>
        public static Phrase MakePhrase(string TextToOutput, bool MakeBold)
        {
            return MakePhrase(TextToOutput, MakeBold, null, null);
        }

        /// <summary>
        /// Make a text string phrase
        /// </summary>
        /// <param name="TextToOutput">text to output</param>
        /// <param name="MakeBold">Do you want to make the text bold?</param>
        /// <param name="FontColor">Font Color</param>
        /// <param name="FontSize">Font Size</param>
        /// <returns>Phrase</returns>
        public static Phrase MakePhrase(string TextToOutput, bool? MakeBold, BaseColor FontColor, int? FontSize)
        {
            //holds the font family to use
            const Font.FontFamily FontToUse = Font.FontFamily.HELVETICA;

            //holds the font size
            int FontSizeToUse = 8;

            //holds the font color
            BaseColor FontColorToRender;

            //holds the conversion if the cell is bold
            int IsBold;

            //if we wan't it bold then make it bold
            if (MakeBold.HasValue && MakeBold.Value)
            {
                IsBold = Font.BOLD;
            }
            else
            {
                IsBold = Font.UNDEFINED;
            }

            //if we pass in a font color
            if (FontColor == null)
            {
                FontColorToRender = BaseColor.BLACK;
            }
            else
            {
                FontColorToRender = FontColor;
            }

            //if we pass in a font size then set it now
            if (FontSize.HasValue)
            {
                FontSizeToUse = FontSize.Value;
            }

            //build up the phrase and return it
            return new Phrase(TextToOutput, new iTextSharp.text.Font(FontToUse, FontSizeToUse, IsBold, FontColorToRender));
        }

        #endregion

        #region Build Image

        /// <summary>
        /// Build a iTextSharp Image From .NET Image File
        /// </summary>
        /// <param name="ImageFile">.NET Image File</param>
        /// <param name="ImageFormatToRender">What Image Format</param>
        /// <returns>iTextSharp Image File</returns>
        public static Image BuildPDFImage(System.Drawing.Image ImageFile, System.Drawing.Imaging.ImageFormat ImageFormatToRender)
        {
            //create the memory stream
            using (var ImageMemoryStream = new MemoryStream())
            {
                //go put the image into the memory stream
                ImageFile.Save(ImageMemoryStream, ImageFormatToRender);

                //return the itextSharp Image
                return Image.GetInstance(ImageMemoryStream.ToArray());
            }
        }

        #endregion

        #region HorizontalAlignment

        /// <summary>
        /// Convert the enum to the int which can be set in itextsharp
        /// </summary>
        /// <param name="WhichAlignment">Which Alignment to use</param>
        /// <returns>int value to set in itextsharp</returns>
        public static int SetHorizontalAlignmentEnum(HorizontalAlignmentEnum WhichAlignment)
        {
            //return the int value
            return (int)WhichAlignment;
        }

        #endregion

        #region Build Cell

        /// <summary>
        /// Builds a table cell
        /// </summary>
        /// <param name="TextInCell">Text In Cell</param>
        /// <param name="MakeBold">Make the text bold?</param>
        /// <param name="HorAlignment">Horizontal Alignment</param>
        /// <param name="BackgroundColorOfCell">Background Color Of Cell</param>
        /// <param name="Formatter">Format Of Cell Need this to determine if we should make font red, etc.</param>
        /// <param name="ColumnSpanInCells">How many cells to span horizontally</param>
        /// <param name="PaddingLeft">Padding Left</param>
        /// <param name="GridLines">GridLines In Cell</param>
        /// <returns>PdfPCell</returns>
        public static PdfPCell BuildCell(string TextInCell,
                                        string Formatter,
                                        bool MakeBold,
                                        HorizontalAlignmentEnum? HorAlignment,
                                        BaseColor BackgroundColorOfCell,
                                        int? ColumnSpanInCells,
                                        float? PaddingLeft,
                                        bool GridLines)
        {
            //let's go build up the cell with the formatted value
            PdfPCell CellToReturn = new PdfPCell(MakePhrase(TextInCell, MakeBold));

            //add the alignment
            if (HorAlignment.HasValue)
            {
                CellToReturn.HorizontalAlignment = SetHorizontalAlignmentEnum(HorAlignment.Value);
            }

            //if we have a background color then set it
            if (BackgroundColorOfCell != null)
            {
                CellToReturn.BackgroundColor = BackgroundColorOfCell;
            }

            //if we want to span horizontally then set it now
            if (ColumnSpanInCells.HasValue)
            {
                CellToReturn.Colspan = ColumnSpanInCells.Value;
            }

            //if we want a padding left then add it now
            if (PaddingLeft.HasValue)
            {
                CellToReturn.PaddingLeft = PaddingLeft.Value;
            }

            //if we don't want grid lines then remove them now
            if (!GridLines)
            {
                CellToReturn.Border = 0;
            }

            //return the cell
            return CellToReturn;
        }

        #endregion

        #region Table Builder

        /// <summary>
        /// Create a table object with it's column headers and the default settings
        /// </summary>
        /// <param name="ColumnCount">How Many Columns In This Table</param>
        /// <returns>PdfPTable</returns>
        /// <remarks>Defaults To 100% of Page</remarks>
        public static PdfPTable BuildTableTemplate(int ColumnCount)
        {
            return BuildTableTemplate(ColumnCount, null, null);
        }

        /// <summary>
        /// Create a table object with it's column headers and the default settings
        /// </summary>
        /// <param name="ColumnCount">How Many Columns In This Table</param>
        /// <param name="TableAlignment">Table Alignment</param>
        /// <param name="TableWidthInPercentage">Table Width In Percent Relative To Page. 100% would be entire width of page</param>
        /// <returns>PdfPTable</returns>
        public static PdfPTable BuildTableTemplate(int ColumnCount, HorizontalAlignmentEnum? TableAlignment, float? TableWidthInPercentage)
        {
            //create the table object
            PdfPTable TableTemplateToBuild = new PdfPTable(ColumnCount);

            //set the table SetHorizontalAlignmentEnum
            if (TableAlignment.HasValue)
            {
                TableTemplateToBuild.HorizontalAlignment = SetHorizontalAlignmentEnum(TableAlignment.Value);
            }

            //set the table width of the page (100%)
            if (TableWidthInPercentage.HasValue)
            {
                TableTemplateToBuild.WidthPercentage = TableWidthInPercentage.Value;
            }

            //return the table
            return TableTemplateToBuild;
        }

        #endregion

        #region Build Anchor

        /// <summary>
        /// Build a hyperlink
        /// </summary>
        /// <param name="TextToDisplay">Text To Display</param>
        /// <param name="UrlToLinkTo">Url To Link To</param>
        /// <returns>Anchor</returns>
        private Anchor BuildAnchor(string TextToDisplay, string UrlToLinkTo)
        {
            //example on how to add this
            //var Link = new Paragraph();
            //Link.SpacingAfter = 5;
            //Link.Add(new Chunk("View Your Search: ", new iTextSharp.text.Font(Font.FontFamily.HELVETICA, SubHeadingFontSize, Font.BOLD)));
            //Link.Add(BuildAnchor());
            //Doc.Add(Link);

            //build the anchor
            Anchor AnchorToRender = new Anchor(TextToDisplay, new iTextSharp.text.Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, new BaseColor(0, 0, 255)));

            //set the link (the uri)
            AnchorToRender.Reference = UrlToLinkTo;

            //return the anchor
            return AnchorToRender;
        }

        #endregion

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //dispose of the document
                    Doc.Dispose();

                    //memory stream
                    Ms.Dispose();

                    //pdf writer
                    Writer.Dispose();
                }
            }
            disposed = true;
        }

        #endregion

    }

}
