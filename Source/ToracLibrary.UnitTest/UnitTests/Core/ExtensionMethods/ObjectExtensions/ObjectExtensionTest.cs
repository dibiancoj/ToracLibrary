using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.UnitTest.Framework;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to Object Extension Methods
    /// </summary>
    public class ObjectExtensionTest
    {

        #region Framework

        public abstract class MyObject
        {
            public const int MyValue = 1;

            public int MyValueGetter
            {
                get { return MyValue; }
            }
        }

        public class MyDerivedObject : MyObject
        {
        }

        #endregion

        #region As Unit Tests

        /// <summary>
        /// Try to convert a class to something else
        /// </summary>
        [Fact]
        public void ObjectAsTest1()
        {
            var ObjectToTest = new MyDerivedObject();

            Assert.Equal(MyObject.MyValue, ObjectToTest.As<MyObject>().MyValueGetter);
        }

        /// <summary>
        /// Try to convert a class to something that isn't castable
        /// </summary>
        [Fact]
        public void ObjectAsToNullTest1()
        {
            var ObjectToTest = DummyObject.CreateDummyRecord();

            Assert.Null(ObjectToTest.As<MyObject>()?.MyValueGetter);
        }

        #endregion

        #region Is Unit Tests

        /// <summary>
        /// Try to convert a class to something else and see what "Is" returns
        /// </summary>
        [Fact]
        public void ObjectIsTest1()
        {
            Assert.True(new MyDerivedObject().Is<MyObject>());
        }

        /// <summary>
        /// Try to convert a class to something that isn't castable
        /// </summary>
        [Fact]
        public void ObjectIsToNullTest1()
        {
            Assert.False(DummyObject.CreateDummyRecord().Is<MyObject>());
        }

        #endregion

        #region Single Object To Array Types

        /// <summary>
        /// Unit test to create an IEnumerable from a single object
        /// </summary>
        [Fact]
        public void SingleObjectToIEnumerableTest1()
        {
            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.Single(DummyObject.CreateDummyRecord().ToIEnumerableLazy());
        }

        /// <summary>
        /// Unit test to create an IList from a single object
        /// </summary>
        [Fact]
        public void SingleObjectToListTest1()
        {
            //grab a single record and push to an ienumerable
            var IListBuiltFromSingleObject = DummyObject.CreateDummyRecord().ToIList();

            //make sure we only have 1 record. This should prove it's in a form of ienumerable
            Assert.Equal(1, IListBuiltFromSingleObject.Count);

            //add another record so we can make sure it increments
            IListBuiltFromSingleObject.Add(DummyObject.CreateDummyRecord());

            //check the count
            Assert.Equal(2, IListBuiltFromSingleObject.Count);
        }

        #endregion

    }

}