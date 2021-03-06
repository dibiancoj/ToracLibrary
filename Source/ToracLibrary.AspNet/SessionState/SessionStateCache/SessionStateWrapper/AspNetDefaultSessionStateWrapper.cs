﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ToracLibrary.AspNet.SessionState.Cache.SessionStateImplementation
{

    /// <summary>
    /// Default wrapper for session state
    /// </summary>
    public class AspNetDefaultSessionStateWrapper : ISessionStateWrapper
    {

        #region Constructor

        /// <summary>
        /// Constructor. This overload uses System.Web.HttpContext.Current.Session
        /// </summary>
        public AspNetDefaultSessionStateWrapper()
            : this(new HttpSessionStateWrapper(HttpContext.Current.Session))
        {
        }

        /// <summary>
        /// Constructor. This overload uses whatever session the user passes in
        /// </summary>
        /// <param name="SessionStateContainerToSet">Session state to use</param>
        public AspNetDefaultSessionStateWrapper(HttpSessionState SessionStateContainerToSet)
             : this(new HttpSessionStateWrapper(HttpContext.Current.Session))
        {
        }

        /// <summary>
        /// Constructor. This overload uses whatever session state is passed in
        /// </summary>
        /// <param name="SessionStateContainerToSet">Session state to use</param>
        public AspNetDefaultSessionStateWrapper(HttpSessionStateBase SessionStateContainerToSet)
        {
            //use whatever the user passed in
            SessionStateContainer = SessionStateContainerToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the session state container
        /// </summary>
        private HttpSessionStateBase SessionStateContainer { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get an item from session state
        /// </summary>
        /// <param name="SessionKey">Session key used to try to retrieve the file from</param>
        /// <returns>Object from session. Null if not found</returns>
        public object GetFromSession(string SessionKey)
        {
            //try to find the object if we have an object in session
            return SessionStateContainer[SessionKey];
        }

        /// <summary>
        /// Store an item in session state
        /// </summary>
        /// <param name="SessionKey">Session key to store the object in session with</param>
        /// <param name="CacheExpirationInSeconds">Cache expiration in seconds. If any</param>
        /// <param name="ObjectToStoreInSession">Object to insert into session</param>
        public void SetSessionObject(string SessionKey, object ObjectToStoreInSession)
        {
            //set session state
            SessionStateContainer[SessionKey] = ObjectToStoreInSession;
        }

        #endregion

    }

}
