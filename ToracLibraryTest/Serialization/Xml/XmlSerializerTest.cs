using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.Xml;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Serialization
{

    /// <summary>
    /// Unit test xml serialization
    /// </summary>
    [TestClass]
    public class XmlSerializerTest
    {

        #region Framework

        /// <summary>
        /// Can't use dummy object because xml serializer needs a parameterless constructor
        /// </summary>
        [Serializable]
        public class XmlSerializerTestClass
        {

            #region Properties

            public int Id { get; set; }
            public string Description { get; set; }

            #endregion

            #region Methods

            public static XmlSerializerTestClass CreateDummyRecord()
            {
                return new XmlSerializerTestClass { Id = 5, Description = "Test - 5" };
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Test to find a specific exception from an error stack trace
        /// </summary>
        [TestCategory("Serializations.Xml")]
        [TestCategory("Serializations")]
        [TestMethod]
        public void XmlSerializationTest1()
        {
            //create the dummy record
            var RecordToTest = XmlSerializerTestClass.CreateDummyRecord();

            //let's serialize it
            var SerializedString = XMLSerialization.SerializeObject(RecordToTest);

            //lets serialize it to an xelement
            var SerializedXElement = XMLSerialization.SerializeObjectToXElement(RecordToTest);

            //let's de-serialize it back
            var DeserializedStringObject = XMLSerialization.DeserializeObject<XmlSerializerTestClass>(SerializedString);

            //grab the xelement and deserialize it
            var DeserializedXElementObject = XMLSerialization.DeserializeObject<XmlSerializerTestClass>(SerializedXElement);

            //let's test the data
            Assert.IsNotNull(DeserializedStringObject);
            Assert.IsNotNull(DeserializedXElementObject);

            //check the properties. check the id
            Assert.AreEqual(RecordToTest.Id, DeserializedStringObject.Id);
            Assert.AreEqual(RecordToTest.Id, DeserializedXElementObject.Id);

            //check the description
            Assert.AreEqual(RecordToTest.Description, DeserializedStringObject.Description);
            Assert.AreEqual(RecordToTest.Description, DeserializedXElementObject.Description);
        }

    }

}
