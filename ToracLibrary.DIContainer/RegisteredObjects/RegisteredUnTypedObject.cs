using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.DIContainer.ScopeImplementation;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Holds a untyped configuration object that will be used to resolve and register objects internally
    /// </summary>
    /// <remarks>Some properties are immutable. The fluent style properties are not</remarks>
    public class RegisteredUnTypedObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ObjectScopeToSet">Scope of the object to use</param>
        /// <param name="TypeToResolveToSet">Type to resolve. Usually an interface</param>
        /// <param name="ConcreteTypeToSet">Concrete type to create. Usually a class that implements TTypeToResolveToSet.</param>
        public RegisteredUnTypedObject(ToracDIContainer.DIContainerScope ObjectScopeToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet)
        {
            //set the immutable properties
            TypeToResolve = TypeToResolveToSet;
            ConcreteType = ConcreteTypeToSet;
            ObjectScope = ObjectScopeToSet;

            //grab the constructor info
            var ConstructorInfoToUse = ConcreteType.GetConstructors().First();

            //grab the constructor parameters and store them
            ConstructorInfoOfConcreteType = ConstructorInfoToUse.GetParameters();

            //depending on which scope to use, create the implementation
            if (ObjectScopeToSet == ToracDIContainer.DIContainerScope.Singleton)
            {
                ScopeImplementation = new SingletonScopedObject();
            }
            else
            {
                ScopeImplementation = new TransientScopedObject(ConstructorInfoToUse);
            }
        }

        #endregion

        #region Immutable Properties

        /// <summary>
        /// The scope implementation that is tied to this configuration. We use this for the "rules" for the scope set
        /// </summary>
        internal IScopeImplementation ScopeImplementation { get; }

        /// <summary>
        /// Type to resolve. ie: ILogger
        /// </summary>
        internal Type TypeToResolve { get; }

        /// <summary>
        /// Implementation of the Type to resolve. ie: TextLogger
        /// </summary>
        internal Type ConcreteType { get; }

        /// <summary>
        /// How long does does the object last in the di container
        /// </summary>
        internal ToracDIContainer.DIContainerScope ObjectScope { get; }

        /// <summary>
        /// We are going to store the constructor info of the concrete class. This way when we go to resolve it multiple times we can cache this. For singleton, we need to allow them to register everything first. So we need to store this for all cases
        /// </summary>
        internal ParameterInfo[] ConstructorInfoOfConcreteType { get; }

        #endregion

        #region Fluent Mutable Properties

        /// <summary>
        /// Factory name if it was ever set. Null if never set
        /// </summary>
        protected internal string FactoryName { get; protected set; }

        /// <summary>
        /// User controlled object creation lambda. The typed RegisteredObject makes sure it is type of TConcrete
        /// </summary>
        protected internal Func<object> CreateObjectWithThisConstructor { get; protected set; }

        #endregion

    }

}
