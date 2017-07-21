using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.Core.ToracAttributes;
using ToracLibrary.Serialization.Json;

namespace ToracLibrary.HttpClientService
{

    /// <summary>
    /// Wrapper around HttpClient so we can mock this in an application unit test project
    /// </summary>
    [ClassIsNotTestable("Not going to simulate http requests now. Or create a controller to run a web request. Maybe at a later date")]
    public class HttpService : IHttpService
    {

        #region Constructor

        /// <summary>
        /// Constructor where the httpclient will be created on each request
        /// </summary>
        public HttpService()
        {
        }

        /// <summary>
        /// Constructor if you want to use a specific http client for each request. Ie: Send in a client with a base address
        /// </summary>
        /// <param name="HttpClientToUse">Http client to use on each request</param>
        public HttpService(HttpClient HttpClientToUse)
        {
            ClientToUse = HttpClientToUse;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Http client to use for each request. Can be null if the calling code wants to create a new http client each time
        /// </summary>
        private HttpClient ClientToUse { get; }

        #endregion

        #region Enum

        /// <summary>
        /// Accept type this wrapper supports
        /// </summary>
        public enum AcceptTypeEnum
        {

            /// <summary>
            /// Json response type
            /// </summary>
            JSON = 0

        }

        #endregion

        #region Constants

        /// <summary>
        /// Json content type
        /// </summary>
        private const string JsonContentType = "application/json";

        #endregion

        #region To Unit Test

        //[Test]
        //public void GenerateAccessToken_Test1()
        //{
        //    var mockHttpService = new Mock<IHttpClientService>();
        //    var sessionStateFullMock = new SessionStateServiceFullMock();

        //    var accessTokenToTest = Guid.NewGuid().ToString();
        //    var userTokenToTest = Guid.NewGuid().ToString();

        //    var responseMock = new HttpResponseMessage
        //    {
        //        Content = new StringContent(JsonConvert.SerializeObject(new { access_token = accessTokenToTest }))
        //    };

        //    responseMock.Headers.Add("UserToken", userTokenToTest);

        //    mockHttpService.Setup(x => x.MakeRequest(HttpMethod.Post, It.IsAny<string>(), HttpClientService.AcceptTypeEnum.JSON, It.IsAny<IEnumerable<KeyValuePair<string, string>>>(), It.IsAny<ByteArrayContent>()))
        //        .Returns(Task.FromResult(responseMock));

        //    var mobileSvc = new MobileService(new MockLogger(), mockHttpService.Object, sessionStateFullMock);

        //    mobileSvc.GenerateAccessToken("portaluser1", "password1", "callerInfo1");

        //    //make sure the session tokens get set
        //    Assert.AreEqual(accessTokenToTest, sessionStateFullMock.GetFromSession<string>(mobileSvc.AccessTokenSessionName));
        //    Assert.AreEqual(userTokenToTest, sessionStateFullMock.GetFromSession<string>(mobileSvc.PatContextSessionName));
        //}

        #endregion

        #region Methods

        /*  request.Content = new FormUrlEncodedContent(new[]
                  {
                      new KeyValuePair<string, string>("UserName", userID),
                      new KeyValuePair<string, string>("Password", password),
                      new KeyValuePair<string, string>("CallerInfo", callerInfo),
                  });


            request.Content = new StringContent(JsonConvert.SerializeObject(
                      new
                      {
                          PatID = successfulSwitchedAuthenticatedUser.SessionUser.PatId,
                          MRN = successfulSwitchedAuthenticatedUser.SessionUser.MRN
                      }), Encoding.UTF8, "application/json");

       */

        /// <summary>
        /// Converts an object into a string content which you send in a json request
        /// </summary>
        /// <typeparam name="T">Type of the param object to send</typeparam>
        /// <param name="objectParameters">parameter object to send</param>
        /// <returns>String content which you can pass into MakeRequest</returns>
        public static StringContent JsonDataToSendInRequest<T>(T objectParameters)
        {
            return new StringContent(JsonNetSerializer.Serialize(objectParameters), Encoding.UTF8, JsonContentType);
        }

        /// <summary>
        /// Make a web request
        /// </summary>
        /// <param name="RequestType">Request type to make. Get, Post, etc.</param>
        /// <param name="Url">Url to make the web request into</param>
        /// <param name="AcceptType">Accept typ</param>
        /// <param name="Headers">Additional headers to make to the request</param>
        /// <param name="BodyParameters">Parameters that get sent in the content. ** if using httpget then pass in null **</param>
        /// <returns>Built up task with the wrapped up call</returns>
        public Task<HttpResponseMessage> MakeRequestAsync(HttpMethod RequestType, string Url, AcceptTypeEnum AcceptType, IEnumerable<KeyValuePair<string, string>> Headers, ByteArrayContent BodyParameters)
        {
            //http client we are going to use. Don't wrap this in a dispose because the calling method will fail
            //if we have a client to use ...then use it. Otherwise just create a basic http client
            var HttpClientToUse = ClientToUse ?? new HttpClient();

            //add the accept type
            HttpClientToUse.DefaultRequestHeaders.Accept.Add(AcceptTypeToMediaQuality(AcceptType));

            //go create the initial request
            HttpRequestMessage RequestToMake = new HttpRequestMessage(RequestType, Url);

            //do we have any headers
            if (Headers.AnyWithNullCheck())
            {
                //loop through the headers and add them
                foreach (var HeaderToAdd in Headers)
                {
                    //add the specific header
                    RequestToMake.Headers.Add(HeaderToAdd.Key, HeaderToAdd.Value);
                }
            }

            //make sure a get doesn't pass in a body
            if (RequestType == HttpMethod.Get && BodyParameters != null)
            {
                throw new ArgumentOutOfRangeException(nameof(BodyParameters), "HttpGet Should Be Called With A Null Body As Bodys Are Not Permitted For An HttpGet.");
            }

            //set the request content (either JSON or FormUrlEncodedContent or null [if http get])
            RequestToMake.Content = BodyParameters;

            //go send the request and return the task
            return HttpClientToUse.SendAsync(RequestToMake);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Determine what media type header value we are setting from the accept type enum value we pass in
        /// </summary>
        /// <param name="acceptTypeId">Accept type id we want to use for this request</param>
        /// <returns>Converted MediaTypeWithQualityHeaderValue</returns>
        private static MediaTypeWithQualityHeaderValue AcceptTypeToMediaQuality(AcceptTypeEnum acceptTypeId)
        {
            //what accept type do we want
            if (acceptTypeId == AcceptTypeEnum.JSON)
            {
                return new MediaTypeWithQualityHeaderValue(JsonContentType);
            }

            throw new NotImplementedException();
        }

        #endregion

    }

}
