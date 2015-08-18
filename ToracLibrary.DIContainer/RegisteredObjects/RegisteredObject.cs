using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RegisteredObject(ToracDIContainer.DIContainerScope ObjectScopeToSet)
            : base(ObjectScopeToSet, typeof(TTypeToResolveToSet), typeof(TConcrete))
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a factory name to a configuration record. Use this when implementing the same type multiple times. ie. abstract factory. This allows you to return a specific instance using a name
        /// </summary>
        /// <param name="FactoryNameToSet">Factory name to use.</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithFactoryName(string FactoryNameToSet)
        {
            //set the factory name
            FactoryName = FactoryNameToSet;

            //return the object so we can chain it
            return this;
        }

        /// <summary>
        /// Add a specific function that will be used to create an object instance. This gives the developer full control of how objects are created
        /// </summary>
        /// <param name="CreateObjectWith">A lambda which returns the new object</param>
        /// <returns>The typed RegisteredObject which you can build off of</returns>
        public RegisteredObject<TTypeToResolveToSet, TConcrete> WithConstructorImplementation(Func<TConcrete> CreateObjectWith)
        {
            //set the lambda
            CreateObjectWithThisConstructor = CreateObjectWith;

            //return the object so we can chain it
            return this;
        }

        #endregion

    }

}
