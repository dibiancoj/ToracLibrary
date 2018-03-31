using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.AspNet.AspNetMVC.CustomFilters
{

    /// <summary>
    /// Enables you to return model validation on an ajax post and have the ui update in mvc.
    /// This is written code rather then built into the framework
    /// </summary>
    public class AjaxJSONPostValidationAttribute : ActionFilterAttribute
    {
        //* note: jquery.validate updates the validation as the user types. Adding to solution will have the real time response for the user.
        //you will also need the elements to be in a form if you want the real time as you type. If they aren't in a form then it will be updated only when going to the server

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

            //grab all the validation elements
            var validationElements = $('.field-validation-valid');

            //if we have no errors the clear all the errors
            if (errObject == null) {
                validationElements.html('');
            }
            else if (errObject.HasValidationErrors) {

                //loop through the validation elements
                for (var i = 0; i < validationElements.length; i++) {
                    
                    //grab the element
                    var element = $(validationElements[i]);

                    //go try to find this error span in the list
                    var errorFound = FindInList(element.attr('data-valmsg-for'), errObject.ErrorLookup);

                    //did we find it in the list?
                    if (errorFound == null) {

                        //this element is not an error anymore...clear it out
                        element.html('');
                    }
                    else {

                        //this element has an error...set the text
                        element.html(errorFound.Errors[0]);
                    }

                }
            }
        }

        function FindInList(key, list) {
            //loop through the list and try to find the error by element key
            for (var i = 0; i < list.length; i++) {
                if (list[i].Key == key) {
                    //this element has an error
                    return list[i];
                }
            }

            //this element doesn't have an error
            return null;
        }
         */

        /// <summary>
        /// On the method executing it will run this method. 
        /// </summary>
        /// <param name="filterContext">filter context running</param>
        [MethodIsNotTestable("Unit Test Not Implemented Yet")]
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
                                       where ModelStateOfMethod[MyKey].Errors.Any()
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
