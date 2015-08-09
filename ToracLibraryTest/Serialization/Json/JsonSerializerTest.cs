using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    }

}
