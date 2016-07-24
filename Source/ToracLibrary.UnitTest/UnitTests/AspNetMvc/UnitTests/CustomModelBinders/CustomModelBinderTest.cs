using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.CustomModelBinders;
using Xunit;

namespace ToracLibraryTest.UnitsTest.AspNet.AspNetMVC
{

    /// <summary>
    /// Unit test for a custom model binders
    /// </summary>
    public class CustomModelBinderTest
    {

        #region Unit Tests

        [Fact]
        public void DecimalCustomModelBinderTest1()
        {
            //method parameter name we expect
            //ie:  public ActionResult TestDecimal(decimal DecimalToTest)
            //{
            //return Json("ResultValue");
            //}

            //parameter name into the controller method that we are binding too
            const string ParameterName = "DecimalToTest";

            //the value we are testing
            const int DecimalValueToTest = 2;

            //let's set the parameters we pass into the method
            var MethodParameters = new NameValueCollection
                                    {
                                        { ParameterName, DecimalValueToTest.ToString() }
                                    };

            //let's create the value Provider with the methood parameters that we are going to pass in
            var ValueProviderForMethod = new NameValueCollectionValueProvider(MethodParameters, null);

            //let's declare the type we are expecting. This method is expecting a decimal type
            var MetaDataForMethod = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(decimal));

            //let's create a new binding context
            var BindingContext = new ModelBindingContext
            {
                ModelName = ParameterName,
                ValueProvider = ValueProviderForMethod,
                ModelMetadata = MetaDataForMethod
            };

            //let's create a new controller context
            var ControllerContext = new ControllerContext();

            //let's crewate the new decimal model binder
            var ModelBinder = new DecimalModelBinder();

            //let's try to go bind this guy
            var Result = (decimal)ModelBinder.BindModel(ControllerContext, BindingContext);

            //let's make sure we have a value
            Assert.Equal(DecimalValueToTest, Result);
        }

        #endregion

    }

}
