using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.ExcelConverterTuesPechkin.WhatToRun;
using TuesPechkin;

namespace ToracLibrary.ExcelConverterTuesPechkin
{

    /// <summary>
    /// Converts html to a pdf. Renders it and saves it. 
    /// </summary>
    public class HtmlToPdfConverter
    {

        /*
        The visual c++ runtime is a requirement besides the nuget packages.
        error if you don't have the run time is Error was  System.DllNotFoundException: Unable to load DLL 'wkhtmltox.dll' error 0x8007007E.

        If you don’t want to install the runtime you can do the following:

        http://wkhtmltopdf.org/downloads.html
        Download the Windows (MinGW-w64)

        Then just throw everything in the bin in with your application.

        Then in your code use: (ie load from your current directory) new StaticDeployment(AppDomain.CurrentDomain.BaseDirectory))));
        */

        /*
        Also, dont create a converter or use multiple converters
        Documentation says to use the api library in a singleton!

        This is for the 64 bit version. if you want the 32, download the 32 bit version and change all the references.
        */

        #region Public Methods

        #region Convert Methods

        /// <summary>
        /// Convert a url to a pdf
        /// </summary>
        /// <param name="ConverterToUse">converter to use. Use TuesConverter.PdfConverter</param>
        /// <param name="ProduceTableOfContentsOutline">Product the table of contents outline</param>
        /// <param name="PaperSize">Paper Size</param>
        /// <param name="WhatToRun">What to run. Either Html or a url</param>
        /// <param name="ModeToRun">What mode is the What To Run In.</param>
        /// <param name="HtmlToConvert">Html to convert</param>
        /// <param name="MarginTop">Margin Top</param>
        /// <param name="MarginRight">Margin Right</param>
        /// <param name="MarginBottom">Margin Bottom</param>
        /// <param name="MarginLeft">Margin Left</param>
        /// <param name="UsePrintMediaCssSelectors">When running do you want to run under the css 3 print media selectors</param>
        /// <returns>pdf file byte array</returns>
        private static byte[] ConvertAUrlToPdf(IConverter ConverterToUse,
                                              bool ProduceTableOfContentsOutline,
                                              System.Drawing.Printing.PaperKind PaperSize,
                                              IWhatToRunParameter WhatToRun,
                                              double? MarginTop,
                                              double? MarginRight,
                                              double? MarginBottom,
                                              double? MarginLeft,
                                              bool UsePrintMediaCssSelectors)
        {
            //let's build up the object settings
            var ObjectSettingsToUse = WhatToRun.ToObjectSettings();

            //add anything else now
            ObjectSettingsToUse.WebSettings = new WebSettings { PrintMediaType = UsePrintMediaCssSelectors };

            //go build the main settings to use
            var DocumentSettings = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ProduceOutline = ProduceTableOfContentsOutline,
                    PaperSize = PaperSize,
                    Margins =
                    {
                        Top = MarginTop,
                        Right = MarginRight,
                        Bottom = MarginBottom,
                        Left = MarginLeft,
                        Unit = Unit.Inches
                    }
                },
                Objects =
                {
                    ObjectSettingsToUse
                }
            };

            //go convert and return it
            return ConverterToUse.Convert(DocumentSettings);
        }

        #endregion

        #region IConverter Creator

        /// <summary>
        /// Build the converter and return. IIS vs single threaded app. ***MAKE SURE YOU DON'T CALL THIS MULTIPLE TIMES IN AN APP LIFETIME...MAKE THE RETURN OF THIS A SINGLETON***
        /// </summary>
        /// <param name="IsIISApplication">iis application ie. multi-threaded. vs console single threaded</param>
        /// <returns>converter to use. Put this in a singleton. Otherwise you will get issues</returns>
        public static IConverter BuildConverter(bool IsIISApplication)
        {
            //*** Don't call this multiple times. Documentation says to use the api library in a singleton! Calling app needs to deal with storing it***

            if (IsIISApplication)
            {
                //build the path
                string PathToUse = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "wkhtml");

                //return the threadsafe converter
                return new ThreadSafeConverter(new RemotingToolset<PdfToolset>(new Win64EmbeddedDeployment(new StaticDeployment(PathToUse))));
            }

            //console app version
            return new StandardConverter(new PdfToolset(new Win64EmbeddedDeployment(new TempFolderDeployment())));
        }

        #endregion

        #endregion

    }

}
