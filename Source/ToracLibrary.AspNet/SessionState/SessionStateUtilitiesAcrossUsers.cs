using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using ToracLibrary.Core.ToracAttributes;

namespace ToracLibrary.AspNet.SessionState
{

    /// <summary>
    /// Contains Utilities Dealing With Session State Across Users (Basically Hacking In Proc Session State)
    /// </summary>
    public static class SessionStateUtilitiesAcrossUsers
    {

        #region Public Static Methods

        /// <summary>
        /// Gets all the session variables ACROSS ALL USERS using reflection. ie.
        /// </summary>
        /// <returns>List Of RetrieveSessionDataAcrossUsersResult. This is lazy loaded</returns>
        [MethodIsNotTestable("Can't Mock Session State Across Users. This Method Hacks Into Session As It Is, Trying To Test It Is Not Really Testing It")]
        public static IEnumerable<RetrieveSessionDataAcrossUsersResult> RetrieveSessionDataAcrossUsersLazy()
        {
            //grab the session data and loop through it 
            foreach (var AUsersSessionData in GetSessionDataHelper())
            {
                //we have a user's session...lets loop through each user's session now
                foreach (string SessionKey in AUsersSessionData)
                {
                    //let's go yield return each of the guys
                    yield return new RetrieveSessionDataAcrossUsersResult(SessionKey, AUsersSessionData[SessionKey]);
                }
            }
        }

        /// <summary>
        /// Kill A Session Across Users. First Session It Finds With This Name, It Will Kill It, And Return Back Out Of The Method.
        /// If the session is unique across users then no problem. If each user has the same session name, then keep calling this method in a loop would work.
        /// Maybe we could modify this method to change from foreach to for so we dont have any problems modifing the collection while looping through.
        /// Don't need to right now, so not going to complicate it, because I'm worried about not locking the data and collissions.
        /// </summary>
        /// <param name="SessionNameToKill">Session Key To Kill</param>
        /// <returns>True if we found the session and its deleted. False if we didn't find any session with that name</returns>
        /// <remarks>Going to keep this simple since this whole session across user is shady. Not going to try to kill multiple sessions or what not</remarks>
        [MethodIsNotTestable("Can't Mock Session State Across Users. This Method Hacks Into Session As It Is, Trying To Test It Is Not Really Testing It")]
        public static bool KillSession(string SessionNameToKill)
        {
            //we have some sort of session item, let's loop through the keys now
            foreach (var AUsersSessionData in GetSessionDataHelper())
            {
                //we have some sort of session item, let's loop through the keys now
                foreach (string SessionKey in AUsersSessionData)
                {
                    //we want to kill some session
                    if (string.Equals(SessionKey, SessionNameToKill, StringComparison.OrdinalIgnoreCase))
                    {
                        //set the session object to null (which kills the actual object in the collection)
                        AUsersSessionData[SessionKey] = null;

                        //we found the session, so return the positive result
                        return true;
                    }
                }
            }

            //never found the session, so return false
            return false;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets all the session variables ACROSS ALL USERS using reflection. ie. (Helper Method
        /// </summary>
        /// <returns>a list of session items. A session state item is broken out by user. So multiple users on the site will each have their own collection (which has multiple session items inside it).</returns>
        private static IEnumerable<SessionStateItemCollection> GetSessionDataHelper()
        {
            //holds the list to be returned
            var SessionStateItemsToReturn = new List<SessionStateItemCollection>();

            //first let's grab the http run time Cache Internal Object
            object RuntimeCacheObject = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);

            //let's now go grab the caches sub object (which is a list of caches)
            object[] CacheObjects = (object[])RuntimeCacheObject.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(RuntimeCacheObject);

            //let's loop through the objects now
            foreach (object CacheItem in CacheObjects)
            {
                //now let's grab all the entries for this cache item now
                Hashtable CacheEntries = (Hashtable)CacheItem.GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CacheItem);

                //let's go loop through the cache entries now
                foreach (DictionaryEntry CacheEntry in CacheEntries)
                {
                    //let's grab the value of the cache item
                    object CacheEntryValue = CacheEntry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CacheEntry.Value, null);

                    //is this guy the in proc session state
                    if (string.Equals(CacheEntryValue.GetType().ToString(), "System.Web.SessionState.InProcSessionState", StringComparison.OrdinalIgnoreCase))
                    {
                        //this is a session object...let's go grab the items in this key
                        var SessionObjects = (SessionStateItemCollection)CacheEntryValue.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CacheEntryValue);

                        //make sure we have something before we try to loop through the keys
                        if (SessionObjects != null)
                        {
                            //this is a session object...let's go grab the items in this key
                            SessionStateItemsToReturn.Add((SessionStateItemCollection)CacheEntryValue.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(CacheEntryValue));
                        }
                    }
                }
            }

            //return the list now
            return SessionStateItemsToReturn;
        }

        #endregion

    }

}
