using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.AspNet.AspNetMVC.ControllerCreation
{

    /// <summary>
    /// Generic MVC Controller Methods
    /// </summary>
    public static class ControllerHelpers
    {

        #region Create Controller With Context

        /* This is mainly used so we can call a RenderViewToString when we actually aren't in a controller
           So if i only have a context and i'm not in a controller, i can create a dummy controller then render the view to string

            so i only have a context...i can create a new controller with the current httpcontext...then i can call render view to string when i'm not in the mvc realm
            */


        /// <summary>
        /// Create a controller from the current httpcontext when we aren't in the mvc realm
        /// </summary>
        /// <typeparam name="TController">Controller type to create</typeparam>
        /// <param name="CurrentContext">Current Context. Ie: HttpContext.Current</param>
        /// <returns>controller instance that was created</returns>
        [MethodIsNotTestable("Regular Asp.Net uses HttpContext. Mvc Uses contextBase. The regular context is hard to mock. I'm going to leave this as no unit testing")]
        public static TController CreateControllerWithContext<TController>(HttpContext CurrentContext)
           where TController : Controller, new()
        {
            //use the overload
            return CreateControllerWithContext<TController>(CurrentContext, null);
        }

        /// <summary>
        /// Create a controller from the current httpcontext when we aren't in the mvc realm
        /// </summary>
        /// <typeparam name="TController">Controller type to create</typeparam>
        /// <param name="CurrentContext">Current Context. Ie: HttpContext.Current</param>
        /// <param name="RouteData ">route data if any</param>
        /// <returns>controller instance that was created</returns>
        [MethodIsNotTestable("Regular Asp.Net uses HttpContext. Mvc Uses contextBase. The regular context is hard to mock. I'm going to leave this as no unit testing")]
        public static TController CreateControllerWithContext<TController>(HttpContext CurrentContext, RouteData RouteData)
           where TController : Controller, new()
        {
            //create the new controller instance
            var ControllerInstance = new TController();

            // Create a context wrapper so we can create the context
            var ContextWrapper = new HttpContextWrapper(CurrentContext);

            //do we have any route data
            if (RouteData == null)
            {
                //create a new instance of the route data
                RouteData = new RouteData();
            }

            //is the controller in the route data
            if (!RouteData.Values.ContainsKey("controller") && !RouteData.Values.ContainsKey("Controller"))
            {
                //add the controller name if we don't have it
                RouteData.Values.Add("controller", ControllerInstance.GetType().Name.ToLower().Replace("controller", string.Empty));
            }

            //set the controller context
            ControllerInstance.ControllerContext = new ControllerContext(ContextWrapper, RouteData, ControllerInstance);

            //return the instance
            return ControllerInstance;
        }

        #endregion

    }

}
