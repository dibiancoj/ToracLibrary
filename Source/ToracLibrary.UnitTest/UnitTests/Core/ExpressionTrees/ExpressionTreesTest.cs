using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.ExpressionTrees;
using ToracLibrary.UnitTest.Core.DataProviders;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using ToracLibrary.UnitTest.Framework;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for expression trees
    /// </summary>
    public class ExpressionTreeHelpersTest
    {

        #region Framework

        #region Build New Objects

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

        #region Select new Object

        /// <summary>
        /// don't inherit from the other guy. I want completely different properties to test. Found issue when converting that it only works with derived class properties. Seperate properties weren't working
        /// </summary>
        private class SelectNewObjectFrom
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        ///  don't inherit from the other guy. I want completely different properties to test. Found issue when converting that it only works with derived class properties. Seperate properties weren't working
        /// </summary>
        private class SelectNewObjectTo
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        #endregion

        #endregion

        #region Build New Object

        /// <summary>
        /// build a new object using expression trees (no parameters)
        /// </summary>
        [Fact]
        public void BuildNewObjectWithNoConstructorParametersTest1()
        {
            //let's go build the expression
            var NewObject = ExpressionTreeHelpers.BuildNewObject(typeof(BuildNewObjectNoParams).GetConstructors().First(), Array.Empty<ParameterInfo>()).Compile().Invoke(Array.Empty<object>());

            //let's test to make sure we have an instance
            Assert.True(NewObject.GetType() == typeof(BuildNewObjectNoParams));
        }

        /// <summary>
        /// build a new object using expression trees (with parameters)
        /// </summary>
        [Fact]
        public void BuildNewObjectWithConstructorParametersTest1()
        {
            //cache the constructor info
            var ConstructorInfoToUse = typeof(BuildNewObjectWithParams).GetConstructors().First();

            //let's go build the expression
            var NewObject = ExpressionTreeHelpers.BuildNewObject(ConstructorInfoToUse, ConstructorInfoToUse.GetParameters()).Compile().Invoke(new object[] { BuildNewObjectWithParams.DescriptionValueToTest });

            //let's test to make sure we have an instance
            Assert.True(NewObject.GetType() == typeof(BuildNewObjectWithParams));

            //make sure we hvae the correct property
            Assert.Equal(BuildNewObjectWithParams.DescriptionValueToTest, ((BuildNewObjectWithParams)NewObject).Description);
        }

        #endregion

        #region Select New From Object - Copying Over Properties

        /// <summary>
        /// build a new object from an existing object copying over the properties using expression trees for linq to objects
        /// </summary>
        [Fact]
        public void SelectNewFromObjectForLinqToObjects()
        {
            //values that we will check for
            const int IdToCheck = 9999;
            const string DescriptionToCheck = "TestSelectnew";

            //let's build the object we will copy from
            var FromObject = new SelectNewObjectFrom { Id = IdToCheck, Description = DescriptionToCheck };

            //let's create the expression now
            var ExpressionThatWasBuilt = ExpressionTreeHelpers.SelectNewFromObject<SelectNewObjectFrom, SelectNewObjectTo>(typeof(SelectNewObjectFrom).GetProperties());

            //let's go invoke this
            var ToObjectToTest = ExpressionThatWasBuilt.Compile().Invoke(FromObject);

            //let's compare the values
            Assert.Equal(IdToCheck, ToObjectToTest.Id);
            Assert.Equal(DescriptionToCheck, ToObjectToTest.Description);
        }

        /// <summary>
        /// build a new object using expression trees (with parameters)
        /// </summary>
        [Fact]
        public void SelectNewFromObjectForForEntityFramework()
        {
            DataProviderSetupTearDown.TearDownAndBuildUpDbEnvironment();

            //grab the ef data provider
            using (var DP = DIUnitTestContainer.DIContainer.Resolve<EntityFrameworkDP<EntityFrameworkEntityDP>>(EntityFrameworkTest.ReadonlyDataProviderName))
            {
                //values that we will check for
                const int IdToCheck = 1;

                //let's grab the original ef record so we can compare it
                var RefTestRecordToTest = DP.Fetch<Ref_Test>(false).First(x => x.Id == IdToCheck);

                //let's create the expression now
                var ExpressionThatWasBuilt = ExpressionTreeHelpers.SelectNewFromObject<Ref_Test, SelectNewObjectTo>(typeof(Ref_Test).GetProperties());

                //let's go invoke this
                var ToObjectToTest = DP.Fetch<Ref_Test>(false).Where(x => x.Id == 1).Select(ExpressionThatWasBuilt).First();

                //let's compare the values
                Assert.Equal(RefTestRecordToTest.Id, ToObjectToTest.Id);
                Assert.Equal(RefTestRecordToTest.Description, ToObjectToTest.Description);
            }
        }

        #endregion

    }

}
