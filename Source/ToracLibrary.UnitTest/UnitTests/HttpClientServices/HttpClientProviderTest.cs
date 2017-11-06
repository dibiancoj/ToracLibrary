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
            var Provider = new HttpClientProvider();

            var Service1 = new KeyValuePair<string, IHttpService>("1", new HttpService(new HttpClient()));
            var Service2 = new KeyValuePair<string, IHttpService>("2", new HttpService(new HttpClient()));

            Provider.RegisterHttpClientService(Service1.Key, Service1.Value);
            Provider.RegisterHttpClientService(Service2.Key, Service2.Value);

            Assert.Equal(Service1.Value, Provider.ResolveHttpClientService("1"));
            Assert.Equal(Service2.Value, Provider.ResolveHttpClientService("2"));

            //make sure this isn't equal
            Assert.NotEqual(Service2.Value, Provider.ResolveHttpClientService("1"));
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
