using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.HttpClientService.HttpServiceClient
{

    /// <summary>
    /// Interface to abstract the sending of the request so we can mock it in unit tests
    /// </summary>
    public interface IHttpService
    {

        /// <summary>
        /// Http client that will send the request
        /// </summary>
        HttpClient HttpClientToUse { get; }

        /// <summary>
        /// The actual sending of the message will go through this method. This is the method that will be mocked
        /// </summary>
        /// <param name="HttpRequestMessageToSend">Message to send</param>
        /// <returns>Task With Response Of The Request</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage HttpRequestMessageToSend);

    }
}
