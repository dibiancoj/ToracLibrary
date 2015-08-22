using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Enabled a user to unit test the ViewEngine
    /// </summary>
    public class MockIViewEngine : IViewEngine
    {

        #region Constructor

        /// <summary>
        /// Views to use for testing purposes
        /// </summary>
        ///<param name="Views">Views to look through</param>
        public MockIViewEngine(IDictionary<string, IView> Views)
        {
            ViewsToMock = Views;
        }

        #endregion

        #region Properties

        private IDictionary<string, IView> ViewsToMock { get; }

        #endregion

        #region Implementation

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new NotImplementedException();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //make sure we can find the view
            if (!ViewsToMock.ContainsKey(viewName))
            {
                throw new ArgumentNullException("Can't Find View In Views To Mock");
            }

            return new ViewEngineResult(ViewsToMock[viewName], this);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
