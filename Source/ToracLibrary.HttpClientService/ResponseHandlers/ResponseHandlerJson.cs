﻿using System.Net.Http;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService.RequestBuilder;
using ToracLibrary.Serialization.Json;
using static ToracLibrary.HttpClientService.RequestBuilder.HttpRequestBuilder;

namespace ToracLibrary.HttpClientService.ResponseHandlers
{

    /// <summary>
    /// Handles a json accept type. HttpRequestBuilder will create a new instance once it says "Accept Json"
    /// </summary>
    /// <typeparam name="TResponseType">Response type that will be deserialized from json into a model of T</typeparam>
    public class ResponseHandlerJson<TResponseType> : IResponseHandler<TResponseType>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RequestBuilderToSet">Request builder that has the parameters we have already built up</param>
        public ResponseHandlerJson(HttpRequestBuilder RequestBuilderToSet)
        {
            RequestBuilder = RequestBuilderToSet;
            RequestBuilder.AcceptType = AcceptTypeEnum.JSON;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Request builder that has the parameters we have already built up
        /// </summary>
        private HttpRequestBuilder RequestBuilder { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send a request and get the raw response back.
        /// </summary>
        /// <returns>task of HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> SendRawRequestAsync()
        {
            return await RequestBuilder.HttpClientService.SendAsync(RequestBuilder.ToHttpRequestMessage()).ConfigureAwait(false);
        }

        /// <summary>
        /// Send the request and return the response
        /// </summary>
        /// <returns>Task of the response model</returns>
        public async Task<TResponseType> SendRequestAsync()
        {
            //go make the request
            var RawRequestResponse = await SendRawRequestAsync().ConfigureAwait(false);

            //make sure it succeeded. If it didn't, it will throw an error. (different then just checking the status because this will throw)
            RawRequestResponse.EnsureSuccessStatusCode();

            //we are "ok" with a 200. Go deserialize the response and return the result
            return JsonNetSerializer.DeserializeFromStream<TResponseType>(await RawRequestResponse.Content.ReadAsStreamAsync());
        }

        #endregion

    }

}
