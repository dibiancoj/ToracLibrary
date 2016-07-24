using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Binary;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibraryTest.UnitsTest.Serialization
{

    /// <summary>
    /// Unit test binary serialization
    /// </summary>
    public class BinarySerializerTest
    {

        [Fact]
        public void BinarySerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it
            var SerializedBytes = BinarySerialization.SerializeObject(RecordToTest);

            //let's de-serialize it back
            var DeserializedObject = BinarySerialization.DeserializeObject<DummyObject>(SerializedBytes);

            //let's test the data
            Assert.NotNull(DeserializedObject);

            //check the properties. check the id
            Assert.Equal(RecordToTest.Id, DeserializedObject.Id);

            //check the description
            Assert.Equal(RecordToTest.Description, DeserializedObject.Description);
        }

        [Fact]
        public void CompressedBinarySerializationTest1()
        {
            //create the dummy record
            var RecordToTest = DummyObject.CreateDummyRecord();

            //let's serialize it
            var SerializedBytes = BinarySerialization.SerializeAndCompress(RecordToTest);

            //let's de-serialize it back
            var DeserializedObject = BinarySerialization.DecompressAndDeserialize<DummyObject>(SerializedBytes);

            //let's test the data
            Assert.NotNull(DeserializedObject);

            //check the properties. check the id
            Assert.Equal(RecordToTest.Id, DeserializedObject.Id);

            //check the description
            Assert.Equal(RecordToTest.Description, DeserializedObject.Description);
        }

    }

}
