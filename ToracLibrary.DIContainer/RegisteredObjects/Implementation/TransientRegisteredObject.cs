using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Registered Object for a transient
    /// </summary>
    internal class TransientRegisteredObject : BaseRegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        internal TransientRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementation)
            : base(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation)
        {
        }

        #endregion

        #region Abstract Properties

        /// <summary>
        ///  In a singleton pattern we will try to resolve the issue without creating it first. If this flag is set to true, then we will try to eager load the items
        /// </summary>
        override internal bool SupportsEagerCachingOfObjects { get { return false; } }

        #endregion

    }

}
