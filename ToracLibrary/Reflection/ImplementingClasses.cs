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
    public class ImplementingClasses
    {

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
            //let loop through all the assemblies
            foreach (var AssemblyToCheck in AppDomain.CurrentDomain.GetAssemblies())
            {
                //now loop through all the types in this assembly (ignore interfaces)
                foreach (Type TypeInAssemblyToCheck in AssemblyToCheck.GetTypes().Where(x => !x.IsInterface))
                {
                    //if we are checking for interfaces then see if it's assignable from
                    if (InterfaceOrBaseClass.IsInterface && InterfaceOrBaseClass.IsAssignableFrom(TypeInAssemblyToCheck))
                    {
                        //we are testing for interfaces (we have a match)
                        yield return TypeInAssemblyToCheck;
                    }
                    else if (TypeInAssemblyToCheck.IsSubclassOf(InterfaceOrBaseClass))
                    {
                        //we are testing for base classes (we have a match)
                        yield return TypeInAssemblyToCheck;
                    }
                }
            }
        }

        #endregion

    }

}
