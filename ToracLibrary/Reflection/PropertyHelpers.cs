using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic
{

    /// <summary>
    /// Helper class dealing with properties in a reflection - dynamic manner
    /// </summary>
    public static class PropertyHelpers
    {

        #region Property Info Is Nullable<T>

        /// <summary>
        /// Is the property we pass in a nullable Of T..ie int?, int64?
        /// </summary>
        /// <param name="PropertyToCheck">Property To Check</param>
        /// <returns>It is nullable of t</returns>
        /// <remarks>to get the underlying data type ie int32 or date use thisProperty.PropertyType.GetGenericArguments()[0]</remarks>
        public static bool IsNullableOfT(PropertyInfo PropertyToCheck)
        {
            //to get the underlying type...int 32, or date, etc.
            //thisProperty.PropertyType.GetGenericArguments()[0]

            //go check the generic type
            return PropertyToCheck.PropertyType.IsGenericType && PropertyToCheck.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        #endregion

        #region Property Info Is Collection

        /// <summary>
        /// Checks the property and returns if it is an IEnumerable (A Collection)
        /// </summary>
        /// <param name="PropertyToCheck">Property Info To Check</param>
        /// <returns>Result If It's A Collection</returns>
        public static bool PropertyInfoIsIEnumerable(PropertyInfo PropertyToCheck)
        {
            //we need to exclude strings because they implement IEnumerable...
            return (PropertyToCheck.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(PropertyToCheck.PropertyType));
        }

        #endregion

        #region Get Sub Property - Traverses The Tree

        /// <summary>
        /// Gets all the property info's for the path passed in. This supports sub objects. ie. Property1.Property2.
        /// </summary>
        /// <param name="TypeOfT">Type of the object to start travesing down the tree</param>
        /// <param name="PropertyPath">Property Path To Traverse Down Too. Use "." for property paths</param>
        /// <returns>List of property info (lazy loaded using yield)</returns>
        public static IEnumerable<PropertyInfo> GetSubPropertiesLazy(Type TypeOfT, string PropertyPath)
        {
            //holds the working property info
            PropertyInfo WorkingPropertyInfo = null;

            //loop through all the properties
            foreach (string LevelToTraverse in PropertyPath.Split('.'))
            {
                //remove the .
                string LevelName = LevelToTraverse.Replace(".", string.Empty);

                //do we the working property set?
                if (WorkingPropertyInfo == null)
                {
                    //grab the item
                    WorkingPropertyInfo = TypeOfT.GetProperty(LevelName);
                }
                else
                {
                    //set the working property on the sub object
                    WorkingPropertyInfo = WorkingPropertyInfo.PropertyType.GetProperty(LevelName);
                }

                //always yield the working property as it has changed
                yield return WorkingPropertyInfo;
            }
        }

        #endregion

        #region Get Value Using Expression Trees At Run Time

        //the compile is the thing that takes the longest, if you can cache the func for the property name...then it will be 100x faster.
        //the more items the more the cost of the compile will be tolerable. Property Info would be faster if you have a few items and you can't cache the func for some reason

        /// <summary>
        /// Get a property value which is multiple level deeps. Call this method so you can reuse it and call the function multiple times to be more efficient
        /// </summary>
        /// <typeparam name="T">Type Of Object To Look In</typeparam>
        /// <typeparam name="TProperty">Type Of The Property That Will Be Returned</typeparam>
        /// <param name="PropertyPath">Property Path. thisObject.NestedObject.Id. If you just pass in Id for single level, it will handle it too</param>
        /// <returns>Expression to be used to select the property. See method below to see how to call this</returns>
        /// <remarks>You can pass in Object as TProperty If You Don't Know It. Property Path Can Handle Nested Child Properties</remarks>
        public static Expression<Func<T, TProperty>> GetPropertyOfObjectExpressionFunc<T, TProperty>(string PropertyPath)
        {
            //use the overload
            return GetPropertyOfObjectExpressionFunc<T, TProperty>(GetSubPropertiesLazy(typeof(T), PropertyPath));
        }

        /// <summary>
        /// Get a property value which is multiple level deeps. Call this method so you can reuse it and call the function multiple times to be more efficient
        /// </summary>
        /// <typeparam name="T">Type Of Object To Look In</typeparam>
        /// <typeparam name="TProperty">Type Of The Property That Will Be Returned</typeparam>
        /// <param name="PropertyPaths">Property Path. thisObject.NestedObject.Id. If you just pass in Id for single level, it will handle it too. Contains all the property infos</param>
        /// <returns>Expression to be used to select the property. See method below to see how to call this</returns>
        /// <remarks>You can pass in Object as TProperty If You Don't Know It. Property Path Can Handle Nested Child Properties</remarks>
        public static Expression<Func<T, TProperty>> GetPropertyOfObjectExpressionFunc<T, TProperty>(IEnumerable<PropertyInfo> PropertyPaths)
        {
            //example of how to call this
            //var t = PropertyHelpers.GetSubObjectProperty<Jason, int>("NestedObject.Id").Compile();
            //var result = t(jay);

            //let's grab the member expression stuff
            var ExpressionParametersWeNeed = GetLambdaParametersForPropertyPaths(typeof(T), PropertyPaths);

            //if its an object then convert the item, this helps if you only have a property path string or you truly don't know the data type...instead of looping through the object in reflection just conver it
            //convert the value to an object so we can use an int and cast it to an object and have the compiler figure out the type
            if (typeof(TProperty) == typeof(object))
            {
                //convert the member expression to an object, build the lambda, and return it
                return Expression.Lambda<Func<T, TProperty>>(Expression.Convert(ExpressionParametersWeNeed.Item1, typeof(object)), ExpressionParametersWeNeed.Item2);
            }

            //go build up the lambda expression with the real data type passed in and return it
            return Expression.Lambda<Func<T, TProperty>>(ExpressionParametersWeNeed.Item1, ExpressionParametersWeNeed.Item2);
        }

        /// <summary>
        /// Get the member and parameter expression for the sub property paths passed in
        /// </summary>
        /// <param name="TypeOfT">Type Of T</param>
        /// <param name="PropertyPaths">Property paths to loop through to grab the expression members</param>
        /// <returns>tuple of member expression and parameter expression</returns>
        public static Tuple<MemberExpression, ParameterExpression> GetLambdaParametersForPropertyPaths(Type TypeOfT, IEnumerable<PropertyInfo> PropertyPaths)
        {
            //declare the lambda argument
            ParameterExpression LambdaArgument = Expression.Parameter(TypeOfT, "x");

            //holds the working member expression
            MemberExpression WorkingMemberExpression = null;

            //let's loop through the properties and build up the expression (we split it and build up the levels)
            foreach (PropertyInfo PropertyInfoToSet in PropertyPaths)
            {
                //is this the first item?
                if (WorkingMemberExpression == null)
                {
                    //set the working expression
                    WorkingMemberExpression = Expression.Property(LambdaArgument, PropertyInfoToSet);
                }
                else
                {
                    //set the working expression
                    WorkingMemberExpression = Expression.Property(WorkingMemberExpression, PropertyInfoToSet);
                }
            }

            //return the tuple now
            return new Tuple<MemberExpression, ParameterExpression>(WorkingMemberExpression, LambdaArgument);
        }

        #endregion

    }

}
