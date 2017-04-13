using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ReflectionDynamic.Invoke.BaseClasses;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Method finder for a non generic instance methods
    /// </summary>
    public class NonGenericInstanceMethodFinder : BaseMethodTypeFinder, IMethodTypeFinder
    {

        #region Constructor

        /// <summary>
        /// Constructor helper
        /// </summary>
        /// <param name="InstanceOfClassThatContainsTheMethod">The instance of the class that contains the method. Ie: instance of class1 which contains the method we want to run</param>
        /// <param name="MethodNameToFind">Method name to find</param>
        /// <param name="ParametersToPassToMethod">Parameters of the method. Pass in null or empty array if there are no parameters</param>
        public NonGenericInstanceMethodFinder(object InstanceOfClassThatContainsTheMethod, string MethodNameToFind, IEnumerable<Type> ParametersToPassToMethod)
        {
            InstanceOfClass = InstanceOfClassThatContainsTheMethod;
            MethodName = MethodNameToFind;
            ParametersOfMethod = ParametersToPassToMethod ?? Array.Empty<Type>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The instance of the class that contains the method. Ie: instance of class1 which contains the method we want to run. This will only be populated based on the constructor overload you use
        /// </summary>
        private object InstanceOfClass { get; set; }

        /// <summary>
        /// Method name to find
        /// </summary>
        private string MethodName { get; }

        /// <summary>
        /// Parameters of the method
        /// </summary>
        private IEnumerable<Type> ParametersOfMethod { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Find the method that we can invoke dynamically. The method will be fully configured so invoke can be called
        /// </summary>
        /// <returns>MethodInfo ready to be invoke at run time with everything configured/returns>
        public MethodInfo FindMethodToInvoke()
        {
            //grab the type from the instance and grab the method
            return new NonGenericStaticMethodFinder(InstanceOfClass.GetType(), MethodName, ParametersOfMethod).FindMethodToInvoke();
        }

        #endregion

    }

}
