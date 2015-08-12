using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Singleton registered object
    /// </summary>
    internal class SingletonRegisteredObject : BaseRegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="InstanceToSet">Instance to use in the singleton</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        internal SingletonRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementation)
            : base(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation)
        {
        }

        #endregion

        #region Singleton Specific

        /// <summary>
        /// if they want a singleton, then we store the instance here
        /// </summary>
        internal object Instance { get; private set; }

        #endregion

        #region Override Methods

        /// <summary>
        /// In a singleton pattern we will try to resolve the issue without creating it first. For transient this will return null
        /// </summary>
        /// <returns>null if the object needs to be created. Object if we have already created the object and we can use it</returns>
        internal override object EagerResolveObject()
        {
            return Instance;
        }

        /// <summary>
        /// Stores the instance for the any calls after this. Singleton pattern
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object to store</param>
        internal override void StoreInstance(object ObjectInstanceToStore)
        {
            //set the instance property
            Instance = ObjectInstanceToStore;
        }

        #endregion

    }

}
