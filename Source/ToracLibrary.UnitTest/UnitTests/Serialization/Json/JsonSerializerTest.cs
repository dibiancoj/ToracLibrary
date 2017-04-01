using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using ToracLibrary.Serialization.Json;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.Serialization
{

    /// <summary>
    /// Unit test for Json serialization using Json.net
    /// </summary>
    public class JsonSerializerTest
    {

        #region Serialization 

        [Fact]
        public void JsonSerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //let's de-serialize it back
            var DeserializedStringObject = JsonNetSerializer.Deserialize<DummyObject>(SerializedJsonString);

            //let's test the data
            Assert.NotNull(DeserializedStringObject);

            //check the properties. check the id
            Assert.Equal(RecordToTest.Id, DeserializedStringObject.Id);

            //check the description
            Assert.Equal(RecordToTest.Description, DeserializedStringObject.Description);
        }

        #endregion

        #region JQuery Path Test

        #region Frameworks

        private class TestJsonPath
        {

            #region Constants

            internal static readonly int IdToTest = 5;
            internal static readonly int ChildIdToTest = 10;

            #endregion

            #region Properties

            public int Id { get; set; }
            public TestJsonPath Child { get; set; }

            #endregion

            #region Methods

            internal static TestJsonPath CreateDummyRecord()
            {
                return new TestJsonPath
                {
                    Id = IdToTest,
                    Child = new TestJsonPath
                    {
                        Id = ChildIdToTest
                    }
                };
            }

            #endregion

        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Test string ot use for the json query path tests
        /// </summary>
        private static readonly string SerializedJsonString = JsonNetSerializer.Serialize(TestJsonPath.CreateDummyRecord());

        #endregion

        #region Unit Tests

        /// <summary>
        /// test converting to a string using the non generic version of the method
        /// </summary>
        [Fact]
        public void JsonQueryPathNonGenericTest1()
        {
            //test the base id
            Assert.Equal(TestJsonPath.IdToTest, JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Id)).Value<int>());

            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child), nameof(TestJsonPath.Id)).Value<int>());
        }

        /// <summary>
        /// test converting to an object using the non generic version of the method
        /// </summary>
        [Fact]
        public void JsonQueryPathNonGenericTest2()
        {
            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child)).ToObject<TestJsonPath>().Id);
        }

        /// <summary>
        /// test converting to an object where we can't find the object
        /// </summary>
        [Fact]
        public void JsonQueryPathNonGenericCantFindPropertyTest1()
        {
            //go test the child id now
            Assert.Null(JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), "PropertyNameDoesntExist"));
        }

        /// <summary>
        /// test the conversion to an object using the generic overload
        /// </summary>
        [Fact]
        public void JsonQueryPathGenericTest1()
        {
            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, JsonNetSerializer.JsonValueFromPath<TestJsonPath>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child)).Id);
        }

        /// <summary>
        /// test the conversion to a primitive type using the generic version
        /// </summary>
        [Fact]
        public void JsonQueryPathGenericTest2()
        {
            //test the base id
            Assert.Equal(TestJsonPath.IdToTest, JsonNetSerializer.JsonValueFromPath<int>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Id)));

            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, JsonNetSerializer.JsonValueFromPath<int>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child), nameof(TestJsonPath.Id)));
        }

        /// <summary>
        /// test the conversion to a nullable type that is not found. Should return default (T) which is null for a nullable int
        /// </summary>
        [Fact]
        public void JsonQueryPathGenericTestCantFindPropertyTest1()
        {
            //test the base id
            Assert.Null(JsonNetSerializer.JsonValueFromPath<int?>(JObject.Parse(SerializedJsonString), "PropertyNameDoesntExist"));

            //go test the child id now
            Assert.Null(JsonNetSerializer.JsonValueFromPath<int?>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child), "PropertyNameDoesntExist"));
        }

        /// <summary>
        /// test the conversion to a primitive type that is not found. Should return default (T). This test doesn't use a nullable type so we want the result to be default(int)
        /// </summary>
        [Fact]
        public void JsonQueryPathGenericTestCantFindPropertyTest2()
        {
            //test the base id
            Assert.Equal(default(int), JsonNetSerializer.JsonValueFromPath<int>(JObject.Parse(SerializedJsonString), "PropertyNameDoesntExist"));

            //go test the child id now
            Assert.Equal(default(int), JsonNetSerializer.JsonValueFromPath<int>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child), "PropertyNameDoesntExist"));
        }

        /// <summary>
        /// test the conversion to a class type that is not found. This is a class test that should result in a null value.
        /// </summary>
        [Fact]
        public void JsonQueryPathGenericTestCantFindPropertyForClassObjectTest2()
        {
            Assert.Null(JsonNetSerializer.JsonValueFromPath<TestJsonPath>(JObject.Parse(SerializedJsonString), "PropertyNameDoesntExist"));
        }

        #endregion

        #endregion

        #region JObjects

        #region Load From Stream

        /// <summary>
        /// test creating an jtoken from a stream
        /// </summary>
        [Fact]
        public void JObjectFromStreamTest1()
        {
            //create the stream to use
            using (var StreamToUse = @"{ ""Value"":""State""}".ToStream())
            {
                //go run the test method
                var LoadFromStreamResult = JsonNetSerializer.JObjectFromStream(StreamToUse);

                //validate we can grab the value object
                Assert.Equal("State", LoadFromStreamResult["Value"].Value<string>());
            }
        }

        #endregion

        #endregion

    }

}
