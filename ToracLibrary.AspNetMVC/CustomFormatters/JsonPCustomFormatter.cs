//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using System.Web;

//namespace ToracLibrary.AspNetMVC.CustomFormatters
//{

//    //Used when you want to call Web API From Cross Domain Or Cross Servers From Javascript.

//    //****** Make Sure You Do This *******
//    //In Global.asax - protected void Application_Start() 
//    //  GlobalConfiguration.Configuration.Formatters.Clear();
//    //  GlobalConfiguration.Configuration.Formatters.Add(new JsonPMediaTypeFormatter());
//    //  ** Note if you need xml don't hit clear because it clears out the xml type. 
//    // 
//    //  If you want to keep both xml,json, etc. then use the following in jquery ajax
//    //  $.ajax({
//    //            headers: {
//    //                Accept: "application/json;",
//    //            },
//    //  -If using jquery.customAjax.js pass extra argument addJsonAcceptHeader = true
//    //  This says "Give me Json back and not xml"
//    //************************************

//    /// <summary>
//    /// Class is used to serialize JsonP
//    /// </summary>
//    public class JsonPCustomFormatter : JsonMediaTypeFormatter
//    {

//        #region Constructor

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public JsonPMediaTypeFormatter()
//        {
//            //add the default json media type
//            SupportedMediaTypes.Add(DefaultMediaType);

//            //now add the jsonP header value
//            SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("text/javascript"));

//            //add the uri path extension mapping
//            MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
//        }

//        #endregion

//        #region Properties

//        /// <summary>
//        /// Holds the callback Query Parameter
//        /// </summary>
//        private string _callbackQueryParameter;

//        /// <summary>
//        /// Holds the callback Query Parameter
//        /// </summary>
//        public string CallbackQueryParameter
//        {
//            get { return _callbackQueryParameter ?? "callback"; }
//            set { _callbackQueryParameter = value; }
//        }

//        #endregion

//        #region Override Methods

//        /// <summary>
//        /// Override the method - Write the stream async. We need to build the JsonP method which holds the result (the jsonp extra padding)
//        /// </summary>
//        /// <returns>Task</returns>
//        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
//        {
//            //IsJsonpRequest set's the callback and it also returns yes / no depending if it's a jsonp request.
//            //we could return a struct or a class but it's a little overkill for 2 values. if we need to add to it then
//            //make it struct or class
//            string callback;

//            //let's go check if it's a jsonP request
//            if (IsJsonPRequest(out callback))
//            {
//                //this is JsonP call...Let's go serialize the data
//                return Task.Factory.StartNew(() =>
//                {
//                    //let's create the stream writer...i wrapped it a using statement
//                    //it looks like that is fine. if we start get underlying stream closed, then just create it and don't dispose of it
//                    using (StreamWriter writer = new StreamWriter(writeStream))
//                    {
//                        //let's start writing the extra padding that we will return
//                        writer.Write(callback + "(");

//                        //flush the writer
//                        writer.Flush();

//                        //let's write the stream now and wait for it to be done
//                        base.WriteToStreamAsync(type, value, writeStream, content, transportContext).Wait();

//                        //write the end brace
//                        writer.Write(")");

//                        //flush the writer and return the task now
//                        writer.Flush();
//                    }
//                });
//            }
//            else
//            {
//                //it's not a JsonP request...just use the base method and return the task
//                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
//            }
//        }

//        #endregion

//        #region Helper Methods

//        /// <summary>
//        /// Is this a JsonP Request.
//        /// </summary>
//        /// <param name="callback">Returns the callback method name which is passed in in the query string for get. and form for post</param>
//        /// <returns>Result if its a JsonP request. Also sets the callback method</returns>
//        private bool IsJsonPRequest(out string callback)
//        {
//            //reset the callback string to null
//            callback = null;

//            //Let's check if its a post or a get
//            if (string.Equals(HttpContext.Current.Request.HttpMethod, "Post", StringComparison.OrdinalIgnoreCase))
//            {
//                //It's a post...let's grab it from the form data
//                callback = HttpContext.Current.Request.Form[CallbackQueryParameter];
//            }
//            else
//            {
//                //It's a get...let's grab it from the query string
//                callback = HttpContext.Current.Request.QueryString[CallbackQueryParameter];
//            }

//            //if the result is not null then it's a JsonP Call
//            return !string.IsNullOrEmpty(callback);
//        }

//        #endregion

//    }

//}
