using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.HttpClientService;
using ToracLibrary.HttpClientService.HttpServiceClient;
using ToracLibrary.HttpClientService.RequestBuilder;
using Xunit;

namespace ToracLibrary.UnitTest.HttpClientServices
{
    public class HtmlRequestBuilderTest
    {

        #region Helper Methods

        private static bool UriMatch(Uri RequestUri, string ExpectedUri)
        {
            return RequestUri == new Uri(ExpectedUri, UriKind.RelativeOrAbsolute);
        }

        private static bool HeaderIsFound(HttpRequestHeaders RequestHeaders, string Key, string Value)
        {
            return RequestHeaders.Any(x => x.Key == Key && x.Value.Any(y => y.Contains(Value)));
        }

        private static bool JsonAcceptHeaderIsFound(HttpRequestHeaders RequestHeaders)
        {
            return RequestHeaders.Any(x => x.Key == "Accept" && x.Value.Any(y => y.Contains(ContentTypeLookup.JsonContentType)));
        }

        private static bool HtmlAcceptHeaderIsFound(HttpRequestHeaders RequestHeaders)
        {
            return RequestHeaders.Any(x => x.Key == "Accept" && x.Value.Any(y => y.Contains(ContentTypeLookup.HtmlContentType)));
        }

        private static bool RequestBodyMatches<T>(HttpRequestMessage HttpRequest, T ExpectedBody)
        {
            return HttpRequest.Content.ReadAsStringAsync().Result == HttpRequestBuilder.JsonDataToSendInRequest(ExpectedBody).ReadAsStringAsync().Result;
        }

        #endregion

        #region Unit Tests

