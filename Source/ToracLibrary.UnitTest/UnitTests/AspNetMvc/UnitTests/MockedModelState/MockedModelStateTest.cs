using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.UnitTestMocking;
using Xunit;

namespace ToracLibrary.UnitTest.UnitTests.AspNetMvc.UnitTests.MockedModelState
{

    /// <summary>
    /// Tests for model state mocking
    /// </summary>
    public class MockedModelStateTest
    {

        #region Framework

        private class TestController : Controller
        {
            public ActionResult Test(TestModel model)
            {
                return null;
            }
        }

        private class TestModel
        {
            [Required]
            public int? Id { get; set; }
        }

        #endregion

        #region Model Error Tets

        /// <summary>
        /// Test the mocking of model state. This tests that the errors are raised
        /// </summary>
        [Fact]
        public void ValidateModelErrorsTest1()
        {
            //grab the errors. should be 1 because id is not populated
            Assert.Equal(1, MockModelState.ValidateModel(new TestModel()).Count);

            //now test one which shouldn't have any errors
            Assert.Equal(0, MockModelState.ValidateModel(new TestModel { Id = 0 }).Count);
        }

        #endregion

        #region Model State Tests

        /// <summary>
        /// Test the mocking of model state with errors. This tests that the errors are raised
        /// </summary>
        [Fact]
        public void ValidateModelStateWithErrorsTest1()
        {
            //controller to use
            var MockedController = new TestController();

            //go run the model
            MockModelState.ValidateModelErrorsToModelState(MockedController, new TestModel());

            //verify we have 1 error in the model state
            Assert.Equal(1, MockedController.ModelState.Where(x => x.Value.Errors.Any()).Sum(x => x.Value.Errors.Count));
        }

        /// <summary>
        /// Test the mocking of model state without errors. This tests that the errors are raised
        /// </summary>
        [Fact]
        public void ValidateModelStateWithOutErrorsTest1()
        {
            //controller to use
            var MockedController = new TestController();

            //go run the model
            MockModelState.ValidateModelErrorsToModelState(MockedController, new TestModel { Id = 0 });

            //verify we have 0 error in the model state
            Assert.Equal(0, MockedController.ModelState.Where(x => x.Value.Errors.Any()).Sum(x => x.Value.Errors.Count));
        }

        #endregion

    }

}
