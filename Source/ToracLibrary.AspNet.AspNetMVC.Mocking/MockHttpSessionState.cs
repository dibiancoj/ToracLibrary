using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ToracLibrary.AspNet.AspNetMVC.Mocking
{

    /// <summary>
    /// Helps build session state for mvc controller unit testing
    /// </summary>
    public class MockHttpSessionState : HttpSessionStateBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SessionItemsToSet">Session items to set in the controller</param>
        public MockHttpSessionState(SessionStateItemCollection SessionItemsToSet)
        {
            SessionItems = SessionItemsToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Session items to set
        /// </summary>
        private SessionStateItemCollection SessionItems { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// How many session items
        /// </summary>
        public override int Count
        {
            get { return SessionItems.Count; }
        }

        /// <summary>
        /// Keys
        /// </summary>
        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get { return SessionItems.Keys; }
        }

        /// <summary>
        /// indexer getter
        /// </summary>
        /// <param name="SessionName">Session Name</param>
        /// <returns>session object</returns>
        public override object this[string SessionName]
        {
            get { return SessionItems[SessionName]; }
            set { SessionItems[SessionName] = value; }
        }

        /// <summary>
        /// Get item by index
        /// </summary>
        /// <param name="Index">Index</param>
        /// <returns>session object</returns>
        public override object this[int Index]
        {
            get { return SessionItems[Index]; }
            set { SessionItems[Index] = value; }
        }

        /// <summary>
        /// Add a session key
        /// </summary>
        /// <param name="SessionName">Session Name</param>
        /// <param name="SessionValue">Session Value</param>
        public override void Add(string SessionName, object SessionValue)
        {
            SessionItems[SessionName] = SessionValue;
        }

        /// <summary>
        /// Remove a session item by name
        /// </summary>
        /// <param name="SessionName">Session Name</param>
        public override void Remove(string SessionName)
        {
            SessionItems.Remove(SessionName);
        }

        /// <summary>
        /// Get the enumerator to loop through the session object
        /// </summary>
        /// <returns>IEnumerator</returns>
        public override IEnumerator GetEnumerator()
        {
            return SessionItems.GetEnumerator();
        }

        #endregion

    }

}
