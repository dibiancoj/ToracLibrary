﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer.ScopeImplementation
{

    /// <summary>
    /// Registered Object for a transient
    /// </summary>
    internal class TransientScopedObject : IScopeImplementation
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConstructorToCreateObjectsWith">Constructor information to use to create the object with</param>
        internal TransientScopedObject(ConstructorInfo ConstructorToCreateObjectsWith)
        {
            //go create the cached activator. With the fluent style we dont know if they will pass in there own constructor lambda. so we just build this each time. This is cached only when the app starts so it isn't a performance issue
            CachedActivator = ExpressionTreeHelpers.BuildNewObject(ConstructorToCreateObjectsWith, ConstructorToCreateObjectsWith.GetParameters()).Compile();
        }

        #endregion

        #region Private Transient Specific Properties

        /// <summary>
        /// Instead of using Activator.CreateInstance, we are going to an expression tree to create a new object. This gets compiled on the first time we request the item
        /// </summary>
        private Func<object[], object> CachedActivator { get; }

        #endregion

        #region Interface Properties

        /// <summary>
        /// We don't want to support caching of objects since they want a new instance of each resolve call
        /// </summary>
        public bool SupportsEagerCachingOfObjects { get { return false; } }

        #endregion

        #region Interface Methods

        /// <summary>
        /// We don't implement this interface so throw an exception if this gets called. We want a new instance each time we resolve an issue
        /// </summary>
        /// <returns>null</returns>
        public object EagerResolveObject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// We don't want to cache the object. We want a new instance each time we resolve an issue
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object instance to store</param>
        public void StoreInstance(object ObjectInstanceToStore)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        public object CreateInstance(RegisteredUnTypedObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //use the activator and go create the instance
            //return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);

            //**so expression tree is slower if you are just running resolve a handful of times. You would need to get into the 10,000 resolves before it starts getting faster.
            //**since an asp.net mvc site will handle request after request the pool won't get recycled before 10,000. So we are going to build it for scalability with expression trees

            //instead of using activator, we are going to use an expression tree which is a ton faster.

            //so we are going to build a func that takes a params object[] and then we just set it to each item.

            //if we haven't already built the expression, then let's build and compile it now   

            //transients will benefit from the expression tree. singleton will only create it once, so singleton's will use the regular activator

            //we have the expression, so let's go invoke it and return the results
            return CachedActivator.Invoke(ConstructorParameters);
        }

        #endregion

    }

}