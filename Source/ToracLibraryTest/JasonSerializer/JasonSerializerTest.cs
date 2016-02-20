using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.JasonSerializer;
using ToracLibrary.Serialization.Json;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.Serialization
{

    /// <summary>
    /// Unit test jason json serializer
    /// </summary>
    [TestClass]
    public class JasonSerializerTest
    {

        #region Unit Test

        [TestCategory("Serializations.Jason.Json")]
        [TestCategory("Serializations.Jason")]
        [TestMethod]
        public void JasonSerializeSingleObjectTest1()
        {
            //go build the data to set test
            var SingleObjectToTest = DummyObject.CreateDummyRecord(); //TestObject.BuildObjectsLazy(1).First();

            //go render this in json.net...we will test my json serializer against the value in json.net
            var JsonNetResult = JsonNetSerializer.Serialize(SingleObjectToTest);

            //go render my object now
            var JasonResult = new JasonSerializerContainer().SerializeJson(SingleObjectToTest);

            //make sure they are equal
            Assert.AreEqual(JsonNetResult, JasonResult);
        }

        [TestCategory("Serializations.Jason.Json")]
        [TestCategory("Serializations.Jason")]
        [TestMethod]
        public void JasonSerializeArrayObjectTest1()
        {
            //go build the data to set test
            var SingleObjectToTest = DummyObject.CreateDummyListLazy(2).ToArray();

            //go render this in json.net...we will test my json serializer against the value in json.net
            var JsonNetResult = JsonNetSerializer.Serialize(SingleObjectToTest);

            //go render my object now
            var JasonResult = new JasonSerializerContainer().SerializeJson(SingleObjectToTest);

            //make sure they are equal
            Assert.AreEqual(JsonNetResult, JasonResult);
        }

        #endregion

    }

}
