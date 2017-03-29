using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using ToracLibrary.Serialization.Json;
using ToracLibrary.Serialization.Json.ExtensionMethods;
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

        #region Unit Tests

        /// <summary>
        /// test converting to a string
        /// </summary>
        [Fact]
        public void JsonQueryPathTest1()
        {
            //create the dummy record
            var RecordToTest = TestJsonPath.CreateDummyRecord();

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //test the base id
            Assert.Equal(TestJsonPath.IdToTest, Convert.ToInt32(JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Id))));

            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, Convert.ToInt32(JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child), nameof(TestJsonPath.Id))));
        }

        /// <summary>
        /// test the conversion to an object overload
        /// </summary>
        [Fact]
        public void JsonQueryPathTest2()
        {
            //create the dummy record
            var RecordToTest = TestJsonPath.CreateDummyRecord();

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //go test the child id now
            Assert.Equal(TestJsonPath.ChildIdToTest, JsonNetSerializer.JsonValueFromPath<TestJsonPath>(JObject.Parse(SerializedJsonString), nameof(TestJsonPath.Child)).Id);
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

        #region ConvertToEnum

        /// <summary>
        /// enum to test JTokenConvertToEnum
        /// </summary>
        private enum TestEnumForJTokenConvertToEnum
        {
            Town = 1,
            State = 2,
            Country = 3
        }

        /// <summary>
        /// test converting a jtoken object to an enum
        /// </summary>
        [Fact]
        public void JTokenConvertToEnumTest1()
        {
            //test token for strings
            var TestTokenForStrings = JToken.Parse(@"{ ""Value"":""State""}")["Value"];

            //test token for numbers
            var TestTokenForNumbers = JToken.Parse(@"{ ""Value"":""3""}")["Value"];

            //shouldn't be able to parse string
            var TestTokenForNonParseableString = JToken.Parse(@"{ ""Value"":""CantParse""}")["Value"];

            //test with a number value
            Assert.Equal(TestEnumForJTokenConvertToEnum.State, TestTokenForStrings.ToEnum<TestEnumForJTokenConvertToEnum>().Value);

            //test with a string value
            Assert.Equal(TestEnumForJTokenConvertToEnum.Country, TestTokenForNumbers.ToEnum<TestEnumForJTokenConvertToEnum>().Value);

            //make sure we can't parse this string
            Assert.False(TestTokenForNonParseableString.ToEnum<TestEnumForJTokenConvertToEnum>().HasValue);
        }

        #endregion

        #endregion

    }

}
