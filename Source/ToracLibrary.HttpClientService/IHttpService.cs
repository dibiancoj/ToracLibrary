using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ToracLibrary.HttpClientService.HttpService;

namespace ToracLibrary.HttpClientService
{

    /// <summary>
    /// Wrapper around Microsoft.HttpClient to allow for mocking in a unit test
    /// </summary>
    public interface IHttpService
    {

        /// <summary>
        /// Make a web request
        /// </summary>
        /// <param name="RequestType">Request type to make. Get, Post, etc.</param>
        /// <param name="Url">Url to make the web request into</param>
        /// <param name="AcceptType">Accept typ</param>
        /// <param name="Headers">Additional headers to make to the request</param>
        /// <param name="BodyParameters">Parameters that get sent in the content</param>
        /// <returns>Built up task with the wrapped up call</returns>
        Task<HttpResponseMessage> MakeRequestAsync(HttpMethod RequestType, string Url, AcceptTypeEnum AcceptType, IEnumerable<KeyValuePair<string, string>> Headers, ByteArrayContent BodyParameters);

    }

}
