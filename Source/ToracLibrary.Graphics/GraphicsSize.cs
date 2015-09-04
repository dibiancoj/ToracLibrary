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
    /// Measures the size of text when you don't have access to a user control
    /// </summary>
    public static class GraphicsSize
    {

        /// <summary>
        /// Returns the size of a string.Useful for height or width
        /// </summary>
        /// <param name="TextToMeasure">Text To Measure</param>
        /// <param name="FontToMeasureWith">Font To Measure Width</param>
        /// <returns>SizeF.Width Or SizeF.Height (In Pixels)</returns>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static SizeF GetSizeOfString(string TextToMeasure, Font FontToMeasureWith)
        {
            //create a graphics instance
            using (var GraphicsHandle = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                //go measure it and return it
                return GraphicsHandle.MeasureString(TextToMeasure, FontToMeasureWith);
            }
        }

        /// <summary>
        /// Returns the size of a string.Useful for height or width. Uses a default font
        /// </summary>
        /// <param name="TextToMeasure">Text To Measure</param>
        /// <returns>SizeF.Width Or SizeF.Height</returns>
        [MethodIsNotTestable("I guess you could add an image and test it. This api isn't used much. Will just port it and not add a unit test")]
        public static SizeF GetSizeOfString(string TextToMeasure)
        {
            //go use the method
            return GetSizeOfString(TextToMeasure, SystemFonts.DefaultFont);
        }

    }

}
