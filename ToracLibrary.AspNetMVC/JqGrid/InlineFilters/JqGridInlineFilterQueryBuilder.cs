using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees.API;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.AspNetMVC.JqGrid.InlineFilters
{

    /// <summary>
    /// Helper Method To Build The Linq Expression For The Inline Filters Built In JqGrid
    /// </summary>
    public static class JqGridInlineFilterQueryBuilder
    {

        #region Public Methods

        /// <summary>
        /// Builds the expression that should be added to the base query for the jqgrid inline filters
        /// </summary>
        /// <typeparam name="T">Type of the record to be queried</typeparam>
        /// <param name="GridFilter">Filters to be used to build the expression</param>
        /// <param name="IsLinqToObjectQuery">Is The End Expression Going To Be Used For LinqToObjects Or Entity Framework. Determines Which Methods Can Be Used</param>
        /// <returns>Expression to add to the query</returns>
        public static Expression<Func<T, bool>> BuildInlineFilterQuery<T>(JqGridInlineFilters GridFilter, bool IsLinqToObjectQuery)
        {
            //do we have any filters?
            if (GridFilter == null && !GridFilter.Filters.AnyWithNullCheck())
            {
                //just return a true expression
                return x => true;
            }

            //we have some filters, so let's go through them and build up my expression to filter my data

            //working expression to be added to
            Expression<Func<T, bool>> WorkingExpression = null;

            //let's grab the T's properties
            PropertyInfo[] TProperties = typeof(T).GetProperties();

            //which operation in the dynamic format enum
            var WhichOperationInDynamicApiEnum = GridFilter.Operation == JqGridInlineFilters.JqGridOperationType.AND ?
                                                                              ExpressionCombiner.CombineType.AndAlso :
                                                                              ExpressionCombiner.CombineType.OrElse;

            //let's loop through the filters
            foreach (var FilterToProcess in GridFilter.Filters)
            {
                //let's grab this properties type
                PropertyInfo PropertyOfFilter = TProperties.First(x => x.Name == FilterToProcess.ColumnName);

                //let's go build the filter for this guy
                var ExpressionForFilterToProcess = AddFilterCriteria<T>(PropertyOfFilter.PropertyType, FilterToProcess.ColumnName, FilterToProcess.UserEnteredValue, IsLinqToObjectQuery);

                //let's go build this filter
                if (WorkingExpression == null)
                {
                    //we don't have any filters yet so just set the working expression
                    WorkingExpression = ExpressionForFilterToProcess;
                }
                else
                {
                    //combine the expression with either and "AndAlso" or an "OrElse"
                    WorkingExpression = ExpressionCombiner.CombineExpressions(WorkingExpression, WhichOperationInDynamicApiEnum, ExpressionForFilterToProcess);
                }
            }

            //have everything built, just return the working expression
            return WorkingExpression;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the specified filter critiera and return the expression with the filter added. Used to build a singular GridFilter.FilterData
        /// </summary>
        /// <typeparam name="T">Type of the iqueryable</typeparam>
        /// <param name="ColumnDataType">Column data type</param>
        /// <param name="ColumnName">Field name of the filter</param>
        /// <param name="InlineFilterEnteredValue">The value the user entered in the inline filter</param>
        /// <param name="IsLinqToObjectQuery">Is The End Expression Going To Be Used For LinqToObjects Or Entity Framework. Determines Which Methods Can Be Used</param>
        /// <returns>Expression To Be Added To The Base Query</returns>
        private static Expression<Func<T, bool>> AddFilterCriteria<T>(Type ColumnDataType, string ColumnName, string InlineFilterEnteredValue, bool IsLinqToObjectQuery)
        {
            //based on the column type run the specified filter
            if (ColumnDataType == typeof(string))
            {
                //its a string filter, make sure they aren't trying to search with a blank string
                if (!string.IsNullOrEmpty(InlineFilterEnteredValue))
                {
                    //special scenarios where we need to run where we need to pull it in and use the linq to objects (translation db)
                    if (IsLinqToObjectQuery)
                    {
                        //run the linq because we care about case
                        return ExpressionBuilder.StringContains<T>(ColumnName, InlineFilterEnteredValue, false, true);
                    }
                    else
                    {
                        //run the ef without worrying about case
                        return ExpressionBuilder.StringContains<T>(ColumnName, InlineFilterEnteredValue, false, false);
                    }
                }
            }
            else if (ColumnDataType == typeof(bool) || ColumnDataType == typeof(bool?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                bool ParsedBooleanValue;

                //try to parse the value 
                if (bool.TryParse(InlineFilterEnteredValue, out ParsedBooleanValue))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(bool))
                    {
                        //we parsed it...go build the boolean value
                        return ExpressionBuilder.BuildStatement<T, bool>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedBooleanValue);
                    }
                    else
                    {
                        //we parsed it...go build the nullable boolean value
                        return ExpressionBuilder.BuildStatement<T, bool?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedBooleanValue);
                    }
                }
            }
            else if (ColumnDataType == typeof(int) || ColumnDataType == typeof(int?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                int ParseInt;

                //try to parse the value 
                if (int.TryParse(InlineFilterEnteredValue, out ParseInt))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(int))
                    {
                        //we parsed it...go build the int value
                        return ExpressionBuilder.BuildStatement<T, int>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParseInt);
                    }
                    else
                    {
                        //we parsed it...go build the nullable int value
                        return ExpressionBuilder.BuildStatement<T, int?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParseInt);
                    }
                }
            }
            else if (ColumnDataType == typeof(DateTime) || ColumnDataType == typeof(DateTime?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                DateTime ParsedDate;

                //try to parse the value 
                if (DateTime.TryParse(InlineFilterEnteredValue, out ParsedDate))
                {
                    //calculate the end date
                    DateTime ParsedEndDateCalculation = ParsedDate.AddDays(1);

                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(DateTime))
                    {
                        //build the start date
                        var StartDate = ExpressionBuilder.BuildStatement<T, DateTime>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedDate);

                        //build the end date
                        var EndDate = ExpressionBuilder.BuildStatement<T, DateTime>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.LessThan, ParsedEndDateCalculation);

                        //combine the 2
                        return ExpressionCombiner.CombineExpressions(StartDate, ExpressionCombiner.CombineType.AndAlso, EndDate);
                    }
                    else
                    {
                        //build the start date
                        var StartDate = ExpressionBuilder.BuildStatement<T, DateTime?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedDate);

                        //build the end date
                        var EndDate = ExpressionBuilder.BuildStatement<T, DateTime?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.LessThan, ParsedEndDateCalculation);

                        //combine the 2
                        return ExpressionCombiner.CombineExpressions(StartDate, ExpressionCombiner.CombineType.AndAlso, EndDate);
                    }
                }
            }
            else if (ColumnDataType == typeof(Int16) || ColumnDataType == typeof(Int16?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                Int16 ParsedInt16;

                //try to parse the value 
                if (Int16.TryParse(InlineFilterEnteredValue, out ParsedInt16))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(Int16))
                    {
                        //build the int16 value
                        return ExpressionBuilder.BuildStatement<T, Int16>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedInt16);
                    }
                    else
                    {
                        //build the nullable int16 value
                        return ExpressionBuilder.BuildStatement<T, Int16?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedInt16);
                    }
                }
            }
            else if (ColumnDataType == typeof(Int64) || ColumnDataType == typeof(Int64?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                Int64 ParsedInt64;

                //try to parse the value 
                if (Int64.TryParse(InlineFilterEnteredValue, out ParsedInt64))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(Int64))
                    {
                        //build the int64 value
                        return ExpressionBuilder.BuildStatement<T, Int64>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedInt64);
                    }
                    else
                    {
                        //bulid the nullable int64 value
                        return ExpressionBuilder.BuildStatement<T, Int64?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedInt64);
                    }
                }
            }
            else if (ColumnDataType == typeof(double) || ColumnDataType == typeof(double?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                double ParsedDouble;

                //try to parse the value 
                if (double.TryParse(InlineFilterEnteredValue, out ParsedDouble))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(double))
                    {
                        //build the double value
                        return ExpressionBuilder.BuildStatement<T, double>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedDouble);
                    }
                    else
                    {
                        //build the nullable double value
                        return ExpressionBuilder.BuildStatement<T, double?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedDouble);
                    }
                }
            }
            else if (ColumnDataType == typeof(float) || ColumnDataType == typeof(float?))
            {
                //try to parse the value entered into the grid to the data type we are going to query
                float ParsedFloat;

                //try to parse the value 
                if (float.TryParse(InlineFilterEnteredValue, out ParsedFloat))
                {
                    //it is a nullable field or a regular field?
                    if (ColumnDataType == typeof(float))
                    {
                        //build the float value
                        return ExpressionBuilder.BuildStatement<T, float>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedFloat);
                    }
                    else
                    {
                        //build the nullable float value
                        return ExpressionBuilder.BuildStatement<T, float?>(ParameterBuilder.BuildParameterFromStringName<T>(ColumnName), ExpressionBuilder.DynamicUtilitiesEquations.Equal, ParsedFloat);
                    }
                }
            }

            //if we get there then we couldn't find the data type so throw an error
            throw new NotImplementedException();
        }

        #endregion

    }

}
