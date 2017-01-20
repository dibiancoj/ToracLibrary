using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToracLibrary.AspNet.AspNetMVC.Mocking
{

    /// <summary>
    /// Modelstate doesnt come through when you mock. This set's the model state for unit tests
    /// </summary>
    public static class MockModelState
    {

        /// <summary>
        /// Set the model state for any errors that are raised
        /// </summary>
        /// <typeparam name="TController">Controller type</typeparam>
        /// <typeparam name="TModel">Model Type</typeparam>
        /// <param name="MockedController">Mocked controller</param>
        /// <param name="ModelToValidate">Model to validate</param>
        public static void ValidateModelErrorsToModelState<TController, TModel>(TController MockedController, TModel ModelToValidate)
           where TController : Controller
        {
            //loop through all the validation errors
            foreach (var error in ValidateModel(ModelToValidate))
            {
                //set the controller model state
                MockedController.ModelState.AddModelError(error.MemberNames.First(), error.ErrorMessage);
            }
        }

        /// <summary>
        /// Validate the model and return any validation errors that were raised
        /// </summary>
        /// <typeparam name="TModel">Modle type to validate</typeparam>
        /// <param name="ModelToValidate">Model to validate</param>
        /// <returns>Validation Errors raised</returns>
        public static IList<ValidationResult> ValidateModel<TModel>(TModel ModelToValidate)
        {
            //Create the validation context
            var ValidationContextToUse = new ValidationContext(ModelToValidate, null, null);

            //validation errors that we will populate
            var ValidationResults = new List<ValidationResult>();

            //go validate the model
            Validator.TryValidateObject(ModelToValidate, ValidationContextToUse, ValidationResults, true);

            //return the errors
            return ValidationResults;
        }

    }

}
