using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Registered Object for a transient
    /// </summary>
    internal class TransientRegisteredObject : BaseRegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        internal TransientRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementation)
            : base(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation)
        {
        }

        #endregion

        #region Abstract Properties

        /// <summary>
        ///  In a singleton pattern we will try to resolve the issue without creating it first. If this flag is set to true, then we will try to eager load the items
        /// </summary>
        override internal bool SupportsEagerCachingOfObjects { get { return false; } }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Build the cache activator. Transients will build a new object every time resolved, so we want a fast expression tree. So we cache the expression to be used
        /// </summary>
        /// <param name="ConstructorInfo">Constructor Info for the concrete class</param>
        /// <param name="ConstructorParameters">Constructor parameters and what needs to be passed into the constructor when creating a new object</param>
        /// <returns>The cached activator. Null if the derived class doesn't want to implement it</returns>
        internal override Func<object[], object> ConfigureTheCachedActivator(ConstructorInfo ConstructorInfo, IEnumerable<ParameterInfo> ConstructorParameters)
        {
            //let's go set the activator func
            return ExpressionTreeHelpers.BuildNewObject(ConstructorInfo, ConstructorInfoOfConcreteType).Compile();
        }

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        override internal object CreateInstance(BaseRegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //use the activator and go create the instance
            //return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);

            //**so expression tree is slower if you are just running resolve a handful of times. You would need to get into the 10,000 resolves before it starts getting faster.
            //**since an asp.net mvc site will handle request after request the pool won't get recycled before 10,000. So we are going to build it for scalability with expression trees

            //instead of using activator, we are going to use an expression tree which is a ton faster.

            //so we are going to build a func that takes a params object[] and then we just set it to each item.

            //if we haven't already built the expression, then let's build and compile it now   

            //transients will benefit from the expression tree. singleton will only create it once, so singleton's will use the regular activator

            //we have the expression, so let's go invoke it and return the results
            return CachedActivator.Invoke(ConstructorParameters);
        }

        #endregion

    }

}
