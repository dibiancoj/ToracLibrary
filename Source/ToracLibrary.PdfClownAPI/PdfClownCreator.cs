using org.pdfclown.documents;
using org.pdfclown.documents.contents.composition;
using org.pdfclown.documents.interaction.annotations;
using org.pdfclown.documents.interaction.forms;
using org.pdfclown.files;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.PdfClownAPI
{

    /// <summary>
    /// Class wrapper for pdf clown. Pdf clown is completely free unlike Itextsharp
    /// </summary>
    public class PdfClownCreator : IDisposable
    {

        /* Example
           //Instantiates the page inside the document context.
            var page = new org.pdfclown.documents.Page(Doc);

            //Puts the page in the pages collection.
            Doc.Pages.Add(page);

            //Create a content composer for the page
            var composer = new PrimitiveComposer(page);

            //Set the font to use (we will start with the huge "fax" label.
            composer.SetFont(new StandardType1Font(Doc, StandardType1Font.FamilyEnum.Helvetica, false, false), 48);

            //add the image of msk logo
            composer.ShowXObject(AddImage(logoPath), new PointF(100, 30));

            //write some text
            composer.SetFillColor(DeviceRGBColor.Get(System.Drawing.Color.LightGray));
            composer.ShowText("FAX", new PointF(110, 200));

            //draw a line (instead of underline text)
            composer.DrawLine(new PointF(112, 365), new PointF(250 + 300, 365));
            composer.Fill();

            //you can add javascript / actions to the page
            page.Actions.OnOpen = new JavaScript(Doc, "app.alert('test123');");

            // Flush the contents into the page!
            composer.Flush();
        */

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PdfClownCreator()
        {
            //create a new document
            PdfFile = new org.pdfclown.files.File();

            //create the document now
            Doc = PdfFile.Document;
        }

        #endregion

        #region Properties

        /// <summary>
        /// File To Build
        /// </summary>
        public org.pdfclown.files.File PdfFile { get; set; }

        /// <summary>
        /// Holds the document for the pdf
        /// </summary>
        public org.pdfclown.documents.Document Doc { get; set; }

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
        /// Adds a page to the current document
        /// </summary>
        /// <returns>Page</returns>
        public org.pdfclown.documents.Page AddPage()
        {
            //create a new page
            var AddedPage = new org.pdfclown.documents.Page(Doc);

            //add it to the doc
            Doc.Pages.Add(AddedPage);

            //return it now
            return AddedPage;
        }

        /// <summary>
        /// Creates a new composer
        /// </summary>
        /// <param name="Page">Page to add the composer into</param>
        /// <returns>The new PrimitiveComposer</returns>
        public PrimitiveComposer CreateAComposer(org.pdfclown.documents.Page Page)
        {
            //Create a content composer for the page
            return new PrimitiveComposer(Page);
        }

        /// <summary>
        /// Flush a composer so it will write all the data to the page after we build up the composer
        /// </summary>
        /// <param name="ComposerToFlush">comoser to flush</param>
        public void FlushComposer(PrimitiveComposer ComposerToFlush)
        {
            ComposerToFlush.Flush();
        }

        /// <summary>
        /// Add an image to the page. This will return an XObject which you can add using composer.ShowXObject
        /// </summary>
        /// <param name="logoPath">path to the image</param>
        /// <returns>Image in an itextsharp object</returns>
        public org.pdfclown.documents.contents.xObjects.XObject AddImage(string logoPath)
        {
            //go grab the image
            using (var ImageToAdd = System.Drawing.Image.FromFile(logoPath))
            {
                //we need to encode the image which is specific to clown pdf
                using (var EncodeParameters = new EncoderParameters(3))
                {
                    //add the specified parameters
                    EncodeParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                    EncodeParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ScanMethod, (int)EncoderValue.ScanMethodInterlaced);
                    EncodeParameters.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)EncoderValue.RenderNonProgressive);

                    //create the memory stream
                    using (var MemoryStreamToUse = new System.IO.MemoryStream())
                    {
                        //if it's a jpeg we need to set the encoder info
                        var JpegEncoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(x => x.MimeType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase));

                        //go save the image to the memory stream
                        ImageToAdd.Save(MemoryStreamToUse, JpegEncoder, EncodeParameters);

                        //reset the memory stream
                        MemoryStreamToUse.Position = 0;

                        //grab the image from the stream and return the xobject
                        return org.pdfclown.documents.contents.entities.Image.Get(MemoryStreamToUse).ToXObject(Doc);
                    }
                }
            }
        }

        /// <summary>
        /// Write a text box to a form field
        /// </summary>
        /// <param name="TextFieldName">textbox form field name</param>
        /// <param name="TextValue">text value to set in the textbox</param>
        /// <param name="PageToUse">page to put the textbox on</param>
        /// <param name="XCoordinate">x coordinate</param>
        /// <param name="YCoordinate">y coordinate</param>
        /// <param name="WidthOfTextBox">width of the text box</param>
        /// <param name="HeightOfTextBox">height of the textbox</param>
        /// <returns>TextField. Call  Doc.Form.Fields.Add(WriteFormField());</returns>
        public TextField WriteFormField(string TextFieldName, string TextValue, Page PageToUse, float XCoordinate, float YCoordinate, float WidthOfTextBox, float HeightOfTextBox)
        {
            //add the field (when you get it back from this method call Doc.Form.Fields.Add(WriteFormToField())...
            return new TextField(TextFieldName, new Widget(PageToUse, new RectangleF(XCoordinate, YCoordinate, WidthOfTextBox, HeightOfTextBox)), TextValue);

            //to make bold
            //var fieldStyle = new DefaultStyle();
            //fieldStyle.FontSize = 12;
            //fieldStyle.Apply({{The Result of this method}});

            //remove spell check
            //TextBoxFieldToAdd.SpellChecked = false; // Avoids text spell check.
            
            //add field actions
            //FieldActions fieldActions = new FieldActions(Doc);
            //field.Actions = fieldActions;
            //fieldActions.OnValidate = new JavaScript(document,
            //  "app.alert(\"Text '\" + this.getField(\"myText\").value + \"' has changed!\",3,0,\"Validation event\");"
            //  );

            //add the field to the form
            //Doc.Form.Fields.Add(TextBoxFieldToAdd); // 4.2. Field insertion into the fields collection           
        }

        #region Saving Pdf

        /// <summary>
        /// After you are done building the Pdf. Method will return the Pdf to a byte array
        /// </summary>
        /// <returns>Byte Array</returns>
        public byte[] ToPdfStream()
        {
            //create the new memory stream
            using (var MemoryStreamToUse = new System.IO.MemoryStream())
            {
                //go save the pdf
                PdfFile.Save(new org.pdfclown.bytes.Stream(MemoryStreamToUse), SerializationModeEnum.Standard);

                //return the saved bytes
                return MemoryStreamToUse.ToArray();
            }
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
                    //pdf writer
                    PdfFile.Dispose();
                }
            }
            disposed = true;
        }

        #endregion

        #endregion

    }

}
