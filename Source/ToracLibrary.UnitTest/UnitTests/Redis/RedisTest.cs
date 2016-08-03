using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ToracLibrary.Redis;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for redis
    /// </summary>
    /// <remarks>not using the di container so we can exclude these tests without issues</remarks>
    public class RedisTest
    {

        #region Flag To Turn On Or Off Test

        /// <summary>
        /// Holds the reason for not running the redis tests. Flip this to a blank string to run all the tests. This way you don't have to modify each attribute
        /// </summary>
        private const string TurnOnOffFlag = "RedisServerNotLoaded";

        #endregion

        #region Constants

        /// <summary>
        /// Redis server ip address
        /// </summary>
        private const string RedisServerIpAddress = "Fedora"; //localhost

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

        static RedisTest()
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
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisLowLevelPingTest()
        {
            using (var Redis = BuildClient())
            {
                //make sure we get a pong back
                Assert.Equal("PONG", Redis.SendCommand<string>("ping"));
            }
        }

        /// <summary>
        /// Simple Set and Get For A String
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisLowLevelSetGetSimpleStringTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleStringTest);

                //value to test
                const string ValueToTest = "TestStringValue123";

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

                //get the test value
                var Response = Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}")));

                //make sure we get a pong back
                Assert.Equal(ValueToTest, Response);
            }
        }

        /// <summary>
        /// Simple Set and Get For A String With A Space
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisLowLevelSetGetSimpleStringWithSpaceTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleStringWithSpaceTest);

                //value to test
                const string ValueToTest = "Test StringValue 123";

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.SendCommand("Set", Key, ValueToTest));

                //get the test value
                var Response = Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}")));

                //make sure we get a pong back
                Assert.Equal(ValueToTest, Response);
            }
        }

        /// <summary>
        /// Simple Set and Get For A Number
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisLowLevelSetGetSimpleNumberTest()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisLowLevelSetGetSimpleNumberTest);

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.SendCommand<string>(string.Format($"Set {Key} {ValueToTest}")));

                //get the test value
                var Response = Convert.ToInt32(Redis.ByteArrayToString(Redis.SendCommand<byte[]>(string.Format($"Get {Key}"))));

                //make sure we get a pong back
                Assert.Equal(ValueToTest, Response);
            }
        }

        #endregion

        #region Higher Level - Abstracted Methods

        /// <summary>
        /// Simple Set and Get For A String
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelSetStringNoExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetStringNoExpiration) + " with space";

                //value to test
                const string ValueToTest = "HighLevel 123 abc";

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.StringSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.Equal(ValueToTest, Redis.StringGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For A String with an expiration
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelSetStringWithExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetStringWithExpiration) + " with space";

                //value to test
                const string ValueToTest = "HighLevel 123 abc";

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.StringSet(Key, ValueToTest, 1));

                //make sure we have a key
                Assert.True(Redis.KeyExists(Key));

                //spin wait for a second so we can wait for the key to expire
                SpinWaitForXSeconds(2);

                //make sure we don't have a key now
                Assert.False(Redis.KeyExists(Key));
            }
        }

        /// <summary>
        /// Try to get a value in the cache when the key is not found
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelGetStringWhenNotInCache()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelGetStringWhenNotInCache) + " with space";

                //make sure we get a null back
                Assert.Null(Redis.StringGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For An Int with no expiration
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelSetIntNoExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetIntNoExpiration) + " with space";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.IntSet(Key, ValueToTest));

                //make sure we get a pong back
                Assert.Equal(ValueToTest, Redis.IntGet(Key));
            }
        }

        /// <summary>
        /// Simple Set and Get For An Int with an expiration
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelSetIntWithExpiration()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelSetIntWithExpiration) + " with space";

                //value to test
                const int ValueToTest = 123;

                //go save the test value
                Assert.Equal(RedisClient.OKCommandResult, Redis.IntSet(Key, ValueToTest, 1));

                //make sure we have a key
                Assert.True(Redis.KeyExists(Key));

                //spin wait for a second so we can wait for the key to expire
                SpinWaitForXSeconds(2);

                //make sure we don't have a key now
                Assert.False(Redis.KeyExists(Key));
            }
        }

        /// <summary>
        /// Try to get a value in the cache when the key is not found
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelGetIntWhenNotInCache()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelGetIntWhenNotInCache) + " with space";

                //if we make it here we are ok
                Assert.Equal(new int?(), Redis.IntGet(Key));
            }
        }

        /// <summary>
        /// Increment a number
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelIncrementInt()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelIncrementInt) + " with space";

                //should be 1 because there is no key there yet
                Assert.Equal(1, Redis.IncrementInt(Key));

                //should be 2 because we already incremented it
                Assert.Equal(2, Redis.IncrementInt(Key));
            }
        }

        /// <summary>
        /// Decrement a number
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelDecrementInt()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelDecrementInt) + " with space";

                //should be -1 because there is no key there yet
                Assert.Equal(-1, Redis.DecrementInt(Key));

                //should be -1 because we already set it to 0
                Assert.Equal(-2, Redis.DecrementInt(Key));
            }
        }

        /// <summary>
        /// remove a cache item
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelRemoveCacheItem()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelRemoveCacheItem) + " with space";

                //add an item
                Redis.StringSet(Key, "Test");

                //go remove it. Should be 1 because we removed 1 item
                Assert.Equal(1, Redis.RemoveItemFromCache(Key));

                //should be 0 now because there are no more items to remove now
                Assert.Equal(0, Redis.RemoveItemFromCache(Key));
            }
        }

        /// <summary>
        /// key exists
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelKeyExistsItem()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelKeyExistsItem) + " with space";

                //key shouldn't exist
                Assert.False(Redis.KeyExists(Key));

                //add the item
                Redis.StringSet(Key, "Test");

                //key should exist now
                Assert.True(Redis.KeyExists(Key));

                //remove the key
                Redis.RemoveItemFromCache(Key);

                //it shouldn't exist now
                Assert.False(Redis.KeyExists(Key));
            }
        }

        /// <summary>
        /// List data type
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
        public void RedisHigherLevelListLevelDataType()
        {
            using (var Redis = BuildClient())
            {
                //key to use
                string Key = nameof(RedisHigherLevelListLevelDataType) + " with space";

                //formatter for value
                Func<int, string> FormatterForValue = i => i + " Item";

                //go build the 3 items we want to save
                var ItemsToInsert = new List<string>();

                //go add the items
                for (int i = 0; i < 3; i++)
                {
                    //add the item
                    ItemsToInsert.Add(FormatterForValue(i));
                }

                //go insert 1 item
                Assert.Equal(1, Redis.ListItemInsert(Key, ItemsToInsert[0], RedisClient.ListInsertType.InsertAtTopOfList));

                //add another item  to the top
                Assert.Equal(2, Redis.ListItemInsert(Key, ItemsToInsert[1], RedisClient.ListInsertType.InsertAtTopOfList));

                //add another item at the bottom
                Assert.Equal(3, Redis.ListItemInsert(Key, ItemsToInsert[2], RedisClient.ListInsertType.InsertAtEndOfList));

                //let's grab all the items in the list now (so we can test the select)
                var ItemsInList = Redis.ListItemSelectLazy(Key).ToArray();

                //make sure we have the correct items in the list with the correct order
                Assert.Equal(ItemsToInsert[1], ItemsInList[0]);
                Assert.Equal(ItemsToInsert[0], ItemsInList[1]);
                Assert.Equal(ItemsToInsert[2], ItemsInList[2]);
            }
        }

        #endregion

        #region Pipeline Test

        /// <summary>
        /// Pipeline test
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
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
                        Assert.Equal("PONG", ResponseForSingularCommand.ToString());
                    }
                    else
                    {
                        //go check the value
                        Assert.Equal(RedisClient.OKCommandResult, ResponseForSingularCommand.ToString());
                    }

                    //increase the tally
                    Index++;
                }

                //make sure we have the same amount of records
                Assert.Equal(NumberOfCalls + 1, Index);
            }
        }

        #endregion

        #region Pipeline Test

        /// <summary>
        /// Transaction test that succeeds
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
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
                    Assert.Null(Redis.IncrementInt(KeyToUseForIncrement));

                    //now set a cache key
                    Assert.Equal(RedisClient.QueuedCommandResult, Redis.StringSet(KeyToUseForSetKey, KeyValueToSet));

                    //result line
                    int Index = 0;

                    //grab the result and loop through them
                    foreach (var ResultLine in Transaction.CommitTheTransaction())
                    {
                        if (Index == 0)
                        {
                            //should be 1
                            Assert.Equal(IncrementShouldEqual, Convert.ToInt32(ResultLine));
                        }
                        else if (Index == 1)
                        {
                            //should be ok
                            Assert.Equal(RedisClient.OKCommandResult, ResultLine);
                        }

                        //increment 
                        Index++;
                    }

                    //make sure we only have 2 responses
                    Assert.Equal(2, Index);

                    //go test the results to make sure the records got updated
                    Assert.Equal(IncrementShouldEqual, Redis.IntGet(KeyToUseForIncrement));

                    //check the string now
                    Assert.Equal(KeyValueToSet, Redis.StringGet(KeyToUseForSetKey));
                }
            }
        }

        /// <summary>
        /// Transaction test the discard
        /// </summary>
        [Fact(Skip = TurnOnOffFlag)]
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
                    Assert.Null(Redis.IncrementInt(KeyToUseForDiscard));

                    //discard the transaction now
                    Assert.Equal(RedisClient.OKCommandResult, Transaction.DiscardTransaction());

                    //go test the value
                    Assert.Null(Redis.IntGet(KeyToUseForDiscard));
                }
            }
        }

        #endregion

        #endregion

    }

}