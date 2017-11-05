using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ToracLibrary.HttpClientService.HttpServiceClient;

namespace ToracLibrary.HttpClientService.Provider
{

    /// <summary>
    /// Provides the provider that will resolve the http client service will essentially is a wrapper around HttpClient
    /// </summary>
    public interface IHttpClientProvider
    {

        /// <summary>
        /// Register the http client in the provider
        /// </summary>
        /// <param name="Key">Key to register the service under</param>
        /// <param name="HttpClientServiceToSet">The instance of the service to set</param>
        void RegisterHttpClientService(string Key, IHttpService HttpClientServiceToSet);

        /// <summary>
        /// Resolves the http client service for the given key
        /// </summary>
        /// <param name="Key">Key to lookup the service under</param>
        /// <returns>IHttpClientService. Throws error if not found</returns>
        IHttpService ResolveHttpClientService(string Key);

    }

}
