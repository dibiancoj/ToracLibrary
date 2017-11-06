using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.HttpClientService.HttpServiceClient;
using ToracLibrary.HttpClientService.ResponseHandlers;

[assembly: InternalsVisibleTo("ToracLibrary.UnitTest")]

namespace ToracLibrary.HttpClientService.RequestBuilder
{

    /// <summary>
    /// Is a fluent api to build up a http request and send it
    /// </summary>
    public class HttpRequestBuilder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HttpClientServiceToSet">Http Client Services Which Does The Actual Sending Of The Message</param>
        /// <param name="UrlToSend">Url To Send The Web Request To</param>
        /// <param name="HttpRequestMethodTypeToSet">Web Request Type. ie: Get, Post, Put, etc.</param>
        public HttpRequestBuilder(IHttpService HttpClientServiceToSet, string UrlToSend, HttpMethod HttpRequestMethodTypeToSet)
        {
            HttpClientService = HttpClientServiceToSet;
            UrlToSendRequestTo = UrlToSend;
            HttpRequestMethodType = HttpRequestMethodTypeToSet;
        }

        #endregion

        #region Enum

        /// <summary>
        /// Accept type this wrapper supports
        /// </summary>
        public enum AcceptTypeEnum
        {

            /// <summary>
            /// Json response type
            /// </summary>
            JSON = 0,

            /// <summary>
            /// Html response type
            /// </summary>
            Html = 1,

            /// <summary>
            /// Plain text response type
            /// </summary>
            Text = 2,

            /// <summary>
            /// When no response is needed. ie: head
            /// </summary>
            EmptyResponse = 3

        }

        #endregion

        #region Properties

        /// <summary>
        /// Http Client Services Which Does The Actual Sending Of The Message
        /// </summary>
        internal IHttpService HttpClientService { get; private set; }

        /// <summary>
        /// Holds any interceptors you want before you send the request
        /// </summary>
        internal List<Action<HttpRequestBuilder>> PreRequestInterceptors { get; private set; }

        /// <summary>
        /// Url to send the request to
        /// </summary>
        internal string UrlToSendRequestTo { get; private set; }

        /// <summary>
        /// Web Request Type. ie: Get, Post, Put, etc.
        /// </summary>
        internal HttpMethod HttpRequestMethodType { get; private set; }

        /// <summary>
        /// Accept type which contains the data formats your request will except. ie: Json, Xml, etc.
        /// </summary>
        internal AcceptTypeEnum AcceptType { get; set; }

        /// <summary>
        /// Headers for the request
        /// </summary>
        internal List<KeyValuePair<string, string>> Headers { get; private set; }

        /// <summary>
        /// Content Body. The body parameters for the request
        /// </summary>
        internal ByteArrayContent Body { get; private set; }

        #endregion

        #region Fluent Methods

        /// <summary>
        /// Add a pre request interceptor to adjust the request
        /// </summary>
        /// <param name="InterceptorToAdd">Interceptor to add</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder AddPreRequestInterceptor(Action<HttpRequestBuilder> InterceptorToAdd)
        {
            if (PreRequestInterceptors == null)
            {
                PreRequestInterceptors = new List<Action<HttpRequestBuilder>>();
            }

            PreRequestInterceptors.Add(InterceptorToAdd);
            return this;
        }

        /// <summary>
        /// Add basic authentication to the header list
        /// </summary>
        /// <param name="UserName">User name to add to the basic authentication header</param>
        /// <param name="Password">Password to add to the basic authentication header</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder AddBasicAuthentication(string UserName, string Password)
        {
            AddHeader("Authorization", $"Basic {BasicAuthenticationHeaderValue(UserName, Password)}");
            return this;
        }

        /// <summary>
        /// Add a header to the request
        /// </summary>
        /// <param name="Key">Key of the header to add</param>
        /// <param name="Value">Value of the header to add</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder AddHeader(string Key, string Value)
        {
            if (Headers == null)
            {
                Headers = new List<KeyValuePair<string, string>>();
            }

            Headers.Add(new KeyValuePair<string, string>(Key, Value));
            return this;
        }

        /// <summary>
        /// Add a list of headers to the request
        /// </summary>
        /// <param name="HeadersToAdd">Headers to add to the request</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder AddHeaders(IEnumerable<KeyValuePair<string, string>> HeadersToAdd)
        {
            if (Headers == null)
            {
                Headers = new List<KeyValuePair<string, string>>();
            }

            Headers.AddRange(HeadersToAdd);
            return this;
        }

        /// <summary>
        /// Add a json request body
        /// </summary>
        /// <typeparam name="TRequestBodyType">Type of the request body</typeparam>
        /// <param name="RequestParameterBody">Instance of the request body for the parameter</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder SetJsonRequestParameters<TRequestBodyType>(TRequestBodyType RequestParameterBody)
        {
            if (!CanHaveBody(HttpRequestMethodType))
            {
                throw new ArgumentOutOfRangeException("Request Body Not Available On " + HttpRequestMethodType.ToString());
            }

            Body = JsonDataToSendInRequest(RequestParameterBody);
            return this;
        }

