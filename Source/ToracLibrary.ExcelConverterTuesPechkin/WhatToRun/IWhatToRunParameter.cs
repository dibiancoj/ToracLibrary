using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;

namespace ToracLibrary.ExcelConverterTuesPechkin.WhatToRun
{

    /// <summary>
    /// Interface so we can pass in either html or a url and run it
    /// </summary>
    internal interface IWhatToRunParameter
    {

        /// <summary>
        /// Abstract method which gets passed into the converter
        /// </summary>
        /// <returns>ObjectSettings to use in the conversion</returns>
        ObjectSettings ToObjectSettings();

    }

}
