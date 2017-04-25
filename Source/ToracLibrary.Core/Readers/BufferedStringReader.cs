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
            //create a new buffer
            Buffer = new Queue<int>();

            //create a new string reader to iterate over
            Reader = new StringReader(StringToRead);
        }

        #endregion

        #region Properties

        /// <summary>
        /// String reader to iterate over
        /// </summary>
        private readonly StringReader Reader;

        /// <summary>
        /// Buffer used so we can peek with and read back.
        /// </summary>
        private readonly Queue<int> Buffer;

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
        /// <returns>The character at the specified index</returns>
        public int Peek(int CharacterIndex)
        {
            ///loop through the numbers of characters you want to read
            for (int i = 0; i <= CharacterIndex; i++)
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
                    Buffer.Enqueue(ReadValue);
                }
            }

            //go grab the specified item from the buffer now
            return Buffer.ElementAt(CharacterIndex);
        }

        /// <summary>
        /// Read the next character and consume it
        /// </summary>
        /// <returns>the specified character with consume it</returns>
        public int Read()
        {
            //if we have nothing in the buffer
            if (Buffer.Count == 0)
            {
                //nothing in the buffer...read it from the reader and return it
                return Reader.Read();
            }

            //we have stuff in the buffer...go grab it and return it
            return Buffer.Dequeue();
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
