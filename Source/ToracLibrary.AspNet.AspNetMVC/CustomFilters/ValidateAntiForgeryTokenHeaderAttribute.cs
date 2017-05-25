using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.AspNet.AspNetMVC.CustomFilters
{

    /// <summary>
    /// Custom attribute to check for a antiforgery token in the header. Used for angular, jquery ajax calls outside of a form.
    /// </summary>
    [MethodIsNotTestable("It might be, haven't had the time to create a unit test for this. In Asp.net 5 they added this as a default so this will be going away soon anyway")]
    public class ValidateAntiForgeryTokenHeaderAttribute : AuthorizeAttribute
    {

        /*
         [ValidateAntiForgeryTokenOnJqueryAjax("__RequestVerificationToken")]
         ControllerMethod()


         //in jquery (per request)
           $.ajax({
            url: $('#EmailDialogSend').attr('data-Url'),
            method: "POST",
            data: { url: encodeURIComponent(window.location.href), email: encodeURIComponent($('#EmailDialogSendAddress').val()) },
            headers: { '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val() }
        }).done(function (response) {

        //in jquery as a global setting
        $.ajaxSetup({
            headers: { 'RequestVerificationToken': $('#__RequestVerificationToken').val() }
        });

        //see torac golf for angular 1 example using an http interceptor
        */

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RequestHeaderKeyToLookIn">Holds the key in the header that we will search for the token with</param>
        public ValidateAntiForgeryTokenHeaderAttribute(string RequestHeaderKeyToLookIn)
        {
            RequestHeaderKeyToLookForToken = RequestHeaderKeyToLookIn;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the key in the header that we will search for the token with
        /// </summary>
        private string RequestHeaderKeyToLookForToken { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Authorization method needs to be overridden to make sure they have the proper token
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Grab the request
            var Request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (Request.HttpMethod == System.Net.WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (Request.IsAjaxRequest())
                {
                    //Grab the cookie
                    var AntiForgeryCookie = Request.Cookies[System.Web.Helpers.AntiForgeryConfig.CookieName];

                    //grab the cookie value to validate
                    var CookieValue = AntiForgeryCookie != null
                        ? AntiForgeryCookie.Value
                        : null;

                    //go validate the parameters we have
                    System.Web.Helpers.AntiForgery.Validate(CookieValue, Request.Headers[RequestHeaderKeyToLookForToken]);
                }
                else
                {
                    //this is not an ajax post, just go authorize this
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
        }

        #endregion

    }

}
