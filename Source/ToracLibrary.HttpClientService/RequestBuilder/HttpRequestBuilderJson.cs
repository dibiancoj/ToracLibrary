using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Json;
using static ToracLibrary.HttpClientService.HttpService;

namespace ToracLibrary.HttpClientService.RequestBuilder
{

    /// <summary>
    /// Request builder for json response types
    /// </summary>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    public class HttpRequestBuilderJson<TResponse>
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RequestBuilderToSet">Request builder to use</param>
        public HttpRequestBuilderJson(HttpRequestBuilder RequestBuilderToSet)
        {
            RequestBuilder = RequestBuilderToSet;
            RequestBuilder.AcceptType = AcceptTypeEnum.JSON;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the request builder base request
        /// </summary>
        private HttpRequestBuilder RequestBuilder { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Go send a request
        /// </summary>
        /// <returns>The actual response to the request</returns>
        public async Task<TResponse> SendRequestAsync()
        {
            if (RequestBuilder.PreRequestInterceptors != null)
            {
                foreach (var preRequest in RequestBuilder.PreRequestInterceptors)
                {
                    RequestBuilder = preRequest(RequestBuilder);
                }
            }

            var RequestResult = await RequestBuilder.HttpClientService.MakeRequestAsync(RequestBuilder.HttpRequestMethod, RequestBuilder.Url, RequestBuilder.AcceptType, RequestBuilder.Headers, RequestBuilder.Body).ConfigureAwait(false);

            RequestResult.EnsureSuccessStatusCode();

            return JsonNetSerializer.DeserializeFromStream<TResponse>(await RequestResult.Content.ReadAsStreamAsync());
        }

        #endregion

    }

}
