using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace ToracLibrary.AspNetMVC.CustomModelBinders
{

    /// <summary>
    /// Model Binder For Decimal's. Problem in MVC 3 where decimal data type doesnt come through to the controller for javascript calls
    /// </summary>
    /// <remarks>Please see notes on how to add to Global.asax</remarks>
    public class DecimalModelBinder : System.Web.Mvc.IModelBinder
    {

        //to add to application, in the global.ascx
        //protected void Application_Start()
        //{
        //ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

        /// <summary>
        /// Bind Model (Interface Methods)  Model Binder For Decimal's. Problem in MVC 3 where decimal data type doesnt come through to the controller for javascript calls
        /// </summary>
        /// <param name="ControllerContextToUse">Controller Context</param>
        /// <param name="BindingContext">Binding Context</param>
        /// <returns>Value Of Parameter</returns>
        public object BindModel(ControllerContext ControllerContextToUse, System.Web.Mvc.ModelBindingContext BindingContext)
        {
            //grab the value
            var ValueResult = BindingContext.ValueProvider.GetValue(BindingContext.ModelName);

            //set the model state
            var ModelStateToUse = new System.Web.Mvc.ModelState { Value = ValueResult };

            //value to return 
            object ActualValue = null;

            try
            {
                //let's go and try to convert the value
                ActualValue = Convert.ToDecimal(ValueResult.AttemptedValue);
            }
            catch (FormatException e)
            {
                //if we can't convert set the model state error
                ModelStateToUse.Errors.Add(e);
            }

            //add the model and model state
            BindingContext.ModelState.Add(BindingContext.ModelName, ModelStateToUse);

            //return the actual value
            return ActualValue;
        }

    }

}
