using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.CustomActionsResults;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using ToracLibrary.DIContainer;
using ToracLibraryTest.Framework;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC.CustomActionResults
{

    /// <summary>
    /// Unit test for a custom action result in asp.net mvc. This is for json.net (json serializer)
    /// </summary>
    [TestClass]
    public class JsonActionResultTest : IDependencyInject
    {

        #region IDependency Injection Methods

        /// <summary>
        /// Configure the DI container for this unit test. Get's called because the class has IDependencyInject - DIUnitTestContainer.ConfigureDIContainer
        /// </summary>
        /// <param name="DIContainer">container to modify</param>
        public void ConfigureDIContainer(ToracDIContainer DIContainer)
        {
            //let's mock up the controller for each of these tests

            //now let's register the actual html helper we are going to mock
            DIContainer.Register<JsonNetActionControllerTest>()
                .WithFactoryName(JsonActionResultFactoryName)
                .WithConstructorImplementation((di) => JsonNetActionControllerTest.MockController(di));
        }

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        internal const string JsonActionResultFactoryName = "JsonActionResultFactoryName";

        #endregion

        #endregion

        #region Framework

        #region Test Model

        private class CustomActionResultViewModel
        {

            #region Constructor

            public CustomActionResultViewModel()
            {
                Id = 5;
                Description = "Description5";
                CreatedDate = new DateTime(2015, 9, 1);
            }

            #endregion

            #region Properties

            [JsonProperty("JsonId")]
            public int Id { get; }

            [JsonProperty("JsonDescription")]
            public string Description { get; }

            public DateTime CreatedDate { get; }

            #endregion

        }

        #endregion

        #region Test Controller

        /// <summary>
        /// This would be the test model that would be passed into the view
        /// </summary>
        internal class JsonNetActionControllerTest : Controller
        {

            #region DI Controller Creation

            public static JsonNetActionControllerTest MockController(ToracDIContainer DIContainer)
            {
                //create the controller
                var MockedController = new JsonNetActionControllerTest();

                //create the Mock controller
                MockedController.ControllerContext = new MockControllerContext(MockedController, 
                                                                               DIContainer.Resolve<MockPrincipal>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockIdentity>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpRequest>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpResponse>(AspNetDIContainerSharedMock.AspNetMockFactoryName),
                                                                               DIContainer.Resolve<MockHttpSessionState>(AspNetDIContainerSharedMock.AspNetMockFactoryName));

                //return the controller now
                return MockedController;
            }

            #endregion

            #region Methods

            public ActionResult JsonNetResult()
            {
                return new JsonNetResult(new CustomActionResultViewModel());
            }

            public ActionResult SerializeToJsonNet<T>(T ObjectToSerialize)
            {
                return new JsonNetResult(ObjectToSerialize);
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Tests

        [TestCategory("AspNetMVC.CustomActionsResults.JsonNet")]
        [TestCategory("AspNetMVC.CustomActionsResults")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JsonCustomActionResultTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonNetActionControllerTest>(JsonActionResultFactoryName);

            //let's go execute the action result
            TestController.JsonNetResult().ExecuteResult(TestController.ControllerContext);

            //let's check the result now
            Assert.AreEqual("{\"JsonId\":5,\"JsonDescription\":\"Description5\",\"CreatedDate\":\"2015-09-01T00:00:00\"}", ((MockHttpResponse)TestController.Response).HtmlOutput.ToString());
        }

        #endregion

    }

}
