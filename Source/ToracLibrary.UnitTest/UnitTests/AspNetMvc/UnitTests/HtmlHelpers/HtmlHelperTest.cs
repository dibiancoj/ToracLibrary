using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.HtmlHelpers;
using ToracLibrary.AspNet.AspNetMVC.Mocking;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNetMVC.HtmlHelpers
{

    /// <summary>
    /// Unit test for asp.net mvc html helpers
    /// </summary>
    public class HtmlHelperTest
    {

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        internal const string HtmlHelperTestDIFactoryName = "HtmlHelperMocks";

        #endregion

        #region Test Model

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        internal class HtmlHelperTestViewModel
        {

            #region Constructor

            public HtmlHelperTestViewModel()
            {
                Id = IdValueToUseForTest;
            }

            #endregion

            #region Constants

            internal const int IdValueToUseForTest = 5;

            #endregion

            #region Properties

            public int Id { get; }

            #endregion

        }

        #endregion

        #region Unit Tests

        #region Custom Output Attribute

        //test attribute value to output
        const string AttributeToOutput = "data-id=5";

        [Fact]
        public void CustomOutputAttributeTest1()
        {
            //we have everything we need to mock up the html helper with the DI...so grab the mocked HtmlHelper from the di container
            var MockedHtmlHelper = DIUnitTestContainer.DIContainer.Resolve<HtmlHelper<HtmlHelperTestViewModel>>(HtmlHelperTestDIFactoryName);

            //go use the common method to test it
            CustomAttributeTestLogic(MockedHtmlHelper, new HtmlHelperTestViewModel());
        }

        [Fact]
        public void CustomOutputAttributeUsingMoqTest2()
        {
            //mock TModel which is present in the view (no need to put this into the di container)
            var MockedModel = new HtmlHelperTestViewModel();

            //this test using moq for the html helper. Use the helper method to test it
            CustomAttributeTestLogic(MockHtmlHelper.GetMockedHtmlHelper(MockedModel), MockedModel);
        }

        #endregion

        #region Helper

        private void CustomAttributeTestLogic(HtmlHelper<HtmlHelperTestViewModel> MockedHtmlHelper, HtmlHelperTestViewModel MockedModel)
        {
            //let's test the html helper now
            Assert.Equal(AttributeToOutput, MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == HtmlHelperTestViewModel.IdValueToUseForTest, AttributeToOutput));

            //let's test if the value is not correct. So we pass in -9999 which should evaluate to false and return null
            Assert.Null(MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == -9999, AttributeToOutput));
        }

        #endregion

        #endregion

    }

}
