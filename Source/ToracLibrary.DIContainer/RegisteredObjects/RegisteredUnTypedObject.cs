using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.DIContainer.Parameters.ConstructorParameters;
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
            ConcreteConstructorParameters = ConstructorInfoToUse.GetParameters();

            //depending on which scope to use, create the implementation
            ScopeImplementation = CreateScopeImplementation(ObjectScope, ConstructorInfoToUse);
        }

        #endregion

        #region Immutable Properties

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

        #endregion

        #region Fluent Mutable Properties

        /// <summary>
        /// Factory name if it was ever set. Null if never set
        /// </summary>
        protected internal string FactoryName { get; protected set; }

        /// <summary>
        /// User controlled object creation lambda. The typed RegisteredObject makes sure it is type of TConcrete
        /// </summary>
        protected internal Func<ToracDIContainer, object> CreateObjectWithThisConstructor { get; protected set; }

        /// <summary>
        /// Build the new object with the constructor parameters specifiedf
        /// </summary>
        protected internal IConstructorParameter[] CreateObjectWithConstructorParameters { get; protected set; }

        /// <summary>
        /// The scope implementation that is tied to this configuration. We use this for the "rules" for the scope set
        /// </summary>
        internal IScopeImplementation ScopeImplementation { get; set; }

        /// <summary>
        /// Constructor parameters for the concrete class. This way we can cache it and re-use it
        /// </summary>
        protected internal ParameterInfo[] ConcreteConstructorParameters { get; protected set; }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get the constructor parameters to pass into the constructor
        /// </summary>
        /// <param name="DIContainer">Container to build off of</param>
        /// <returns>Parameters to pass into the constructor</returns>
        internal IEnumerable<object> ResolveConstructorParametersLazy(ToracDIContainer DIContainer)
        {
            //loop through the parameters and yield it (i don't want an interator inside interator so ResolveWhichConstructorParametersToImplement will not be lazy
            foreach (var ConstructorParameter in ResolveWhichConstructorParametersToImplement())
            {
                //return this value
                yield return ConstructorParameter.GetParameterValue(DIContainer);
            }
        }

        /// <summary>
        /// Grabs the constructor parameters that we want to pass in to build the object
        /// </summary>
        /// <returns>IEnumerable of IConstructorParameter</returns>
        private IEnumerable<IConstructorParameter> ResolveWhichConstructorParametersToImplement()
        {
            //do we have constructor parameters specifically passed in? So the user said i want these variables passed into the constructor
            if (CreateObjectWithConstructorParameters.AnyWithNullCheck())
            {
                //use the specific parameters the user specified
                return CreateObjectWithConstructorParameters;
            }

            //we are going to create the constructor parameters to resolve
            return ConcreteConstructorParameters.Select(x => new ResolveTypeNonGenericCtorParameter(x.ParameterType)).ToArray();
        }

        /// <summary>
        /// Builds and returns the concrete implementation of the IScopeImplementation.
        /// </summary>
        /// <param name="Scope">Scope of the object to use</param>
        /// <param name="ConstructorToUse">Constructor to use to create the concrete type</param>
        /// <returns>new IScopeImplementation implementation</returns>
        internal IScopeImplementation CreateScopeImplementation(ToracDIContainer.DIContainerScope Scope, ConstructorInfo ConstructorToUse)
        {
            //which scope is it?
            if (Scope == ToracDIContainer.DIContainerScope.Singleton)
            {
                return new SingletonScopedObject();
            }

            if (Scope == ToracDIContainer.DIContainerScope.PerThreadLifetime)
            {
                return new PerThreadScopedObject(ConstructorToUse);
            }

            return new TransientScopedObject(ConstructorToUse);
        }

        #endregion

    }

}
