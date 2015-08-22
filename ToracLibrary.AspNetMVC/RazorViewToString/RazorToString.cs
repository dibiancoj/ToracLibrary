using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.AspNetMVC.RazorViewToString
{

    /// <summary>
    /// ASP.Net Mvc Class To Render A ViewEngineResult View To A String. Used If You Have A Partial View Or View And Want To Render The Html. Used As A Helper Class
    /// Coming From PartialViewToString And ViewToString
    /// </summary>
    internal static class RazorToString
    {

        #region Main Helper Method

        /// <summary>
        /// Renders A ViewEngineResult (Either A Partial View Or A View) To A String
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="RazorViewToRender">Partial view or view to render</param>
        /// <param name="Model">Model that gets passed into the view</param>
        /// <param name="ViewBagData">View Bag that is accessible when the view is rendered</param>
        /// <returns>Rendered partial view as string</returns>
        internal static string RenderRazorViewToString(Controller ControllerToRenderWith, ViewEngineResult RazorViewToRender, object Model, ViewDataDictionary ViewBagData)
        {
            //set the model
            ControllerToRenderWith.ViewData.Model = Model;

            //make sure the view bag data is not null{
            if (ViewBagData.AnyWithNullCheck())
            {
                //we need to make sure we include both the controller and the view data we pass in...so add the view data to the controllers view data
                foreach (KeyValuePair<string, object> thisViewBagItem in ViewBagData)
                {
                    //add the view data to the controller's view data
                    ControllerToRenderWith.ViewData.Add(thisViewBagItem);
                }
            }

            //create the string write to output the html
            using (var ViewStringWriter = new StringWriter())
            {
                //create the view context with all the view data..and render that control to the view context
                var ViewContextToUse = new ViewContext(ControllerToRenderWith.ControllerContext, RazorViewToRender.View, ControllerToRenderWith.ViewData, ControllerToRenderWith.TempData, ViewStringWriter);

                //Now go render the partial view to the string writer
                RazorViewToRender.View.Render(ViewContextToUse, ViewStringWriter);

                //return the string which contains the html for this partial view with the model and view data embedded
                return ViewStringWriter.GetStringBuilder().ToString();
            }
        }

        #endregion

    }

}
