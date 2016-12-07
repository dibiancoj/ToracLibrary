using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;

namespace ToracLibrary.HtmlToPdfConverterTuesPechkin.WhatToRun
{

    /// <summary>
    /// Go render html from a string
    /// </summary>
    public class HtmlStringToPdfParameter : IWhatToRunParameter
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HtmlToConvert">html to convert</param>
        public HtmlStringToPdfParameter(string HtmlToConvert)
        {
            HtmlToConvertToPdf = HtmlToConvert;
        }

        #endregion

        #region Immutable Variables

        /// <summary>
        /// Holds the html to convert to a pdf
        /// </summary>
        private string HtmlToConvertToPdf { get; }

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
                //PageUrl = urlToConvert, //you can also give it a url and it will make a web request
                HtmlText = HtmlToConvertToPdf
            };
        }

        #endregion

    }

}
