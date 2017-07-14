using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNet.AspNetMVC.Mocking
{

    /// <summary>
    /// Helps mock a html helper with a model
    /// </summary>
    public static class MockHtmlHelper
    {

        /// <summary>
        /// Mock an html helper. We already have tests which create the necessary objects to create a helper. This is an example using moq.
        /// </summary>
        /// <typeparam name="TModel">type of model</typeparam>
        /// <param name="ModelToUse">Model to use for the test</param>
        /// <returns>Html Helper object</returns>
        public static HtmlHelper<TModel> GetMockedHtmlHelper<TModel>(TModel ModelToUse)
        {
            //create the view data (which contains the model)
            var ViewDataToUse = new ViewDataDictionary<TModel>(ModelToUse);

            //mock the view context
            var MockViewContext = new Mock<ViewContext> { CallBase = true };

            //setup the return value
            MockViewContext.Setup(c => c.ViewData).Returns(ViewDataToUse);

            //setup the view context
            MockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());

            //return the mocked html helper
            return new HtmlHelper<TModel>(MockViewContext.Object, new MockIViewDataContainer(ViewDataToUse));
        }

    }

}
