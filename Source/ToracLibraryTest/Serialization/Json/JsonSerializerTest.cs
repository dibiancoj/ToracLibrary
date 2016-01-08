using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Json;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Serialization
{

    /// <summary>
    /// Unit test for Json serialization using Json.net
    /// </summary>
    [TestClass]
    public class JsonSerializerTest
    {

        #region Serialization 

        [TestCategory("Serializations.Json")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void JsonSerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //let's de-serialize it back
            var DeserializedStringObject = JsonNetSerializer.Deserialize<DummyObject>(SerializedJsonString);

            //let's test the data
            Assert.IsNotNull(DeserializedStringObject);

            //check the properties. check the id
            Assert.AreEqual(RecordToTest.Id, DeserializedStringObject.Id);

            //check the description
            Assert.AreEqual(RecordToTest.Description, DeserializedStringObject.Description);
        }

        #endregion

        #region JQuery Path Test

        private class TestJsonPath
        {
            public int Id { get; set; }
            public TestJsonPath Child { get; set; }
        }

        [TestCategory("Serializations.Json")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void JsonQueryPathTest1()
        {
            //id to test
            const int IdToTest = 5;
            const int ChildIdToTest = 10;

            //create the dummy record
            var RecordToTest = new TestJsonPath { Id = IdToTest, Child = new TestJsonPath { Id = ChildIdToTest } };

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //test the base id
            Assert.AreEqual(IdToTest, Convert.ToInt32(JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), "Id")));

            //go test the child id now
            Assert.AreEqual(ChildIdToTest, Convert.ToInt32(JsonNetSerializer.JsonValueFromPath(JObject.Parse(SerializedJsonString), "Child", "Id")));
        }

        [TestCategory("Serializations.Json")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void JsonQueryPathTest2()
        {
            //This will test the serialization back

            //id to test
            const int IdToTest = 5;
            const int ChildIdToTest = 10;

            //create the dummy record
            var RecordToTest = new TestJsonPath { Id = IdToTest, Child = new TestJsonPath { Id = ChildIdToTest } };

            //let's serialize it into a json string
            var SerializedJsonString = JsonNetSerializer.Serialize(RecordToTest);

            //go test the child id now
            Assert.AreEqual(ChildIdToTest, JsonNetSerializer.JsonValueFromPath<TestJsonPath>(JObject.Parse(SerializedJsonString), "Child").Id);
        }

        #endregion

    }

}
