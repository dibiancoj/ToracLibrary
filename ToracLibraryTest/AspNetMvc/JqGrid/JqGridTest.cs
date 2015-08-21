using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.AspNetMVC.JqGrid;
using ToracLibrary.AspNetMVC.UnitTestMocking;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;

namespace ToracLibraryTest.UnitsTest.AspNetMVC
{

    /// <summary>
    /// Unit test for a the JqGrid column modals
    /// </summary>
    [TestClass]
    public class JqGridTest
    {

        #region Unit Tests

        #region Grid Model

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JqGridJsonPropertyNamesTest1()
        {
            //we are going to borrow the JsonNet result to test the properties of the jqgrid
            var TestController = DIUnitTestContainer.DIContainer.Resolve<JsonActionResultTest.JsonNetActionControllerTest>(JsonActionResultTest.JsonActionResultFactoryName);

            //let's go build our data source
            var GridDataSource = DummyObject.CreateDummyListLazy(3).ToArray();

            //let's build the test JqGridData
            var GridData = JqGridData<DummyObject>.BuildJqGridData(GridDataSource, x => x.Id, 1, 10);

            //let's go execute the action result
            TestController.SerializeToJsonNet(GridData).ExecuteResult(TestController.ControllerContext);

            //let's check the result now
            Assert.AreEqual("{\"total\":1,\"page\":1,\"records\":3,\"rows\":[{\"Id\":0,\"Description\":\"Test_0\"},{\"Id\":1,\"Description\":\"Test_1\"},{\"Id\":2,\"Description\":\"Test_2\"}]}",
                ((MockHttpResponse)TestController.Response).HtmlOutput.ToString());
        }

        #endregion

        #region Grid Inline Filters

        [TestCategory("AspNetMVC.JqGrid")]
        [TestCategory("AspNetMVC")]
        [TestMethod]
        public void JqGridInlineFilterQueryBuilderTest1()
        {
            //test functionality
            Assert.Fail();
        }

        #endregion

        #endregion

    }

}
