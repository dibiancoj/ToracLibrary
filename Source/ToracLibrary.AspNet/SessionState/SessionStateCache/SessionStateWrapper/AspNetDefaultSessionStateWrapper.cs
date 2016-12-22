using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace ToracLibrary.AspNet.SessionState.Cache.SessionStateImplementation
{

    /// <summary>
    /// Default wrapper for session state
    /// </summary>
    public class AspNetDefaultSessionStateWrapper : BaseSessionStateWrapper
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AspNetDefaultSessionStateWrapper()
        {
            //not going to pass this in for now. Set the session state container
            SessionStateContainer = System.Web.HttpContext.Current.Session;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the session state container
        /// </summary>
        public HttpSessionState SessionStateContainer { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get an item from session state
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="SessionKey">Session key used to try to retrieve the file from</param>
        /// <returns>T. Null if not found</returns>
        internal override SessionStateCacheModel<T> GetFromCache<T>(string SessionKey)
        {
            //try to find the object if we have an object in session
            return (SessionStateContainer[SessionKey] as SessionStateCacheModel<T>);
        }

        /// <summary>
        /// Store an item in session state
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="SessionKey">Session key to store the object in session with</param>
        /// <param name="CacheExpirationInSeconds">Cache expiration in seconds. If any</param>
        /// <param name="ObjectToStoreInSession">Object to insert into session</param>
        internal override void SetSessionCache<T>(string SessionKey, SessionStateCacheModel<T> ObjectToStoreInSession)
        {
            //set session state
            SessionStateContainer[SessionKey] = ObjectToStoreInSession;
        }

        #endregion

    }

}
