using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ToracLibrary.Core.ReflectionDynamic.Invoke.GenericStaticMethodFinder;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Method finder for a generic instance methods
    /// </summary>
    public class GenericInstanceMethodFinder : IMethodTypeFinder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InstanceThatContainsMethodToSet">The instance that contains the method which we want to invoke</param>
        /// <param name="MethodNameToFind">Method name to find</param>
        /// <param name="GenericMethodTypes">The types for the generic method</param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method. If there are no parameters in the method then pass in null or an empty array</param>
        public GenericInstanceMethodFinder(object InstanceThatContainsMethodToSet, string MethodNameToFind, IEnumerable<Type> GenericMethodTypesToUse, IList<GenericTypeParameter> ParametersToPassToMethod)
        {
            InstanceThatContainsMethod = InstanceThatContainsMethodToSet;
            MethodName = MethodNameToFind;
            GenericMethodTypes = GenericMethodTypesToUse;
            ParametersOfMethod = ParametersToPassToMethod ?? Array.Empty<GenericTypeParameter>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of the class that contains the method
        /// </summary>
        private object InstanceThatContainsMethod { get; }

        /// <summary>
        /// Method name to find
        /// </summary>
        private string MethodName { get; }

        /// <summary>
        /// The types that make up the generic method.
        /// </summary>
        private IEnumerable<Type> GenericMethodTypes { get; }

        /// <summary>
        /// Parameters to pass into the method. If there are no parameters in the method then pass in null or an empty array
        /// </summary>
        private IList<GenericTypeParameter> ParametersOfMethod { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Find the method that we can invoke dynamically. The method will be fully configured so invoke can be called
        /// </summary>
        /// <returns>MethodInfo ready to be invoke at run time with everything configured</returns>
        public MethodInfo FindMethodToInvoke()
        {
            return new GenericStaticMethodFinder(InstanceThatContainsMethod.GetType(), MethodName, GenericMethodTypes, ParametersOfMethod).FindMethodToInvoke();
        }

        #endregion

    }

}
