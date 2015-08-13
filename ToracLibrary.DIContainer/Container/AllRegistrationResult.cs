using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer
{

    /// <summary>
    /// Holds a result when the GetAllRegistrationsMethod is called
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class AllRegistrationResult
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FactoryNameToSet"> Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages</param>
        /// <param name="ContainerScopeToSet">How long does does the object last in the di container</param>
        /// <param name="ConcreteTypeToSet">Implementation of the Type to resolve. ie: TextLogger</param>
        /// <param name="ObjectScopeToSet">Type to resolve. ie: ILogger</param>
        public AllRegistrationResult(string FactoryNameToSet, ToracDIContainer.DIContainerScope ObjectScopeToSet, Type TypeToResolveToSet, Type ConcreteTypeToSet)
        {
            //set all the properties
            FactoryName = FactoryNameToSet;
            TypeToResolve = TypeToResolveToSet;
            ConcreteType = ConcreteTypeToSet;
            ObjectScope = ObjectScopeToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique Identifier when you have the same types to resolve. Abstract Factory Pattern usages
        /// </summary>
        public string FactoryName { get; }

        /// <summary>
        /// Type to resolve. ie: ILogger
        /// </summary>
        public Type TypeToResolve { get; }

        /// <summary>
        /// Implementation of the Type to resolve. ie: TextLogger
        /// </summary>
        public Type ConcreteType { get; }

        /// <summary>
        /// How long does does the object last in the di container
        /// </summary>
        public ToracDIContainer.DIContainerScope ObjectScope { get; }

        #endregion

    }

}
