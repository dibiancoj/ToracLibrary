using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Excel;
using ToracLibrary.Core.ExpressionTrees;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for expression trees
    /// </summary>
    [TestClass]
    public class ExpressionTreeHelpersTest
    {

        #region Framework

        private class BuildNewObjectNoParams
        {
        }

        private class BuildNewObjectWithParams
        {

            #region Constructor

            public BuildNewObjectWithParams(string DescriptionToSet)
            {
                Description = DescriptionToSet;
            }

            #endregion

            #region Properties

            public string Description { get; }

            #endregion

            #region Consts

            internal const string DescriptionValueToTest = "test123";

            #endregion

        }

        #endregion

        #region Build New Object

        /// <summary>
        /// build a new object using expression trees (no parameters)
        /// </summary>
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void BuildNewObjectWithNoConstructorParametersTest1()
        {
            //let's go build the expression
            var NewObject = ExpressionTreeHelpers.BuildNewObject(typeof(BuildNewObjectNoParams).GetConstructors().First(), Array.Empty<ParameterInfo>()).Compile().Invoke(Array.Empty<object>());

            //let's test to make sure we have an instance
            Assert.IsInstanceOfType(NewObject, typeof(BuildNewObjectNoParams));
        }


        /// <summary>
        /// build a new object using expression trees (with parameters)
        /// </summary>
        [TestCategory("Core.ExpressionTrees")]
        [TestCategory("Core")]
        [TestMethod]
        public void BuildNewObjectWithConstructorParametersTest1()
        {
            //cache the constructor info
            var ConstructorInfoToUse = typeof(BuildNewObjectWithParams).GetConstructors().First();

            //let's go build the expression
            var NewObject = ExpressionTreeHelpers.BuildNewObject(ConstructorInfoToUse, ConstructorInfoToUse.GetParameters()).Compile().Invoke(new object[] { BuildNewObjectWithParams.DescriptionValueToTest });

            //let's test to make sure we have an instance
            Assert.IsInstanceOfType(NewObject, typeof(BuildNewObjectWithParams));

            //make sure we hvae the correct property
            Assert.AreEqual(BuildNewObjectWithParams.DescriptionValueToTest, ((BuildNewObjectWithParams)NewObject).Description);
        }

        #endregion

    }

}
