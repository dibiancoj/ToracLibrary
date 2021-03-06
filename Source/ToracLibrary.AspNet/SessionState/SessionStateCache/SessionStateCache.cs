﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNet.SessionState.Cache.SessionStateImplementation;

namespace ToracLibrary.AspNet.SessionState.Cache
{

    /// <summary>
    /// Use session state like a cache
    /// </summary>
    public class SessionStateCache
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SessionStateWrapperContainerToSet">Session State Container. For any web app Please Pass In new AspNetDefaultSessionStateWrapper()</param>
        public SessionStateCache(ISessionStateWrapper SessionStateWrapperContainerToSet)
        {
            SessionStateWrapperContainer = SessionStateWrapperContainerToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Session State Container
        /// </summary>
        private ISessionStateWrapper SessionStateWrapperContainer { get; }

        #endregion

        #region Public Methods

        ///<summary>
        /// Get the data from session or reload it from the data source with no cache expiration.
        /// </summary>
        /// <typeparam name="T">Type of the record stored in cache</typeparam>
        /// <param name="SessionKey">session key to retrieve it by</param>
        /// <param name="ReloadDataFromSource">if not found in session, how do we reload it</param>
        /// <returns>Item from either session or the data data source</returns>
        public T GetFromSessionCache<T>(string SessionKey, Func<T> ReloadDataFromSource) where T : class
        {
            //use the overload
            return GetFromSessionCache<T>(SessionKey, ReloadDataFromSource, null);
        }

        /// <summary>
        /// Get the data from session or reload it from the data source with a cache expiration
        /// </summary>
        /// <typeparam name="T">Type of the record stored in cache</typeparam>
        /// <param name="SessionKey">session key to retrieve it by</param>
        /// <param name="ReloadDataFromSource">if not found in session, how do we reload it</param>
        /// <param name="CacheExpirationInSeconds">How long before the cache expires in seconds</param>
        /// <returns>Item from either session or the data data source</returns>
        public T GetFromSessionCache<T>(string SessionKey, Func<T> ReloadDataFromSource, int? CacheExpirationInSeconds) where T : class
        {
            //try to get it from session
            var TryToGetFromSession = SessionStateWrapperContainer.GetFromSession(SessionKey) as SessionStateCacheModel<T>;

            //do we have it in session? Or is it expired?
            if (TryToGetFromSession == null || TryToGetFromSession.CacheIsExpired())
            {
                //don't have it in session. grab from source
                TryToGetFromSession =  new SessionStateCacheModel<T>(CacheExpirationInSeconds, ReloadDataFromSource());

                //stick it in session
                SessionStateWrapperContainer.SetSessionObject(SessionKey, TryToGetFromSession);
            }

            //return just whatever the object they are looking for
            return TryToGetFromSession.CachedObject;
        }

        #endregion

    }

}
