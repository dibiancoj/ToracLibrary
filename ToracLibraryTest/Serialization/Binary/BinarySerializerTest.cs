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
            var SerializedBytes = BinarySerialization.SerializeObject(RecordToTest);

            //let's de-serialize it back
            var DeserializedObject = BinarySerialization.DeserializeObject<DummyObject>(SerializedBytes);

            //let's test the data
            Assert.IsNotNull(DeserializedObject);

            //check the properties. check the id
            Assert.AreEqual(RecordToTest.Id, DeserializedObject.Id);

            //check the description
            Assert.AreEqual(RecordToTest.Description, DeserializedObject.Description);
        }

        /// <summary>
        /// Test to find a specific exception from an error stack trace
        /// </summary>
        [TestCategory("Serializations.Binary")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void CompressedBinarySerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it
            var SerializedBytes = BinarySerialization.SerializeAndCompress(RecordToTest);

            //let's de-serialize it back
            var DeserializedObject = BinarySerialization.DecompressAndDeserialize<DummyObject>(SerializedBytes);

            //let's test the data
            Assert.IsNotNull(DeserializedObject);

            //check the properties. check the id
            Assert.AreEqual(RecordToTest.Id, DeserializedObject.Id);

            //check the description
            Assert.AreEqual(RecordToTest.Description, DeserializedObject.Description);
        }

    }

}
