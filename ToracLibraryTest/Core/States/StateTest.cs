using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.Core.States;
using ToracLibrary.Core.Xml.Schema;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test to test the state listing
    /// </summary>
    [TestClass]
    public class StateTest
    {

        #region United States Tests

        /// <summary>
        /// US States - Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        /// </summary>
        [TestCategory("Core.States")]
        [TestCategory("Core.Xml.Schema")]
        [TestCategory("Core")]
        [TestMethod]
        public void UnitedStatesValidateXmlAgainstSchemaTest1()
        {
            try
            {
                //go run the validation
                Assert.IsTrue(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(State.UnitedStatesXmlResource(), State.UnitedStatesXmlSchemaResource()));
            }
            catch (Exception)
            {
                //failed, fail the unit test now
                Assert.Fail("State Xml Doesn't Meet Schema Validation");
            }
        }

        /// <summary>
        /// Test builds the united states listing and verifies everything is correct and running
        /// </summary>
        [TestCategory("Core.States")]
        [TestCategory("Core")]
        [TestMethod]
        public void UnitedStatesSelectTest1()
        {
            //grab the us state listing
            var StateListing = State.UnitedStatesStateListing();

            //grab how many states we should have
            Assert.AreEqual(52, StateListing.Count);

            //check random states
            Assert.AreEqual("Alabama", StateListing["AL"]);
            Assert.AreEqual("Wyoming", StateListing["WY"]);
        }

        #endregion

        #region Canada Provinces Tests

        /// <summary>
        /// US States - Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        /// </summary>
        [TestCategory("Core.States")]
        [TestCategory("Core.Xml.Schema")]
        [TestCategory("Core")]
        [TestMethod]
        public void CanadaProvincesValidateXmlAgainstSchemaTest1()
        {
            try
            {
                //go run the validation
                Assert.IsTrue(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(State.CanadaProvinceXmlResource(), State.CanadaProvinceXmlSchemaResource()));
            }
            catch (Exception)
            {
                //failed, fail the unit test now
                Assert.Fail("Canada Province Xml Doesn't Meet Schema Validation");
            }
        }

        /// <summary>
        /// Test builds the canada state (provinces) listing and verifies everything is correct and running
        /// </summary>
        [TestCategory("Core.States")]
        [TestCategory("Core")]
        [TestMethod]
        public void CanadaProvincesSelectTest1()
        {
            //grab the province listing
            var ProvinceListing = State.CanadaProvincesListing();

            //grab how many provinces we should have
            Assert.AreEqual(13, ProvinceListing.Count);

            //check random states
            Assert.AreEqual("Ontario", ProvinceListing["ON"]);
            Assert.AreEqual("Yukon", ProvinceListing["YT"]);
        }

        #endregion

    }

}