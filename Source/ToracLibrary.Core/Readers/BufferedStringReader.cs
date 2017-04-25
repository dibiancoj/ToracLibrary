using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Readers
{

    /// <summary>
    /// Buffered string reader so you can peek multiple lines and still have them read back
    /// </summary>
    public class BufferedStringReader : IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringToRead">String to iterate over</param>
        public BufferedStringReader(string StringToRead)
        {
            //make sure we have a value (We only throw if null. Blank strings are fine)
            if (StringToRead == null)
            {
                throw new ArgumentNullException(nameof(StringToRead));
            }

            //create a new buffer
            Buffer = new List<int>();

            //create a new string reader to iterate over
            Reader = new StringReader(StringToRead);
        }

        #endregion

        #region Constants

        /// <summary>
        /// No more characters value.
        /// </summary>
        public const int NoMoreCharacters = -1;

        #endregion

        #region Properties

        /// <summary>
        /// String reader to iterate over
        /// </summary>
        private readonly StringReader Reader;

        /// <summary>
        /// Buffer used so we can peek with and read back. Using a list instead of a queue collection so we don't have to call ElementAt In Peek. Benchmark Dot Net said the list is much faster based on the code that is implemented.
        /// A queue doesn't have an indexer so I can't grab a value at a specific index without looping through the entire collection
        /// </summary>
        private readonly List<int> Buffer;

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Peek at a character and don't consume it
        /// </summary>
        /// <param name="CharacterIndex">What index stream do you want to read. 0 would be the next character in line</param>
        /// <returns>The character at the specified index. -1 returned if we are at EndOfFile</returns>
        public int Peek(int CharacterIndex)
        {
            ///loop through the numbers of characters you want to read. Start at the buffer last index since we don't need to add those to the queue
            for (int i = Buffer.Count; i <= CharacterIndex; i++)
            {
                //do we have this in our buffer?
                if (Buffer.Count == 0 || i >= Buffer.Count)
                {
                    //grab the result
                    int ReadValue = Reader.Read();

                    //if its at the end...then return
                    if (ReadValue == -1)
                    {
                        //at the end. Go return that value
                        return ReadValue;
                    }

                    //we don't have it in the buffer. Go read it and put it in the buffer
                    Buffer.Add(ReadValue);
                }
            }

            //go grab the specified item from the buffer now
            return Buffer[CharacterIndex];
        }

        /// <summary>
        /// Read the next character and consume it
        /// </summary>
        /// <returns>the specified character while consuming it. -1 returned if we are at EndOfFile</returns>
        public int Read()
        {
            //if we have nothing in the buffer
            if (Buffer.Count == 0)
            {
                //nothing in the buffer...read it from the reader and return it
                return Reader.Read();
            }

            //we have stuff in the buffer...go grab it and return it. Always grab the element at 0 (basically a queue collection)
            int ItemToReturn = Buffer[0];

            //remove the item from the buffer now
            Buffer.RemoveAt(0);

            //now return the item we found
            return ItemToReturn;
        }

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
            if (!this.Disposed)
            {
                if (disposing)
                {
                    Reader.Dispose();
                }
            }
            Disposed = true;
        }

        #endregion

    }

}
