using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService.RequestBuilder;
using static ToracLibrary.HttpClientService.RequestBuilder.HttpRequestBuilder;

namespace ToracLibrary.HttpClientService.ResponseHandlers
{

    /// <summary>
    /// Handles a json accept type. HttpRequestBuilder will create a new instance once it says "Accept Json"
    /// </summary>
    /// <typeparam name="TResponseType"></typeparam>
    public class ResponseHandlerJson<TResponseType>
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
        private HttpRequestBuilder RequestBuilder { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send the request and return the response
        /// </summary>
        /// <returns>Task of the response model</returns>
        public async Task<TResponseType> SendRequestAsync()
        {
            //go make the request
            HttpResponseMessage RawRequestResponse = await RequestBuilder.HttpClientService.SendAsync(RequestBuilder.ToHttpRequestMessage()).ConfigureAwait(false);

            //make sure it succeeded. If it didn't, it will throw an error. (different then just checking the status because this will throw)
            RawRequestResponse.EnsureSuccessStatusCode();

            //we are "ok" with a 200. Go deserialize the response and return the result
            return await DeserializeToJson(RawRequestResponse);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Deserialize the response using streams for better GC with larger responses
        /// </summary>
        /// <param name="RawHttpResponse">Raw http response from the request</param>
        /// <returns>Task of the response instance</returns>
        private async Task<TResponseType> DeserializeToJson(HttpResponseMessage RawHttpResponse)
        {
            //serialize a little faster with less gc for a long json document. Using streams instead of ReadAsStringAsync()

            using (var StreamToUse = await RawHttpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var StreamReaderToUse = new StreamReader(StreamToUse))
            using (var JsonReaderToUse = new JsonTextReader(StreamReaderToUse))
            {
                // read the json from a stream
                // json size doesn't matter because only a small piece is read at a time from the HTTP request
                return new JsonSerializer().Deserialize<TResponseType>(JsonReaderToUse);
            }
        }

        #endregion

    }

}
