using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer.ScopeImplementation
{

    /// <summary>
    /// Singleton registered object
    /// </summary>
    internal class SingletonScopedObject : IScopeImplementation
    {

        #region Singleton Specific Properties

        /// <summary>
        /// if they want a singleton, then we store the instance here so we can reuse it
        /// </summary>
        private object Instance { get; set; }

        #endregion

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #region Interface Properties

        /// <summary>
        ///  In a singleton pattern we will try to resolve the issue without creating it first. If this flag is set to true, then we will try to eager load the items
        /// </summary>
        public bool SupportsEagerCachingOfObjects { get { return true; } }

        #endregion

        #region Interface Methods

        /// <summary>
        /// In a singleton pattern we will try to resolve the issue without creating it first. For transient this will return null
        /// </summary>
        /// <returns>null if the object needs to be created. Object if we have already created the object and we can use it</returns>
        public object EagerResolveObject()
        {
            //just return the instance that we store
            return Instance;
        }

        /// <summary>
        /// Stores the instance for the any calls after this. Singleton pattern
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object to store</param>
        public void StoreInstance(object ObjectInstanceToStore)
        {
            //set the instance property
            Instance = ObjectInstanceToStore;
        }

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        public object CreateInstance(RegisteredUnTypedObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //**so expression tree is slower if you are just running resolve a handful of times. You would need to get into the 10,000 resolves before it starts getting faster.
            //**since an asp.net mvc site will handle request after request the pool won't get recycled before 10,000. So we are going to build it for scalability with expression trees

            //instead of using activator, we are going to use an expression tree which is a ton faster.

            //so we are going to build a func that takes a params object[] and then we just set it to each item.

            //if we haven't already built the expression, then let's build and compile it now   

            //singleton will only create it once, so singleton's will use the regular activator because it won't benefit of creating the object once. The cost
            //of the expression tree compile is too hight.

            //we have the expression, so let's go invoke it and return the results
            return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);
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
