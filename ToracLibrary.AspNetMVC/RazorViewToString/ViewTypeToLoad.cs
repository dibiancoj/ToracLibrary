using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNetMVC.RazorViewToString
{

    /// <summary>
    /// Holds Enum For The View Type To Load To A String
    /// </summary>
    public static class ViewTypeToLoad
    {

        /// <summary>
        /// What is the view type to render to a string
        /// </summary>
        public enum ViewTypeToRender
        {

            /// <summary>
            /// View To Render
            /// </summary>
            View = 0,

            /// <summary>
            /// Partial View To Render
            /// </summary>
            PartialView = 1

        }

    }

}
