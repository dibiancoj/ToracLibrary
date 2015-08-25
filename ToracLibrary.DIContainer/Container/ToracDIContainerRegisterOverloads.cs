using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.Exceptions;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer
{

    /// <summary>
    /// Holds all the register overloads. There are a good amount of overloads and I don't want to cloud the main logic in the container class
    /// </summary>
    public partial class ToracDIContainer
    {

        #region Single Type Registrations

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        public RegisteredObject<TRegisterType, TRegisterType> Register<TRegisterType>() where TRegisterType : class
        {
            return Register<TRegisterType, TRegisterType>(DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public RegisteredObject<TRegisterType, TRegisterType> Register<TRegisterType>(DIContainerScope ObjectScope) where TRegisterType : class
        {
            return Register<TRegisterType, TRegisterType>(ObjectScope);
        }

        #endregion

        #region From And To Registrations

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        public RegisteredObject<TTypeToResolve, TConcrete> Register<TTypeToResolve, TConcrete>()
            where TTypeToResolve : class
            where TConcrete : class
        {
            return Register<TTypeToResolve, TConcrete>(DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public RegisteredObject<TTypeToResolve, TConcrete> Register<TTypeToResolve, TConcrete>(DIContainerScope ObjectScope)
            where TTypeToResolve : class
            where TConcrete : class
        {
            //go build the base configuration object. This is a typed object
            var ConfiguredObject = new RegisteredObject<TTypeToResolve, TConcrete>(ObjectScope, this);

            //let's build the tuple so we don't need to re-create it twice
            var Hash = new Tuple<string, Type>(ConfiguredObject.FactoryName, ConfiguredObject.TypeToResolve);

            //make sure we don't already have this key
            if (RegisteredObjectsInContainer.ContainsKey(Hash))
            {
                //we have multiple types, throw an error
                throw new MultipleTypesFoundException(typeof(TTypeToResolve));
            }

            //add the item to our list (we used an untyped version so we can mix and match in the generic list)
            RegisteredObjectsInContainer.Add(Hash, ConfiguredObject);

            //we want to prevent them from adding multiple types so validate it when they input it (so we are going to run this method which validates everything)
            //FindRegisterdObject(RegisteredObjectsInContainer, null, typeof(TTypeToResolve));

            //return the typed object
            return ConfiguredObject;
        }

        #endregion

    }

}
