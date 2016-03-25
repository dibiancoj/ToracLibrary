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
            //create a new list to use, which will store all my settings (dictionary key is the "factoryname" & "concrete type / type to resolve")
            RegisteredObjectsInContainer = new Dictionary<Tuple<string, Type>, RegisteredUnTypedObject>();
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Holds all the registered objects for this DI container. Dictionary key is the "factoryname" & "concrete type / type to resolve"
        /// </summary>
        internal Dictionary<Tuple<string, Type>, RegisteredUnTypedObject> RegisteredObjectsInContainer { get; }

        #endregion

        #region Constants

        /// <summary>
        /// Holds the default scope to use if the overload doesn't specify one
        /// </summary>
        public const DIContainerScope DefaultScope = DIContainerScope.Transient;

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
            Singleton = 1,

            /// <summary>
            /// Holds a weak reference to the object. So a mix between a transient and a singleton.
            /// </summary>
            PerThreadLifetime = 2

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
            foreach (var FactoryToResolve in RegisteredObjectsInContainer.Values.Where(x => x.TypeToResolve == typeof(TTypeToResolve)))
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
            //go remove all the items that are this type. Pushing this to a list will avoid the "removing while enumerating problem"
            foreach (var ItemToRemove in RegisteredObjectsInContainer.Where(x => x.Key.Item2 == typeof(TTypeToResolve)).ToList())
            {
                RegisteredObjectsInContainer.Remove(new Tuple<string, Type>(ItemToRemove.Key.Item1, ItemToRemove.Key.Item2));
            }
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
            //use the overload and pass in null
            return AllRegistrationSelectLazy(null);
        }

        /// <summary>
        /// Returns all the registrations for a specific type 
        /// </summary>
        /// <param name="ResolveTypeToFilterFor">Resolve type to only filter by</param>
        /// <returns>iterator of AllRegistrationResult</returns>
        public IEnumerable<AllRegistrationResult> AllRegistrationSelectLazy(Type ResolveTypeToFilterFor)
        {
            //do we want to resolve a type?
            bool LookForASpecificType = ResolveTypeToFilterFor != null;

            //loop through all the registered objects
            foreach (var FactoryToResolve in RegisteredObjectsInContainer.Where(x => LookForASpecificType ? x.Key.Item2 == ResolveTypeToFilterFor : true))
            {
                //return the new object using yield
                yield return new AllRegistrationResult(FactoryToResolve.Value.FactoryName, FactoryToResolve.Value.ObjectScope, FactoryToResolve.Value.TypeToResolve, FactoryToResolve.Value.ConcreteType);
            }
        }

        #endregion

        #region Instance Fetching

        /// <summary>
        /// Get's the instance when we try to resolve this registered object
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>The object for the consumer to use</returns>
        private object GetInstance(RegisteredUnTypedObject RegisteredObjectToBuild)
        {
            //first we will try to build it using the func
            if (RegisteredObjectToBuild.CreateObjectWithThisConstructor == null)
            {
                //they never passed in the func, so go create an instance
                return RegisteredObjectToBuild.ScopeImplementation.ResolveInstance(RegisteredObjectToBuild, RegisteredObjectToBuild.ResolveConstructorParametersLazy(this).ToArray());
            }

            //we have the func that creates the object, go invoke it and return the result
            return RegisteredObjectToBuild.CreateObjectWithThisConstructor.Invoke(this);
        }

        #endregion

        #region Finding The Correct Registered Object

        /// <summary>
        /// Finds the correct registered object to resolve an item. Will validate everything based on parameters
        /// </summary>
        /// <param name="RegisteredObjectsInContainer">Registered objects in the container</param>
        /// <param name="FactoryName">Factory name if there are more</param>
        /// <param name="TypeToResolve">Type to resolve</param>
        /// <returns>BaseRegisteredObject. Throws an TypeNotRegisteredException exception if no item is found</returns>
        private static RegisteredUnTypedObject FindRegisterdObject(IDictionary<Tuple<string, Type>, RegisteredUnTypedObject> RegisteredObjectsInContainer, string FactoryName, Type TypeToResolve)
        {
            //configuration to try to get from the dictionary
            RegisteredUnTypedObject TryToGetConfiguration;

            //try to fetch the configuration
            if (RegisteredObjectsInContainer.TryGetValue(new Tuple<string, Type>(FactoryName, TypeToResolve), out TryToGetConfiguration))
            {
                //we found the item, just return it
                return TryToGetConfiguration;
            }

            //**** so you are asking how do we get down there... ****

            //the only child in the container has a factory name. So if we have 1 and only 1 item with a factory name we need to resolve the child. 

            //Container1.Register<Child>().WithFactoryName("F1");
            //Container1.Register<Tester>();

            //class Tester
            //{
            //      Tester(Child child)
            //}
            //don't remove the code below

            //if we still can't find it, check just for the type. This scenario would be a secondary type doesn't know which factory name it's set with
            //are we checking for factory names (let's cache this in a variable

            //go start the query and try to grab the specific type we are looking for
            var FoundRegisteredObjectsQuery = RegisteredObjectsInContainer.Where(x => x.Key.Item2 == TypeToResolve).AsQueryable();

            //do we have a factory name we want to search with?
            if (!string.IsNullOrEmpty(FactoryName))
            {
                //add to the where clause
                FoundRegisteredObjectsQuery = FoundRegisteredObjectsQuery.Where(x => x.Key.Item1 == FactoryName);
            }

            //now go grab the results
            //why are we taking 2...we don't want to run through every object in the container. We really just care if there are 0 items...or if there are more then 2 items
            var FoundRegisteredObjectsResults = FoundRegisteredObjectsQuery.Select(x => x.Value).Take(2).ToArray();

            //did we find any items? (decided to use length since it's a property rather then Any() which will have to create an enumerator)
            if (FoundRegisteredObjectsResults.Length == 0)
            {
                //throw an exception
                throw new TypeNotRegisteredException(TypeToResolve);
            }

            //do we have more then 1? we need to blow this up and tell the calling code that we have more then 1 type found.
            if (FoundRegisteredObjectsResults.Length != 1)
            {
                //do we have too many items registered? They didn't use factory names
                throw new MultipleTypesFoundException(TypeToResolve);
            }

            //we have a found registered object
            return FoundRegisteredObjectsResults[0];
        }

        #endregion

    }

}
