using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.ITextSharpPdfAPI
{

    /// <summary>
    /// Modify a editable pdf from a template and save it.
    /// </summary>
    public class PdfFormEditor : IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FilePathOfTemplate">Template file location</param>
        public PdfFormEditor(string FilePathOfTemplate)
        {
            //Set the memory stream
            Ms = new MemoryStream();

            //set the pdf reader
            Reader = new PdfReader(FilePathOfTemplate);

            //set the stamper
            Stamper = new PdfStamper(Reader, Ms);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pdf reader that we load the form with
        /// </summary>
        protected PdfReader Reader { get; }

        /// <summary>
        /// Pdf Stamper to retrieve the form data
        /// </summary>
        public PdfStamper Stamper { get; }

        /// <summary>
        /// Memory Stream To Write Too
        /// </summary>
        protected MemoryStream Ms { get; }

        #region Dispose Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool disposed { get; set; }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the acro fields listing
        /// </summary>
        /// <returns>AcroFields</returns>
        public AcroFields GetFieldsInPdf()
        {
            //just return the fields
            return Stamper.AcroFields;
        }

        /// <summary>
        /// Set a value to a field
        /// </summary>
        /// <typeparam name="T">Type of the field value to set</typeparam>
        /// <param name="FieldName">Field name to set</param>
        /// <param name="FieldValueToSet">field value to set</param>
        public void SetFieldValue<T>(string FieldName, T FieldValueToSet)
        {
            //to set a radio set it to "On" or "Off".
            //checkbox would be whatever the value in the pdf is
            //i'm passing in a true here because the checkbox / radio style gets altered by itextsharp.

            //make sure we have a value
            if (FieldValueToSet != null)
            {
                //go set the fields
                GetFieldsInPdf().SetField(FieldName, FieldValueToSet.ToString(), true);
            }
        }

        /// <summary>
        /// Set the font size of a text field. When I did this the value was smaller then normal. So I just set the font size
        /// </summary>
        /// <param name="FieldName">Field name to set</param>
        /// <param name="FontSize">Font size to set</param>
        public void SetFieldFont(string FieldName, float FontSize)
        {
            GetFieldsInPdf().SetFieldProperty(FieldName, "textsize", FontSize, null);
        }

        /// <summary>
        /// save the file to a byte array
        /// </summary>
        /// <returns>File Bytes</returns>
        public byte[] SaveToByteArray()
        {
            //close the stamper
            Stamper.Close();

            //close the reader
            Reader.Close();

            //return the memory stream to a byte array
            return Ms.ToArray();
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    //dispose of the document
                    Reader.Dispose();

                    //memory stream
                    Ms.Dispose();

                    //pdf writer
                    Stamper.Dispose();
                }
            }
            disposed = true;
        }

        #endregion

    }

}
