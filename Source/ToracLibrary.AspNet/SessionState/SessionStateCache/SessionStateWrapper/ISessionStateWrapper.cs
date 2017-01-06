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
    public interface ISessionStateWrapper
    {

        /// <summary>
        /// Get an item from session state
        /// </summary>
        /// <param name="SessionKey">Session key used to try to retrieve the file from</param>
        /// <returns>Object from session</returns>
        object GetFromSession(string SessionKey);

        /// <summary>
        /// Store an item in session state
        /// </summary>
        /// <param name="SessionKey">Session key to store the object in session with</param>
        /// <param name="ObjectToStoreInSession">Object to insert into session</param>
        void SetSessionObject(string SessionKey, object ObjectToStoreInSession);

    }

}
