using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ToracLibrary.AspNetMVC.UnitTestMocking
{

    /// <summary>
    ///  Class used to mock a HttpResponse. This is a class i implemented for JsonNetResult mocking. It might need to be expanded to handle more complext scenario's
    /// </summary>
    public class MockHttpResponse : HttpResponseBase
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MockHttpResponse()
        {
            //we are going to write the html to a string builder
            HtmlOutput = new StringBuilder();
        }

        #endregion

        #region Mock Properties

        /// <summary>
        /// The results of the response
        /// </summary>
        public StringBuilder HtmlOutput { get; }

        #endregion

        #region Override Properties

        /// <summary>
        /// Content type. Need to override this so everything works
        /// </summary>
        public override string ContentType { get; set; }

        /// <summary>
        ///  Content encoding. Need to override this so everything works
        /// </summary>
        public override Encoding ContentEncoding { get; set; }

        #endregion

        #region Override Methods 

        /// <summary>
        /// For the action result, it outputs html, so we append the html result
        /// </summary>
        /// <param name="s">Html to write</param>
        public override void Write(string s)
        {
            //add the results to the string builder
            HtmlOutput.Append(s);
        }

        #endregion

    }

}
