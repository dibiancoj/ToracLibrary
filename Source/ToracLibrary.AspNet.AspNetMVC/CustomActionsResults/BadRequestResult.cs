using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNet.AspNetMVC.CustomActionsResults
{

    /// <summary>
    /// Used to validation errors with ajax calls. Will return a 400 error. Works like web api.
    /// </summary>
    public class BadRequestResult : JsonResult
    {
        //client side
        //--to get data from above call use jqxhr.responseText
        //$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        //            if (jqxhr.status == 400)
        //            {
        //                  $('#errorFound').text(JSON.parse(jqxhr.responseJSON)); //or use jqxhr.responseText
        //              $('#errorBanner').show();
        //            }
        //        });

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ModelState">Model state</param>
        public BadRequestResult(ModelStateDictionary ModelState)
        {
            this.Data = ModelState.Values.SelectMany(x => x.Errors);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute the results
        /// </summary>
        /// <param name="Context">Context</param>
        public override void ExecuteResult(ControllerContext Context)
        {
            //set the response status code to 400
            Context.RequestContext.HttpContext.Response.StatusCode = 400;

            //go run the base
            base.ExecuteResult(Context);
        }

        #endregion

    }

}
