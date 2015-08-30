using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Class used to mock a HttpContextBase
    /// </summary>
    public class MockHttpContext : HttpContextBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>       
        /// <param name="PrincipalToSet">Principal</param>
        /// <param name="RequestToSet">Request to mock</param>
        /// <param name="ResponseToSet">Response to mock</param>
        /// <param name="SessionStateToSet">Mocked session state</param>
        public MockHttpContext(MockPrincipal PrincipalToSet,
                               MockHttpRequest RequestToSet,
                               MockHttpResponse ResponseToSet,
                               MockHttpSessionState SessionStateToSet)
        {
            Principal = PrincipalToSet;
            MockedSessionState = SessionStateToSet;
            MockedHttpResponse = ResponseToSet;
            MockedHttpRequest = RequestToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Principal
        /// </summary>
        private MockPrincipal Principal { get; }

        /// <summary>
        /// Mocked http request
        /// </summary>
        private HttpRequestBase MockedHttpRequest { get; }

        /// <summary>
        /// Mocked http response
        /// </summary>
        private MockHttpResponse MockedHttpResponse { get; }

        /// <summary>
        /// Mocked session state
        /// </summary>
        private MockHttpSessionState MockedSessionState { get; }

        #endregion

        #region Override HttpContextBase Methods

        /// <summary>
        /// Request
        /// </summary>
        public override HttpRequestBase Request
        {
            get
            {
                //return the mocked request
                return MockedHttpRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return MockedHttpResponse;
            }
        }

        /// <summary>
        /// User
        /// </summary>
        public override IPrincipal User
        {
            get
            {
                return Principal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Session State
        /// </summary>
        public override HttpSessionStateBase Session
        {
            get
            {
                return MockedSessionState;
            }
        }

        #endregion

    }

}
