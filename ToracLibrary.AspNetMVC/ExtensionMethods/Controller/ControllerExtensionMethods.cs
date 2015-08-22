using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNetMVC.CustomActionsResults;
using ToracLibrary.AspNetMVC.RazorViewToString;

namespace ToracLibrary.AspNetMVC.ExtensionMethods.ControllerExtensions
{

    /// <summary>
    /// Controller Extension Methods
    /// </summary>
    public static class ControllerExtensionMethods
    {

        #region To Json Net Result

        /// <summary>
        /// Pushes Json To Json.Net From An Asp.net Controller
        /// </summary>
        /// <param name="ControllerContext">Controller To Return The Json</param>
        /// <param name="DataToSerialize">Json Data To Return</param>
        /// <returns>JsonNetResult</returns>
        public static JsonNetResult ToJsonNet(this Controller ControllerContext, object DataToSerialize)
        {
            return new JsonNetResult(DataToSerialize);
        }

        #endregion

        #region View Or Partial View To String

        #region Public Static Methods

        #region Basic View With No Model Or View Data

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">View to render</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, null, null, null);
        }

        /// <summary>
        /// Renders a view to string with a master page
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">View to render</param>
        /// <param name="LayoutPage">Master page render the view with</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender, string LayoutPage)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, null, null, LayoutPage);
        }

        #endregion

        #region View With Model

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">Partial view to render</param>
        /// <param name="Model">Model to be passed into the view</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender, object Model)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, Model, null, null);
        }

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">Partial view to render</param>
        /// <param name="Model">Model to be passed into the view</param>
        /// <param name="LayoutPage">Layout page render the view with</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender, object Model, string LayoutPage)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, Model, null, LayoutPage);
        }

        #endregion

        #region View With Model And DataDictionary

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">View to render</param>
        /// <param name="Model">Model to be passed into the view</param>
        /// <param name="ViewBagData">View Bag Data that is accessible in the view</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender, object Model, ViewDataDictionary ViewBagData)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, Model, ViewBagData, null);
        }

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewTypeToLoad">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">View to render</param>
        /// <param name="Model">Model to be passed into the view</param>
        /// <param name="ViewBagData">View Bag Data that is accessible in the view</param>
        /// <param name="LayoutPage">Layout page render the view with</param>
        /// <returns>Rendered partial view as string</returns>
        public static string RenderViewToString(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewTypeToLoad, string ViewNameToRender, object Model, ViewDataDictionary ViewBagData, string LayoutPage)
        {
            //use the main helper method
            return RenderViewToStringHelper(ControllerToRenderWith, ViewTypeToLoad, ViewNameToRender, Model, ViewBagData, LayoutPage);
        }

        #endregion

        #endregion

        #region Private Main Helper

        /// <summary>
        /// Main Helper Method Which All The Overloads Funnel Down
        /// </summary>
        /// <param name="ControllerToRenderWith">Controller to render the view with</param>
        /// <param name="ViewType">Which view type to find and then render to a string</param>
        /// <param name="ViewNameToRender">View to render</param>
        /// <param name="Model">Model to be passed into the view</param>
        /// <param name="ViewBagData">View Bag Data that is accessible in the view</param>
        /// <param name="LayoutPage">Layout page render the view with</param>
        /// <returns>Rendered partial view as string</returns>
        private static string RenderViewToStringHelper(this Controller ControllerToRenderWith, ViewTypeToLoad.ViewTypeToRender ViewType, string ViewNameToRender, object Model, ViewDataDictionary ViewBagData, string LayoutPage)
        {
            //holds the view to render
            ViewEngineResult ViewResult;

            //is this a view or partial view
            if (ViewType == ViewTypeToLoad.ViewTypeToRender.PartialView)
            {
                //it's a view go find me the partial view
                ViewResult = ViewEngines.Engines.FindPartialView(ControllerToRenderWith.ControllerContext, ViewNameToRender);
            }
            else
            {
                //its a partial view, go grab the view
                ViewResult = ViewEngines.Engines.FindView(ControllerToRenderWith.ControllerContext, ViewNameToRender, LayoutPage);
            }

            //let's go validate we found the partial view (wan't to do it here rather then going into render razor..otherwise developer has to back track and look up the callstack to find out why it's null)
            if (ViewResult == null)
            {
                throw new ArgumentNullException("ViewResult", "Not Able to Find View: " + ViewNameToRender);
            }

            if (ViewResult.View == null)
            {
                throw new ArgumentNullException("ViewResult.View", "Not Able to Find View: " + ViewNameToRender);
            }

            if (ViewResult.ViewEngine == null)
            {
                throw new ArgumentNullException("ViewResult.ViewEngine", "Not Able to Find View: " + ViewNameToRender);
            }

            //go render the string and return it
            return RazorToString.RenderRazorViewToString(ControllerToRenderWith, ViewResult, Model, ViewBagData);
        }

        #endregion

        #endregion

    }

}
