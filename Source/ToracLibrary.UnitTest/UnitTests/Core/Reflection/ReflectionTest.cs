using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ReflectionDynamic;
using ToracLibrary.UnitTest.Framework;
using Xunit;
using static ToracLibrary.Core.ReflectionDynamic.ImplementingClasses;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit tests for reflection based functionality
    /// </summary>
    public class ReflectionTest
    {

        #region Framework

        #region Attribute To Test

        private class DescriptionAttribute : Attribute
        {

            #region Constructor

            public DescriptionAttribute(string DescriptionToSet)
            {
                Description = DescriptionToSet;
            }

            #endregion

            #region Constants

            public const string DescriptionValueToTest = "123";

            #endregion

            #region Properties

            public string Description { get; }

            #endregion

        }

        #endregion

        #region Test Derived Classes

        /// <summary>
        /// This is the derived class which is built off of the base
        /// </summary>
        private class DeriveReflectionClass : BaseDeriveReflectionClass
        {
        }

        /// <summary>
        /// base class which we will fetch by using this type. It should return anything that uses this class as a base
        /// </summary>
        private class BaseDeriveReflectionClass
        {

            #region Properties

            /// <summary>
            /// Null property that we are going to use for the "PropertyNullable" Tests
            /// </summary>
            [Description(DescriptionAttribute.DescriptionValueToTest)]
            public int? NullIdProperty { get; set; }

            /// <summary>
            /// IEnumerable property that we are going to use for the "PropertyIsCollection" Tests
            /// </summary>
            public List<string> IEnumerablePropertyTest { get; set; }

            /// <summary>
            /// For the attribute find, we want to make sure the overload works
            /// </summary>
            [Description(DescriptionAttribute.DescriptionValueToTest)]
            public string AttributeFindOffOfField = "TestFieldValue";

            #endregion

            #region Methods

            public void OverloadedMethod()
            {
            }

            public void OverloadedMethod(string Parameter1)
            {

            }

            public void OverloadedMethod(bool Parameter1)
            {

            }

            #endregion

        }

        #endregion

        #region Sub Property Test

        private class SubPropertyBase
        {
            public string Description { get; set; }

            public SubPropertyChild Child { get; set; }
        }

        private class SubPropertyChild
        {
            public int ChildId { get; set; }
        }

        #endregion

        #region Implement Interface Test

        /// <summary>
        /// Interface to test with
        /// </summary>
        private interface ITestImplementInterface
        {
        }

        private class TestImplementInterfaceClass1 : ITestImplementInterface
        {
        }

        private class TestImplementInterfaceClass2 : ITestImplementInterface
        {
        }

        #endregion

        #endregion

        #region Implementing Classes

        /// <summary>
        /// Test Implementing classes. Even though the DI uses this functionality, I'm still going to give it the test it deserves
        /// </summary>
        [Fact]
        public void ImplementingInterfacesFromClass()
        {
            //let's hand code the items we are going to build to test
            var ExpectedTypes = new Type[]
            {
                typeof(TestImplementInterfaceClass1),
                typeof(TestImplementInterfaceClass2)
            };

            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(ITestImplementInterface)).ToArray();

            //how many implementations should we currently have compared with the expected results
            Assert.Equal(ExpectedTypes.Length, ImplementationResults.Length);

            //let's loop through all the expected types and make sure it's in the result list
            foreach (var ExpectedType in ExpectedTypes)
            {
                //is this guy in there?
                Assert.True(ImplementationResults.Any(x => x == ExpectedType), $"Can't Find Type = {ExpectedType.Name} In ImplementingInterfacesFromClass. Result List");
            }
        }

        /// <summary>
        /// Test Derived Classes
        /// </summary>
        [Fact]
        public void DeriveClassFromBaseClass()
        {
            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(BaseDeriveReflectionClass)).ToArray();

            //we should currently have (class is above)
            Assert.Single(ImplementationResults);

            //check it's the sql server data provider
            Assert.Contains(ImplementationResults, x => x == typeof(DeriveReflectionClass));
        }

        #endregion

        #region Property Info Is Nullable Of T

        /// <summary>
        /// Test that a nullable property is noted at run time in a dynamic - reflection manner
        /// </summary>
        [Fact]
        public void PropertyNullableOfTTest1()
        {
            //make sure it picks it up
            Assert.True(PropertyHelpers.IsNullableOfT(typeof(BaseDeriveReflectionClass).GetProperty(nameof(BaseDeriveReflectionClass.NullIdProperty))));

            //test to make sure it doesn't pick these guys up
            Assert.False(PropertyHelpers.IsNullableOfT(typeof(DummyObject).GetProperty(nameof(DummyObject.Id))));
            Assert.False(PropertyHelpers.IsNullableOfT(typeof(DummyObject).GetProperty(nameof(DummyObject.Description))));
        }

        #endregion

        #region Property Info Is Collection

        /// <summary>
        /// Test that a property is a collection in a dynamic - reflection manner 
        /// </summary>
        [Fact]
        public void PropertyIsCollectionTest1()
        {
            //make sure it picks it up
            Assert.True(PropertyHelpers.PropertyInfoIsIEnumerable(typeof(BaseDeriveReflectionClass).GetProperty(nameof(BaseDeriveReflectionClass.IEnumerablePropertyTest))));

            //test to make sure it doesn't pick these guys up
            Assert.False(PropertyHelpers.PropertyInfoIsIEnumerable(typeof(DummyObject).GetProperty(nameof(DummyObject.Id))));
            Assert.False(PropertyHelpers.PropertyInfoIsIEnumerable(typeof(DummyObject).GetProperty(nameof(DummyObject.Description))));
        }

        #endregion

        #region Get Sub Properties

        /// <summary>
        /// Test that a property is a collection in a dynamic - reflection manner 
        /// </summary>]
        [Fact]
        public void GetSubPropertiesTest1()
        {
            //build up the path using the nameof
            string PathToTest = $"{nameof(SubPropertyBase.Child)}.{nameof(SubPropertyChild.ChildId)}";

            //go grab the results for the child id
            var ChildPropertyResults = PropertyHelpers.GetSubPropertiesLazy(typeof(SubPropertyBase), PathToTest).ToArray();

            //now make sure we have 2 child property ("Child" then "ChildId")
            Assert.Equal(2, ChildPropertyResults.Length);

            //make sure the first element data type is SubPropertyChild
            Assert.Equal(typeof(SubPropertyChild), ChildPropertyResults[0].PropertyType);

            //make sure ChildId in an int
            Assert.Equal(typeof(int), ChildPropertyResults[1].PropertyType);
        }

        #endregion

        #region Get Value Using Expression Trees At Run Time

        /// <summary>
        /// Instead of using Property.GetValue you can grab this cache the expression tree and it will be a ton faster. Only faster if you cache the expression (compile of expression is expensive)
        /// </summary>
        [Fact]
        public void GetValueUsingExpressionTreesTest1()
        {
            //id to test
            const int ChildIdToTest = 10;

            //string description to test off of the main object
            const string DescriptionToTest = "Test";

            //we are going to create a dummy object to test
            var ObjectToTest = new SubPropertyBase { Description = DescriptionToTest, Child = new SubPropertyChild { ChildId = ChildIdToTest } };

            //grab the first level path 
            string FirstLevelPathToTest = nameof(SubPropertyBase.Description);

            //grab the child path
            string ChildPathToTest = $"{nameof(SubPropertyBase.Child)}.{nameof(SubPropertyChild.ChildId)}";

            //let's go build up the property on the 1st level property
            var FirstLevelExpressionToGetPropertyTyped = PropertyHelpers.GetPropertyOfObjectExpressionFunc<SubPropertyBase, string>(FirstLevelPathToTest).Compile();

            //let's go build up the property on the 1st level property
            var FirstLevelExpressionToGetPropertyNotTyped = PropertyHelpers.GetPropertyOfObjectExpressionFunc<SubPropertyBase, object>(FirstLevelPathToTest).Compile();

            //build up the expression (test the int path)
            var ChildExpressionToGetPropertyTyped = PropertyHelpers.GetPropertyOfObjectExpressionFunc<SubPropertyBase, int>(ChildPathToTest).Compile();

            //grab another expression using the object data type - where we don't want to invoke dynamically or we really don't know the type
            var ChildExpressionToGetPropertyNotTyped = PropertyHelpers.GetPropertyOfObjectExpressionFunc<SubPropertyBase, object>(ChildPathToTest).Compile();

            //--------------------------------------------------
            //let's test the first level stuff
            //let's test the typed value
            Assert.Equal(DescriptionToTest, FirstLevelExpressionToGetPropertyTyped.Invoke(ObjectToTest));

            //let's test the untyped value
            Assert.Equal(DescriptionToTest, FirstLevelExpressionToGetPropertyNotTyped.Invoke(ObjectToTest));

            //--------------------------------------------------
            //let's test the child values now

            //let's test the typed value
            Assert.Equal(ChildIdToTest, ChildExpressionToGetPropertyTyped.Invoke(ObjectToTest));

            //let's test the untyped value
            Assert.Equal(ChildIdToTest, ChildExpressionToGetPropertyNotTyped.Invoke(ObjectToTest));
        }

        #endregion

        #region Attributes

        [Fact]
        public void FindAttributeTest1()
        {
            //let's try to find the description attribute
            Assert.Equal(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetProperty(nameof(DeriveReflectionClass.NullIdProperty))).Description);

            //let's make sure the field info works
            Assert.Equal(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetField(nameof(DeriveReflectionClass.AttributeFindOffOfField))).Description);

            //check the other overload
            Assert.Equal(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttributeInPropertyName<DescriptionAttribute>(typeof(DeriveReflectionClass), nameof(DeriveReflectionClass.NullIdProperty)).Description);

            //run a test where the property does not have the attribute...so it should return null
            Assert.Null(AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetProperty(nameof(DeriveReflectionClass.IEnumerablePropertyTest))));
        }

        #endregion

        #region Properties that have TAttribute defined

        [Fact]
        public void PropertiesThatHaveAttributeDefinedTest1()
        {
            //let's try to find the properties that have the description attribute defined
            var ResultsOfMethod = AttributeHelpers.PropertiesThatHasAttributeDefinedLazy<DeriveReflectionClass, DescriptionAttribute>(false).ToArray();

            //let's make sure we have 1 properties (the field doesn't get picked up in this method)
            Assert.Single(ResultsOfMethod);

            //make sure we have the correct property
            Assert.Contains(ResultsOfMethod, x => x.Name == nameof(DeriveReflectionClass.NullIdProperty));
        }

        #endregion

        #region Properties that have TAttribute defined. Will return the property and the TAttribute value

        [Fact]
        public void PropertiesThatHaveAttributeDefinedWithAttributeValueTest1()
        {
            //let's try to find the properties that have the description attribute defined
            var ResultsOfMethod = AttributeHelpers.PropertiesThatHasAttributeWithAttributeValueLazy<DeriveReflectionClass, DescriptionAttribute>(false).ToArray();

            //let's make sure we have 1 properties (the field doesn't get picked up in this method)
            Assert.Single(ResultsOfMethod);

            //make sure we have the correct property
            Assert.Contains(ResultsOfMethod, x => x.Key.Name == nameof(DeriveReflectionClass.NullIdProperty));

            //check that we have the correct value now
            Assert.Equal(DescriptionAttribute.DescriptionValueToTest, ResultsOfMethod.First(x => x.Key.Name == nameof(DeriveReflectionClass.NullIdProperty)).Value.Description);

        }

        #endregion 

    }

}
