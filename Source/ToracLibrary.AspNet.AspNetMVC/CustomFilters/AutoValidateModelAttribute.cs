using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.CustomActionsResults;

namespace ToracLibrary.AspNet.AspNetMVC.CustomFilters
{

    /// <summary>
    /// Add this attribute to your controller method for ajax calls and it will return a 400 bad request if the modelstate isn't valid
    /// </summary>
    public class AutoValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.Controller.ViewData.ModelState.IsValid)
            {
                context.Result = new BadRequestResult(context.Controller.ViewData.ModelState);
            }
        }
    }
}
