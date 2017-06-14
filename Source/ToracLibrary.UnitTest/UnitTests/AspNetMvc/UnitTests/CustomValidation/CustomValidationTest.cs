using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.AspNet.AspNetMVC.CustomModelBinders.CustomValidators;
using Xunit;

namespace ToracLibrary.UnitTest.AspNet.AspNetMVC.CustomValidationTest
{

    /// <summary>
    /// Unit test for a custom validations
    /// </summary>
    public class CustomValidationTest
    {

        #region Framework

        #region Test Model

        private class CustomValidationViewModel
        {

            #region Constructor

            public CustomValidationViewModel(IEnumerable<string> ListToTest)
            {
                IEnumerableListTest = ListToTest;
                IListTest = ListToTest.ToList();
                ArrayTest = ListToTest.ToArray();
            }

            #endregion

            #region Constants

            public const int MinimumNumberOfElements = 2;

            #endregion

            #region Properties

            [EnsureMinimumElements(MinimumNumberOfElements)]
            public IEnumerable<string> IEnumerableListTest { get; }

            [EnsureMinimumElements(MinimumNumberOfElements)]
            public IList<string> IListTest { get; }

            [EnsureMinimumElements(MinimumNumberOfElements)]
            public string[] ArrayTest { get; }

            #endregion

            #region Methods

            public IEnumerable<string> IteratorTest()
            {
                foreach(var ItemToReturn in IListTest)
                {
                    yield return ItemToReturn;
                }
            }

            #endregion

        }

        #endregion

        #endregion

        #region Unit Tests

        [Fact]
        public void CustomValidationTest1()
        {
            //test that (should fail)
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { }).IEnumerableListTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { }).IListTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { }).ArrayTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { }).IteratorTest()));

            //add 1 element should still fail
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1" }).IEnumerableListTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1" }).IListTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1" }).ArrayTest));
            Assert.False(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1" }).IteratorTest()));

            //2 elements should pass now
            Assert.True(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1", "2" }).IEnumerableListTest));
            Assert.True(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1", "2" }).IListTest));
            Assert.True(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1", "2" }).ArrayTest));
            Assert.True(new EnsureMinimumElementsAttribute(CustomValidationViewModel.MinimumNumberOfElements).IsValid(new CustomValidationViewModel(new string[] { "1", "2" }).IteratorTest()));

            //this should throw
            Assert.ThrowsAny<Exception>(() => new EnsureMinimumElementsAttribute(1).IsValid(1));
        }

        #endregion

    }

}
