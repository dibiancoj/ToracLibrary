using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;

namespace ToracLibrary.Core.DiskIO
{

    /// <summary>
    /// Write A File To The User's Local Drive
    /// </summary>
    /// <remarks>Use Environment.NewLine For Line Spacing If You Use A Single String Or A String Builder. Static Class. Only Set To Static Because At The Present Time There Is No Plan To Instantiate This Class</remarks>
    public static class FileWriter
    {

        #region Public Methods

        /// <summary>
        /// Write the contents of a byte array to a file
        /// </summary>
        /// <param name="FilePathToSave">File Path</param>
        /// <param name="FileModeToUse">File Mode</param>
        /// <param name="FileContent">File Content</param>
        public static void WriteFileFromByteArray(string FilePathToSave, FileMode FileModeToUse, byte[] FileContent)
        {
            //create the file stream to write the file
            using (var FileStreamToWrite = new FileStream(FilePathToSave, FileModeToUse))
            {
                //write the contents of the file
                FileStreamToWrite.Write(FileContent, 0, FileContent.Length);

                //close the stream. Dispose will take care of this but it could be a while before garbage collection runs.
                //that would lock the file until its GC'd.
                FileStreamToWrite.Close();
            }
        }

        /// <summary>
        /// Write Text To A File To The Computer
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <param name="TextToOutput">Text To Output (string value)</param>
        /// <param name="FileStatus">File Status - System.IO.FileMode</param>
        /// <param name="WhereToInsert">Where To Insert The Text - System.IO.SeekOrigin</param>
        public static void WriteFile(string FilePath, string TextToOutput, FileMode FileStatus, SeekOrigin WhereToInsert)
        {
            //use the helper method
            WriteFileHelper(FilePath, TextToOutput.ToIEnumerable(), FileStatus, WhereToInsert);
        }

        /// <summary>
        /// Write A List Of Text To A File On The Computer
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <param name="LinesToOutput">Text To Output (IEnumerable string)</param>
        /// <param name="FileStatus">File Status - System.IO.FileMode</param>
        /// <param name="WhereToInsert">Where To Insert The Text - System.IO.SeekOrigin</param>>
        public static void WriteFile(string FilePath, IEnumerable<string> LinesToOutput, FileMode FileStatus, SeekOrigin WhereToInsert)
        {
            //use the helper method
            WriteFileHelper(FilePath, LinesToOutput, FileStatus, WhereToInsert);
        }

        /// <summary>
        /// Write A String Builder To A File On The Computer
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <param name="sb">String Builder To Write. Will Call StringBuilder.ToString();</param>
        /// <param name="FileStatus">File Status - System.IO.FileMode</param>
        /// <param name="WhereToInsert">Where To Insert The Text - System.IO.SeekOrigin</param>
        public static void WriteFile(string FilePath, StringBuilder sb, FileMode FileStatus, SeekOrigin WhereToInsert)
        {
            //use the overload
            WriteFile(FilePath, sb.ToString(), FileStatus, WhereToInsert);
        }

        #region Async Methods

        /// <summary>
        /// Write to a file async
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <param name="TextToOutput">Text To Output (string value)</param>
        /// <param name="FileStatus">File Status - System.IO.FileMode</param>
        /// <returns>awaitable Task</returns>
        public static Task WriteFileAsync(string FilePath, string TextToOutput, FileMode FileStatus)
        {
            //grab the bytes to write
            byte[] EncodedTextToWrite = Encoding.Unicode.GetBytes(TextToOutput);

            //create the file stream
            using (var FileStreamToWrite = new FileStream(FilePath, FileStatus, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                //go write the data...and await it to be completed
                return FileStreamToWrite.WriteAsync(EncodedTextToWrite, 0, EncodedTextToWrite.Length);
            }
        }

        #endregion

        #endregion

        #region Helper Method

        /// <summary>
        /// Helper Method. Runs The Work For All The Public Methods
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <param name="LinesToOutput">Text To Output (IEnumerable string)</param>
        /// <param name="FileStatus">File Status - System.IO.FileMode</param>
        /// <param name="WhereToInsert">Where To Insert The Text - System.IO.SeekOrigin</param>
        private static void WriteFileHelper(string FilePath, IEnumerable<string> LinesToOutput, FileMode FileStatus, SeekOrigin WhereToInsert)
        {
            //validate
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentNullException("File Path Can't Be Null.");
            }
            //end of validation

            //Declare the stream write and set the parameters with the paramters you pass in
            using (var WriterToStream = new StreamWriter(File.Open(FilePath, FileStatus)))
            {
                //Run to where we want to start to insert
                WriterToStream.BaseStream.Seek(0, WhereToInsert);

                //Loop through each of the lines and output
                foreach (string LineToWrite in LinesToOutput)
                {
                    //write the line we are up to
                    WriterToStream.WriteLine(LineToWrite);
                }

                //flush the buffer
                WriterToStream.Flush();
            }
        }

        #endregion

    }

}