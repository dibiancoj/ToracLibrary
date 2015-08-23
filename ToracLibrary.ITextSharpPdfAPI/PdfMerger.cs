using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ITextSharpPdfAPI
{

    /// <summary>
    /// Mergers multiple pdf's into 1 pdf file
    /// </summary>
    public class PdfMerger
    {

        /// <summary>
        /// Merges multiple pdf files into 1 pdf file.
        /// </summary>
        /// <param name="FilesToMerge">Files to merger (Byte array for each file)</param>
        /// <returns>1 file with all the files passed in</returns>
        public static byte[] MergeFiles(IEnumerable<byte[]> FilesToMerge)
        {
            //Declare the memory stream to use
            using (var MemoryStreamToWritePdfWith = new MemoryStream())
            {
                //declare the new document which we will merge all the files into
                using (var NewFileToMergeIntoWith = new Document())
                {
                    //holds the byte array which we will return
                    byte[] ByteArrayToReturn;

                    //declare the pdf copy to write the data with
                    using (var PdfCopyWriter = new PdfCopy(NewFileToMergeIntoWith, MemoryStreamToWritePdfWith))
                    {
                        //set the page size of the new file
                        //NewFileToMergeIntoWith.SetPageSize(PageSize.GetRectangle("Letter").Rotate().Rotate());

                        //go open the new file that we are writing into
                        NewFileToMergeIntoWith.Open();

                        //now loop through all the files we want to merge
                        foreach (var FileToMerge in FilesToMerge)
                        {
                            //declare the pdf reader so we can copy it
                            using (var PdfFileReader = new PdfReader(FileToMerge))
                            {
                                //figure out how many pages are in this pdf, so we can add each of the pdf's
                                int PdfFilePageCount = PdfFileReader.NumberOfPages;

                                // loop over document pages (start with page 1...not a 0 based index)
                                for (int i = 1; i <= PdfFilePageCount; i++)
                                {
                                    //set the file size for this page
                                    NewFileToMergeIntoWith.SetPageSize(PdfFileReader.GetPageSize(i));

                                    //add a new page for the next page
                                    NewFileToMergeIntoWith.NewPage();

                                    //now import the page
                                    PdfCopyWriter.AddPage(PdfCopyWriter.GetImportedPage(PdfFileReader, i));
                                }
                            }
                        }

                        //now close the new file which we merged everyting into
                        NewFileToMergeIntoWith.Close();

                        //grab the buffer and throw it into a byte array to return
                        ByteArrayToReturn = MemoryStreamToWritePdfWith.GetBuffer();

                        //flush out the memory stream
                        MemoryStreamToWritePdfWith.Flush();
                    }

                    //now return the byte array
                    return ByteArrayToReturn;
                }
            }
        }

    }

}
