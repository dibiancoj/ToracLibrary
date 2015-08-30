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

        /// <summary>
        /// Holds the views or partial views that we want to mock. So we hold them in a dictionary instead of what mvc does by searching the view / partial view directory
        /// </summary>
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
                //we have the view, return it
                return new ViewEngineResult(ViewsToMock[ViewNameToFind], this);
            }

            //we can find it the view, throw an exception
            throw new ArgumentNullException("Can't Find View Or Partial View In ViewsToMock To Mock");
        }

        #endregion

        #region Implementation

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            //go grab the partial view and return it
            return FindMockedIView(partialViewName);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //go grab the view and return it
            return FindMockedIView(viewName);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            //don't need this for the mocked version
            throw new NotImplementedException();
        }

        #endregion

    }

}
