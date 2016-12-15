using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.RegisteredObjects;
using ToracLibrary.DIContainer.ScopeImplementation.Implementation.CacheActivatorBase;

namespace ToracLibrary.DIContainer.ScopeImplementation
{

    /// <summary>
    /// Is a mix of a transient and a sington. Will hold a weak reference to the "singleton" object
    /// </summary>
    internal class PerThreadScopedObject : CacheActivatorBaseScopedObject, IScopeImplementation
    {

        // **** since this could be created many times depending on what the application does and how many times gc run's...we are going to use expression tree's to cache the activator ****

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConstructorToCreateObjectsWith">Constructor information to use to create the object with</param>
        internal PerThreadScopedObject(ConstructorInfo ConstructorToCreateObjectsWith)
            : base(ConstructorToCreateObjectsWith)

        {
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Holds the weak reference to the object
        /// </summary>
        private WeakReference Instance { get; set; }

        #endregion

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #region Interface Methods

        /// <summary>
        /// resolves an instance of this type
        /// </summary>
        /// <param name="Container">Container holding the registerd object</param>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>The resolved instance</returns>
        public object ResolveInstance(ToracDIContainer Container, RegisteredUnTypedObject RegisteredObjectToBuild)
        {
            //if we have a valid object and its still alive then return it
            if (Instance == null || !Instance.IsAlive)
            {
                //at this point we don't have a valid object, we need to create it and put it in the property
                Instance = new WeakReference(CachedActivator.Invoke(RegisteredObjectToBuild.ResolveConstructorParametersLazy(Container).ToArray()));
            }

            //now just return the instance's target.
            return Instance.Target;
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    //for singleton's instance that implement idisposable, we want to eagerly call dispose the object

                    //do we have an instance?
                    if (Instance != null)
                    {
                        //so we have an instance...now does it implement idisposable?
                        var IDisposeCheck = Instance as IDisposable;

                        //does it implement IDisposable?
                        if (IDisposeCheck != null)
                        {
                            //now call dispose on this object
                            IDisposeCheck.Dispose();
                        }
                    }
                }
            }
            this.Disposed = true;
        }

        #endregion

    }

}
