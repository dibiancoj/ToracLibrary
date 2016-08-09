using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.Core.DiskIO
{

    /// <summary>
    /// Read A File From The User's Local Drive
    /// </summary>
    public static class FileReader
    {

        #region Read File

        /// <summary>
        /// Read A File And Return It To A String
        /// </summary>
        /// <param name="FilePath">File Path To Open</param>
        /// <returns>Returns A String</returns>
        [MethodIsNotTestable("No Test Added")]
        public static string ReadFile(string FilePath)
        {
            //validate
            if (FilePath.IsNullOrEmpty())
            {
                throw new ArgumentNullException("File Path Can't Be Null.");
            }

            //make sure the file is there
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Can't Find File At: " + FilePath);
            }
            //end of validation

            //Open the file and let the stream reader consume it.
            using (var Reader = new StreamReader(File.Open(FilePath, FileMode.Open)))
            {
                //return the entire file to the end.
                return Reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Read A File And Return It To A List Of Strings For Each Line
        /// </summary>
        /// <param name="FilePath">File Path To Open</param>
        /// <returns>IEnumerable - String. List Of Row Data. Data is Lazy Loaded Using Yield Return. Call ToArray() To Push Data Right Away</returns>
        [MethodIsNotTestable("No Test Added")]
        public static IEnumerable<string> ReadFileToIEnumerableLazy(string FilePath)
        {
            //validate
            if (FilePath.IsNullOrEmpty())
            {
                throw new ArgumentNullException("File Path Can't Be Null.");
            }

            //make sure the file is there
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("Can't Find File At: " + FilePath);
            }
            //end of validation

            //Open the file and let the stream reader consume it.
            using (var Reader = new StreamReader(File.Open(FilePath, FileMode.Open)))
            {
                //use the overload for the main logic using a stream reader
                foreach (string LineToRead in ReadFileToIEnumerableLazy(Reader))
                {
                    //go return the line right now using yield return
                    yield return LineToRead;
                }
            }
        }

        /// <summary>
        /// Read A Stream Reader And Return It To A List Of Strings For Each Line
        /// </summary>
        /// <param name="Reader">Stream Reader To Use</param>
        /// <returns>IEnumerable - String. List Of Row Data. Data is Lazy Loaded Using Yield Return. Call ToArray() To Push Data Right Away</returns>
        [MethodIsNotTestable("No Test Added")]
        public static IEnumerable<string> ReadFileToIEnumerableLazy(StreamReader Reader)
        {
            //Loop until we are at the end. (sr.Peek()) 
            //Reads the next character from the input stream and advances the character position by one character.
            // The next character from the input stream represented as an System.Int32 or -1 if no more characters are available.
            using (Reader)
            {
                //make sure we are not at the end of the file
                while (Reader.Peek() > -1)
                {
                    //go read the line and return it using yield return
                    yield return Reader.ReadLine();
                }
            }
        }

        #region Async Methods

        /// <summary>
        /// Read A File And Return It To A List Of Strings For Each Line
        /// </summary>
        /// <param name="FilePath">File Path To Open</param>
        /// <returns>IEnumerable - String. List Of Row Data. Data is Lazy Loaded Using Yield Return. Call ToArray() To Push Data Right Away</returns>
        [MethodIsNotTestable("No Test Added")]
        public async static Task<IEnumerable<string>> ReadFileToIEnumerableAsync(string FilePath)
        {
            //go grab the file text
            string FileText = await ReadFileAsync(FilePath);

            //now go split the file by line break
            return FileText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// Read a file async
        /// </summary>
        /// <param name="FilePath">File Path To Save The File</param>
        /// <returns>Awaitable Task Of String IEnumerable - String. List Of Row Data. Data is Lazy Loaded Using Yield Return. Call ToArray() To Push Data Right Away</returns>
        [MethodIsNotTestable("No Test Added")]
        public async static Task<string> ReadFileAsync(string FilePath)
        {
            //create the file stream so we can grab the data
            using (var FileStreamToWriteInto = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                //create the string builder which we will build up the return text and return it
                var ReturnString = new StringBuilder();

                //byte buffer to write into
                byte[] BufferToWriteTo = new byte[0x1000];

                //holds the number of reads
                int NumberOfReads;

                //keep reading until we run out of lines
                while ((NumberOfReads = await FileStreamToWriteInto.ReadAsync(BufferToWriteTo, 0, BufferToWriteTo.Length)) != 0)
                {
                    //add the text to the string builder
                    ReturnString.Append(Encoding.Unicode.GetString(BufferToWriteTo, 0, NumberOfReads));
                }

                //we have everything we need, return the string
                return ReturnString.ToString();
            }
        }

        #endregion

        #endregion


    }

}
