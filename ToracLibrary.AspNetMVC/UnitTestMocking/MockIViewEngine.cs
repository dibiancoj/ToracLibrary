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

        #region Mock Methods

        /// <summary>
        /// Finds the mocked view / partial view and returns it
        /// </summary>
        /// <param name="ViewNameToFind">View name or partial view name to return</param>
        /// <returns></returns>
        private ViewEngineResult FindMockedIView(string ViewNameToFind)
        {
            //view to find
            IView ViewFetchAttempt;

            //go try to find the view
            if (ViewsToMock.TryGetValue(ViewNameToFind, out ViewFetchAttempt))
            {
                return new ViewEngineResult(ViewsToMock[ViewNameToFind], this);
            }

            //we can find it the view, throw an exception
            throw new ArgumentNullException("Can't Find View Or Partial View In ViewsToMock To Mock");
        }

        #endregion

        #region Implementation

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return FindMockedIView(partialViewName);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return FindMockedIView(viewName);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
