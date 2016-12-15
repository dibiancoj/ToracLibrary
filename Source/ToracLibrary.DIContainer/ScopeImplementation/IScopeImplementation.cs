﻿using System;
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
    internal interface IScopeImplementation : IDisposable
    {

        #region Methods

        /// <summary>
        /// Creates the actual instance when we are ready to resolve an object
        /// </summary>
        /// <param name="Container">Container holding the registerd object</param>
        /// <param name="RegisteredObjectToBuild">Registered object to build</param>
        /// <returns>The instance of the resolved object</returns>
        object ResolveInstance(ToracDIContainer Container, RegisteredUnTypedObject RegisteredObjectToBuild);

        #endregion

    }

}
