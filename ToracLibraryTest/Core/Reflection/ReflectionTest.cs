using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.Core.Reflection;
using ToracLibrary.Core.ReflectionDynamic;
using ToracLibrary.Core.ReflectionDynamic.Invoke;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.AspNetMVC;
using ToracLibraryTest.UnitsTest.Caching;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
using ToracLibraryTest.UnitsTest.EmailSMTP;
using static ToracLibrary.Core.ReflectionDynamic.ImplementingClasses;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit tests for reflection based functionality
    /// </summary>
    [TestClass]
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
            /// Null property that we are going to use for the "PropertyIsNullable" Tests
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

        #region Invoke Dynamically

        private class InvokeRegularMethod
        {

            #region Constants

            public const int InvokeStaticMethodResult = 5;

            #endregion

            #region Static Methods

            public static int InvokeStaticMethod()
            {
                return InvokeStaticMethodResult;
            }

            public static int InvokeStaticMethodWithParameter(int BaseNumber)
            {
                return BaseNumber + InvokeStaticMethodResult;
            }

            #endregion

            #region Instance Methods

            public int InvokeInstanceMethod()
            {
                return InvokeStaticMethodResult;
            }

            public static int InvokeInstanceMethodWithParameter(int BaseNumber)
            {
                return BaseNumber + InvokeStaticMethodResult;
            }

            #endregion

            #region Generic Static Methods

            public static Type InvokeGenericStaticMethod<T>()
            {
                return typeof(T);
            }

            public static Type InvokeGenericStaticMethodWithParameter<T>(Type TypeToTest)
            {
                return TypeToTest;
            }

            #endregion

        }

        #endregion

        #endregion

        #region Implementing Classes

        /// <summary>
        /// Test Implementing classes. Even though the DI uses this functionality, I'm still going to give it the test it deserves
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void ImplementingInterfacesFromClass()
        {
            //let's hand code the items we are going to build to test
            var ExpectedTypes = new Type[]
            {
                typeof(SqlDataProviderTest),
                typeof(EntityFrameworkTest),
                typeof(InMemoryCacheTest),
                typeof(SqlCacheDependencyTest),
                typeof(EncryptionSecurityTest),
                typeof(EmailTest),
                typeof(HtmlHelperTest)
            };

            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(IDependencyInject)).ToArray();

            //how many implementations should we currently have compared with the expected results
            Assert.AreEqual(ExpectedTypes.Length, ImplementationResults.Length);

            //let's loop through all the expected types and make sure it's in the result list
            foreach (var ExpectedType in ExpectedTypes)
            {
                //is this guy in there?
                Assert.IsTrue(ImplementationResults.Any(x => x == ExpectedType), $"Can't Find Type = {ExpectedType.Name} In ImplementingInterfacesFromClass. Result List");
            }
        }

        /// <summary>
        /// Test Derived Classes
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void DeriveClassFromBaseClass()
        {
            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(BaseDeriveReflectionClass)).ToArray();

            //we should currently have (class is above)
            Assert.AreEqual(1, ImplementationResults.Length);

            //check it's the sql server data provider
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(DeriveReflectionClass)));
        }

        #endregion

        #region Property Info Is Nullable Of T

        /// <summary>
        /// Test that a nullable property is noted at run time in a dynamic - reflection manner
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void PropertyIsNullableOfTTest1()
        {
            //make sure it picks it up
            Assert.AreEqual(true, PropertyHelpers.IsNullableOfT(typeof(BaseDeriveReflectionClass).GetProperty(nameof(BaseDeriveReflectionClass.NullIdProperty))));

            //test to make sure it doesn't pick these guys up
            Assert.AreEqual(false, PropertyHelpers.IsNullableOfT(typeof(DummyObject).GetProperty(nameof(DummyObject.Id))));
            Assert.AreEqual(false, PropertyHelpers.IsNullableOfT(typeof(DummyObject).GetProperty(nameof(DummyObject.Description))));
        }

        #endregion

        #region Property Info Is Collection

        /// <summary>
        /// Test that a property is a collection in a dynamic - reflection manner 
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void PropertyIsCollectionTest1()
        {
            //make sure it picks it up
            Assert.AreEqual(true, PropertyHelpers.PropertyInfoIsIEnumerable(typeof(BaseDeriveReflectionClass).GetProperty(nameof(BaseDeriveReflectionClass.IEnumerablePropertyTest))));

            //test to make sure it doesn't pick these guys up
            Assert.AreEqual(false, PropertyHelpers.PropertyInfoIsIEnumerable(typeof(DummyObject).GetProperty(nameof(DummyObject.Id))));
            Assert.AreEqual(false, PropertyHelpers.PropertyInfoIsIEnumerable(typeof(DummyObject).GetProperty(nameof(DummyObject.Description))));
        }

        #endregion

        #region Get Sub Properties

        /// <summary>
        /// Test that a property is a collection in a dynamic - reflection manner 
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void GetSubPropertiesTest1()
        {
            //build up the path using the nameof
            string PathToTest = string.Format($"{nameof(SubPropertyBase.Child)}.{nameof(SubPropertyChild.ChildId)}");

            //go grab the results for the child id
            var ChildPropertyResults = PropertyHelpers.GetSubPropertiesLazy(typeof(SubPropertyBase), PathToTest).ToArray();

            //now make sure we have 2 child property ("Child" then "ChildId")
            Assert.AreEqual(2, ChildPropertyResults.Count());

            //make sure the first element data type is SubPropertyChild
            Assert.AreEqual(typeof(SubPropertyChild), ChildPropertyResults[0].PropertyType);

            //make sure ChildId in an int
            Assert.AreEqual(typeof(int), ChildPropertyResults[1].PropertyType);
        }

        #endregion

        #region Get Value Using Expression Trees At Run Time

        /// <summary>
        /// Instead of using Property.GetValue you can grab this cache the expression tree and it will be a ton faster. Only faster if you cache the expression (compile of expression is expensive)
        /// </summary>
        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
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
            string ChildPathToTest = string.Format($"{nameof(SubPropertyBase.Child)}.{nameof(SubPropertyChild.ChildId)}");

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
            Assert.AreEqual(DescriptionToTest, FirstLevelExpressionToGetPropertyTyped.Invoke(ObjectToTest));

            //let's test the untyped value
            Assert.AreEqual(DescriptionToTest, FirstLevelExpressionToGetPropertyNotTyped.Invoke(ObjectToTest));

            //--------------------------------------------------
            //let's test the child values now

            //let's test the typed value
            Assert.AreEqual(ChildIdToTest, ChildExpressionToGetPropertyTyped.Invoke(ObjectToTest));

            //let's test the untyped value
            Assert.AreEqual(ChildIdToTest, ChildExpressionToGetPropertyNotTyped.Invoke(ObjectToTest));
        }

        #endregion

        #region Invoke Dynamically

        #region Non Generic Method

        #region Static Methods

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeRegularStaticMethodWithNoParametersTest1()
        {
            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(InvokeRegularMethod.InvokeStaticMethodResult, InvokeDynamically.InvokeMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeStaticMethod), false, null));
        }

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeRegularStaticMethodWithParameterTest1()
        {
            //base number
            const int BaseNumber = 100;

            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(BaseNumber + InvokeRegularMethod.InvokeStaticMethodResult, InvokeDynamically.InvokeMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeStaticMethodWithParameter), false, BaseNumber.ToIEnumerableLazy().Cast<object>()));
        }

        #endregion

        #region Instance Methods

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeRegularInstanceMethodWithNoParametersTest1()
        {
            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(InvokeRegularMethod.InvokeStaticMethodResult, InvokeDynamically.InvokeMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeInstanceMethod), true, null));
        }

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeRegularInstanceMethodWithParameterTest1()
        {
            //base number
            const int BaseNumber = 100;

            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(BaseNumber + InvokeRegularMethod.InvokeStaticMethodResult, InvokeDynamically.InvokeMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeInstanceMethodWithParameter), true, BaseNumber.ToIEnumerableLazy().Cast<object>()));
        }

        #endregion

        #endregion

        #region Generic Method

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeGenericStaticMethodWithNoParametersTest1()
        {
            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(InvokeRegularMethod.InvokeGenericStaticMethod<string>(), InvokeDynamically.InvokeGenericMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeGenericStaticMethod), typeof(string).ToIEnumerableLazy(), null));
        }

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void InvokeGenericStaticMethodWithParameterTest1()
        {
            //let's go invoke this method dynamically (static method)
            Assert.AreEqual(InvokeRegularMethod.InvokeGenericStaticMethodWithParameter<string>(typeof(int)), InvokeDynamically.InvokeGenericMethod(typeof(InvokeRegularMethod), nameof(InvokeRegularMethod.InvokeGenericStaticMethodWithParameter), typeof(string).ToIEnumerableLazy(), typeof(int).ToIEnumerableLazy()));
        }

        #endregion

        #endregion

        #region Attributes

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void FindAttributeTest1()
        {
            //let's try to find the description attribute
            Assert.AreEqual(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetProperty(nameof(DeriveReflectionClass.NullIdProperty))).Description);

            //let's make sure the field info works
            Assert.AreEqual(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetField(nameof(DeriveReflectionClass.AttributeFindOffOfField))).Description);

            //check the other overload
            Assert.AreEqual(DescriptionAttribute.DescriptionValueToTest, AttributeHelpers.FindAttributeInPropertyName<DescriptionAttribute>(typeof(DeriveReflectionClass), nameof(DeriveReflectionClass.NullIdProperty)).Description);

            //run a test where the property does not have the attribute...so it should return null
            Assert.IsNull(AttributeHelpers.FindAttribute<DescriptionAttribute>(typeof(DeriveReflectionClass).GetProperty(nameof(DeriveReflectionClass.IEnumerablePropertyTest))));
        }

        #endregion

        #region Properties that have TAttribute defined

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void PropertiesThatHaveAttributeDefinedTest1()
        {
            //let's try to find the properties that have the description attribute defined
            var ResultsOfMethod = AttributeHelpers.PropertiesThatHasAttributeDefinedLazy<DeriveReflectionClass, DescriptionAttribute>(false).ToArray();

            //let's make sure we have 1 properties (the field doesn't get picked up in this method)
            Assert.AreEqual(1, ResultsOfMethod.Length);

            //make sure we have the correct property
            Assert.IsTrue(ResultsOfMethod.Any(x => x.Name == nameof(DeriveReflectionClass.NullIdProperty)));
        }

        #endregion

        #region Properties that have TAttribute defined. Will return the property and the TAttribute value

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void PropertiesThatHaveAttributeDefinedWithAttributeValueTest1()
        {
            //let's try to find the properties that have the description attribute defined
            var ResultsOfMethod = AttributeHelpers.PropertiesThatHasAttributeWithAttributeValueLazy<DeriveReflectionClass, DescriptionAttribute>(false).ToArray();

            //let's make sure we have 1 properties (the field doesn't get picked up in this method)
            Assert.AreEqual(1, ResultsOfMethod.Length);

            //make sure we have the correct property
            Assert.IsTrue(ResultsOfMethod.Any(x => x.Key.Name == nameof(DeriveReflectionClass.NullIdProperty)));

            //check that we have the correct value now
            Assert.AreEqual(DescriptionAttribute.DescriptionValueToTest, ResultsOfMethod.First(x => x.Key.Name == nameof(DeriveReflectionClass.NullIdProperty)).Value.Description);

        }

        #endregion

        #region Overload Method Finder

        [TestCategory("Core.ReflectionDynamic")]
        [TestCategory("Core")]
        [TestMethod]
        public void OverloadMethodFinderTest1()
        {
            //method name to look for
            const string MethodNameToLookFor = nameof(BaseDeriveReflectionClass.OverloadedMethod);

            //the class type to look in
            var ClassTypeToLookIn = typeof(BaseDeriveReflectionClass);

            //let's try to find the overload with 0 parameters
            var MethodInfoWith0ParametersPassingInNull = OverloadedMethodFinder.FindOverloadedMethodToCall(MethodNameToLookFor, ClassTypeToLookIn, null);
            var MethodInfoWith0Parameters = OverloadedMethodFinder.FindOverloadedMethodToCall(MethodNameToLookFor, ClassTypeToLookIn, Array.Empty<Type>());
            var MethodInfoWith1ParameterString = OverloadedMethodFinder.FindOverloadedMethodToCall(MethodNameToLookFor, ClassTypeToLookIn, new Type[] { typeof(string) });
            var MethodInfoWith1ParametersBoolean = OverloadedMethodFinder.FindOverloadedMethodToCall(MethodNameToLookFor, ClassTypeToLookIn, new Type[] { typeof(bool) });
            var MethodThatCantBeFound = OverloadedMethodFinder.FindOverloadedMethodToCall(MethodNameToLookFor, ClassTypeToLookIn, new Type[] { typeof(string), typeof(string) });

            //make sure the first 4 can be found
            Assert.IsNotNull(MethodInfoWith0ParametersPassingInNull);

            //make sure the method exists with 0 parameters passing in an empty array
            Assert.IsNotNull(MethodInfoWith0Parameters);

            //let's find overload with (string parameter)
            Assert.IsNotNull(MethodInfoWith1ParameterString);

            //let's find overload with (string parameter, bool parameter)
            Assert.IsNotNull(MethodInfoWith1ParametersBoolean);

            //let's make sure the method returns null if it can't find the overload
            Assert.IsNull(MethodThatCantBeFound);

            //now let's make sure the parameters are the same
            Assert.AreEqual(0, MethodInfoWith0Parameters.GetParameters().Count());

            //make sure the first parameter is a string
            Assert.AreEqual(1, MethodInfoWith1ParameterString.GetParameters().Count());
            Assert.AreEqual(typeof(string), MethodInfoWith1ParameterString.GetParameters()[0].ParameterType);

            //make sure this overload has a boolean in the first spot
            Assert.AreEqual(1, MethodInfoWith1ParametersBoolean.GetParameters().Count());
            Assert.AreEqual(typeof(bool), MethodInfoWith1ParametersBoolean.GetParameters()[0].ParameterType);
        }

        #endregion

    }

}
