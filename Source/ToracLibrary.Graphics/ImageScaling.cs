using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.Graphics
{

    /// <summary>
    /// Scale Images
    /// </summary>
    public static class ImageScaling
    {

        /* '*******
        *    'Example Call
        *    'Image myImage;
        *    'Image myThumbnail;
        *
        *    'myImage = Image.FromFile("c:\\logo.jpg");
        *
        *    'myThumbnail = ScaleImage(myImage, 100, 50);
        *
        *    PictureBox1.Image = myImage;
        *    PictureBox2.Image = myThumbnail;
        *    '*******
        */

        /// <summary>
        /// Scale An Image To A Smaller Value. Keeps The Prospective And Keeps The Ratio The Same
        /// </summary>
        /// <param name="ImageToScale">Image To Scale - myImage = Image.FromFile("c:\logo.jpg")</param>
        /// <param name="ScaledToHeight">The Height To Scale Too</param>
        /// <param name="ScaleToWidth">The Width To Scall Too</param>
        /// <returns>The Smaller Scaled Image</returns>
        /// <remarks>If you pass in items that will not scale to keep the ratio it will adjust it</remarks>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static Image ScaleImage(Image ImageToScale, double ScaledToHeight, double ScaleToWidth)
        {
            //Hold the min value. Is it the height or the width - Figure out which is the minimum value...the width or the height. Keeps the ratio
            double ScalingValue = Math.Min((ScaleToWidth / ImageToScale.Width), (ScaledToHeight / ImageToScale.Height));

            //Set the final scaled width 
            double ScaledWidth = (ScalingValue * ImageToScale.Width);

            //Set the final scaled height
            double ScaledHeight = (ScalingValue * ImageToScale.Height);

            //set the callback to the private method - ThumbnailCallback
            //just use a lamda since the method doesn't do anything
            Image.GetThumbnailImageAbort CallBackForConversion = new Image.GetThumbnailImageAbort(() =>
            {
                try
                {
                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            });

            //return the image that is going to be scaled
            return ImageToScale.GetThumbnailImage(Convert.ToInt32(ScaledWidth), Convert.ToInt32(ScaledHeight), CallBackForConversion, IntPtr.Zero);
        }
    }

}
