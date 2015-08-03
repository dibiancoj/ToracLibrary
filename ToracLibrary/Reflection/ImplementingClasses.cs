using System;
using System.Collections.Concurrent;
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

        /// <summary>
        /// Retrieve a list of classes that implement the interface passed in. Pass in typeof(MyInterface). This method gets parallized because the number of assemblies and types could be very large
        /// </summary>
        /// <param name="InterfaceType">typeof(MyInterface). Type of the interface you want to check</param>
        /// <returns>list of classes that implement this item. You can call Activator.CreateInstance(thisType) to create an instance of the type passed back.</returns>
        public static IEnumerable<Type> RetrieveImplementingClassesLazy(Type InterfaceType)
        {
            //let loop through all the assemblies
            foreach (var AssemblyToCheck in AppDomain.CurrentDomain.GetAssemblies())
            {
                //now loop through all the types in this assembly
                foreach (Type TypeInAssemblyToCheck in AssemblyToCheck.GetTypes())
                {
                    //make sure it can be assigned from the class you want and is an interface
                    if (InterfaceType.IsAssignableFrom(TypeInAssemblyToCheck) && !TypeInAssemblyToCheck.IsInterface)
                    {
                        //we have a match so return the type
                        yield return TypeInAssemblyToCheck;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve a list of classes that derive from this base class. This method gets parallized because the number of assemblies and types could be very large
        /// </summary>
        /// <param name="BaseClass">typeof(BaseClass). Type of the base class you want to check</param>
        /// <returns>list of classes that inherit this item. You can call Activator.CreateInstance(thisType) to create an instance of the type passed back.</returns>
        public static IEnumerable<Type> RetrieveDerivedClassesLazy(Type BaseClass)
        {
            //let loop through all the assemblies
            foreach (var AssemblyToCheck in AppDomain.CurrentDomain.GetAssemblies())
            {
                //now loop through all the types in this assembly
                foreach (Type TypeInAssemblyToCheck in AssemblyToCheck.GetTypes())
                {
                    //make sure it can be assigned from the class you want and is an interface
                    if (TypeInAssemblyToCheck.IsSubclassOf(BaseClass) && !TypeInAssemblyToCheck.IsInterface)
                    {
                        //we have a match so return the type
                        yield return TypeInAssemblyToCheck;
                    }
                }
            }
        }

    }

}
