using System;
using ToracLibrary.Core.States;
using ToracLibrary.Core.Xml.Schema;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test to test the state listing
    /// </summary>
    public class StateTest
    {

        #region United States Tests

        /// <summary>
        /// US States - Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        [Fact]
        public void UnitedStatesValidateXmlAgainstSchemaTest1()
        {
            //go run the validation
            Assert.True(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(State.UnitedStatesXmlResource(), State.UnitedStatesXmlSchemaResource()));
        }

        /// <summary>
        /// Test builds the united states listing and verifies everything is correct and running
        /// </summary>
        [Fact]
        public void UnitedStatesSelectTest1()
        {
            //grab the us state listing
            var StateListing = State.UnitedStatesStateListing();

            //grab how many states we should have
            Assert.Equal(52, StateListing.Count);

            //check random states
            Assert.Equal("Alabama", StateListing["AL"]);
            Assert.Equal("Wyoming", StateListing["WY"]);
        }

        #endregion

        #region Canada Provinces Tests

        /// <summary>
        /// US States - Make sure the xml conforms to the schema. I don't have the runtime performance hit to check it on each method call
        /// </summary>
        [Fact]
        public void CanadaProvincesValidateXmlAgainstSchemaTest1()
        {
            //go run the validation
            Assert.True(XMLSchemaValidation.ValidateXMLAgainstSchemaAndRaiseExceptions(State.CanadaProvinceXmlResource(), State.CanadaProvinceXmlSchemaResource()));
        }

        /// <summary>
        /// Test builds the canada state (provinces) listing and verifies everything is correct and running
        /// </summary>
        [Fact]
        public void CanadaProvincesSelectTest1()
        {
            //grab the province listing
            var ProvinceListing = State.CanadaProvincesListing();

            //grab how many provinces we should have
            Assert.Equal(13, ProvinceListing.Count);

            //check random states
            Assert.Equal("Ontario", ProvinceListing["ON"]);
            Assert.Equal("Yukon", ProvinceListing["YT"]);
        }

        #endregion

    }

}