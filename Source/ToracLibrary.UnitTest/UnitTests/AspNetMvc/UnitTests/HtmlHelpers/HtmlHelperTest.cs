using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.HtmlHelpers;
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

        [Fact]
        public void CustomOutputAttributeTest1()
        {
            //test attribute value to output
            const string AttributeToOutput = "data-id=5";

            //mock TModel which is present in the view (no need to put this into the di container)
            var MockedModel = new HtmlHelperTestViewModel();

            //we have everything we need to mock up the html helper with the DI...so grab the mocked HtmlHelper from the di container
            var MockedHtmlHelper = DIUnitTestContainer.DIContainer.Resolve<HtmlHelper<HtmlHelperTestViewModel>>(HtmlHelperTestDIFactoryName);

            //let's test the html helper now
            Assert.Equal(AttributeToOutput, MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == HtmlHelperTestViewModel.IdValueToUseForTest, AttributeToOutput));

            //let's test if the value is not correct. So we pass in -9999 which should evaluate to false and return null
            Assert.Null(MockedHtmlHelper.CustomOutputAttribute(MockedModel.Id == -9999, AttributeToOutput));
        }

        #endregion

    }

}
