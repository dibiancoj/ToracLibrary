using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.Graphics
{

    /// <summary>
    /// Result of the image from json result in Graphics utilities. 
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class ImageFromJsonResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MimeTypeToSet">Mime type to set</param>
        /// <param name="FileFormatToSet">File format to set</param>
        /// <param name="FileBytesToSet">File bytes to set</param>
        public ImageFromJsonResult(string MimeTypeToSet, string FileFormatToSet, byte[] FileBytesToSet)
        {
            MimeType = MimeTypeToSet;
            FileFormat = FileFormatToSet;
            FileBytes = FileBytesToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mime type of file
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// File format to use. Should be 64 encoded
        /// </summary>
        public string FileFormat { get; }

        /// <summary>
        /// The actual file in bytes
        /// </summary>
        public byte[] FileBytes { get; set; }

        #endregion

    }

}
