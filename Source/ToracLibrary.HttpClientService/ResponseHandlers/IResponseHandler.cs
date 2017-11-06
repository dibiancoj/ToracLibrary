using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.HttpClientService.ResponseHandlers
{

    /// <summary>
    /// Contract to ensure all handlers implement the necessary properties
    /// </summary>
    /// <typeparam name="TResponseType">Response type</typeparam>
    public interface IResponseHandler<TResponseType>
    {

        /// <summary>
        /// Send a raw request so the end caller has access to the raw response
        /// </summary>
        /// <returns>Raw response message</returns>
        Task<HttpResponseMessage> SendRawRequestAsync();

        /// <summary>
        /// Send a request and get the response
        /// </summary>
        /// <returns>Task of TResponse Type</returns>
        Task<TResponseType> SendRequestAsync();

    }

}
