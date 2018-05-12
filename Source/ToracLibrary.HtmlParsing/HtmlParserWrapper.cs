using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.HtmlParsing
{

    /// <summary>
    /// Wrapper around the Html agility pack that provides common methods which makes using the agility pack easier
    /// </summary>
    /// <remarks>Html is case insensitive and so is this tool. You don't need to run case insentive searches on items</remarks>
    public class HtmlParserWrapper
    {

        //i got an error when i added this to the project. the calling project might need to add this
        //  <dependentAssembly>
        //  <assemblyIdentity name = "HtmlAgilityPack" publicKeyToken="bd319b19eaf3b43a" culture="neutral" />
        //  <bindingRedirect oldVersion = "0.0.0.0-1.4.9.0" newVersion="1.4.9.0" />
        //</dependentAssembly>

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HtmlToParse">Html to parsef</param>
        public HtmlParserWrapper(string HtmlToParse)
        {
            //create the new html document
            HtmlDoc = new HtmlDocument();

            //now go load the html
            HtmlDoc.LoadHtml(HtmlToParse);
        }

        /// <summary>
        /// Constructor overload when you want to bring your own html document
        /// </summary>
        /// <param name="HtmlDocumentToParse">Html to parsef</param>
        public HtmlParserWrapper(HtmlDocument HtmlDocumentToParse)
        {
            //bring your own html document
            HtmlDoc = HtmlDocumentToParse;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the html document that is used to parse items
        /// </summary>
        public HtmlDocument HtmlDoc { get; }

        #endregion

        #region Methods

        //This is how you find all the children or descendants
        //HtmlDocument.DocumentNode.Descendants().Where(x => x.Name == HtmlTagToLookForAndAlter))

        //element.InnerText hows the inner text value

        //to set the inner text
        //element.SetTextElement("<span>jason</span>")

        /// <summary>
        /// Get the full html document from the html doc.
        /// </summary>
        /// <returns>Html string output</returns>
        public string ToHtml()
        {
            return HtmlDoc.DocumentNode.OuterHtml;
        }

        #endregion

    }

}
