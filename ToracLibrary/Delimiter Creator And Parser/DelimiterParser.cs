using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DiskIO;

namespace ToracLibrary.Delimiter
{

    /*
     <example>
       ParsedDataRows = ToracTechnologies.Library.Parser.DelimiterParser.Parse("file.txt", ',');
             foreach (IEnumerable<ParsedRowResult> thisRow in ParsedDataRows)
             {
                 foreach (string thisCol in thisRow.ColumnData)
                 {
                     Console.WriteLine(thisCol);
                 }
             }
     </example>
     */

    /// <summary>
    /// Parse A File With Delimiter's.
    /// </summary>
    /// <remarks>Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class DelimiterParser
    {

        #region Public Static Methods

        #region Parse From File

        /// <summary>
        /// Parse The File
        /// </summary>
        /// <param name="FileToParse">File Path To Parse</param>
        /// <param name="Delimiter">Delimiter That Each Column Is Seperated By</param>
        /// <returns>IEnumerable ParseRowResult. Holds each of the rows. Inside that object holds the columns for that row</returns>
        /// <remarks>Method is lazy loaded. File will be locked until method is complete. Call ToArray() To Push To List</remarks>
        public static IEnumerable<DelimiterRow> ParseFromFileLazy(string FileToParse, string Delimiter)
        {
            //use the overload (using to an array so we don't have 3 iterators in the chain)
            return ParseFromTextLinesLazy(FileReader.ReadFileToIEnumerableLazy(FileToParse).ToArray(), Delimiter);
        }

        #endregion

        #region Parse From Text

        /// <summary>
        /// Parse The File From A Text String
        /// </summary>
        /// <param name="TextToParse">Text To Parse. This Will Contain The Entire Document</param>
        /// <param name="Delimiter">Delimiter That Each Column Is Seperated By</param>
        /// <returns>IEnumerable ParseRowResult. Holds each of the rows. Inside that object holds the columns for that row</returns>
        /// <remarks>Method is lazy loaded. File will be locked until method is complete. Call ToArray() To Push To List</remarks>
        public static IEnumerable<DelimiterRow> ParseFromTextLazy(string TextToParse, string Delimiter)
        {
            //use the overload
            return ParseFromTextLinesLazy(TextToParse.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries), Delimiter);
        }

        #endregion

        #region Parse From Text Lines

        /// <summary>
        /// Parse The File From A Text String
        /// </summary>
        /// <param name="TextLinesToParse">Text To Parse Broken Out By Each Line</param>
        /// <param name="Delimiter">Delimiter That Each Column Is Seperated By</param>
        /// <returns>IEnumerable ParseRowResult. Holds each of the rows. Inside that object holds the columns for that row</returns>
        /// <remarks>Method is lazy loaded. File will be locked until method is complete. Call ToArray() To Push To List</remarks>
        public static IEnumerable<DelimiterRow> ParseFromTextLinesLazy(IEnumerable<string> TextLinesToParse, string Delimiter)
        {
            //throw the delimiter into an array so we don't have to keep creating it
            string[] DelimiterToSplitWith = new string[] { Delimiter };

            //let's loop through the rows in the file
            foreach (string RawRowData in TextLinesToParse)
            {
                //now let's split the raw row data into the specific columns
                yield return new DelimiterRow(RawRowData.Split(DelimiterToSplitWith, StringSplitOptions.None));
            }
        }

        #endregion

        #endregion

    }

}
