using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using static ToracLibrary.HttpClientService.HttpService;

namespace ToracLibrary.HttpClientService.RequestBuilder
{

    /// <summary>
    /// Allows you to build up an http call in a fluent api
    /// </summary>
    public class HttpRequestBuilder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HttpClientServiceToSet">http client service to use</param>
        /// <param name="UrlToSet">url to send the request to</param>
        /// <param name="HttpRequestMethodToSet">request type to use</param>
        public HttpRequestBuilder(IHttpService HttpClientServiceToSet, string UrlToSet, HttpMethod HttpRequestMethodToSet)
        {
            Url = UrlToSet;
            HttpRequestMethod = HttpRequestMethodToSet;
            HttpClientService = HttpClientServiceToSet;
            Headers = new List<KeyValuePair<string, string>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Http client service
        /// </summary>
        internal IHttpService HttpClientService { get; private set; }

        /// <summary>
        /// Request interceptors
        /// </summary>
        internal List<Func<HttpRequestBuilder, HttpRequestBuilder>> PreRequestInterceptors { get; private set; }

        /// <summary>
        /// Url to use
        /// </summary>
        internal string Url { get; private set; }

        /// <summary>
        /// Request method type
        /// </summary>
        internal HttpMethod HttpRequestMethod { get; private set; }

        /// <summary>
        /// accept type
        /// </summary>
        internal AcceptTypeEnum AcceptType { get; set; }

        /// <summary>
        /// headers
        /// </summary>
        internal List<KeyValuePair<string, string>> Headers { get; private set; }

        /// <summary>
        /// body of the request
        /// </summary>
        internal ByteArrayContent Body { get; private set; }

        #endregion

        #region Fluent Methods

        /// <summary>
        /// add a pre request interceptor
        /// </summary>
        /// <param name="InterceptorToAdd">Interceptor to add</param>
        /// <returns>request builder object</returns>
        public HttpRequestBuilder AddPreRequestInterceptor(Func<HttpRequestBuilder, HttpRequestBuilder> InterceptorToAdd)
        {            
            //lazy init the list
            if (PreRequestInterceptors == null)
            {
                PreRequestInterceptors = new List<Func<HttpRequestBuilder, HttpRequestBuilder>>();
            }

            PreRequestInterceptors.Add(InterceptorToAdd);
            return this;
        }

        /// <summary>
        /// add a single request header
        /// </summary>
        /// <param name="Key">header key</param>
        /// <param name="Value">header value</param>
        /// <returns>HttpRequestBuilder object</returns>
        public HttpRequestBuilder AddHeader(string Key, string Value)
        {
            Headers.Add(new KeyValuePair<string, string>(Key, Value));
            return this;
        }

        /// <summary>
        /// Add a basic authentication header
        /// </summary>
        /// <param name="UserName">user name</param>
        /// <param name="Password">password</param>
        /// <returns>HttpRequestBuilder object</returns>
        public HttpRequestBuilder AddBasicAuthentication(string UserName, string Password)
        {
            Headers.Add(new KeyValuePair<string, string>("Authorization", $"Basic {BasicAuthenticationHeaderValue(UserName, Password)}"));
            return this;
        }

        /// <summary>
        /// add a list of request headers
        /// </summary>
        /// <param name="headersToAdd">List of headers to add</param>
        /// <returns>HttpRequestBuilder object</returns>
        public HttpRequestBuilder AddHeaders(IEnumerable<KeyValuePair<string, string>> headersToAdd)
        {
            Headers.AddRange(headersToAdd);
            return this;
        }

        /// <summary>
        /// Add a request body
        /// </summary>
        /// <typeparam name="T">Request body type</typeparam>
        /// <param name="Parameters">Parameter to add</param>
        /// <returns>HttpRequestBuilder object</returns>
        public HttpRequestBuilder SetJsonRequestParameters<T>(T Parameter)
        {
            if (!CanHaveBodyParameter(HttpRequestMethod))
            {
                throw new ArgumentOutOfRangeException("Request Body Not Available On " + HttpRequestMethod.ToString());
            }

            Body = JsonDataToSendInRequest(Parameter);
            return this;
        }

        /// <summary>
        /// Use form encoded body parameters
        /// </summary>
        /// <param name="Parameters">Parameters to use in the body</param>
        /// <returns>HttpRequestBuilder object</returns>
        public HttpRequestBuilder SetFormUrlEncodedContentRequestParameter(IEnumerable<KeyValuePair<string, string>> Parameters)
        {
            if (!CanHaveBodyParameter(HttpRequestMethod))
            {
                throw new ArgumentOutOfRangeException("Request Body Not Available On " + HttpRequestMethod.ToString());
            }

            Body = new FormUrlEncodedContent(Parameters);
            return this;
        }

        /// <summary>
        /// Turn this request object into a json response
        /// </summary>
        /// <typeparam name="TResponse">Request response type to deserialize</typeparam>
        /// <returns>HttpRequestBuilderJson of TResponse</returns>
        public HttpRequestBuilderJson<TResponse> AcceptJsonResponse<TResponse>()
        {
            return new HttpRequestBuilderJson<TResponse>(this);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Is a body parameter availabel for this http method
        /// </summary>
        /// <param name="HttpMethod">Method type</param>
        /// <returns>If it is ok and passes validation</returns>
        private static bool CanHaveBodyParameter(HttpMethod HttpMethod)
        {
            return HttpMethod != HttpMethod.Get &&
                HttpMethod != HttpMethod.Options &&
                HttpMethod != HttpMethod.Head;
        }

        /// <summary>
        /// Helper to create the basic authentication value
        /// </summary>
        /// <param name="UserName">User name</param>
        /// <param name="Password">password</param>
        /// <returns>Encoded</returns>
        private static string BasicAuthenticationHeaderValue(string UserName, string Password)
        {
            return $"{UserName}:{Password}".ToBase64Encode();
        }

        #endregion

    }

}
