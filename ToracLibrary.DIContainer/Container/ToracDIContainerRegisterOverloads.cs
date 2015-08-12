using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.RegisteredObjects;

namespace ToracLibrary.DIContainer
{

    /// <summary>
    /// Holds all the register overloads. There are a good amount of overloads and I don't want to cloud the main logic in the container class
    /// </summary>
    public partial class ToracDIContainer
    {

        #region Register Overloads

        #region Only 1 Type (From And To)

        /* There are 4 parameters, so we need every iteration for the overload */

        #region Single Parmeters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        public void Register<TRegisterType>() where TRegisterType : class
        {
            Register<TRegisterType, TRegisterType>(DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        public void Register<TRegisterType>(string FactoryName) where TRegisterType : class
        {
            Register<TRegisterType, TRegisterType>(FactoryName, DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TRegisterType>(DIContainerScope ObjectScope) where TRegisterType : class
        {
            Register<TRegisterType, TRegisterType>(null, ObjectScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TRegisterType>(Func<TRegisterType> CreateConcreteImplementation) where TRegisterType : class
        {
            Register<TRegisterType, TRegisterType>(DefaultScope, CreateConcreteImplementation);
        }

        #endregion

        #region Two Parameters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TRegisterType>(string FactoryName, DIContainerScope ObjectScope) where TRegisterType : class
        {
            //add the item to our list
            Register<TRegisterType, TRegisterType>(FactoryName, ObjectScope, null);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TRegisterType>(string FactoryName, Func<TRegisterType> CreateConcreteImplementation) where TRegisterType : class
        {
            //add the item to our list
            Register<TRegisterType, TRegisterType>(FactoryName, DefaultScope, CreateConcreteImplementation);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TRegisterType>(DIContainerScope ObjectScope, Func<TRegisterType> CreateConcreteImplementation) where TRegisterType : class
        {
            //add the item to our list
            Register<TRegisterType, TRegisterType>(null, ObjectScope, CreateConcreteImplementation);
        }

        #endregion

        #region Three Parameters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TRegisterType">The type you will register and return when it gets resolved</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TRegisterType>(string FactoryName, DIContainerScope ObjectScope, Func<TRegisterType> CreateConcreteImplementation) where TRegisterType : class
        {
            Register<TRegisterType, TRegisterType>(FactoryName, ObjectScope, CreateConcreteImplementation);
        }

        #endregion

        #endregion

        #region From And To Type

        /* There are 4 parameters, so we need every iteration for the overload */

        #region Single Parmeters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        public void Register<TTypeToResolve, TConcrete>()
            where TTypeToResolve : class
            where TConcrete : class
        {
            Register<TTypeToResolve, TConcrete>(DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        public void Register<TTypeToResolve, TConcrete>(string FactoryName)
            where TTypeToResolve : class
            where TConcrete : class
        {
            Register<TTypeToResolve, TConcrete>(FactoryName, DefaultScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TTypeToResolve, TConcrete>(DIContainerScope ObjectScope)
            where TTypeToResolve : class
            where TConcrete : class
        {
            Register<TTypeToResolve, TConcrete>(null, ObjectScope);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TTypeToResolve, TConcrete>(Func<TConcrete> CreateConcreteImplementation)
            where TTypeToResolve : class
            where TConcrete : class
        {
            Register<TTypeToResolve, TConcrete>(DefaultScope, CreateConcreteImplementation);
        }

        #endregion

        #region Two Parameters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        public void Register<TTypeToResolve, TConcrete>(string FactoryName, DIContainerScope ObjectScope)
            where TTypeToResolve : class
            where TConcrete : class
        {
            //add the item to our list
            Register<TTypeToResolve, TConcrete>(FactoryName, ObjectScope, null);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TTypeToResolve, TConcrete>(string FactoryName, Func<TConcrete> CreateConcreteImplementation)
            where TTypeToResolve : class
            where TConcrete : class
        {
            //add the item to our list
            Register<TTypeToResolve, TConcrete>(FactoryName, DefaultScope, CreateConcreteImplementation);
        }

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TTypeToResolve, TConcrete>(DIContainerScope ObjectScope, Func<TConcrete> CreateConcreteImplementation)
            where TTypeToResolve : class
            where TConcrete : class
        {
            //add the item to our list
            Register<TTypeToResolve, TConcrete>(null, ObjectScope, CreateConcreteImplementation);
        }

        #endregion

        #region Three Parameters

        /// <summary>
        /// Register a dependency in the container
        /// </summary>
        /// <typeparam name="TTypeToResolve">Type Of T To Resolve</typeparam>
        /// <typeparam name="TConcrete">Type of the concrete class</typeparam>
        /// <param name="FactoryName">Name of the factory. Only necessary when you have registered 2 items of the same type. ie abstract factory</param>
        /// <param name="ObjectScope">Holds hold long an object lives in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        public void Register<TTypeToResolve, TConcrete>(string FactoryName, DIContainerScope ObjectScope, Func<TConcrete> CreateConcreteImplementation)
            where TTypeToResolve : class
            where TConcrete : class
        {
            //function to create to cast to object
            Func<object> ConcreteCreation = null;

            //do we have a function passed in?
            if (CreateConcreteImplementation != null)
            {
                //create a new function that returns an object...can't cast from TConcrete to object, so we create a func that calls the method which gets casted automatically
                ConcreteCreation = () => CreateConcreteImplementation();
            }

            //add the item to our list
            RegisteredObjectsInContainer.Add(BaseRegisteredObject.BuildRegisteredObject(FactoryName, typeof(TTypeToResolve), typeof(TConcrete), ObjectScope, ConcreteCreation));

            //we want to prevent them from adding multiple types so validate it when they input it (so we are going to run this method which validates everything)
            FindRegisterdObject(RegisteredObjectsInContainer, FactoryName, typeof(TTypeToResolve));
        }

        #endregion

        #endregion

        #endregion

    }

}
