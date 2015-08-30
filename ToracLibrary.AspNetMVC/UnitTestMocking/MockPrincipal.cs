using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Class used to mock a principal user
    /// </summary>
    public class MockPrincipal : IPrincipal
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IdentityToSet">Identity To Use</param>
        /// <param name="RolesToSet">Roles to use</param>
        public MockPrincipal(IIdentity IdentityToSet, IEnumerable<string> RolesToSet)
        {
            UserIdentity = IdentityToSet;
            Roles = RolesToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Identity to store
        /// </summary>
        private IIdentity UserIdentity { get; set; }

        /// <summary>
        /// Roles to store
        /// </summary>
        private IEnumerable<string> Roles { get; set; }

        #endregion

        #region Implementation - IPrincipal Items

        /// <summary>
        /// Itdentity return item
        /// </summary>
        public IIdentity Identity
        {
            get { return UserIdentity; }
        }

        /// <summary>
        /// check if it a role
        /// </summary>
        /// <param name="RolesToCheck">Roles to check</param>
        /// <returns></returns>
        public bool IsInRole(string RolesToCheck)
        {
            //do we have roles
            return Roles.AnyWithNullCheck() && Roles.Contains(RolesToCheck);
        }

        #endregion

    }

}
