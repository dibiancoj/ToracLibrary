using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Invoke a method dynamically
    /// </summary>
    public static class InvokeDynamically
    {

        #region General Invoke (Public Methods)

        /// <summary>
        /// Invoke a method and return it's result. Use this overload if you don't need parameters
        /// </summary>
        /// <param name="ClassType">Class Type. TypeOf(thisClassWithMethod)</param>
        /// <param name="MethodName">Method Name To Call</param>
        /// <param name="IsInstanceMethod">Does this method need an instance (static method or instance method)</param>
        /// <returns>System.Object</returns>
        /// <remarks>Will throw an error if it can't find it</remarks>
        public static object InvokeMethod(Type ClassType, string MethodName, bool IsInstanceMethod)
        {
            //use the overload and just pass in null for method parameters
            return InvokeMethodHelper(ClassType, MethodName, IsInstanceMethod, null, null);
        }

        /// <summary>
        /// Invoke a method and return it's result. Use this overload if you need parameters
        /// </summary>
        /// <param name="ClassType">Class Type. TypeOf(thisClassWithMethod)</param>
        /// <param name="MethodName">Method Name To Call</param>
        /// <param name="IsInstanceMethod">Does this method need an instance (static method or instance method)</param>
        /// <param name="MethodParameters">Array Of Objects For Parameters To Pass Into The Method</param>
        /// <returns>System.Object</returns>
        /// <remarks>Will throw an error if it can't find it</remarks>
        public static object InvokeMethod(Type ClassType, string MethodName, bool IsInstanceMethod, IEnumerable<object> MethodParameters)
        {
            return InvokeMethodHelper(ClassType, MethodName, IsInstanceMethod, null, MethodParameters.Select(x => new InvokeDynamicallyParameter(x)).ToArray());
        }

        /// <summary>
        /// Use this overload when we want to call an overloaded method. (we need to know the parameter types) - can't do a gettype of each parameter because they could be null. Then we wont be able to get the type
        /// </summary>
        /// <param name="ClassType">Class Type. TypeOf(thisClassWithMethod)</param>
        /// <param name="MethodName">Method Name To Call</param>
        /// <param name="IsInstanceMethod">Does this method need an instance (static method or instance method)</param>
        /// <param name="MethodParameters">Array Of Parameter Types And Values. Types Can Be Null If We Aren't Calling An Overload. Will be validated</param>
        /// <returns>System.Object</returns>
        /// <remarks>Will throw an error if it can't find it</remarks>
        public static object InvokeMethod(Type ClassType, string MethodName, bool IsInstanceMethod, IEnumerable<InvokeDynamicallyParameter> MethodParameters)
        {
            //use the overload
            return InvokeMethodHelper(ClassType, MethodName, IsInstanceMethod, null, MethodParameters);
        }

        #endregion

        #region General Generic Type Invoked (Public Methods)

        /// <summary>
        /// Invoke a generic method which takes a data type of T. When the type is not known at compile time. Use this overload when the method does not have any parameters
        /// </summary>
        /// <param name="ClassType">Class Type Which Contains The Method You Want To Call. ie typeof(DataProvider)</param>
        /// <param name="MethodName">Method Name To Call Which Is Located In The Class From The ClassType Parameter. ie DataProvider.Fetch (Fetch)</param>
        /// <param name="TypeOfTList">List Of Generics If More Than 1. Type which you want the parameter to be. So the methodof T,x,Y, etc....this is type of T/></param>
        /// <returns>Object</returns>
        public static object InvokeGenericMethod(Type ClassType, string MethodName, IEnumerable<Type> TypeOfTList)
        {
            //use the helper method
            return InvokeMethodHelper(ClassType, MethodName, false, TypeOfTList, null);
        }

        /// <summary>
        /// Invoke a generic method which takes a data type of T. When the type is not known at compile time. Use this overload when the method does not have any parameters and you are not calling an overloaded method
        /// </summary>
        /// <param name="ClassType">Class Type Which Contains The Method You Want To Call. ie typeof(DataProvider)</param>
        /// <param name="MethodName">Method Name To Call Which Is Located In The Class From The ClassType Parameter. ie DataProvider.Fetch (Fetch)</param>
        /// <param name="TypeOfTList">List Of Generics If More Than 1. Type which you want the parameter to be. So the methodof T,x,Y, etc....this is type of T/></param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method</param>
        /// <returns>Object</returns>
        public static object InvokeGenericMethod(Type ClassType, string MethodName, IEnumerable<Type> TypeOfTList, IEnumerable<object> ParametersToPassToMethod)
        {
            //use the helper method
            return InvokeMethodHelper(ClassType, MethodName, false, TypeOfTList, ParametersToPassToMethod.Select(x => new InvokeDynamicallyParameter(x)).ToArray());
        }

        /// <summary>
        /// Invoke a generic method which takes a data type of T. When the type is not known at compile time. Use this overload when you are calling a generic overloaded method. Each parameter must contain it's data type
        /// </summary>
        /// <param name="ClassType">Class Type Which Contains The Method You Want To Call. ie typeof(DataProvider)</param>
        /// <param name="MethodName">Method Name To Call Which Is Located In The Class From The ClassType Parameter. ie DataProvider.Fetch (Fetch)</param>
        /// <param name="TypeOfTList">List Of Generics If More Than 1. Type which you want the parameter to be. So the methodof T,x,Y, etc....this is type of T/></param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method. Contains Parameter types and values. Types can we can call the correct overloaded method</param>
        /// <returns>Object</returns>
        public static object InvokeGenericMethod(Type ClassType, string MethodName, IEnumerable<Type> TypeOfTList, IEnumerable<InvokeDynamicallyParameter> ParametersToPassToMethod)
        {
            //use the helper method
            return InvokeMethodHelper(ClassType, MethodName, false, TypeOfTList, ParametersToPassToMethod);
        }

        #endregion

        #region Private Main Helper Methods

        /// <summary>
        /// Invoke a method at run time. This helper accepts a list of TypeOfTList for generic methods
        /// </summary>
        /// <param name="ClassType">Class Type Which Contains The Method You Want To Call. ie typeof(DataProvider)</param>
        /// <param name="IsInstanceMethod">Does this method need an instance (static method or instance method)</param>
        /// <param name="MethodName">Method Name To Call Which Is Located In The Class From The ClassType Parameter. ie DataProvider.Fetch (Fetch)</param>
        /// <param name="TypeOfTList">List Of Generics If More Than 1. Type which you want the parameter to be. So the methodof T,x,Y, etc....this is type of T/></param>
        /// <param name="ParametersToPassToMethod">Parameters to pass into the method. Contains parameter value and type. Type could be null if we aren't calling an overloaded method</param>
        /// <returns>Object</returns>
        private static object InvokeMethodHelper(Type ClassType, string MethodName, bool IsInstanceMethod, IEnumerable<Type> TypeOfTList, IEnumerable<InvokeDynamicallyParameter> ParametersToPassToMethod)
        {
            //if TypeOfTList is null, then we just invoke the regular method

            //scenario when to use this method...(pass in TypeOfTList)
            //if we have a method
            //public void SaveRecord<T>(T thisObject)
            //{

            //}

            //we have an item which we can't type [based on arch. of the app] ie. its of object type (ParameterToPassToMethod) ie. object ParameterToPassToMethod
            //we want to pass it into the generic method with the actual type of that time which is determined by TypeOfParameterOfMethod

            //this basically came into play with the EntityFrameworkDP...we were calling "Add" and passing in an object (which had an underlying class of Class 1)
            //when entity framework went to save it it was looking at the model with the type of object...so it wasn't figuring out the real data type under the hood
            //i was not able to type the parameter passed in based on the project and what we had...So i needed to invoke the method where "T" was the real type and not an object
            //so we invoked the method with the underlying data type which we had...but I'm sure you could figure out using reflection.

            //Instance of the class if we need it
            object InstanceOfClass = null;

            //grab the method from the class
            MethodInfo MethodInfoToRun = null;

            //holds the parameter types if we are using an overload
            Type[] ParameterTypes = null;

            //holds the parameters to pass into the method
            object[] ParametersIntoMethod = null;

            //check to see if we have any parameters first
            if (ParametersToPassToMethod.AnyWithNullCheck())
            {
                //grab the parameter types
                ParameterTypes = InvokeDynamicallyParameter.ParameterTypesSelect(ParametersToPassToMethod).ToArray();

                //go set the parameters
                ParametersIntoMethod = InvokeDynamicallyParameter.ParameterValuesSelect(ParametersToPassToMethod).ToArray();
            }

            //if it's not a static method (needs an instance then create a default instance)
            if (IsInstanceMethod)
            {
                //create the intance
                InstanceOfClass = Activator.CreateInstance(ClassType);

                //do we have any parameters
                if (ParameterTypes.AnyWithNullCheck())
                {
                    //now grab the method to run
                    MethodInfoToRun = InstanceOfClass.GetType().GetMethod(MethodName, ParameterTypes);
                }
                else
                {
                    //now grab the method to run
                    MethodInfoToRun = InstanceOfClass.GetType().GetMethod(MethodName);
                }            
            }
            //let's grab the method now
            else if (ParameterTypes.AnyWithNullCheck())
            {
                //we have an overloaded method we wan't to call
                MethodInfoToRun = ClassType.GetMethod(MethodName, ParameterTypes);
            }
            else
            {
                //no overloaded method
                MethodInfoToRun = ClassType.GetMethod(MethodName);
            }

            //just make sure we found a method
            if (MethodInfoToRun == null)
            {
                throw new Exception("Method Not Found");
            }

            //is this a generic method?
            if (TypeOfTList.AnyWithNullCheck())
            {
                //go grab the methd info with this generic type (create the generic method type)
                var GenericMethodInfo = MethodInfoToRun.MakeGenericMethod(TypeOfTList.ToArray());

                //just make sure we found a method
                if (GenericMethodInfo == null)
                {
                    throw new Exception("Generic Method Not Found");
                }

                //go invoke the method
                return GenericMethodInfo.Invoke(ParametersToPassToMethod, ParametersIntoMethod);
            }

            //this is a regular method, now go invoke the method and return the result
            return MethodInfoToRun.Invoke(InstanceOfClass, ParametersIntoMethod);
        }

        #endregion

    }

}
