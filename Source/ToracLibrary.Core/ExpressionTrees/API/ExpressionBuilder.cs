using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;
using ToracLibrary.Core.ReflectionDynamic;
using ToracLibrary.Core.ReflectionDynamic.Invoke;
using ToracLibrary.Core.ReflectionDynamic.Invoke.Parameters;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExpressionTrees.API
{

    /// <summary>
    /// Root class to add the expression in which you want to search for
    /// </summary>
    public static class ExpressionBuilder
    {

        #region Equation Enum

        /// <summary>
        /// Contains the equation type to use when being passed in (used in linq utilities and ef uilities)
        /// </summary>
        public enum DynamicUtilitiesEquations : int
        {

            /// <summary>
            /// Equal Query
            /// </summary>
            Equal = 0,

            /// <summary>
            /// Greater Than Query
            /// </summary>
            GreaterThan = 1,

            /// <summary>
            /// Greater Than Or Equal Query
            /// </summary>
            GreaterThanOrEqual = 2,

            /// <summary>
            /// Less Than Query
            /// </summary>
            LessThan = 3,

            /// <summary>
            /// Less Than Or Equal Query
            /// </summary>
            LessThanOrEqual = 4,

            /// <summary>
            /// Not equal too
            /// </summary>
            NotEqualTo = 5

        }

        #endregion

        #region Statement Builder

        /// <summary>
        /// Builds a statement with greater than, less than, equal, not equal, etc.
        /// </summary>
        /// <typeparam name="T">Query Record Object Type</typeparam>
        /// <typeparam name="TPropertySelector">The Value of the property to build the statement for</typeparam>
        /// <param name="LeftHandSide">Property To Check Which Has Been Built Into A Member Expression</param>
        /// <param name="Operator">Operation To Run</param>
        /// <param name="ValueToQuery">Value To Check Against - Right Hand Side Of Equation</param>
        /// <returns>Built Up Expression</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> BuildStatement<T, TPropertySelector>(ParameterBuilderResults LeftHandSide, DynamicUtilitiesEquations Operator, TPropertySelector ValueToQuery)
        {
            //example on how ot call this
            //var left = ParameterBuilder.BuildParameterFromLinqPropertySelector<Jason>(x => x.Id);
            //var selector = ExpressionBuilder.BuildStatement<Jason, int>(left, ToracTechnologies.Library.Dynamic.DynamicShared.DynamicUtilitiesEquations.Equal, 5);
            //var result = lst.AsQueryable().Where(selector).ToArray();

            //let's build the right hand side of the formula
            ConstantExpression RightHandSide = Expression.Constant(ValueToQuery, typeof(TPropertySelector));

            //now go build the formula and return it
            return Expression.Lambda<Func<T, bool>>(BuildFormula(LeftHandSide.PropertyMemberExpression, Operator, RightHandSide), LeftHandSide.ParametersForExpression);
        }

        /// <summary>
        /// Builds a statement with greater than, less than, equal, not equal, etc. This is untyped so you don't need to specify what the ValueToQuery data type is
        /// </summary>
        /// <typeparam name="T">Query Record Object Type</typeparam>
        /// <param name="LeftHandSide">Property To Check Which Has Been Built Into A Member Expression</param>
        /// <param name="Operator">Operation To Run</param>
        /// <param name="ValueToQuery">Value To Check Against - Right Hand Side Of Equation</param>
        /// <returns>Built Up Expression</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> BuildStatement<T>(ParameterBuilderResults LeftHandSide, DynamicUtilitiesEquations Operator, object ValueToQuery)
        {
            //example on how ot call this
            //var left = ParameterBuilder.BuildParameterFromLinqPropertySelector<Jason>(x => x.Id);
            //var selector = ExpressionBuilder.BuildStatement<Jason>(left, ToracTechnologies.Library.Dynamic.DynamicShared.DynamicUtilitiesEquations.Equal, 5);
            //var result = lst.AsQueryable().Where(selector).ToArray();

            //let's build the right hand side of the formula
            ConstantExpression RightHandSide = Expression.Constant(ValueToQuery, ValueToQuery.GetType());

            //now go build the formula and return it
            return Expression.Lambda<Func<T, bool>>(BuildFormula(LeftHandSide.PropertyMemberExpression, Operator, RightHandSide), LeftHandSide.ParametersForExpression);
        }

        /// <summary>
        /// Builds an IEnumerable.Contains Statement
        /// </summary>
        /// <typeparam name="T">Query Record Object Type</typeparam>
        /// <typeparam name="TListType">Type of the list passed in</typeparam>
        /// <param name="ListToCheck">List to check against</param>
        /// <param name="RightHandSide">The property to check in the list for</param>
        /// <returns>Expression To check</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> BuildIEnumerableContains<T, TListType>(IEnumerable<TListType> ListToCheck, ParameterBuilderResults RightHandSide)
        {
            ////grab the contains method. difficult because we want to use the Enumerable which is an extension method that all ienumerable uses
            //MethodInfo ContainsOffOfIEnumerable = typeof(Enumerable).GetMethods()
            //                                      .Where(x => x.IsGenericMethod && String.Equals(x.Name, "Contains", StringComparison.OrdinalIgnoreCase))
            //                                      .First(x => x.GetParameters().Length == 2)
            //                                      .MakeGenericMethod(typeof(Z));

            var ContainsOffOfIEnumerable = new GenericStaticMethodFinder(typeof(Enumerable), nameof(Enumerable.Contains), 
                new Type[] 
                {
                    typeof(TListType)
                }, 
                new List<GenericTypeParameter>
                {
                    new GenericTypeParameter(typeof(IEnumerable<>), true),
                    new GenericTypeParameter(typeof(TListType), true)
                }).FindMethodToInvoke();

            //grab the lst and make it a constants
            ConstantExpression ConstantListExpressionParameter = Expression.Constant(ListToCheck, typeof(IEnumerable<TListType>));

            //this is an extension method so you pass in the list too
            //now create the call...basically contains(myList,int to check for)
            MethodCallExpression ContainsMethodCall = Expression.Call(ContainsOffOfIEnumerable, ConstantListExpressionParameter, RightHandSide.PropertyMemberExpression);

            //return the expression now
            return Expression.Lambda<Func<T, bool>>(ContainsMethodCall, RightHandSide.ParametersForExpression);
        }

        /* Example of string contains where we pass in the string to search on through the func...As well as TGridRow and Return IGridRow...And adding the different properties to search on through an OrElse Statement
             public virtual Expression<Func<string, IGridRow, bool>> BuildInlineFilterExpression(SectionStore sectionStore)
        {
            //declare the lambda argument
            ParameterExpression LambdaArgument = Expression.Parameter(typeof(string), "xContains");
            ParameterExpression RecordArgument = Expression.Parameter(typeof(IGridRow), "x");

            Expression<Func<string, IGridRow, bool>> baseExpression = null;

            //loop through all the properties we are going to filter on
            foreach (string propertyName in sectionStore.StringPropertyCache)
            {
                //grab the property of T that we are searching on
                var PropertyToSearchOn = Expression.PropertyOrField(Expression.Convert(RecordArgument, sectionStore.SectionRecord.TypeOfGridRow), propertyName);

                //set the working expression to make sure the item isn't null
                var ExpressionToAdd = Expression.Equal(Expression.Call(typeof(string).GetMethod(nameof(string.IsNullOrEmpty)), PropertyToSearchOn), Expression.Constant(false));

                //grab the contains with the ignore case method (in torac library)
                var ContainsIgnoreCaseMethod = typeof(StringExtensionMethods).GetMethod(nameof(StringExtensionMethods.Contains), new Type[] { typeof(string), typeof(string), typeof(StringComparison) });

                //let's combine the 2 methods (so the Is Not Null Or Empty and then the case insensitive contains clause)
                ExpressionToAdd = Expression.AndAlso(ExpressionToAdd, Expression.Call(ContainsIgnoreCaseMethod, PropertyToSearchOn, LambdaArgument, Expression.Constant(StringComparison.OrdinalIgnoreCase, typeof(StringComparison))));

                //Build up this expression
                var ExpressionToCombine = Expression.Lambda<Func<string, IGridRow, bool>>(ExpressionToAdd, LambdaArgument, RecordArgument);

                //add the working expression to the base expression we are building up
                if (baseExpression == null)
                {
                    //if we have nothing then just set it
                    baseExpression = ExpressionToCombine;
                }
                else
                {
                    //go remap the second expression so we can add an OrElse
                    var NewSecondExpression = new ExpressionParameterRemapper(baseExpression.Parameters, ExpressionToCombine.Parameters).VisitAndConvert(ExpressionToCombine.Body, CombineType.OrElse.ToString());

                    //use an or else and reset the base expression
                    baseExpression = Expression.Lambda<Func<string, IGridRow, bool>>(Expression.OrElse(baseExpression.Body, NewSecondExpression), LambdaArgument, RecordArgument);
                }
            }

            //return it
            return baseExpression;
        }*/

        /// <summary>
        /// Builds a dynamic expression which test if the property name contains the ContainTest string Passed in
        /// </summary>
        /// <typeparam name="T">Type Of The Object Passed In (Object Container Of PropertyNameToCheck)</typeparam>
        /// <param name="PropertyNameToCheck">Property Name To Check</param>
        /// <param name="ContainsTest">What To Check For In The Property</param>
        /// <param name="CaseSensitive">Make it case sensitive on the search</param>
        /// <param name="IsUsedForLinqToObjects">Is used for linq to objects vs Entity Framework. This method uses local methods to check for null strings which Sql Server handles natively...so we don't call that part if we are coming from Entity Framework</param>
        /// <returns>Linq Expression</returns>
        /// <remarks>internal because we don't want external code calling this with the IsUsedForLinqToObjects parameters. We want to hide this parameter and not let the end developer pass it in</remarks>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, bool>> StringContains<T>(string PropertyNameToCheck, string ContainsTest, bool CaseSensitive, bool IsUsedForLinqToObjects)
        {
            //example
            //class Jason
            //
            //id {get;set;}
            //txt {get;set;}

            //example of how to call it
            //lst.AsQueryable().Where(LinqUtilities.StringContains<jason>("id", "2", true)).ToArray();

            //make sure if its ef based, that case sensitive is not true
            if (!IsUsedForLinqToObjects && CaseSensitive)
            {
                throw new ArgumentOutOfRangeException("String Contains Can't Be Case Sensitive When Its In Entity Framework Mode (ie: IsUsedForLinqToObjects = false)");
            }

            //grab the member access so we can get the value of the property at run time
            ParameterBuilderResults ValueAtRuntime = ParameterBuilder.BuildParameterFromStringName<T>(PropertyNameToCheck);

            //hold the working expression
            Expression WorkingExpression = null;

            //in linq to objects we can call to string...for entity framework we can't
            if (IsUsedForLinqToObjects)
            {
                //set the working expression to make sure the item isn't null
                WorkingExpression = Expression.Equal(Expression.Call(typeof(string).GetMethod(nameof(string.IsNullOrEmpty)), ValueAtRuntime.PropertyMemberExpression), Expression.Constant(false));

                //which comparison are we using?
                var WhichComparison = (CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

                //grab the contains with the ignore case method (in torac library)
                var ContainsIgnoreCaseMethod = typeof(StringExtensionMethods).GetMethod(nameof(StringExtensionMethods.Contains), new Type[] { typeof(string), typeof(string), typeof(StringComparison) });

                //let's combine the 2 methods (so the Is Not Null Or Empty and then the case insensitive contains clause)
                WorkingExpression = Expression.AndAlso(WorkingExpression, Expression.Call(ContainsIgnoreCaseMethod, ValueAtRuntime.PropertyMemberExpression, Expression.Constant(ContainsTest, typeof(string)), Expression.Constant(WhichComparison, typeof(StringComparison))));
            }
            else
            {
                //now let's call the Contains Method...passing in the result of the previous expression
                WorkingExpression = Expression.Call(ValueAtRuntime.PropertyMemberExpression, typeof(string).GetMethod(nameof(string.Contains)), Expression.Constant(ContainsTest, typeof(string)));
            }

            //combine the expression and return it
            return Expression.Lambda<Func<T, bool>>(WorkingExpression, ValueAtRuntime.ParametersForExpression);
        }

        #region Select

        /// <summary>
        /// Run a select on a property
        /// </summary>
        /// <typeparam name="T">type of the record you are selecing from</typeparam>
        /// <typeparam name="TResult">property type we are selecting</typeparam>
        /// <param name="ParamResults">The result from Parameter Builder with the field you want to return</param>
        /// <returns>expression to run a select on</returns>
        /// <remarks>For Linq Just Call ThisResult.Compile()</remarks>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<T, TResult>> Select<T, TResult>(ParameterBuilderResults ParamResults)
        {
            //build the lambda and return it
            return Expression.Lambda<Func<T, TResult>>(ParamResults.PropertyMemberExpression, ParamResults.ParametersForExpression);
        }

        #endregion

        #endregion

        #region Main Helper Methods

        /// <summary>
        /// Returns the formula based on the operator and the left and right hand side
        /// </summary>
        /// <param name="ExpressionLeft">Expression Left</param>
        /// <param name="Operator">Operator to grab the binary expression for</param>
        /// <param name="ExpressionRight">Expression Right</param>
        /// <returns>Binary Expression to use in the expression</returns>
        private static BinaryExpression BuildFormula(Expression ExpressionLeft, DynamicUtilitiesEquations Operator, Expression ExpressionRight)
        {
            switch (Operator)
            {
                case DynamicUtilitiesEquations.Equal:
                    return Expression.Equal(ExpressionLeft, ExpressionRight);

                case DynamicUtilitiesEquations.GreaterThan:
                    return Expression.GreaterThan(ExpressionLeft, ExpressionRight);

                case DynamicUtilitiesEquations.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(ExpressionLeft, ExpressionRight);

                case DynamicUtilitiesEquations.LessThan:
                    return Expression.LessThan(ExpressionLeft, ExpressionRight);

                case DynamicUtilitiesEquations.LessThanOrEqual:
                    return Expression.LessThanOrEqual(ExpressionLeft, ExpressionRight);

                case DynamicUtilitiesEquations.NotEqualTo:
                    return Expression.NotEqual(ExpressionLeft, ExpressionRight);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }

}
