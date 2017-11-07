using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace ToracLibrary.HttpClientService
{

    /// <summary>
    /// Lookups for specific content types
    /// </summary>
    public static class ContentTypeLookup
    {

        /// <summary>
        /// Json content type
        /// </summary>
        internal const string JsonContentType = "application/json";

        /// <summary>
        /// Html content type
        /// </summary>
        internal const string HtmlContentType = "text/html";

        /// <summary>
        /// text / plain
        /// </summary>
        internal const string TextPlainContentType = "text/plain";

        /// <summary>
        /// Json media type so we don't need to create a new instance each time.
        /// </summary>
        internal static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new MediaTypeWithQualityHeaderValue(JsonContentType);

        /// <summary>
        /// Html media type so we don't need to create a new instance each time.
        /// </summary>
        internal static readonly MediaTypeWithQualityHeaderValue HtmlMediaType = new MediaTypeWithQualityHeaderValue(HtmlContentType);

        /// <summary>
        /// text media type so we don't need to create a new instance each time.
        /// </summary>
        internal static readonly MediaTypeWithQualityHeaderValue TextMediaType = new MediaTypeWithQualityHeaderValue(TextPlainContentType);

    }

}
