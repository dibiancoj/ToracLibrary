using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ReflectionDynamic;
using ToracLibraryTest.Framework;
using ToracLibraryTest.Framework.DummyObjects;
using ToracLibraryTest.UnitsTest.Caching;
using ToracLibraryTest.UnitsTest.Core.DataProviders;
using ToracLibraryTest.UnitsTest.Core.DataProviders.EntityFrameworkDP;
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

        #region Test Derived Classes

        /// <summary>
        /// This is the derived class which is built off of the base
        /// </summary>
        internal class DeriveReflectionClass : BaseDeriveReflectionClass
        {
        }

        /// <summary>
        /// base class which we will fetch by using this type. It should return anything that uses this class as a base
        /// </summary>
        internal class BaseDeriveReflectionClass
        {

            #region Properties

            /// <summary>
            /// Null property that we are going to use for the "PropertyIsNullable" Tests
            /// </summary>
            public int? NullIdProperty { get; set; }

            /// <summary>
            /// IEnumerable property that we are going to use for the "PropertyIsCollection" Tests
            /// </summary>
            public List<string> IEnumerablePropertyTest { get; set; }

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
            //grab everything that implements IDependencyInject
            var ImplementationResults = RetrieveImplementingClassesLazy(typeof(IDependencyInject)).ToArray();

            //we should currently have 2 (could change)
            Assert.AreEqual(4, ImplementationResults.Length);

            //check it's the sql server data provider
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(SqlDataProviderTest)));

            //make sure ef data provider
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(EntityFrameworkTest)));

            //make sure its the in memory caching now
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(InMemoryCacheTest)));

            //make sure sql cache dep is in there
            Assert.AreEqual(true, ImplementationResults.Any(x => x == typeof(SqlCacheDependencyTest)));            
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

    }

}
