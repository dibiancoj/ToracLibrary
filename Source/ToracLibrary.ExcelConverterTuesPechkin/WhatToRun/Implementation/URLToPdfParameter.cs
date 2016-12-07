using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;

namespace ToracLibrary.HtmlToPdfConverterTuesPechkin.WhatToRun
{

    /// <summary>
    /// Go render a url to a pdf
    /// </summary>
    public class URLToPdfParameter : IWhatToRunParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="URLToConvertToSet">url to convert</param>
        public URLToPdfParameter(string URLToConvertToSet)
        {
            UrlToConvertToPdf = URLToConvertToSet;
        }

        #endregion

        #region Immutable Variables

        /// <summary>
        /// Holds the url to convert to a pdf
        /// </summary>
        private string UrlToConvertToPdf { get; }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Builds the object settings and return its
        /// </summary>
        /// <returns>ObjectSettings</returns>
        public ObjectSettings ToObjectSettings()
        {
            return new ObjectSettings
            {
                PageUrl = UrlToConvertToPdf
            };
        }

        #endregion

    }

}
