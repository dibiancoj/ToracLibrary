using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ITextSharpPdfAPI
{

    //** Note for a full example see the bottom of the page way below

    //Notes...take a look at the example below in OnEndPage to write out page name or some other footer. Just inherit from this class..
    //then pass in your Derived class to pdf creator (constructor - PageEventsBase PageEventHandler)
    //then in your own class you can override whatever you want

    /// <summary>
    /// Helper class to write the page numbers on each page
    /// </summary>
    public class PageEventsBase : PdfPageEventHelper
    {

        #region Properties

        /// <summary>
        /// Contenty byte which you can use to write to the document
        /// </summary>
        protected PdfContentByte ContentByte { get; set; }

        /// <summary>
        /// Template which is used to modify the template
        /// </summary>
        protected PdfTemplate Template { get; set; }

        /// <summary>
        /// Gets the base font
        /// </summary>
        protected BaseFont FontBase { get; set; }

        #endregion

        /// <summary>
        /// On The Document Open...populate our variables
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            ContentByte = writer.DirectContent;
            Template = ContentByte.CreateTemplate(50, 50);
            FontBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        }

        /// <summary>
        /// On close document
        /// </summary>
        /// <param name="writer">Writer</param>
        /// <param name="document">Document</param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            Template.BeginText();
            Template.SetFontAndSize(FontBase, 8);
            Template.SetTextMatrix(0, 0);
            Template.ShowText("" + (writer.PageNumber - 1));
            Template.EndText();
        }

        ///// <summary>
        ///// Example of how to write Page x of y...and how to write the date the report was created on each of the pages
        ///// </summary>
        ///// <param name="writer">Writer</param>
        ///// <param name="document">Document</param>
        //public override void OnEndPage(PdfWriter writer, Document document)
        //{
        //    base.OnEndPage(writer, document);

        //    const int BottomMargin = 3;

        //    int pageN = writer.PageNumber;
        //    String text = "Page " + pageN + " of ";
        //    float len = bf.GetWidthPoint(text, 8);

        //    Rectangle pageSize = document.PageSize;

        //    cb.SetRGBColorFill(100, 100, 100);

        //    cb.BeginText();
        //    cb.SetFontAndSize(bf, 8);
        //    cb.SetTextMatrix(pageSize.GetLeft(document.LeftMargin), pageSize.GetBottom(BottomMargin));
        //    cb.ShowText(text);
        //    cb.EndText();

        //    cb.AddTemplate(template, pageSize.GetLeft(document.LeftMargin) + len, pageSize.GetBottom(BottomMargin));

        //    cb.BeginText();
        //    cb.SetFontAndSize(bf, 8);
        //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
        //                       "Printed On " + DateTime.Now.ToString(),
        //                       pageSize.GetRight(document.RightMargin),
        //                       pageSize.GetBottom(BottomMargin), 0);
        //    cb.EndText();
        //}

    }

    //Full Example Of How To Use This Class And Output Page Footer 

    //    [HttpGet]
    //public ActionResult FileDownload()
    //{
    //    PDFCreator pdf = new PDFCreator(iTextSharp.text.PageSize.A4, true, 10, 10, 10, 10, new jason());
    //    pdf.Doc.Add(PDFCreator.MakePhrase("jason"));
    //    return File(pdf.ToPdfStream(), "application/pdf");
    //}

    //public class jason : ToracTechnologies.Library.Pdf.Creators.PageEvents.PageEventsBase
    //{

    //    public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
    //    {
    //        base.OnEndPage(writer, document);

    //        const int BottomMargin = 3;

    //        int pageN = writer.PageNumber;
    //        String text = "Page " + pageN + " of ";
    //        float len = bf.GetWidthPoint(text, 8);

    //        Rectangle pageSize = document.PageSize;

    //        cb.SetRGBColorFill(100, 100, 100);

    //        cb.BeginText();
    //        cb.SetFontAndSize(bf, 8);
    //        cb.SetTextMatrix(pageSize.GetLeft(document.LeftMargin), pageSize.GetBottom(BottomMargin));
    //        cb.ShowText(text);
    //        cb.EndText();

    //        cb.AddTemplate(template, pageSize.GetLeft(document.LeftMargin) + len, pageSize.GetBottom(BottomMargin));

    //        cb.BeginText();
    //        cb.SetFontAndSize(bf, 8);
    //        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
    //                           "Printed On " + DateTime.Now.ToString(),
    //                           pageSize.GetRight(document.RightMargin),
    //                           pageSize.GetBottom(BottomMargin), 0);
    //        cb.EndText();
    //    }

    //}

}
