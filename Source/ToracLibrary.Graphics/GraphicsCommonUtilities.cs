using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static byte[] ImageToByteArray(Bitmap FileToLoad, System.Drawing.Imaging.ImageFormat ImageFormatToSave)
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

    }
}
