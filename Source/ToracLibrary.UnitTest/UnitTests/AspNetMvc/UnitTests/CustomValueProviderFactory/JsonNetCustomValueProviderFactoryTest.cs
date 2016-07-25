using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC;
using ToracLibrary.AspNet.AspNetMVC.CustomValueProviderFactory;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibrary.Serialization.Json;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNetMVC.CustomValueProviderFactory
{

    /// <summary>
    /// Unit test for a the json net value provider factory
    /// </summary>
    public class JsonNetCustomValueProviderFactoryTest
    {

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        internal const string JsonNetCustomValueProviderFactoryName = "JsonNetCustomValueProviderFactoryName";

        #endregion

        #region Framework

        #region Test Model

        private class AjaxPostModel
        {

            #region Constructor

            public AjaxPostModel(int IdToSet, string DescriptionToSet)
            {
                Id = IdToSet;
                Description = DescriptionToSet;
            }

            #endregion

            #region Properties

            public int Id { get; }
            public string Description { get; }

            #endregion

            #region Methods

            public static AjaxPostModel BuildModel()
            {
                return new AjaxPostModel(5, "5Description");
            }

            #endregion

        }

        #endregion

        #region Test Controller

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        internal class JsonNetCustomValueProviderFactoryControllerTest : Controller
        {

            #region DI Controller Creation

            public static JsonNetCustomValueProviderFactoryControllerTest MockController(ToracDIContainer DIContainer)
            {
                //create the controller
                var MockedController = new JsonNetCustomValueProviderFactoryControllerTest();

                //create the Mock controller
                MockedController.ControllerContext = new MockControllerContext(MockedController,
                                                                               DIContainer.Resolve<MockPrincipal>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockIdentity>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpRequest>(JsonNetCustomValueProviderFactoryName),
                                                                               DIContainer.Resolve<MockHttpResponse>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpSessionState>(AspNetDIContainerSharedMock.AspNetMockFactoryName));

                //return the controller now
                return MockedController;
            }

            public static MockHttpRequest MockRequest(ToracDIContainer DIContainer)
            {
                //let's build a model with a stream
                var MemoryStreamToUse = new MemoryStream();

                //stream writer
                var StreamWriterToUse = new StreamWriter(MemoryStreamToUse);

                //write the string value
                StreamWriterToUse.Write(JsonNetSerializer.Serialize(AjaxPostModel.BuildModel()));

                //flush the data
                StreamWriterToUse.Flush();

                //reset the stream
                MemoryStreamToUse.Position = 0;

                //go build hte request and return it
                return new MockHttpRequest(null, null, null, null, AspNetConstants.JsonContentType, MemoryStreamToUse);
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Tests

        [Fact]
        public void JsonNetCustomValueProviderFactoryTest1()
        {
            //let's grab the model we will test with
            var BaseModel = AjaxPostModel.BuildModel();

            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonNetCustomValueProviderFactoryControllerTest>(JsonNetCustomValueProviderFactoryName);

            //grab the new factory 
            var TestProvider = new JsonNetValueProviderFactory();

            //let's go execute the action result
            var Result = TestProvider.GetValueProvider(TestController.ControllerContext);

            //lets run the test now
            Assert.Equal(BaseModel.Id, Convert.ToInt32(Result.GetValue(nameof(AjaxPostModel.Id)).RawValue));
            Assert.Equal(BaseModel.Description, Result.GetValue(nameof(AjaxPostModel.Description)).RawValue);
        }

        #endregion

    }

}