        /// <summary>
        /// Adds a forms encoded request body
        /// </summary>
        /// <param name="RequestParameters">Request parameters to add to the body which will be url encoded</param>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public HttpRequestBuilder SetFormUrlEncodedContentRequestParameter(IEnumerable<KeyValuePair<string, string>> RequestParameters)
        {
            if (!CanHaveBody(HttpRequestMethodType))
            {
                throw new ArgumentOutOfRangeException("Request Body Not Available On " + HttpRequestMethodType.ToString());
            }

            Body = new FormUrlEncodedContent(RequestParameters);
            return this;
        }

        #region Response Handlers

        /// <summary>
        /// Expect a specific Json model type back for the request. This is returned in the http response
        /// </summary>
        /// <typeparam name="TResponseType">response type</typeparam>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public ResponseHandlerJson<TResponseType> AcceptJsonResponse<TResponseType>()
        {
            return new ResponseHandlerJson<TResponseType>(this);
        }

        /// <summary>
        /// Expect Html back from the response
        /// </summary>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public ResponseHandlerText AcceptHtmlResponse()
        {
            return new ResponseHandlerText(this, AcceptTypeEnum.Html);
        }

        /// <summary>
        /// Expect text back from the response
        /// </summary>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public ResponseHandlerText AcceptTextResponse()
        {
            return new ResponseHandlerText(this, AcceptTypeEnum.Text);
        }

        /// <summary>
        /// Expect no response back ie: head call
        /// </summary>
        /// <returns>HttpRequestBuilder to build up the fluent api</returns>
        public ResponseHandlerEmpty AcceptNoResponse()
        {
            return new ResponseHandlerEmpty(this);
        }

        #endregion

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Convert a request builder to a http request message
        /// </summary>
        /// <returns>Http request message to send</returns>
        internal HttpRequestMessage ToHttpRequestMessage()
        {
            //any pre - request interceptors
            if (PreRequestInterceptors.AnyWithNullCheck())
            {
                foreach (var PreRequestToRun in PreRequestInterceptors)
                {
                    PreRequestToRun(this);
                }
            }

            //go create the initial request
            var RequestToMake = new HttpRequestMessage(HttpRequestMethodType, UrlToSendRequestTo);

            //add the accept headers (could be null if you are calling a head
            var AcceptTypeToUse = AcceptTypeToMediaQuality(AcceptType);

            if (AcceptTypeToUse != null)
            {
                RequestToMake.Headers.Accept.Add(AcceptTypeToUse);
            }

            //do we have any headers
            if (Headers.AnyWithNullCheck())
            {
                //loop through the headers and add them
                foreach (var HeaderToAdd in Headers)
                {
                    //add the specific header
                    RequestToMake.Headers.Add(HeaderToAdd.Key, HeaderToAdd.Value);
                }
            }

            //set the request content (either JSON or FormUrlEncodedContent or null [if http get])
            RequestToMake.Content = Body;

            //return the request
            return RequestToMake;
        }

        /// <summary>
        /// Converts an object into a string content which you send in a json request
        /// </summary>
        /// <typeparam name="T">Type of the param object to send</typeparam>
        /// <param name="objectParameters">parameter object to send</param>
        /// <returns>String content which you can pass into MakeRequest</returns>
        internal static StringContent JsonDataToSendInRequest<T>(T objectParameters)
        {
            return new StringContent(JsonConvert.SerializeObject(objectParameters), Encoding.UTF8, ContentTypeLookup.JsonContentType);
        }


        #endregion

        #region Private Helpers

        /// <summary>
        /// Can the specific method type passed in have a body?
        /// </summary>
        /// <param name="HttpMethodType">Http method type that we want to check</param>
        /// <returns>true if it can have a body</returns>
        private static bool CanHaveBody(HttpMethod HttpMethodType)
        {
            return HttpMethodType != HttpMethod.Get &&
                 HttpMethodType != HttpMethod.Options &&
                 HttpMethodType != HttpMethod.Head;
        }

        /// <summary>
        /// Creates the base 64 encoded value to set in the header
        /// </summary>
        /// <param name="UserName">User name to pass through</param>
        /// <param name="Password">Password to pass through</param>
        /// <returns>Encoded value to add to the header</returns>
        private static string BasicAuthenticationHeaderValue(string UserName, string Password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}"));
        }

        /// <summary>
        /// Determine what media type header value we are setting from the accept type enum value we pass in
        /// </summary>
        /// <param name="acceptTypeId">Accept type id we want to use for this request</param>
        /// <returns>Converted MediaTypeWithQualityHeaderValue</returns>
        private static MediaTypeWithQualityHeaderValue AcceptTypeToMediaQuality(AcceptTypeEnum acceptTypeId)
        {
            //what accept type do we want
            if (acceptTypeId == AcceptTypeEnum.JSON)
            {
                return ContentTypeLookup.JsonMediaType;
            }

            if (acceptTypeId == AcceptTypeEnum.Html)
            {
                return ContentTypeLookup.HtmlMediaType;
            }

            if (acceptTypeId == AcceptTypeEnum.Text)
            {
                return ContentTypeLookup.TextMediaType;
            }

            if (acceptTypeId == AcceptTypeEnum.EmptyResponse)
            {
                return null;
            }

            throw new NotImplementedException();
        }

        #endregion

    }

}
