using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Base registered object
    /// </summary>
    internal abstract class BaseRegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementationToSet">Function to create an concrete implementation</param>
        internal BaseRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementationToSet)
        {
            //set all the properties
            FactoryName = FactoryNameToSet;
            TypeToResolve = TypeToResolveToSet;
            ConcreteType = ConcreteTypeToSet;
            ObjectScope = ObjectScopeToSet;
            CreateConcreteImplementation = CreateConcreteImplementationToSet;

            //grab the construtor info
            var ConstructorInfo = ConcreteType.GetConstructors().First();

            // we are going to create a new instance everytime. We want to cache the constructor parameters so we don't have to keep getting it
            //even to for the singleton, we need them to register everything first. So we can't create the singleton as soon as they register it
            ConstructorInfoOfConcreteType = ConstructorInfo.GetParameters();

            //let's go set the activator func
            CachedActivator = ExpressionTreeHelpers.BuildNewObject(ConstructorInfo, ConstructorInfoOfConcreteType).Compile();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages
        /// </summary>
        internal string FactoryName { get; }

        /// <summary>
        /// Type to resolve. ie: ILogger
        /// </summary>
        internal Type TypeToResolve { get; }

        /// <summary>
        /// Implementation of the Type to resolve. ie: TextLogger
        /// </summary>
        internal Type ConcreteType { get; }

        /// <summary>
        /// We are going to store the constructor info of the concrete class. This way when we go to resolve it multiple times we can cache this. For singleton, we need to allow them to register everything first. So we need to store this for all cases
        /// </summary>
        internal ParameterInfo[] ConstructorInfoOfConcreteType { get; }

        /// <summary>
        /// Function to create an concrete implementation
        /// </summary>
        internal Func<object> CreateConcreteImplementation { get; }

        /// <summary>
        /// Instead of using Activator.CreateInstance, we are going to an expression tree to create a new object. This gets compiled on the first time we request the item
        /// </summary>
        internal Func<object[], object> CachedActivator { get; }

        /// <summary>
        /// How long does does the object last in the di container
        /// </summary>
        internal ToracDIContainer.DIContainerScope ObjectScope { get; }

        #endregion

        #region Abstract Properties

        /// <summary>
        ///  In a singleton pattern we will try to resolve the issue without creating it first. If this flag is set to true, then we will try to eager load the items
        /// </summary>
        abstract internal bool SupportsEagerCachingOfObjects { get; }

        #endregion

        #region Private Static Helpers

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        internal object CreateInstance(BaseRegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //use the activator and go create the instance
            //return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);

            //**so expression tree is slower if you are just running resolve a handful of times. You would need to get into the 10,000 resolves before it starts getting faster.
            //**since an asp.net mvc site will handle request after request the pool won't get recycled before 10,000. So we are going to build it for scalability with expression trees

            //instead of using activator, we are going to use an expression tree which is a ton faster.

            //so we are going to build a func that takes a params object[] and then we just set it to each item.

            //if we haven't already built the expression, then let's build and compile it now   

            //we have the expression, so let's go invoke it and return the results
            return CachedActivator.Invoke(ConstructorParameters);
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Builds a registered object. Figures out which implementation to use for IRegisteredObject
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        /// <returns>IRegisteredObject</returns>
        internal static BaseRegisteredObject BuildRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementation)
        {
            //which scope is it?
            if (ObjectScopeToSet == ToracDIContainer.DIContainerScope.Transient)
            {
                return new TransientRegisteredObject(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation);
            }

            //else return the singleton
            return new SingletonRegisteredObject(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation);
        }

        #endregion

        #region Internal Virtual Methods

        /// <summary>
        /// In a singleton pattern we will try to resolve the issue without creating it first. For transient this will return null
        /// </summary>
        /// <returns>null if the object needs to be created. Object if we have already created the object and we can use it</returns>
        internal virtual object EagerResolveObject()
        {
            //the default value is not to use a cache mechanism...so just return null
            return null;
        }

        /// <summary>
        /// Stores the instance for the any calls after this. Singleton pattern
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object to store</param>
        internal virtual void StoreInstance(object ObjectInstanceToStore)
        {
            //don't do anything by default
        }

        #endregion

    }

}
