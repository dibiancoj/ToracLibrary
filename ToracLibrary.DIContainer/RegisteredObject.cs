using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer
{

    /// <summary>
    /// Holds the object to which every object type is stored internally to the di container
    /// </summary>
    /// <remarks>Class is property immutable</remarks>
    public class RegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNamToSete">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        public RegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet)
        {
            //set all the properties
            FactoryName = FactoryNameToSet;
            TypeToResolve = TypeToResolveToSet;
            ConcreteType = ConcreteTypeToSet;
            ObjectScope = ObjectScopeToSet;

            //now if this is a singleton, let's go create that object
            if (ObjectScope == ToracDIContainer.DIContainerScope.Singleton)
            {
                //go create the new instance
                Instance = GetInstance();
            }
            else
            {
                //we are going to create a new instance everytime. We want to cache the constructor parameters so we don't have to keep getting it
                ConstructorInfoOfConcreteType = ConcreteType.GetConstructors().First().GetParameters();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages
        /// </summary>
        internal string FactoryName { get; }

        /// <summary>
        /// Type to resolve. ie: ILogger
        /// </summary>
        internal Type TypeToResolve { get; }

        /// <summary>
        /// Implementation of the Type to resolve. ie: TextLogger
        /// </summary>
        internal Type ConcreteType { get; }

        #region Singleton Specific

        /// <summary>
        /// if they want a singleton, then we store the instance here
        /// </summary>
        private object Instance { get; }

        #endregion

        #region Transient Specific

        /// <summary>
        /// We are going to store the constructor info of the concrete class. This way when we go to resolve it multiple times we can cache this. Only for a transient object
        /// </summary>
        private ParameterInfo[] ConstructorInfoOfConcreteType { get; }

        #endregion

        /// <summary>
        /// How long does does the object last in the di container
        /// </summary>
        internal ToracDIContainer.DIContainerScope ObjectScope { get; }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Get's the instance when we try to resolve this registered object
        /// </summary>
        /// <param name="Container">Container so we can recurse through the di chain</param>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>The object for the consumer to use</returns>
        internal static object GetInstance(ToracDIContainer Container, RegisteredObject RegisteredObjectToBuild)
        {
            //is this a singleton or a transient
            if (RegisteredObjectToBuild.ObjectScope == ToracDIContainer.DIContainerScope.Singleton)
            {
                //they want a singleton, so just return the instance we have stored
                return RegisteredObjectToBuild.Instance;
            }

            //this is a transient...so they wan't a new object, let's go create it
            return CreateInstance(RegisteredObjectToBuild, ResolveConstructorParameters(Container, RegisteredObjectToBuild).ToArray());
        }

        #endregion

        #region Private Static Helpers

        /// <summary>
        /// Resolved the constructor parameters and returns it in an array
        /// </summary>
        /// <param name="Container">Container so we can recurse through the di chain</param>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <returns>Parameters to be fed into the constructor</returns>
        private static IEnumerable<object> ResolveConstructorParameters(ToracDIContainer Container, RegisteredObject RegisteredObjectToBuild)
        {
            //let's loop through the paramters for this constructor
            foreach (var ConstructorParameter in RegisteredObjectToBuild.ConstructorInfoOfConcreteType)
            {
                //we are going to recurse through this and resolve until we have everything
                yield return Container.Resolve(ConstructorParameter.ParameterType);
            }
        }

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        private static object CreateInstance(RegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //use the activator and go create the instance
            return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);
        }

        #endregion

    }

}
