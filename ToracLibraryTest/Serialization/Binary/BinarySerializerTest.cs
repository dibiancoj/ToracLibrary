using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Binary;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Serialization
{

    /// <summary>
    /// Unit test binary serialization
    /// </summary>
    [TestClass]
    public class BinarySerializerTest
    {

        /// <summary>
        /// Test to find a specific exception from an error stack trace
        /// </summary>
        [TestCategory("Serializations.Binary")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void BinarySerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it
            var SerializedBytes = BinarySerializer.SerializeObject(RecordToTest);

            //let's de-serialize it back
            var DeserializedObject = BinarySerializer.DeserializeObject<DummyObject>(SerializedBytes);

            //let's test the data
            Assert.IsNotNull(DeserializedObject);

            //check the properties
            Assert.AreEqual(RecordToTest.Id, DeserializedObject.Id);

            Assert.AreEqual(RecordToTest.Description, DeserializedObject.Description);
        }

    }

}