        [Fact(DisplayName = "Basic Json Http Request")]
        public async Task BasicHttpJsonRequestTest1()
        {
            const string UrlToCall = "PatientSave";
            var HeaderToAdd = new KeyValuePair<string, string>("H1Key", "H1Value");
            var ResponseFromServiceToTest = new Tuple<string, string>("Value1", "Value2");
            var RequestParameters = new Tuple<string, string>("Parameter1", "ParameterValue1");

            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y =>
                            UriMatch(y.RequestUri, UrlToCall) &&
                            JsonAcceptHeaderIsFound(y.Headers) &&
                            HeaderIsFound(y.Headers, HeaderToAdd.Key, HeaderToAdd.Value) &&
                            RequestBodyMatches(y, RequestParameters))))

                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = HttpRequestBuilder.JsonDataToSendInRequest(ResponseFromServiceToTest) }));

            var Response = await new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Post)
                                    .AddHeader(HeaderToAdd.Key, HeaderToAdd.Value)
                                    .SetJsonRequestParameters(RequestParameters)
                                    .AcceptJsonResponse<Tuple<string, string>>()
                                    .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest, Response);

            MockHttpService.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Fact(DisplayName = "Basic Json Http Request Fails")]
        public async Task BasicHttpJsonRequestFailsTest1()
        {
            const string UrlToCall = "PatientSave";

            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y => UriMatch(y.RequestUri, UrlToCall))))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));

            await Assert.ThrowsAsync<HttpRequestException>(() =>
            {
                return new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Get)
                                        .AcceptJsonResponse<bool>()
                                        .SendRequestAsync();
            });
        }

        [Fact(DisplayName = "Basic Html Http Request")]
        public async Task BasicHttpHtmlRequestTest1()
        {
            const string UrlToCall = "GetPartialViewHtml";
            var ResponseFromServiceToTest = "<html>Text</html>";
            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y =>
                            UriMatch(y.RequestUri, UrlToCall) &&
                            HtmlAcceptHeaderIsFound(y.Headers))))

                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(ResponseFromServiceToTest) }));

            var Response = await new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Get)
                                    .AcceptHtmlResponse()
                                    .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest, Response);

            MockHttpService.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Fact(DisplayName = "Basic Head Http Request. Contains No Response")]
        public async Task BasicHeadRequestWithNoBodyResponseTest1()
        {
            const string UrlToCall = "HeadCommand";
            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y =>
                            UriMatch(y.RequestUri, UrlToCall))))

                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            var Response = await new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Head)
                                    .AcceptNoResponse()
                                    .SendRequestAsync();

            Assert.True(Response.IsSuccessStatusCode);

            MockHttpService.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Fact(DisplayName = "Http Request With PreRequest Interceptors")]
        public async Task JsonRequestWithPreRequestInterceptor()
        {
            const string UrlToCall = "PatientSave";
            var HeaderToAdd = new KeyValuePair<string, string>("H1Key", "H1Value");
            var InterceptorHeaderToAdd = new KeyValuePair<string, string>("H1Key", "H1Value");
            var ResponseFromServiceToTest = new Tuple<string, string>("Value1", "Value2");
            var RequestParameters = new Tuple<string, string>("Parameter1", "ParameterValue1");

            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y =>
                        UriMatch(y.RequestUri, UrlToCall) &&
                        JsonAcceptHeaderIsFound(y.Headers) &&
                        HeaderIsFound(y.Headers, HeaderToAdd.Key, HeaderToAdd.Value) &&
                        HeaderIsFound(y.Headers, InterceptorHeaderToAdd.Key, InterceptorHeaderToAdd.Value) &&
                        RequestBodyMatches(y, RequestParameters))))

                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = HttpRequestBuilder.JsonDataToSendInRequest(ResponseFromServiceToTest) }));

            var Response = await new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Post)
                                    .AddHeader(HeaderToAdd.Key, HeaderToAdd.Value)
                                    .SetJsonRequestParameters(RequestParameters)
                                    .AddPreRequestInterceptor(x => x.AddHeader(InterceptorHeaderToAdd.Key, InterceptorHeaderToAdd.Value))
                                    .AcceptJsonResponse<Tuple<string, string>>()
                                    .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest, Response);

            MockHttpService.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Fact(DisplayName = "Basic Authentication")]
        public async Task BasicAuthenticationTest1()
        {
            const string UrlToCall = "PatientSave";
            const string ResponseFromServiceToTest = "Value1";
            const string RequestParameters = "RequestValue1";

            var MockHttpService = new Mock<IHttpService>();

            MockHttpService.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(y =>
                        UriMatch(y.RequestUri, UrlToCall) &&
                        JsonAcceptHeaderIsFound(y.Headers) &&
                        HeaderIsFound(y.Headers, "Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("UserName1:Password1"))) &&
                        RequestBodyMatches(y, RequestParameters))))

                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = HttpRequestBuilder.JsonDataToSendInRequest(ResponseFromServiceToTest) }));

            var Response = await new HttpRequestBuilder(MockHttpService.Object, UrlToCall, HttpMethod.Post)
                                    .AddBasicAuthentication("UserName1", "Password1")
                                    .SetJsonRequestParameters(RequestParameters)
                                    .AcceptJsonResponse<string>()
                                    .SendRequestAsync();

            Assert.Equal(ResponseFromServiceToTest, Response);

            MockHttpService.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Fact(DisplayName = "Body Parameters Throw For Specific Http Methods For Json Request Body")]
        public void NoBodyAvailableForSpecificHttpMethodForJsonRequestBody()
        {
            var MockHttpService = new Mock<IHttpService>();

            //check the json request parameters
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Get).SetJsonRequestParameters("Test"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Head).SetJsonRequestParameters("Test"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Options).SetJsonRequestParameters("Test"));
        }

        [Fact(DisplayName = "Body Parameters Throw For Specific Http Methods For Forms Encoded Body")]
        public void NoBodyAvailableForSpecificHttpMethodForFormsEncodedBody()
        {
            var MockHttpService = new Mock<IHttpService>();

            var formsEncodedValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Test","Test")
            };

            //check the forms encoded
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Get).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Head).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
            Assert.Throws<ArgumentOutOfRangeException>(() => new HttpRequestBuilder(MockHttpService.Object, "PatientGet", HttpMethod.Options).SetFormUrlEncodedContentRequestParameter(formsEncodedValues));
        }

        #endregion

    }
}
