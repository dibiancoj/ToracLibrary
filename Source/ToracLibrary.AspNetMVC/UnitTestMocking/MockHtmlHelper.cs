//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;

//namespace ToracLibrary.AspNet.AspNetMVC.UnitTestMocking
//{


// **************** I don't want moq in my mvc project. Going to use this as an example



//    /// <summary>
//    /// Helps mock a html helper with a model
//    /// </summary>
//    public static class MockHtmlHelper
//    {

//        public static HtmlHelper<TModel> GetMockedHtmlHelper<TModel>(TModel modelToUse)
//        {
//            var viewData = new ViewDataDictionary<TModel>(modelToUse);
//            var mockViewContext = new Mock<ViewContext> { CallBase = true };
//            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
//            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());

//            return new HtmlHelper<TModel>(mockViewContext.Object, GetMockedViewDataContainer(viewData));
//        }

//        public static IViewDataContainer GetMockedViewDataContainer(ViewDataDictionary viewData)
//        {
//            var mockContainer = new Mock<IViewDataContainer>();
//            mockContainer.Setup(c => c.ViewData).Returns(viewData);
//            return mockContainer.Object;
//        }

//    }

//}
