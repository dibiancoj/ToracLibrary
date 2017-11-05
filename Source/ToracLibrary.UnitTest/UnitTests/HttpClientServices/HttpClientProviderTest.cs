using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService.HttpServiceClient;
using ToracLibrary.HttpClientService.Provider;
using Xunit;

namespace ToracLibrary.UnitTest.UnitTests.HttpClientServices
{

    public class HttpClientProviderTest
    {

        [Fact(DisplayName = "Http Client Provider Test")]
        public void RequestBuilderTest1()
        {
            var provider = new HttpClientProvider();

            var service1 = new KeyValuePair<string, IHttpService>("1", new HttpService(new HttpClient()));
            var service2 = new KeyValuePair<string, IHttpService>("2", new HttpService(new HttpClient()));

            provider.RegisterHttpClientService(service1.Key, service1.Value);
            provider.RegisterHttpClientService(service2.Key, service2.Value);

            Assert.Equal(service1.Value, provider.ResolveHttpClientService("1"));
            Assert.Equal(service2.Value, provider.ResolveHttpClientService("2"));

            //make sure this isn't equal
            Assert.NotEqual(service2.Value, provider.ResolveHttpClientService("1"));
        }

        [Fact(DisplayName = "Http Client Service Provider Can't Find Entry")]
        public void RequestBuilderCantFindEntryTest1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new HttpClientProvider().ResolveHttpClientService("1");
            });
        }

    }

}
