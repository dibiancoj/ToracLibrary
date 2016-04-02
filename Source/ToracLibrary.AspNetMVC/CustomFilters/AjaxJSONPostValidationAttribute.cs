using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNet.AspNetMVC.CustomFilters
{

    /// <summary>
    /// Enables you to return model validation on an ajax post and have the ui update in mvc.
    /// This is written code rather then built into the framework
    /// </summary>
    public class AjaxJSONPostValidationAttribute : ActionFilterAttribute
    {

        /* javascript needed
         *    $(document).ready(function () {
            $('#TestSubmit').click(function () {

                $.ajax({
                    type: "POST",
                    url: '/Home/Test',
                    data: JSON.stringify({ UserName: $('#UserName').val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        RunValidationCheck(null);
                    },
                    error: function (data) {
                        debugger;

                        RunValidationCheck(data.responseJSON);
                    }
                });
            });
        });

        function RunValidationCheck(errObject) {
            debugger;
            if (errObject == null) {
                $('.field-validation-valid').html('');
            }
            else if (errObject.HasValidationErrors) {
                $.each(errObject.ErrorLookup, function (key, value) {
                    if (value != null) {
                        $('[data-valmsg-for=' + value.Key + ']').html(value.Errors[0]);
                    }
                });
            }
        }
         */

        /// <summary>
        /// On the method executing it will run this method. 
        /// </summary>
        /// <param name="filterContext">filter context running</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //make sure you tack on this to your method you want to run this for
            //[AjaxJSONPostValidation]

            //make sure this in your web config. it probably is because at this point you are using mvc validation
            //<add key = "ClientValidationEnabled" value = "true" />
            //<add key = "UnobtrusiveJavaScriptEnabled" value = "true" />

            //the return model for error in mvc is 
            //[{
            //    "Key":"UserName",
            //    "Errors":["The UserName field is required."]
            // },
            // {
            //    "Key":"Description",
            //    "Errors":["The Description field is required."]
            //}]

            //if this is not an ajax request return out of this method
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            //grab the model state
            var ModelStateOfMethod = filterContext.Controller.ViewData.ModelState;

            //is the model state valid?
            if (!ModelStateOfMethod.IsValid)
            {
                //set the result of the errors in a json result (we need to transform it)
                filterContext.Result = new JsonResult()
                {
                    //the select new needs to be in the specific format. Don't change
                    Data = new
                    {
                        HasValidationErrors = true,
                        ErrorLookup = (from MyKey in ModelStateOfMethod.Keys
                                       where ModelStateOfMethod[MyKey].Errors.Count > 0
                                       select new
                                       {
                                           Key = MyKey,
                                           Errors = ModelStateOfMethod[MyKey].Errors.Select(y => y.ErrorMessage).ToArray()
                                       }).ToArray()
                    }
                };

                //set the response to a bad request so the error's will be shown
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

    }

}
