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
        internal TransientRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet)
            : base(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet)
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// In a singleton pattern we will try to resolve the issue without creating it first. For transient this will return null
        /// </summary>
        /// <returns>null if the object needs to be created. Object if we have already created the object and we can use it</returns>
        internal override object EagerResolveObject()
        {
            return null;
        }

        #endregion

    }
}
