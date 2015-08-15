using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            // we are going to create a new instance everytime. We want to cache the constructor parameters so we don't have to keep getting it
            //even to for the singleton, we need them to register everything first. So we can't create the singleton as soon as they register it
            ConstructorInfoOfConcreteType = ConcreteType.GetConstructors().First().GetParameters();
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

        #region Internal Abstract Methods

        /// <summary>
        /// create an instance of this type. Each implementation will create there own. Transient will make use of expression trees (multiple creations). Singleton will only every create a single instance so the expression tree cost won't be beneificial
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        internal abstract object CreateInstance(BaseRegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters);

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
