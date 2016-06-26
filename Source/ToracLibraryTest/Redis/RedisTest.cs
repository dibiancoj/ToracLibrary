using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Redis;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for redis
    /// </summary>
    [TestClass]
    public class RedisTest
    {

        #region Constants

        /// <summary>
        /// Redis server ip address
        /// </summary>
        const string RedisServerIpAddress = "192.168.1.8";

        /// <summary>
        /// Response to a set or command
        /// </summary>
        const string OKCommandResult = "OK";

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

        #region Main Tests

        /* not using the di container so we can exclude these tests without issues */

        /// <summary>
        /// Simple Ping Command
        /// </summary>
        [TestCategory("Redis")]
        [TestMethod]
        public void RedisPingTest()
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
        [TestMethod]
        public void RedisSetGetSimpleStringTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                const string Key = "TestString";

                //value to test
                const string ValueToTest = "TestStringValue123";

                //go save the test value
                Assert.AreEqual(OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

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
        [TestMethod]
        public void RedisSetGetSimpleStringWithSpaceTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                const string Key = "TestString";

                //value to test
                const string ValueToTest = "Test StringValue 123";

                //go save the test value
                Assert.AreEqual(OKCommandResult, Redis.SendCommand("Set", new[] { Redis.StringToByteArray(Key), Redis.StringToByteArray(ValueToTest) }));

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
        [TestMethod]
        public void RedisSetGetSimpleNumberTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                const string Key = "TestNumber";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.AreEqual(OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

                //get the test value
                var Response = Convert.ToInt32(Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}"))));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Response);
            }
        }

        #endregion

    }

}