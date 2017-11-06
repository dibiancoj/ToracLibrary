using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService.RequestBuilder;
using static ToracLibrary.HttpClientService.RequestBuilder.HttpRequestBuilder;

namespace ToracLibrary.HttpClientService.ResponseHandlers
{

    /// <summary>
    /// Handles a response that will be empty. ie: Head command
    /// </summary>
    public class ResponseHandlerEmpty : IResponseHandler<HttpResponseMessage>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RequestBuilderToSet">Request builder that has the parameters we have already built up</param>
        public ResponseHandlerEmpty(HttpRequestBuilder RequestBuilderToSet)
        {
            RequestBuilder = RequestBuilderToSet;
            RequestBuilder.AcceptType = AcceptTypeEnum.EmptyResponse;
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
        /// <returns>Task of the raw response incase you need it</returns>
        public async Task<HttpResponseMessage> SendRequestAsync()
        {
            //go make the request
            HttpResponseMessage RawRequestResponse = await SendRawRequestAsync().ConfigureAwait(false);

            //make sure it succeeded. If it didn't, it will throw an error. (different then just checking the status because this will throw)
            RawRequestResponse.EnsureSuccessStatusCode();

            //we are "ok" with a 200. Go read the reesponse and return it
            return RawRequestResponse;
        }

        #endregion

    }

}
