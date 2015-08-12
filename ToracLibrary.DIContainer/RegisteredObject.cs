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
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
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

            //now if this is a Transient, let's go cache the constructor parameters
            if (ObjectScope == ToracDIContainer.DIContainerScope.Transient)
            {
                // we are going to create a new instance everytime.We want to cache the constructor parameters so we don't have to keep getting it
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
        internal object Instance { get; set; }

        #endregion

        #region Transient Specific

        /// <summary>
        /// We are going to store the constructor info of the concrete class. This way when we go to resolve it multiple times we can cache this. Only for a transient object
        /// </summary>
        internal ParameterInfo[] ConstructorInfoOfConcreteType { get; }

        #endregion

        /// <summary>
        /// How long does does the object last in the di container
        /// </summary>
        internal ToracDIContainer.DIContainerScope ObjectScope { get; }

        #endregion

        #region Private Static Helpers

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        internal static object CreateInstance(RegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //use the activator and go create the instance
            return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);
        }

        #endregion

    }

}
