using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Redis;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for redis
    /// </summary>
    /// <remarks>not using the di container so we can exclude these tests without issues</remarks>
    [TestClass]
    public class RedisTest
    {

        #region Constants

        /// <summary>
        /// Redis server ip address
        /// </summary>
        const string RedisServerIpAddress = "192.168.1.8";

        #endregion

        #region Build Redis Client

        /// <summary>
        /// Builds the Redis client
        /// </summary>
        /// <returns>RediClient configured for test</returns>
        private static RedisClient BuildClient()
        {
            return new RedisClient(RedisServerIpAddress);
        }

        #endregion

        #region Test Init (Class Init)

        [ClassInitialize()]
        public static void RedisTestInit(TestContext Context)
        {
            using (var Redis = BuildClient())
            {
                //remove everything
                Redis.SendCommand("FLUSHALL");
            }
        }

        #endregion

        #region Main Tests

        #region Low Level Tests

        /// <summary>
        /// Simple Ping Command
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.LowLevel")]
        [TestMethod]
        public void RedisLowLevelPingTest()
        {
            using (var Redis = BuildClient())
            {
                //make sure we get a pong back
                Assert.AreEqual("PONG", Redis.SendCommand<string>("ping"));
            }
        }

        /// <summary>
        /// Simple Set and Get For A String
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.LowLevel")]
        [TestMethod]
        public void RedisLowLevelSetGetSimpleStringTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleStringTest);

                //value to test
                const string ValueToTest = "TestStringValue123";

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

                //get the test value
                var Response = Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}")));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Response);
            }
        }

        /// <summary>
        /// Simple Set and Get For A String With A Space
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.LowLevel")]
        [TestMethod]
        public void RedisLowLevelSetGetSimpleStringWithSpaceTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleStringWithSpaceTest);

                //value to test
                const string ValueToTest = "Test StringValue 123";

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.SendCommand("Set", Key, ValueToTest));

                //get the test value
                var Response = Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}")));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Response);
            }
        }

        /// <summary>
        /// Simple Set and Get For A Number
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.LowLevel")]
        [TestMethod]
        public void RedisLowLevelSetGetSimpleNumberTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleNumberTest);

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

                //get the test value
                var Response = Convert.ToInt32(Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}"))));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Response);
            }
        }

        #endregion

        #region Higher Level - Abstracted Methods

        /// <summary>
        /// Simple Set and Get For A String
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelSetString()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetString) + " with space";

                //value to test
                const string ValueToTest = "HighLevel 123 abc";

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.StringSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Redis.StringGet(Key));
            }
        }

        /// <summary>
        /// Try to get a value in the cache when the key is not found
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelGetStringWhenNotInCache()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelGetStringWhenNotInCache) + " with space";

                //make sure we get a null back
                Assert.IsNull(Redis.StringGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For An Int
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelSetInt()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetInt) + " with space";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.IntSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Redis.IntGet(Key));
            }
        }

        /// <summary>
        /// Try to get a value in the cache when the key is not found
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelGetIntWhenNotInCache()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelGetIntWhenNotInCache) + " with space";

                //if we make it here we are ok
                Assert.AreEqual(new int?(), Redis.IntGet(Key));
            }
        }

        /// <summary>
        /// Increment a number
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelIncrementInt()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelIncrementInt) + " with space";

                //should be 1 because there is no key there yet
                Assert.AreEqual(1, Redis.IncrementInt(Key));

                //should be 2 because we already incremented it
                Assert.AreEqual(2, Redis.IncrementInt(Key));
            }
        }

        /// <summary>
        /// Decrement a number
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelDecrementInt()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelDecrementInt) + " with space";

                //should be -1 because there is no key there yet
                Assert.AreEqual(-1, Redis.DecrementInt(Key));

                //should be -1 because we already set it to 0
                Assert.AreEqual(-2, Redis.DecrementInt(Key));
            }
        }

        /// <summary>
        /// remove a cache item
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelRemoveCacheItem()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelRemoveCacheItem) + " with space";

                //add an item
                Redis.StringSet(Key, "Test");

                //go remove it. Should be 1 because we removed 1 item
                Assert.AreEqual(1, Redis.RemoveItemFromCache(Key));

                //should be 0 now because there are no more items to remove now
                Assert.AreEqual(0, Redis.RemoveItemFromCache(Key));
            }
        }

        /// <summary>
        /// key exists
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelKeyExistsItem()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelKeyExistsItem) + " with space";

                //key shouldn't exist
                Assert.IsFalse(Redis.KeyExists(Key));

                //add the item
                Redis.StringSet(Key, "Test");

                //key should exist now
                Assert.IsTrue(Redis.KeyExists(Key));

                //remove the key
                Redis.RemoveItemFromCache(Key);

                //it shouldn't exist now
                Assert.IsFalse(Redis.KeyExists(Key));
            }
        }

        #endregion

        #endregion

    }

}