using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ToracLibrary.Core.DataTypes;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.Core.ReflectionDynamic;

namespace ToracLibrary.Core.DataTableHelpers
{

    /// <summary>
    /// Builds a data table from an object or a list of objects
    /// </summary>
    public static class ToDataTable
    {

        #region Public Methods

        /// <summary>
        /// Builds a data table off of an object
        /// </summary>
        /// <typeparam name="T">Type of the object to build off of</typeparam>
        /// <param name="ObjectToBuildDataTableOffOf">object to build</param>
        /// <param name="TableName">Table Name. Property Off Of The Data Table</param>
        /// <returns>Data Table</returns>
        public static DataTable BuildDataTableFromObject<T>(T ObjectToBuildDataTableOffOf, string TableName) where T : class
        {
            //*** we need to have different method names because if we pass in a list<T> it will go into this overload. Because a list<T> is T.
            // then we get blow ups. we need to manually pass in which method to use

            //if this is a list...then blow it up and tell the user to use the other overload
            if (ObjectToBuildDataTableOffOf is IEnumerable)
            {
                throw new ArgumentOutOfRangeException("ObjectToBuildDataTableOffOf", "Please Use The BuildDataTableFromObjectList Overload When Passing In T Of IEnumerable");
            }

            //use the helper method...create an array of this object and pass it in
            return BuildDataTableFromListOfObjects(ObjectToBuildDataTableOffOf.ToIEnumerableLazy(), TableName);
        }

        /// <summary>
        /// Builds a data table off of the list of objects passed in
        /// </summary>
        /// <typeparam name="T">Type of the object to build off of</typeparam>
        /// <param name="ObjectsToBuildDataTableOffOf">List of objects to build</param>
        /// <param name="TableName">Table Name. Property Off Of The Data Table</param>
        /// <returns>Data table</returns>
        public static DataTable BuildDataTableFromListOfObjects<T>(IEnumerable<T> ObjectsToBuildDataTableOffOf, string TableName) where T : class
        {
            //*** we need to have different method names because if we pass in a list<T> it will go into this overload. Because a list<T> is T.
            // then we get blow ups. we need to manually pass in which method to use

            //create the data table to return
            var DataTableToBuild = new DataTable(TableName);

            //grab the properies we care about
            var PropertiesToBuild = PropertiesToBuildOffOf<T>().ToArray();

            //instead of using reflection to get each of the object's value which is slower (even though we re-use property info which helps)
            //the expression tree is faster...so we are going to store the expression into the dictionary 
            var PropertyCachedGetters = new Dictionary<string, Func<T, object>>();

            //let's loop through the properties to build up the column def's 
            foreach (PropertyInfo PropertyToBuild in PropertiesToBuild)
            {
                //if its a nullable field then we need to 
                if (PropertyHelpers.IsNullableOfT(PropertyToBuild))
                {
                    //its some sort of nullable<int>...data set doesnt support this, so we get the underlying data type and set it to nullable
                    DataTableToBuild.Columns.Add(new DataColumn(PropertyToBuild.Name, PropertyToBuild.PropertyType.GetGenericArguments()[0]) { AllowDBNull = true });
                }
                else
                {
                    //regular column, just add it
                    DataTableToBuild.Columns.Add(new DataColumn(PropertyToBuild.Name, PropertyToBuild.PropertyType));
                }

                //now compile the expression and add it to the dictionary
                PropertyCachedGetters.Add(PropertyToBuild.Name, PropertyHelpers.GetPropertyOfObjectExpressionFunc<T, object>(PropertyToBuild.Name).Compile());
            }

            //now we need to go through each object and add the row
            foreach (T ObjectToBuildRowWith in ObjectsToBuildDataTableOffOf)
            {
                //let's create the new data row
                DataRow NewDataRow = DataTableToBuild.NewRow();

                //let's loop through all the properties and set the column
                foreach (PropertyInfo PropertyToSet in PropertiesToBuild)
                {
                    //grab the value and set it...if its null we set the value to Db Null
                    NewDataRow[PropertyToSet.Name] = PropertyCachedGetters[PropertyToSet.Name](ObjectToBuildRowWith) ?? DBNull.Value;
                }

                //let's add the data row to the data table
                DataTableToBuild.Rows.Add(NewDataRow);
            }

            //return the data table
            return DataTableToBuild;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build the properties to build the data table off of. Ignores collections because we can't model that in a data table
        /// </summary>
        /// <typeparam name="T">Type of the object to build</typeparam>
        /// <returns>list of property info to build off of</returns>
        private static IEnumerable<PropertyInfo> PropertiesToBuildOffOf<T>()
        {
            //grab just PrimitiveTypes we care about. (no collections or anything like that)
            IEnumerable<Type> PrimativeTypesToBuild = PrimitiveTypes.PrimitiveTypesSelect();

            //grab just PrimitiveTypes we care about. No collections or anything like that and return the result
            return typeof(T).GetProperties().Where(x => PrimativeTypesToBuild.Contains(x.PropertyType));
        }

        #endregion

    }

}
