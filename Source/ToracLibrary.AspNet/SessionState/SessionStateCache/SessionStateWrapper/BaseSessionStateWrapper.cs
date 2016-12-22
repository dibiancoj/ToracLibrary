using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.SessionState.Cache.SessionStateImplementation
{

    /// <summary>
    /// Abstract class to wrap session state.
    /// </summary>
    public abstract class BaseSessionStateWrapper
    {

        /// <summary>
        /// Get an item from session state
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="SessionKey">Session key used to try to retrieve the file from</param>
        /// <returns>SessionStateCacheModel Of T. Null if not found</returns>
        internal abstract SessionStateCacheModel<T> GetFromCache<T>(string SessionKey) where T : class;

        /// <summary>
        /// Store an item in session state
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="SessionKey">Session key to store the object in session with</param>
        /// <param name="ObjectToStoreInSession">Object to insert into session. This is the SessionStateCacheModel Of T</param>
        internal abstract void SetSessionCache<T>(string SessionKey, SessionStateCacheModel<T> ObjectToStoreInSession) where T : class;

    }

}
