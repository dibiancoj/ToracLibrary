﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.CustomActionsResults;
using ToracLibrary.AspNet.AspNetMVC.Mocking;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.DIContainer;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNetMVC.CustomActionResults
{

    /// <summary>
    /// Unit test for a custom action result in asp.net mvc. This is for json.net (json serializer)
    /// </summary>
    public class JsonActionResultTest
    {

        #region Constants

        /// <summary>
        /// Factory name for all the mocking we need for the html helper mocks
        /// </summary>
        internal const string JsonActionResultFactoryName = "JsonActionResultFactoryName";

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

        [Fact]
        public void JsonCustomActionResultTest1()
        {
            // resolve the controller from the di container
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonNetActionControllerTest>(JsonActionResultFactoryName);

            //let's go execute the action result
            TestController.JsonNetResult().ExecuteResult(TestController.ControllerContext);

            //let's check the result now
            Assert.Equal("{\"JsonId\":5,\"JsonDescription\":\"Description5\",\"CreatedDate\":\"2015-09-01T00:00:00\"}", TestController.Response.Cast<MockHttpResponse>().HtmlOutput.ToString());
        }

        #endregion

    }

}
