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

        #region Framework

        private class JasonSerializerTestObject
        {

            #region Properties

            public int Id { get; set; }
            public string Txt { get; set; }
            public SubObject SubObject { get; set; }
            public SubObject NullSubObject { get; set; }
            public IEnumerable<SubObject> SubEnumerable { get; set; }
            public IEnumerable<SubObject> NullSubEnumerable { get; set; }

            #endregion

            #region Methods

            public static IEnumerable<JasonSerializerTestObject> BuildObjects(int HowMany)
            {
                for (int i = 0; i < HowMany; i++)
                {
                    yield return new JasonSerializerTestObject
                    {
                        Id = i,
                        Txt = "Test" + HowMany,
                        SubObject = new SubObject { SubObjectId = i },
                        NullSubObject = null,
                        SubEnumerable = BuildSubObject(2, i),
                        NullSubEnumerable = null
                    };
                }
            }

            private static IEnumerable<SubObject> BuildSubObject(int HowMany, int SubObjectIdStartValue)
            {
                //loop through the number of items we want
                for (int i = 0; i < HowMany; i++)
                {
                    //return the object...we will set the value based on the start value plus the number we are on
                    yield return new SubObject { SubObjectId = SubObjectIdStartValue + i };
                }
            }

            #endregion

        }

        private class SubObject
        {
            public int SubObjectId { get; set; }
        }

        #endregion

        #region Unit Test

        [TestCategory("Serializations.Jason.Json")]
        [TestCategory("Serializations.Jason")]
        [TestMethod]
        public void JasonSerializeSingleObjectTest1()
        {
            //go build the data to set test
            var SingleObjectToTest = JasonSerializerTestObject.BuildObjects(1).First();

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
            var SingleObjectToTest = JasonSerializerTestObject.BuildObjects(2).ToArray();

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
