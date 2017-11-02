using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService;
using Xunit;

namespace ToracLibrary.UnitTest.HttpClientServices
{
    public class HtmlRequestTest
    {

        [Fact(DisplayName = "Http Request Builder With No PreRequest Interceptors")]
        public async Task RequestBuilderWOPreRequestInterceptorTest1()
        {
            var ResponseFromServiceToTest = new Tuple<string, string>("Value1", "Value2");
            var RequestParameters = new Tuple<string, string>("Parameter1", "ParameterValue1");

            var MockHttpClient = new Mock<HttpService>() { CallBase = true }.As<IHttpService>();

            Expression<Func<IEnumerable<KeyValuePair<string, string>>, bool>> headerCheck = y => y.Count() == 1 && y.First().Key == "Token" && y.First().Value == "TokenValue123";
            Expression<Func<ByteArrayContent, bool>> bodycheck = y => y.ReadAsStringAsync().Result == HttpService.JsonDataToSendInRequest(RequestParameters).ReadAsStringAsync().Result;

            MockHttpClient.Setup(x => x.MakeRequestAsync(HttpMethod.Post, "PatientGet", HttpService.AcceptTypeEnum.JSON, It.Is(headerCheck), It.Is(bodycheck)))
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = HttpService.JsonDataToSendInRequest(ResponseFromServiceToTest) }));

            var result = await MockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Post)
                                                .AddHeader("Token", "TokenValue123")
                                                .SetJsonRequestParameters(RequestParameters)
                                                .AcceptJsonResponse<Tuple<string, string>>()
                                                .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest.Item1, result.Item1);
            Assert.Equal(ResponseFromServiceToTest.Item2, result.Item2);
        }

        [Fact(DisplayName = "Http Request Builder")]
        public async Task RequestBuilderWPreRequestInterceptorTest1()
        {
            var ResponseFromServiceToTest = new Tuple<string, string>("Value1", "Value2");
            var RequestParameters = new Tuple<string, string>("Parameter1", "ParameterValue1");

            var MockHttpClient = new Mock<HttpService>() { CallBase = true }.As<IHttpService>();

            Expression<Func<IEnumerable<KeyValuePair<string, string>>, bool>> headerCheck = y => y.Count() == 2 && y.First().Key == "Token" && y.First().Value == "TokenValue123" && y.ElementAt(1).Key == "PreRequestKey" && y.ElementAt(1).Value == "PreRequestValue";
            Expression<Func<ByteArrayContent, bool>> bodycheck = y => y.ReadAsStringAsync().Result == HttpService.JsonDataToSendInRequest(RequestParameters).ReadAsStringAsync().Result;

            MockHttpClient.Setup(x => x.MakeRequestAsync(HttpMethod.Post, "PatientGet", HttpService.AcceptTypeEnum.JSON, It.Is(headerCheck), It.Is(bodycheck)))
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = HttpService.JsonDataToSendInRequest(ResponseFromServiceToTest) }));

            var result = await MockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Post)
                                                .AddHeader("Token", "TokenValue123")
                                                .SetJsonRequestParameters(RequestParameters)
                                                .AddPreRequestInterceptor(x => x.AddHeader("PreRequestKey", "PreRequestValue"))
                                                .AcceptJsonResponse<Tuple<string, string>>()
                                                .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest.Item1, result.Item1);
            Assert.Equal(ResponseFromServiceToTest.Item2, result.Item2);
        }

        [Fact(DisplayName = "Basic Authentication")]
        public void BasicAuthenticationTest1()
        {
            var mockHttpClient = new Mock<HttpService>() { CallBase = true }.As<IHttpService>();

            var expectedHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes("jason:pw"));

            var request = mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Post)
                .AddBasicAuthentication("jason", "pw");

            Assert.Single(request.Headers);
            Assert.Equal("Authorization", request.Headers.First().Key);
            Assert.Equal("Basic " + expectedHeaderValue, request.Headers.First().Value);
        }

        [Fact(DisplayName = "Body Parameters Throw For Specific Http Methods")]
        public void NoBodyAvailableForSpecificHttpMethod()
        {
            var mockHttpClient = new Mock<HttpService>() { CallBase = true }.As<IHttpService>();

            //check the json request parameters
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Get).SetJsonRequestParameters("Test"));
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Head).SetJsonRequestParameters("Test"));
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Options).SetJsonRequestParameters("Test"));

            var formsEncodedValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Test","Test")
            };

            //check the forms encoded
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Get).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Head).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
            Assert.Throws<ArgumentOutOfRangeException>(() => mockHttpClient.Object.CreateRequest("PatientGet", HttpMethod.Options).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
        }

    }
}
