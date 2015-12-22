using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
using ToracLibrary.Core.Reflection;
using ToracLibrary.Core.ReflectionDynamic;
using ToracLibrary.Core.ToracAttributes;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExpressionTrees
{

    /// <summary>
    /// Holds expression tree helpers
    /// </summary>
    public static class ExpressionTreeHelpers
    {

        #region New Item From Expression Tree

        /// <summary>
        /// Builds a new instance using an expression tree
        /// </summary>
        /// <param name="ConstructorInfoOfNewType">Constructor info for the new type. Using typeof(T).GetConstructors().First()</param>
        /// <param name="ConstructorParameter">Parameters of the contructor. So the parameters you would pass into the constructor</param>
        /// <returns>Func which creates the new object</returns>
        /// <remarks>Be sure to cache this if you are going to need it again. Call .Compile() to grab the func</remarks>
        [LinqToObjectsCompatible]
        public static Expression<Func<object[], object>> BuildNewObject(ConstructorInfo ConstructorInfoOfNewType, IEnumerable<ParameterInfo> ConstructorParameter)
        {
            //so instead of having to modify the expression you get back. We will use func(params object[] ConstructorParameters)
            //so we basically say for ConstructorParameters[0], ConstructorParameters[1], etc. 
            //this allows us to always pass back the same signature for the funct. Otherwise we would have to spit out which parmeters to get back

            //build the constructor parameter
            var ConstructorParameterName = Expression.Parameter(typeof(object[]), "args");

            //We are going build up all the types that the constructor takes
            var ConstructorParameterTypes = ConstructorParameter
                                                    .Select(x => x.ParameterType)
                                                    .Select((TypeOfParameter, IndexOfParameterInCtor) => Expression.Convert(Expression.ArrayIndex(ConstructorParameterName, Expression.Constant(IndexOfParameterInCtor)), TypeOfParameter)).ToArray();

            //now build the "New Object" expression
            var NewObjectExpression = Expression.New(ConstructorInfoOfNewType, ConstructorParameterTypes);

            //now let's build the lambda and return it
            return Expression.Lambda<Func<object[], object>>(NewObjectExpression, ConstructorParameterName);
        }

        #endregion

        #region Create A New Object With Same Properties

        /// <summary>
        /// If you have 2 objects that have the same property. Select new Object { Id = x.Id, Txt = x.Txt}. It's mainly used to create derived selectors
        /// </summary>
        /// <typeparam name="TFrom">TFrom Record Type. The Record That Sets The New Properites</typeparam>
        /// <typeparam name="TResult">TResult Record Type. The Record Type That You End Up With</typeparam>
        /// <param name="PropertiesToSetFromTFrom">Properties From TFrom, That Will Be Set When Building The Lambda Expression</param>
        /// <returns>Expression That Can Be Used To Create A New TResult With The Properties From TFrom And The Property Info's Passed in</returns>
        /// <remarks>Will Throw Error If We Can't Find The Same Property Name That Was in PropertiesToSetFromTFrom</remarks>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static Expression<Func<TFrom, TResult>> SelectNewFromObject<TFrom, TResult>(IEnumerable<PropertyInfo> PropertiesToSetFromTFrom)
            where TFrom : class
            where TResult : class
        {
            /*Example. 
             * Ref_Employee with Id, Name, DepartmentId
             * I want to display a derived version Ref_EmployeeGrid: Ref_Employee (output a department name)
             * Ref_Department.
             * I want to tack on the division name. So I have something like Ref_EmployeeGrid which has the Division Name. I clone the select from Ref_Employee ==> EmployeeGridView.
             * That will clone all the properties from Ref_Employee to EmployeeGridView
             * Then we can just tack on { DepartmentName = context.Ref_Department.FirstOrDefault(y => y.DepartmentId == x.DepartmentId }
             */

            //passing in the properties (especially when you are using entity framework, you will want to exclude the "includes" or collection / virtual properties
            //typeof(TFrom).GetProperties().Where(x => !x.GetGetMethod().IsVirtual);

            //You can't project into an entity type so never try to do Ref_Table.Select(x => new Ref_Table)!!!

            //declare the list of member bindings (this contains Property1 = x.Property1, Property2 = x.Property2)
            var MergedInitList = new List<MemberBinding>();

            //declare the from parameter (this is TFrom)
            var ArgFrom = Expression.Parameter(typeof(TFrom), "x");

            //let's cache the TResult so we don't have to keep grabbing this
            Type TResultType = typeof(TResult);

            //let's loop through all the properties we want to set
            foreach (var FromProperty in PropertiesToSetFromTFrom)
            {
                //grab the property from TResult, based on name
                var ToProperty = TResultType.GetProperty(FromProperty.Name);

                //if we can't find the name...then throw an error
                if (ToProperty == null)
                {
                    //we can't find that property in TResult...throw an error
                    throw new ArgumentNullException($"Can't Find Property Name = {ToProperty.Name} In TResult ({TResultType.Name}). The Property Names Must Match From The Argument PropertiesToSetFromTFrom And In TResult. Pass In Specific Properties That You Want To Set, But TResult Must Contain That Property Name");
                }

                //now grab the property we are going to set...and bind it, and add it to the list
                MergedInitList.Add(Expression.Bind(ToProperty, Expression.MakeMemberAccess(ArgFrom, FromProperty)));
            }

            //create the actual New Expression ie new { Id = x.Id, Txt = x.Txt}
            var NewExpression = Expression.MemberInit(Expression.New(typeof(TResult)), MergedInitList);

            //let's create the lambda and return it
            return Expression.Lambda<Func<TFrom, TResult>>(NewExpression, new ReadOnlyCollection<ParameterExpression>(ArgFrom.ToIList()));
        }

        #endregion

        #region Order By

        /// <summary>
        /// Modifies Queryable Of T And Orders It For Entity Framework.
        /// </summary>
        /// <typeparam name="T">Type Of The IQueryable</typeparam>
        /// <param name="QueryToModify">Query To Modifiy</param>
        /// <param name="PropertySortPath">Property Name To Sort By</param>
        /// <param name="OrderByAscending">Sort By Ascending or Descending</param>
        /// <param name="IQueryableIsNotSortedYet">Determines if we should run an "orderby" or "thenby". This avoids trying to figure out if IQueryable Or IOrdered is passed in. Little faster this way</param>
        /// <returns>IOrderedQueryable Of T</returns>
        [LinqToObjectsCompatible]
        [EntityFrameworkCompatible]
        public static IOrderedQueryable<T> OrderBy<T>(IQueryable<T> QueryToModify, string PropertySortPath, bool OrderByAscending, bool IQueryableIsNotSortedYet)
        {
            //The problem with the entity framework sort by is that the key (the field to be sorted on could be anything). You can't say expression|func|tablename,object|| because 
            //it can't resolve the object into whatever the key should be. So we need to do it this way. I would have liked to returned just the Linq Expression

            //example of how to call this
            //sql = sql.OrderBy("CreatedDate", true);

            //if i need to run linq to objects with a sub path for the property. ie. Object.SubObject.Id
            //var columnInfo = PropertyHelpers.GetPropertyOfObjectExpressionFunc<TestRow, object>(thisSortColumn.ColumnName).Compile();

            //if (thisSortColumn.DirectionToSort == SortedColumn.SortDirection.Asc)
            //{
            //     lst = lst.AsQueryable().OrderBy(columnInfo).ToList();
            //}
            //else
            //{
            //    lst = lst.AsQueryable().OrderByDescending(columnInfo).ToList();
            //}

            //grab the properties for this guy. So as we traverse the tree we store the properties
            var PropertiesInTree = PropertyHelpers.GetSubPropertiesLazy(typeof(T), PropertySortPath).ToArray();

            //let's throw the last property info into a variable. This way we have it (because its really the property we want to base everything off of. Its the reason we traverse down the tree)
            PropertyInfo LastPropertyInTree = PropertiesInTree.Last();

            //now we need to invoke the func using the correct property type so it will work with EF. lets go invoke that since we know the property we are looking for and its data type
            MethodInfo SortFuncBuilder = OverloadedMethodFinder.FindOverloadedMethodToCall(nameof(PropertyHelpers.GetPropertyOfObjectExpressionFunc), typeof(PropertyHelpers), typeof(IEnumerable<PropertyInfo>));

            //lets create the generic version of the sort func builder function
            MethodInfo SortFuncGenericBuilder = SortFuncBuilder.MakeGenericMethod(typeof(T), LastPropertyInTree.PropertyType);

            //let's go grab the sort predicate
            Expression SortPredicate = (Expression)SortFuncGenericBuilder.Invoke(null, new object[] { PropertiesInTree });

            // use reflection to get and call your own generic method that composes
            // the orderby into the query.
            MethodInfo Method = typeof(ExpressionTreeHelpers).GetMethod(nameof(ExpressionTreeHelpers.OrderByProperty), BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(typeof(T), LastPropertyInTree.PropertyType);

            //go invoke the order by property generically
            return (IOrderedQueryable<T>)Method.Invoke(null, new object[] { QueryToModify, SortPredicate, OrderByAscending, IQueryableIsNotSortedYet });
        }

        /// <summary>
        /// Builds the sort order expression into the IQueryable and returns the IQueryable
        /// </summary>
        /// <typeparam name="T">Type Of The IQueryable</typeparam>
        /// <typeparam name="TKey">Type Of The Key Which Will Be Sorted. Is That Field An Int? Decimal? Date?</typeparam>
        /// <param name="QueryToModify">IQueryable</param>
        /// <param name="SortPredicate">Sort predicate with the property selector</param>
        /// <param name="OrderByAscending">Order By Ascending Or Descending Order</param>
        /// <param name="IQueryableIsNotSortedYet">Determines if we should run an "orderby" or "thenby". This avoids trying to figure out if IQueryable Or IOrdered is passed in. Little faster this way</param>
        /// <returns>IOrdered Queryable Of T</returns>
        [InvokedDynamicallyAtRuntime]
        private static IOrderedQueryable<T> OrderByProperty<T, TKey>(this IQueryable<T> QueryToModify, Expression SortPredicate, bool OrderByAscending, bool IQueryableIsNotSortedYet)
        {
            //**** this method gets invoked dynamically from OrderBy. So it is being used! 

            //based on the way we want to sort go add the expression and return the query
            if (OrderByAscending)
            {
                //is this an additional sort?
                if (IQueryableIsNotSortedYet)
                {
                    //order by ascending
                    return QueryToModify.OrderBy((Expression<Func<T, TKey>>)SortPredicate);
                }

                //tack on the additional "then by"
                return ((IOrderedQueryable<T>)QueryToModify).ThenBy((Expression<Func<T, TKey>>)SortPredicate);
            }

            //is this an additional sort?
            if (IQueryableIsNotSortedYet)
            {
                //order by descending
                return QueryToModify.OrderByDescending((Expression<Func<T, TKey>>)SortPredicate);
            }

            //tack on the additional "then by"
            return ((IOrderedQueryable<T>)QueryToModify).ThenByDescending((Expression<Func<T, TKey>>)SortPredicate);
        }

        #endregion

    }

}
