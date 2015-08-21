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
            //do we have a null value (passed in from the other constructor?)
            if (ViewDataDictionaryToSet == null)
            {
                ViewData = new ViewDataDictionary();
            }
            else
            {
                ViewData = ViewDataDictionaryToSet;
            }
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
