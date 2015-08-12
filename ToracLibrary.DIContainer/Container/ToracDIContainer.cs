using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.Exceptions;

namespace ToracLibrary.DIContainer
{

    /// <summary>
    /// Holds the actual di container that everything derives off of
    /// </summary>
    /// <remarks>Class is property immutable</remarks>
    public class ToracDIContainer
    {

        #region Constructor

        /// <summary>
        /// constructor
        /// </summary>
        public ToracDIContainer()
        {
            //create a new list to use, which will store all my settings
            RegisteredObjectsInContainer = new List<RegisteredObject>();
        }

        #endregion

        #region Readonly Properties

        /// <summary>
        /// Holds all the registered objects for this DI container
        /// </summary>
        private IList<RegisteredObject> RegisteredObjectsInContainer { get; }

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

        #region Register

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        public void Register<TTypeToResolve, TConcrete>()
        {
            Register<TTypeToResolve, TConcrete>(DIContainerScope.Transient);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TTypeToResolve, TConcrete>(DIContainerScope ObjectScope)
        {
            Register<TTypeToResolve, TConcrete>(Guid.NewGuid().ToString(), ObjectScope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TTypeToResolve, TConcrete>(string FactoryName, DIContainerScope ObjectScope)
        {
            //add the item to our list
            RegisteredObjectsInContainer.Add(new RegisteredObject(FactoryName, typeof(TTypeToResolve), typeof(TConcrete), ObjectScope));
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
            //holds the object we are going to use
            RegisteredObject RegisteredObjectToUse = null;

            //let's grab the registered object in the list that we have
            var SearchForRegisteredObject = RegisteredObjectsInContainer.Where(x => x.TypeToResolve == TypeToResolve);

            //now if they have a factory name use it
            if (string.IsNullOrEmpty(FactoryName))
            {
                //we don't have a factory name, just grab the first record
                RegisteredObjectToUse = SearchForRegisteredObject.FirstOrDefault();
            }
            else
            {
                RegisteredObjectToUse = SearchForRegisteredObject.FirstOrDefault(x => x.FactoryName == FactoryName);
            }

            //make sure we found the registered object
            if (RegisteredObjectToUse == null)
            {
                //throw an exception
                throw new TypeNotRegisteredException(TypeToResolve);
            }

            //now go return an instance
            return GetInstance(RegisteredObjectToUse);
        }

        #endregion

        #endregion

        #region Instance Fetching

        /// <summary>
        /// Get's the instance when we try to resolve this registered object
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>The object for the consumer to use</returns>
        private object GetInstance(RegisteredObject RegisteredObjectToBuild)
        {
            //is this a singleton
            bool IsSingleton = RegisteredObjectToBuild.ObjectScope == DIContainerScope.Singleton;

            //is this a singleton and we already created an object
            if (IsSingleton && RegisteredObjectToBuild.Instance != null)
            {
                //they want a singleton, so just return the instance we have stored
                return RegisteredObjectToBuild.Instance;
            }

            //this is a transient...so they wan't a new object, let's go create it
            var ObjectToReturn = RegisteredObject.CreateInstance(RegisteredObjectToBuild, ResolveConstructorParameters(RegisteredObjectToBuild).ToArray());

            //if this is a singleton, go store it
            if (IsSingleton)
            {
                RegisteredObjectToBuild.Instance = ObjectToReturn;
            }

            //all done return the object
            return ObjectToReturn;
        }

        /// <summary>
        /// Resolved the constructor parameters and returns it in an array
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>Parameters to be fed into the constructor</returns>
        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject RegisteredObjectToBuild)
        {
            //let's loop through the paramters for this constructor
            foreach (var ConstructorParameter in RegisteredObjectToBuild.ConstructorInfoOfConcreteType)
            {
                //we are going to recurse through this and resolve until we have everything
                yield return Resolve(ConstructorParameter.ParameterType);
            }
        }

        #endregion

    }

}
