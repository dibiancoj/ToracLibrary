using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer.ScopeImplementation
{

    /// <summary>
    /// A common interface that all scope implementations must implement
    /// </summary>
    internal interface IScopeImplementation
    {

        #region Properties

        /// <summary>
        /// Does this scope support eager loading of objects
        /// </summary>
        bool SupportsEagerCachingOfObjects { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Attempt to try to get the eager cached object
        /// </summary>
        /// <returns>object if found. Otherwise returns null</returns>
        object EagerResolveObject();

        /// <summary>
        /// Store the object if the scope supports it
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object instance to store</param>
        void StoreInstance(object ObjectInstanceToStore);

        /// <summary>
        /// Creates the actual instance when we are ready to resolve an object
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered object to build</param>
        /// <param name="ConstructorParameters">The parameters to pass into the constructor</param>
        /// <returns>The instance of the resolved object</returns>
        object CreateInstance(RegisteredUnTypedObject RegisteredObjectToBuild, params object[] ConstructorParameters);

        #endregion

    }

}
