using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    /// Mocks an implementation of a IViewDataContainer
    /// </summary>
    public class MockIViewDataContainer : IViewDataContainer
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ViewDataDictionaryToSet">View data dictionary to set, if any</param>
        public MockIViewDataContainer(ViewDataDictionary ViewDataDictionaryToSet)
        {
            //just set the view data (we can't have a null view data dictionary, so we just create a new view data if null is passed in)
            ViewData = ViewDataDictionaryToSet ?? new ViewDataDictionary();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MockIViewDataContainer() : this(null)
        {
        }

        #endregion

        #region Inteface Properties

        /// <summary>
        /// Interface property, return the view data dictionary if any
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        #endregion

    }

}
