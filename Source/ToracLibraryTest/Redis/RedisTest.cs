using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        const string RedisServerIpAddress = "Fedora";

        #endregion

        #region Framework Stuff

        /// <summary>
        /// Builds the Redis client
        /// </summary>
        /// <returns>RediClient configured for test</returns>
        private static RedisClient BuildClient()
        {
            return new RedisClient(RedisServerIpAddress);
        }

        /// <summary>
        /// Spin for x amount of seconds
        /// </summary>
        /// <param name="SecondsToSpin">Seconds to spin</param>
        private static void SpinWaitForXSeconds(int SecondsToSpin)
        {
            //grab the time now
            var StartNow = DateTime.Now;

            //spin until we are ready
            SpinWait.SpinUntil(() => DateTime.Now > StartNow.AddSeconds(SecondsToSpin));
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
        public void RedisHigherLevelSetStringNoExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetStringNoExpiration) + " with space";

                //value to test
                const string ValueToTest = "HighLevel 123 abc";

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.StringSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Redis.StringGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For A String with an expiration
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelSetStringWithExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetStringWithExpiration) + " with space";

                //value to test
                const string ValueToTest = "HighLevel 123 abc";

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.StringSet(Key, ValueToTest, 1));

                //make sure we have a key
                Assert.IsTrue(Redis.KeyExists(Key));

                //spin wait for a second so we can wait for the key to expire
                SpinWaitForXSeconds(2);

                //make sure we don't have a key now
                Assert.IsFalse(Redis.KeyExists(Key));
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
        /// Simple Set and Get For An Int with no expiration
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelSetIntNoExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetIntNoExpiration) + " with space";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.IntSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.AreEqual(ValueToTest, Redis.IntGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For An Int with an expiration
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.HighLevel")]
        [TestMethod]
        public void RedisHigherLevelSetIntWithExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetIntWithExpiration) + " with space";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.AreEqual(RedisClient.OKCommandResult, Redis.IntSet(Key, ValueToTest, 1));

                //make sure we have a key
                Assert.IsTrue(Redis.KeyExists(Key));

                //spin wait for a second so we can wait for the key to expire
                SpinWaitForXSeconds(2);

                //make sure we don't have a key now
                Assert.IsFalse(Redis.KeyExists(Key));
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

        #region Pipeline Test

        /// <summary>
        /// Pipeline test
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.Pipeline")]
        [TestMethod]
        public void RedisPipelineTest()
        {
            using (var Redis = BuildClient())
            {
                //how many calls
                const int NumberOfCalls = 10;

                //key formatter
                Func<int, string> BuildKeyFormatter = (i) => string.Format($"PipeLine{i}Key");

                //value formatter
                Func<int, string> BuildValueFormatter = (i) => string.Format($"PipeLine{i}Value");

                //create the pipeline
                var PipelineToRun = Redis.CreatePipeline();

                //we are going to add a pong to make sure we get the correct calls
                PipelineToRun.AddCommandToRun("PING");

                //go make multiple ping commands
                for (int i = 0; i < NumberOfCalls; i++)
                {
                    //go add some records
                    PipelineToRun.AddCommandToRun("SET", BuildKeyFormatter(i), BuildValueFormatter(i));
                }

                //index
                var Index = 0;

                //go save and commit the pipeline
                foreach (var ResponseForSingularCommand in PipelineToRun.SavePipeLine())
                {
                    //first call should be a pong
                    if (Index == 0)
                    {
                        //should be pong
                        Assert.AreEqual("PONG", ResponseForSingularCommand.ToString());
                    }
                    else
                    {
                        //go check the value
                        Assert.AreEqual(RedisClient.OKCommandResult, ResponseForSingularCommand.ToString());
                    }

                    //increase the tally
                    Index++;
                }

                //make sure we have the same amount of records
                Assert.AreEqual(NumberOfCalls + 1, Index);
            }
        }

        #endregion

        #region Pipeline Test

        /// <summary>
        /// Transaction test that succeeds
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.Transaction")]
        [TestMethod]
        public void RedisTransactionCommitTest()
        {
            using (var Redis = BuildClient())
            {
                //start the transaction
                using (var Transaction = Redis.CreateTransaction())
                {

                    //key to use for increment
                    const string KeyToUseForIncrement = "Key." + nameof(RedisTransactionCommitTest);

                    //what the increment should be after all said and done
                    const int IncrementShouldEqual = 1;

                    //key to use for set key
                    const string KeyToUseForSetKey = "SetKey." + nameof(RedisTransactionCommitTest);

                    //value to set the key
                    const string KeyValueToSet = "Test123";

                    //add an increment on this key
                    Assert.IsNull(Redis.IncrementInt(KeyToUseForIncrement));

                    //now set a cache key
                    Assert.AreEqual(RedisClient.QueuedCommandResult, Redis.StringSet(KeyToUseForSetKey, KeyValueToSet));

                    //result line
                    int Index = 0;

                    //grab the result and loop through them
                    foreach (var ResultLine in Transaction.CommitTheTransaction())
                    {
                        if (Index == 0)
                        {
                            //should be 1
                            Assert.AreEqual(IncrementShouldEqual, Convert.ToInt32(ResultLine));
                        }
                        else if (Index == 1)
                        {
                            //should be ok
                            Assert.AreEqual(RedisClient.OKCommandResult, ResultLine);
                        }

                        //increment 
                        Index++;
                    }

                    //make sure we only have 2 responses
                    Assert.AreEqual(2, Index);

                    //go test the results to make sure the records got updated
                    Assert.AreEqual(IncrementShouldEqual, Redis.IntGet(KeyToUseForIncrement));

                    //check the string now
                    Assert.AreEqual(KeyValueToSet, Redis.StringGet(KeyToUseForSetKey));
                }
            }
        }

        /// <summary>
        /// Transaction test the discard
        /// </summary>
        [TestCategory("Redis")]
        [TestCategory("Redis.Transaction")]
        [TestMethod]
        public void RedisTransactionDiscardTest()
        {
            using (var Redis = BuildClient())
            {
                //start the transaction
                using (var Transaction = Redis.CreateTransaction())
                {
                    //key to use for increment
                    const string KeyToUseForDiscard = "Key." + nameof(RedisTransactionDiscardTest);

                    //add an increment on this key
                    Assert.IsNull(Redis.IncrementInt(KeyToUseForDiscard));

                    //discard the transaction now
                    Assert.AreEqual(RedisClient.OKCommandResult, Transaction.DiscardTransaction());

                    //go test the value
                    Assert.IsNull(Redis.IntGet(KeyToUseForDiscard));
                }
            }
        }

        #endregion

        #endregion

    }

}