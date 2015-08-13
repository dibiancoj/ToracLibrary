using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.RegisteredObjects
{

    /// <summary>
    /// Singleton registered object
    /// </summary>
    internal class SingletonRegisteredObject : BaseRegisteredObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet">Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="TypeToResolveToSet">Type to resolve. ie: ILogger</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="InstanceToSet">Instance to use in the singleton</param>
        /// <param name="ObjectScopeToSet">How long does does the object last in the di container</param>
        /// <param name="CreateConcreteImplementation">Function to create an concrete implementation</param>
        internal SingletonRegisteredObject(string FactoryNameToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Func<object> CreateConcreteImplementation)
            : base(FactoryNameToSet, TypeToResolveToSet, ConcreteTypeToSet, ObjectScopeToSet, CreateConcreteImplementation)
        {
        }

        #endregion

        #region Singleton Specific Properties

        /// <summary>
        /// if they want a singleton, then we store the instance here
        /// </summary>
        internal object Instance { get; private set; }

        #endregion

        #region Abstract Properties

        /// <summary>
        ///  In a singleton pattern we will try to resolve the issue without creating it first. If this flag is set to true, then we will try to eager load the items
        /// </summary>
        override internal bool SupportsEagerCachingOfObjects { get { return true; } }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Don't build the cached activator. Singleton's will only build the object once so expression tree's will be slower with the compile time cost.
        /// </summary>
        /// <param name="ConstructorInfo">Constructor Info for the concrete class</param>
        /// <param name="ConstructorParameters">Constructor parameters and what needs to be passed into the constructor when creating a new object</param>
        /// <returns>The cached activator. Null if the derived class doesn't want to implement it</returns>
        internal override Func<object[], object> ConfigureTheCachedActivator(ConstructorInfo ConstructorInfo, IEnumerable<ParameterInfo> ConstructorParameters)
        {
            //we don't want this for singleton's. Cost of expression tree makes it slower for a single instance creation
            return null;
        }

        /// <summary>
        /// In a singleton pattern we will try to resolve the issue without creating it first. For transient this will return null
        /// </summary>
        /// <returns>null if the object needs to be created. Object if we have already created the object and we can use it</returns>
        internal override object EagerResolveObject()
        {
            return Instance;
        }

        /// <summary>
        /// Stores the instance for the any calls after this. Singleton pattern
        /// </summary>
        /// <param name="ObjectInstanceToStore">Object to store</param>
        internal override void StoreInstance(object ObjectInstanceToStore)
        {
            //set the instance property
            Instance = ObjectInstanceToStore;
        }

        /// <summary>
        /// create an instance of this type
        /// </summary>
        /// <param name="RegisteredObjectToBuild">Registered Object To Get The Instance Of</param>
        /// <param name="ConstructorParameters">Constructor Parameters</param>
        override internal object CreateInstance(BaseRegisteredObject RegisteredObjectToBuild, params object[] ConstructorParameters)
        {
            //**so expression tree is slower if you are just running resolve a handful of times. You would need to get into the 10,000 resolves before it starts getting faster.
            //**since an asp.net mvc site will handle request after request the pool won't get recycled before 10,000. So we are going to build it for scalability with expression trees

            //instead of using activator, we are going to use an expression tree which is a ton faster.

            //so we are going to build a func that takes a params object[] and then we just set it to each item.

            //if we haven't already built the expression, then let's build and compile it now   

            //singleton will only create it once, so singleton's will use the regular activator because it won't benefit of creating the object once. The cost
            //of the expression tree compile is too hight.

            //we have the expression, so let's go invoke it and return the results
            return Activator.CreateInstance(RegisteredObjectToBuild.ConcreteType, ConstructorParameters);
        }

        #endregion

    }

}
