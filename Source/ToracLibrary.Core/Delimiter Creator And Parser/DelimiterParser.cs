using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DiskIO;

namespace ToracLibrary.Core.Delimiter
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
            return ParseFromTextLinesLazy(FileReader.ReadFile(FileToParse), Delimiter);
        }

        #endregion

        #region Parse From Text Lines

        /// <summary>
        /// Parse The File From A Text String
        /// </summary>
        /// <param name="ContentToParse">All the lines of content that we are want to parse</param>
        /// <param name="Delimiter">Delimiter That Each Column Is Seperated By</param>
        /// <returns>IEnumerable ParseRowResult. Holds each of the rows. Inside that object holds the columns for that row</returns>
        /// <remarks>Method is lazy loaded. File will be locked until method is complete. Call ToArray() To Push To List</remarks>
        public static IEnumerable<DelimiterRow> ParseFromTextLinesLazy(string ContentToParse, string Delimiter)
        {
            //throw the delimiter into an array so we don't have to keep creating it
            string[] DelimiterToSplitWith = new string[] { Delimiter };

            //i profiled this and its faster and more memory efficient to use a string reader for each row. To do this for every column had to allocate to much.
            //the current implementation is best for reducing memory
            using (var Reader = new StringReader(ContentToParse))
            {
                //loop until we are done
                while (Reader.Peek() != -1)
                {
                    //grab the line to parse and split the raw data into specific columns
                    yield return new DelimiterRow(Reader.ReadLine().Split(DelimiterToSplitWith, StringSplitOptions.None));
                }
            }
        }

        #endregion

        #endregion

    }

}
