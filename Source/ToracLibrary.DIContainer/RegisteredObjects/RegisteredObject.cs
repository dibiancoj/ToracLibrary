using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Holds a fluent typed configuration object that will be used to build off of
    /// </summary>
    /// <typeparam name="TTypeToResolveToSet">Type to resolve. Usually an interface</typeparam>
    /// <typeparam name="TConcrete">Concrete type to create. Usually a class that implements TTypeToResolveToSet.</typeparam>
    public class RegisteredObject<TTypeToResolveToSet, TConcrete> : RegisteredUnTypedObject
        where TTypeToResolveToSet : class
        where TConcrete : class
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="AttachedDIContainer">The container the parameter is in. This way we can set the factory name after we add it to the dictionary</param>
        public RegisteredObject(ToracDIContainer.DIContainerScope ObjectScopeToSet, ToracDIContainer AttachedDIContainer)
            : base(ObjectScopeToSet, typeof(TTypeToResolveToSet), typeof(TConcrete))
        {
            //set the container
            Container = AttachedDIContainer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Holds the container so we can modify the container from a configuration value
        /// </summary>
        private ToracDIContainer Container { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Add a factory name to a configuration record. Use this when implementing the same type multiple times. ie. abstract factory. This allows you to return a specific instance using a name
        /// </summary>
        /// <param name="FactoryNameToSet">Factory name to use.</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithFactoryName(string FactoryNameToSet)
        {
            //grab the resolve type
            var ResolveType = typeof(TTypeToResolveToSet);

            //grab the factory name before we set it, so we can cache the tuple hash so we can remove it from the container correctly
            var CurrentEntryInContainer = new Tuple<string, Type>(FactoryName, ResolveType);

            //set the factory name
            FactoryName = FactoryNameToSet;

            //we need to adjust the container dictionary key. 
            Container.RegisteredObjectsInContainer.Remove(CurrentEntryInContainer);

            //now re-add it
            Container.RegisteredObjectsInContainer.Add(new Tuple<string, Type>(FactoryName, ResolveType), this);

            //return the object so we can chain it
            return this;
        }

        /// <summary>
        /// Add a specific function that will be used to create an object instance. This gives the developer full control of how objects are created
        /// </summary>
        /// <param name="CreateObjectWith">A lambda which returns the new object</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithConstructorImplementation(Func<ToracDIContainer, TConcrete> CreateObjectWith)
        {
            //set the lambda
            CreateObjectWithThisConstructor = CreateObjectWith;

            //return the object so we can chain it
            return this;
        }

        /// <summary>
        /// Passes in a specific set of constructor parameters when creating the new object
        /// </summary>
        /// <param name="ConstructorParametersToUse">Constructor parameters to pass in when creating a new object</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithConstructorParameters(params IConstructorParameter[] ConstructorParametersToUse)
        {
            //set the parameters to use
            CreateObjectWithConstructorParameters = ConstructorParametersToUse;

            //return the object so we can chain it
            return this;
        }

        /// <summary>
        /// Picks the constructor to use when you have an overloaded constructor based on the types passed in
        /// </summary>
        /// <param name="ParameterTypesToPickTheConstructorWith">The contructor types to use to find the overloaded constructor method</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithConstructorOverload(params Type[] ParameterTypesToPickTheConstructorWith)
        {
            //we need to reset a few things. First thing we need to do is find the constructor we want to use
            var ConstructorToUse = ConcreteType.GetConstructor(ParameterTypesToPickTheConstructorWith);

            //make sure we were able to find a constructor with those types
            if (ConstructorToUse == null)
            {
                //throw an error because we can't find those types
                throw new ArgumentNullException("ConstructorToUse", "Can't Find A Constructor With The Types Passed In");
            }

            //now reset the constructor parameters
            ConcreteConstructorParameters = ConstructorToUse.GetParameters();

            //now go rebuild the scope implementation
            ScopeImplementation = CreateScopeImplementation(ObjectScope, ConstructorToUse);

            //return the object so we can chain it
            return this;
        }

        #endregion

    }

}
