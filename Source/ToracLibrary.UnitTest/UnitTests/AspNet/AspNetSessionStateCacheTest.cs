using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ToracLibrary.AspNet.SessionState.Cache;
using ToracLibrary.AspNet.SessionState.Cache.SessionStateImplementation;
using ToracLibrary.Core.ExtensionMethods.IDictionaryExtensions;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNet
{

    /// <summary>
    /// Unit test for asp.net session state cache
    /// </summary>
    public class AspNetSessionStateCacheTest
    {

        #region Properties

        /// <summary>
        /// Holds a dummy blankobject
        /// </summary>
        private static DummyObject BlankDummyObject { get; } = new DummyObject(int.MinValue, string.Empty);

        /// <summary>
        /// First Item To Test
        /// </summary>
        private static KeyValuePair<string, DummyObject> FirstItem { get; } = new KeyValuePair<string, DummyObject>("FirstItem", new DummyObject(1, "Description:1"));

        /// <summary>
        /// Second Item To Test
        /// </summary>
        private static KeyValuePair<string, DummyObject> SecondItem { get; } = new KeyValuePair<string, DummyObject>("SecondItem", new DummyObject(2, "Description:2"));

        #endregion

        #region Framework

        /// <summary>
        /// Mock session state container
        /// </summary>
        private class MockBaseSessionStateWrapper : BaseSessionStateWrapper
        {
            private Dictionary<string, object> SessionStateContainer { get; } = new Dictionary<string, object>();

            internal override SessionStateCacheModel<T> GetFromCache<T>(string SessionKey)
            {
                return SessionStateContainer.TryGet(SessionKey) as SessionStateCacheModel<T>;
            }

            internal override void SetSessionCache<T>(string SessionKey, SessionStateCacheModel<T> ObjectToStoreInSession)
            {
                SessionStateContainer.TryAdd(SessionKey, ObjectToStoreInSession);
            }
        }

        #endregion

        #region Unit Tests

        [Fact]
        public void SessionStateCacheTest1()
        {
            //create the container
            var SessionStateCacheContainer = new SessionStateCache(new MockBaseSessionStateWrapper());

            //items to test
            var ItemsToTest = new List<KeyValuePair<string, DummyObject>>
            {
                FirstItem,
                SecondItem
            };

            //test the items
            for (int i = 0; i < ItemsToTest.Count; i++)
            {
                //item to use / test
                var TestItem = ItemsToTest[i];

                //this will put the item into session because it shouldn't be there
                var FirstInsertIntoCache = SessionStateCacheContainer.GetFromSessionCache(TestItem.Key, () => TestItem.Value);

                //2nd call should grab the value we just inserted. we want to make sure it pulls it from the cache...that is why we use string.empty so we know we are pulling from the cache
                var GetFromCache = SessionStateCacheContainer.GetFromSessionCache(TestItem.Key, () => BlankDummyObject);

                //check the id
                Assert.Equal(TestItem.Value.Id, GetFromCache.Id);

                //get the description
                Assert.Equal(TestItem.Value.Description, GetFromCache.Description);
            }
        }

        /// <summary>
        /// Going to test that the first item gets invalidated 
        /// </summary>
        [Fact]
        public void SessionStateCacheWithExpirationIsInvalidatedTest1()
        {
            //create the container
            var SessionStateCacheContainer = new SessionStateCache(new MockBaseSessionStateWrapper());

            //add the first item which should be invalidated
            var FirstItemFromCache = SessionStateCacheContainer.GetFromSessionCache(FirstItem.Key, () => FirstItem.Value, -120);

            //call the first key with the 2nd item...The second item should come back from the cache now
            var SecondItemFromCache = SessionStateCacheContainer.GetFromSessionCache(FirstItem.Key, () => SecondItem.Value, 120);

            //it should be the 2nd item since the first item got invalidated
            //check the id
            Assert.Equal(SecondItemFromCache.Id, SecondItemFromCache.Id);

            //get the description
            Assert.Equal(SecondItemFromCache.Description, SecondItemFromCache.Description);
        }

        /// <summary>
        /// Going to test that the first item sticks and the is not invalidated
        /// </summary>
        [Fact]
        public void SessionStateCacheWithExpirationNotInvalidatedTest1()
        {
            //create the container
            var SessionStateCacheContainer = new SessionStateCache(new MockBaseSessionStateWrapper());

            //add the first item which should be invalidated
            var FirstItemFromCache = SessionStateCacheContainer.GetFromSessionCache(FirstItem.Key, () => FirstItem.Value, 120);

            //call the first key with the 2nd item...The second item should come back from the cache now
            var SecondItemFromCache = SessionStateCacheContainer.GetFromSessionCache(FirstItem.Key, () => SecondItem.Value, -120);

            //it should be the 1st item since the first item hasn't expired yet
            //check the id
            Assert.Equal(FirstItemFromCache.Id, SecondItemFromCache.Id);

            //get the description
            Assert.Equal(FirstItemFromCache.Description, SecondItemFromCache.Description);
        }

        /// <summary>
        /// Test the CacheIsExpired method
        /// </summary>
        [Fact]
        public void SessionStateCacheModelIsExpiredTest1()
        {
            //values to test with. Can't pass dates in attributes. Item1 = Expiration Date. Item 2 = Expected Results
            var ValuesToTest = new Tuple<DateTime?, bool>[]
            {
                new Tuple<DateTime?, bool>(null, false),
                new Tuple<DateTime?, bool>(DateTime.MinValue, true),
                new Tuple<DateTime?, bool>(DateTime.Now.AddDays(-1), true ),
                new Tuple<DateTime?, bool>(DateTime.Now.AddDays(1), false)
            };

            //check the values now
            foreach (var TestValue in ValuesToTest)
            {
                //create the model to test with
                Assert.Equal(TestValue.Item2, new SessionStateCacheModel<string>(TestValue.Item1, "TestSessionStateCache").CacheIsExpired());
            }
        }

        #endregion

    }

}
