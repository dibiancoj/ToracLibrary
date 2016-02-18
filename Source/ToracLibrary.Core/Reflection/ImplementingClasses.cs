using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic
{

    /// <summary>
    /// Retrieve the classes that implement an interface
    /// </summary>
    public static class ImplementingClasses
    {

        #region Private Static Methods

        /// <summary>
        /// Holds the function that tests that the type passed in inherits from the TestThatItInherits From
        /// </summary>
        private static readonly Func<Type, Type, bool> TypeInheritsFromClass = (TypeToCheck, TestThatItInheritsFromThisClass) => TypeToCheck.IsSubclassOf(TestThatItInheritsFromThisClass);

        /// <summary>
        /// Holds the function that tests that the type to check implements the interface to test.
        /// </summary>
        private static readonly Func<Type, Type, bool> TypeImplementsInterface = (TypeToCheck, TestInterfaceItMustImplement) => TestInterfaceItMustImplement.IsAssignableFrom(TypeToCheck);

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieve a list of classes that implement the interface or base class passed in. Pass in typeof(MyInterface || MyBaseClass).
        /// </summary>
        /// <typeparam name="T">typeof(MyInterface || MyBaseClass). Type of the interface or base classyou want to check</typeparam>
        /// <returns>list of types (classes) that implement or derived from InterfaceOrBaseClass. You can call Activator.CreateInstance(thisType) to create an instance of the type passed back.</returns>
        public static IEnumerable<Type> RetrieveImplementingClassesLazy<T>()
        {
            //use the overload
            return RetrieveImplementingClassesLazy(typeof(T));
        }

        /// <summary>
        /// Retrieve a list of classes that implement the interface or base class passed in. Pass in typeof(MyInterface || MyBaseClass).
        /// </summary>
        /// <param name="InterfaceOrBaseClass">typeof(MyInterface || MyBaseClass). Type of the interface or base classyou want to check</param>
        /// <returns>list of types (classes) that implement or derived from InterfaceOrBaseClass. You can call Activator.CreateInstance(thisType) to create an instance of the type passed back.</returns>
        public static IEnumerable<Type> RetrieveImplementingClassesLazy(Type InterfaceOrBaseClass)
        {
            //are we checking for an interface or a base class
            var MatchesCriteriaFunc = InterfaceOrBaseClass.IsInterface ? TypeImplementsInterface : TypeInheritsFromClass;

            //let loop through all the assemblies
            foreach (var AssemblyToCheck in AppDomain.CurrentDomain.GetAssemblies())
            {
                //now loop through all the types in this assembly (ignore interfaces)
                foreach (Type TypeInAssemblyToCheck in AssemblyToCheck.GetTypes().Where(x => !x.IsInterface && MatchesCriteriaFunc(x, InterfaceOrBaseClass)))
                {
                    //we have a match. This either implements the interface or inherits from the base class
                    yield return TypeInAssemblyToCheck;
                }
            }
        }

        #endregion

    }

}
