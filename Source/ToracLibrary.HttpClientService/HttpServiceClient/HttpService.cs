using System.Net.Http;
using System.Threading.Tasks;

namespace ToracLibrary.HttpClientService.HttpServiceClient
{

    /// <summary>
    /// Implementation of the interface that does the sending of the request so we can mock it in unit tests
    /// </summary>
    public class HttpService : IHttpService
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="HttpClientToSet">Http client that will do the sending</param>
        public HttpService(HttpClient HttpClientToSet)
        {
            HttpClientToUse = HttpClientToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Http client that will send the request
        /// </summary>
        public HttpClient HttpClientToUse { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The actual sending of the message will go through this method. This is the method that will be mocked
        /// </summary>
        /// <param name="HttpRequestMessageToSend">Message to send</param>
        /// <returns>Task With Response Of The Request</returns>
        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage HttpRequestMessageToSend)
        {
            return HttpClientToUse.SendAsync(HttpRequestMessageToSend);
        }

        #endregion

    }

}
