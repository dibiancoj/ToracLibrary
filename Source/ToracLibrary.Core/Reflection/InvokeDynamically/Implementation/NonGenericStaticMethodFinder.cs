using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Reflection.InvokeDynamically.Base;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Method finder for a non generic static methods
    /// </summary>
    public class NonGenericStaticMethodFinder : BaseMethodTypeFinder, IMethodTypeFinder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ClassTypeThatContainsMethod">The type of the class that contains the method</param>
        /// <param name="MethodNameToFind">Method name to find</param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method. If there are no parameters in the method then pass in null or an empty array</param>
        public NonGenericStaticMethodFinder(Type ClassTypeThatContainsMethod, string MethodNameToFind, IEnumerable<Type> ParametersToPassToMethod)
        {
            ClassType = ClassTypeThatContainsMethod;
            MethodName = MethodNameToFind;
            ParametersOfMethod = ParametersToPassToMethod ?? Array.Empty<Type>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of the class that contains the method
        /// </summary>
        private Type ClassType { get; }

        /// <summary>
        /// Method name to find
        /// </summary>
        private string MethodName { get; }

        /// <summary>
        /// Parameters to pass into the method. If there are no parameters in the method then pass in null or an empty array
        /// </summary>
        private IEnumerable<Type> ParametersOfMethod { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Find the method that we can invoke dynamically. The method will be fully configured so invoke can be called
        /// </summary>
        /// <returns>MethodInfo ready to be invoke at run time with everything configured</returns>
        public MethodInfo FindMethodToInvoke()
        {
            var t = ClassType.GetMethods().Where(x => x.Name == MethodName).ToArray();

            return ClassType.GetMethods().First(x => x.Name == MethodName && ParameterTypesMatch(ParametersOfMethod.Select(y => new GenericTypeParameter(y, false)).ToList(), x.GetParameters()));
        }

        #endregion

    }

}
