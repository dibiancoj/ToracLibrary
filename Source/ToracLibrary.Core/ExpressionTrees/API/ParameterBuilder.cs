using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.Core.ReflectionDynamic;

namespace ToracLibrary.Core.ExpressionTrees.API
{

    /// <summary>
    /// Builds the parameter for the expression builder
    /// </summary>
    public static class ParameterBuilder
    {

        #region String Based Property Retrieval

        /// <summary>
        /// Builds a member expression from a property name. Supports Sub Properties. Ie. Property1.Property2
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertyName">Property name to build the member expression off of</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromStringName<T>(string PropertyName)
        {
            //use the overload
            return BuildParameterFromStringName(PropertyName, typeof(T));
        }

        /// <summary>
        /// Builds a member expression from a property name. Supports Sub Properties. Ie. Property1.Property2
        /// </summary>
        /// <param name="PropertyName">Property name to build the member expression off of</param>
        /// <param name="TypeOfRecord">Type of record you are querying</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromStringName(string PropertyName, Type TypeOfRecord)
        {
            //grab the parameters build up so we reuse the same code
            var ParametersToUse = PropertyHelpers.GetLambdaParametersForPropertyPaths(TypeOfRecord, PropertyHelpers.GetSubPropertiesLazy(TypeOfRecord, PropertyName).ToArray());

            //make the member access to the property
            return new ParameterBuilderResults(ParametersToUse.Item1, new ReadOnlyCollection<ParameterExpression>(ParametersToUse.Item2.ToIList()));
        }

        #endregion

        #region Lambda - Func Based Property Retrieval

        //** using overloads so the end developer doesn't need to specify the data type of the parameters

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, string>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, bool>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, bool?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int16>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int16?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int32>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int32?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int64>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, Int64?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, DateTime>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, DateTime?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, double>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, double?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, decimal>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, decimal?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, float>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        /// <summary>
        /// Build a member expression from a linq expression which contains the property. ie x => x.thisFieldName or x => x.Ref_Table.thisFieldName
        /// </summary>
        /// <typeparam name="T">Type of record you are querying</typeparam>
        /// <param name="PropertySelector">Property You Are Selecting</param>
        /// <returns>Member Expression To Be Used To Build The Expression</returns>
        public static ParameterBuilderResults BuildParameterFromLinqPropertySelector<T>(Expression<Func<T, float?>> PropertySelector)
        {
            //return the body of the property selector
            return new ParameterBuilderResults((MemberExpression)PropertySelector.Body, PropertySelector.Parameters);
        }

        #endregion

    }

}
