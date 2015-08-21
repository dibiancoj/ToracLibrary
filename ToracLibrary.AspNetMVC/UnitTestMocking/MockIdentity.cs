using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Helps build identity state for mvc controller unit testing
    /// </summary>
    public class MockIdentity : IIdentity
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="UserName">User Name To Set</param>
        public MockIdentity(string UserName)
        {
            IdentityName = UserName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Identity name to use
        /// </summary>
        private string IdentityName { get; }

        #endregion

        #region Interface IIdentity Methods

        /// <summary>
        /// AuthenticationType
        /// </summary>
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// IsAuthenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(IdentityName); }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get { return IdentityName; }
        }

        #endregion

    }

}
