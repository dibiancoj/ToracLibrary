using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.Graphics
{

    /// <summary>
    /// Common methods for graphics
    /// </summary>
    public class GraphicsCommonUtilities
    {

        /// <summary>
        /// Convert a bit map to a byte array
        /// </summary>
        /// <param name="FileToLoad">File to load</param>
        /// <param name="ImageFormatToSave">Image format to save with</param>
        /// <returns>Byte array of the file contents</returns>
        [MethodIsNotTestable("Haven't build unit tests for graphics yet")]
        public static byte[] ImageToByteArray(Bitmap FileToLoad, ImageFormat ImageFormatToSave)
        {
            //go create the memory stream
            using (var StreamToLoadFileWith = new MemoryStream())
            {
                //go save the image into the stream
                FileToLoad.Save(StreamToLoadFileWith, ImageFormatToSave);

                //return the byte array
                return StreamToLoadFileWith.ToArray();
            }
        }

        /// <summary>
        /// Getting the supported codec info.
        /// </summary>
        /// <param name="mimeType">description of mime type</param>
        /// <returns>image codec info</returns>
        public static ImageCodecInfo GetEncoderInfo(string MimeType)
        {
            //go grab the encoder
            return ImageCodecInfo.GetImageEncoders().First(x => string.Equals(x.MimeType, MimeType, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Compress the image
        /// </summary>
        /// <param name="ImageToCompress">Image To Compress</param>
        /// <param name="CompressionLevel">Compression level</param>
        /// <returns>File that is compressed</returns>
        [MethodIsNotTestable("Haven't build unit tests for graphics yet")]
        public static byte[] CompressImage(Image ImageToCompress, long CompressionLevel)
        {
            //grab the memory stream to save into
            using (var MemoryStreamToSave = new MemoryStream())
            {
                //declare the parameters
                var EncoderParams = new EncoderParameters(1);

                //set the compression level
                EncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, CompressionLevel);

                //grab the codec info
                var EncoderInfo = GetEncoderInfo("image/jpeg");

                //go save the image to the memory stream
                ImageToCompress.Save(MemoryStreamToSave, EncoderInfo, EncoderParams);

                //close the memory stream
                MemoryStreamToSave.Flush();

                //close the memory stream
                MemoryStreamToSave.Close();

                //return the byte array now
                return MemoryStreamToSave.ToArray();
            }
        }

    }

}
