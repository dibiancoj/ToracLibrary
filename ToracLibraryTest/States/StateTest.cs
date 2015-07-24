using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToracLibrary.States;

namespace ToracLibraryTest.CountryTest
{

    /// <summary>
    /// Unit test to test the state listing
    /// </summary>
    [TestClass]
    public class StateTest
    {

        #region Main Tests

        /// <summary>
        /// Test builds the united states listing and verifies everything is correct and running
        /// </summary>
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

        /// <summary>
        /// Test builds the canada state (provinces) listing and verifies everything is correct and running
        /// </summary>
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