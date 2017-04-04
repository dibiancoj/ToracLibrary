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
    /// Method finder for a generic static methods
    /// </summary>
    public class GenericStaticMethodFinder : BaseMethodTypeFinder, IMethodTypeFinder
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ClassTypeThatContainsMethod">The type of the class that contains the method</param>
        /// <param name="MethodNameToFind">Method name to find</param>
        /// <param name="GenericMethodTypes">The types for the generic method</param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method. If there are no parameters in the method then pass in null or an empty array</param>
        public GenericStaticMethodFinder(Type ClassTypeThatContainsMethod, string MethodNameToFind, IEnumerable<Type> GenericMethodTypesToUse, IList<GenericTypeParameter> ParametersToPassToMethod)
        {
            ClassType = ClassTypeThatContainsMethod;
            MethodName = MethodNameToFind;
            GenericMethodTypes = GenericMethodTypesToUse;
            ParametersOfMethod = ParametersToPassToMethod ?? Array.Empty<GenericTypeParameter>();
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
            //generic parameter is where Method<T>(T object> ...so T is passed in
            if (ParametersOfMethod.Any(x => x.IsGenericType))
            {
                //there is a generic parameter in this method
                foreach (var MethodInClass in ClassType.GetMethods().Where(x => x.IsGenericMethod && x.Name == MethodName))
                {
                    //parameters in this method
                    var ParametersInMethod = MethodInClass.GetParameters();

                    //make sure we have the same number of parameters
                    if (ParametersInMethod.Length != ParametersOfMethod.Count)
                    {
                        //parameters don't match. Skip this method because we don't have a match
                        continue;
                    }

                    //do the parameters match?
                    if (ParameterTypesMatch(ParametersOfMethod, ParametersInMethod))
                    {
                        //we have a match...make this a generic method and return it
                        return MethodInClass.MakeGenericMethod(GenericMethodTypes.ToArray());
                    }
                }

                //we never found a match..throw the exception
                throw new ArgumentOutOfRangeException("Can't Find Method To Use");
            }

            //no generic parameters use the regular overload
            return new NonGenericStaticMethodFinder(ClassType, MethodName, ParametersOfMethod.Select(x => x.ParameterType)).FindMethodToInvoke().MakeGenericMethod(GenericMethodTypes.ToArray());
        }

        #endregion

    }

}
