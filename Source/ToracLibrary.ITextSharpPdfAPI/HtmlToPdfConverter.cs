using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ITextSharpPdfAPI
{

    /// <summary>
    /// Converts html into a a pdf using ITextSharp
    /// </summary>
    /// <remarks>This uses itextsharp.xmlworker package</remarks>
    public static class HtmlToPdfConverter
    {

        #region Conversion - Html To Pdf

        /// <summary>
        /// Take's html and puts it into a pdf document. Use this if you don't need css support
        /// </summary>
        /// <param name="Html">html to render to a pdf</param>
        /// <param name="Css">Css file (null if you dont have any css)</param>
        /// <param name="CreatorToUse">Creator to use. This way the end developer can setup the margins and landscape</param>
        /// <returns>CreatorToUse with everything written to it</returns>
        public static PDFCreator HtmlToPdf(string Html, PDFCreator CreatorToUse)
        {
            //create the html stream
            using (var HtmlStream = new StringReader(Html))
            {
                //Parse the HTML
                XMLWorkerHelper.GetInstance().ParseXHtml(CreatorToUse.Writer, CreatorToUse.Doc, HtmlStream);

                //go return the creator now
                return CreatorToUse;
            }
        }

        /// <summary>
        /// Take's html and puts it into a pdf document. Use this if you need css support
        /// </summary>
        /// <param name="Html">html to render to a pdf</param>
        /// <param name="Css">Css file (null if you dont have any css)</param>
        /// <param name="CreatorToUse">Creator to use. This way the end developer can setup the margins and landscape</param>
        /// <returns>CreatorToUse with everything written to it</returns>
        public static PDFCreator HtmlToPdf(string Html, string Css, PDFCreator CreatorToUse)
        {
            //create the pdf creator
            using (var CssStream = new MemoryStream(Encoding.UTF8.GetBytes(Css)))
            {
                //create the html stream
                using (var HtmlStream = new MemoryStream(Encoding.UTF8.GetBytes(Html)))
                {
                    //Parse the HTML
                    XMLWorkerHelper.GetInstance().ParseXHtml(CreatorToUse.Writer, CreatorToUse.Doc, HtmlStream, CssStream);

                    //go return the creator now
                    return CreatorToUse;
                }
            }
        }

        #endregion
    }

}
