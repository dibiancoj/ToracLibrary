using System;
using System.Collections.Concurrent;
using ToracLibrary.HttpClientService.HttpServiceClient;

namespace ToracLibrary.HttpClientService.Provider
{

    /// <summary>
    /// Provides the provider that will resolve the http client service will essentially is a wrapper around HttpClient
    /// </summary>
    public class HttpClientProvider : IHttpClientProvider
    {

        #region Properties

        /// <summary>
        /// Holds each of the instances that were registered
        /// </summary>
        private ConcurrentDictionary<string, IHttpService> RegisteredClients { get; } = new ConcurrentDictionary<string, IHttpService>();

        #endregion

        #region Methods

        /// <summary>
        /// Register the http client in the provider
        /// </summary>
        /// <param name="Key">Key to register the service under</param>
        /// <param name="HttpClientServiceToSet">The instance of the service to set</param>
        public void RegisterHttpClientService(string Key, IHttpService HttpClientServiceToSet)
        {
            RegisteredClients.TryAdd(Key, HttpClientServiceToSet);
        }

        /// <summary>
        /// Resolves the http client service for the given key
        /// </summary>
        /// <param name="Key">Key to lookup the service under</param>
        /// <returns>IHttpClientService. Throws error if not found</returns>
        public IHttpService ResolveHttpClientService(string Key)
        {
            if (RegisteredClients.TryGetValue(Key, out IHttpService tryGetClient))
            {
                return tryGetClient;
            }

            throw new ArgumentOutOfRangeException(nameof(Key), "Registered Http Client Not Registered In Provider");
        }

        #endregion

    }
}
