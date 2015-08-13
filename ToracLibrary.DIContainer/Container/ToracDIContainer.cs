using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.Exceptions;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer
{

    /*Partial class so all the register overloads won't get in the way and cloud the code */

    /// <summary>
    /// Holds the actual di container that everything derives off of
    /// </summary>
    /// <remarks>Class is property immutable</remarks>
    public partial class ToracDIContainer
    {

        //**Register is a partial class in ToracDIContainerRegisterOverloads**

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public ToracDIContainer()
        {
            //create a new list to use, which will store all my settings
            RegisteredObjectsInContainer = new List<BaseRegisteredObject>();
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Holds all the registered objects for this DI container
        /// </summary>
        private List<BaseRegisteredObject> RegisteredObjectsInContainer { get; }

        #endregion

        #region Constants

        /// <summary>
        /// Holds the default scope to use if the overload doesn't specify one
        /// </summary>
        private const DIContainerScope DefaultScope = DIContainerScope.Transient;

        #endregion

        #region Enums

        /// <summary>
        /// Holds hold long an object lives in the di container
        /// </summary>
        public enum DIContainerScope : int
        {

            /// <summary>
            /// A new instance will always be returned from the container when being resolved. (Default)
            /// </summary>
            Transient = 0,

            /// <summary>
            /// Only 1 object will every be created. The same object will always be returned from the container when being resolved
            /// </summary>
            Singleton = 1
        }

        #endregion

        #region Resolve

        #region Generic Based

        /// <summary>
        /// Resolve a type of the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type to resolve</typeparam>
        /// <returns>The resolved type. TTypeToResolve</returns>
        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            //use the overload
            return Resolve<TTypeToResolve>(null);
        }

        /// <summary>
        /// Resolve a type of the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type to resolve</typeparam>
        /// <param name="FactoryName">Factory name if you provided one when registering the object</param>
        /// <returns>The resolved type. TTypeToResolve</returns>
        public TTypeToResolve Resolve<TTypeToResolve>(string FactoryName)
        {
            //use the non generic overload
            return (TTypeToResolve)Resolve(FactoryName, typeof(TTypeToResolve));
        }

        #endregion

        #region Non Generic Based

        /// <summary>
        /// Resolve a type of the container
        /// </summary>
        /// <typeparam name="TypeToResolve">Type to resolve</typeparam>
        /// <returns>The resolved type. TTypeToResolve</returns>
        public object Resolve(Type TypeToResolve)
        {
            //use the overload
            return Resolve(null, TypeToResolve);
        }

        /// <summary>
        /// Resolve a type of the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type to resolve</typeparam>
        /// <param name="FactoryName">Factory name if you provided one when registering the object</param>
        /// <returns>The resolved type. TTypeToResolve</returns>
        public object Resolve(string FactoryName, Type TypeToResolve)
        {
            //now go return an instance
            return GetInstance(FindRegisterdObject(RegisteredObjectsInContainer, FactoryName, TypeToResolve));
        }

        #endregion

        #endregion

        #region Resolve All

        /// <summary>
        /// returns all the factories for a given type
        /// </summary>
        /// <typeparam name="TTypeToResolve">type to resolve all the factories for</typeparam>
        /// <returns>Iterator of all the registered types for that specific implementation. Key is the factory name</returns>
        public IEnumerable<KeyValuePair<string, TTypeToResolve>> ResolveAllLazy<TTypeToResolve>()
        {
            //let's loop through all the types and return them
            foreach (var FactoryToResolve in RegisteredObjectsInContainer.Where(x => x.TypeToResolve == typeof(TTypeToResolve)))
            {
                //go resolve this type and yield it
                yield return new KeyValuePair<string, TTypeToResolve>(FactoryToResolve.FactoryName, Resolve<TTypeToResolve>(FactoryToResolve.FactoryName));
            }
        }

        #endregion

        #region Clear All Registrations

        /// <summary>
        /// Removes all registrations from the container for the specific type passed in
        /// </summary>
        /// <typeparam name="TTypeToResolve">The type that gets cleared. This should be the type that resolves to. ie. the interface</typeparam>
        public void ClearAllRegistrationsForSpecificType<TTypeToResolve>()
        {
            //go remove all the items that are this type.
            RegisteredObjectsInContainer.RemoveAll(x => x.TypeToResolve == typeof(TTypeToResolve));
        }

        /// <summary>
        /// Removes all registrations from the container
        /// </summary>
        public void ClearAllRegistrations()
        {
            //just clear the list
            RegisteredObjectsInContainer.Clear();
        }

        #endregion

        #region Get All Registrations

        /// <summary>
        /// Returns all the registrations for troubleshooting or just seeing what is registered in the container
        /// </summary>
        /// <returns>iterator of AllRegistrationResult</returns>
        public IEnumerable<AllRegistrationResult> AllRegistrationSelectLazy()
        {
            //loop through all the registered objects
            foreach (var FactoryToResolve in RegisteredObjectsInContainer)
            {
                //return the new object using yield
                yield return new AllRegistrationResult(FactoryToResolve.FactoryName, FactoryToResolve.ObjectScope, FactoryToResolve.TypeToResolve, FactoryToResolve.ConcreteType);
            }
        }

        #endregion

        #region Instance Fetching

        /// <summary>
        /// Get's the instance when we try to resolve this registered object
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>The object for the consumer to use</returns>
        private object GetInstance(BaseRegisteredObject RegisteredObjectToBuild)
        {
            //does this registered type support eager loading?
            if (RegisteredObjectToBuild.SupportsEagerCachingOfObjects)
            {
                //try to grab the instance without creating it
                var EagerResolveObject = RegisteredObjectToBuild.EagerResolveObject();

                //do we have an instance
                if (EagerResolveObject != null)
                {
                    //they want a singleton, so just return the instance we have stored
                    return EagerResolveObject;
                }
            }

            //object to return
            object ObjectToReturn;

            //first we will try to build it using the func
            if (RegisteredObjectToBuild.CreateConcreteImplementation == null)
            {
                //they never passed in the func, so go create an instance
                ObjectToReturn = RegisteredObjectToBuild.CreateInstance(RegisteredObjectToBuild, ResolveConstructorParameters(RegisteredObjectToBuild).ToArray());
            }
            else
            {
                //we have the func that creates the object, go invoke it and return the result
                ObjectToReturn = RegisteredObjectToBuild.CreateConcreteImplementation.Invoke();
            }

            //if we support eager loading, then go store this item
            if (RegisteredObjectToBuild.SupportsEagerCachingOfObjects)
            {
                //if this is a singleton, go store it
                RegisteredObjectToBuild.StoreInstance(ObjectToReturn);
            }

            //all done return the object
            return ObjectToReturn;
        }

        /// <summary>
        /// Resolved the constructor parameters and returns it in an array
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>Parameters to be fed into the constructor</returns>
        private IEnumerable<object> ResolveConstructorParameters(BaseRegisteredObject RegisteredObjectToBuild)
        {
            //let's loop through the paramters for this constructor
            foreach (var ConstructorParameter in RegisteredObjectToBuild.ConstructorInfoOfConcreteType)
            {
                //we are going to recurse through this and resolve until we have everything
                yield return Resolve(ConstructorParameter.ParameterType);
            }
        }

        #endregion

        #region Finding The Correct Registered Object

        /// <summary>
        /// Finds the correct registered object to resolve an item. Will validate everything based on parameters
        /// </summary>
        /// <param name="RegisteredObjectsInContainer">Registered objects in the container</param>
        /// <param name="FactoryName">Factory name if there are more</param>
        /// <param name="TypeToResolve">Type to resolve</param>
        /// <returns>BaseRegisteredObject. Null if not found</returns>
        /// <remarks>will throw errors if more then 1 registered object is found</remarks>
        private static BaseRegisteredObject FindRegisterdObject(IList<BaseRegisteredObject> RegisteredObjectsInContainer, string FactoryName, Type TypeToResolve)
        {
            //are we checking for factory names (let's cache this in a variable
            bool CheckingForFactoryNames = !string.IsNullOrEmpty(FactoryName);

            //easier and cleaner to write in linq
            var FoundRegisteredObjects = (from DataSet in RegisteredObjectsInContainer
                                          where DataSet.TypeToResolve == TypeToResolve
                                          && (CheckingForFactoryNames ? FactoryName == DataSet.FactoryName : true)
                                          select DataSet).ToArray();

            //did we find any items?
            if (!FoundRegisteredObjects.Any())
            {
                //throw an exception
                throw new TypeNotRegisteredException(TypeToResolve);
            }
            else if (FoundRegisteredObjects.Length != 1)
            {
                //do we have too many items registered? They didn't use factory names
                throw new MultipleTypesFoundException(TypeToResolve);
            }

            //we are ok, just return the first item
            return FoundRegisteredObjects[0];
        }

        #endregion

    }

}
