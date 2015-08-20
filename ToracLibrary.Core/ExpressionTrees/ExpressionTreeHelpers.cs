using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.ObjectExtensions;
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
            var ConstructorParameterTypes = ConstructorParameter.Select(x => x.ParameterType).Select((t, i) => Expression.Convert(Expression.ArrayIndex(ConstructorParameterName, Expression.Constant(i)), t)).ToArray();

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
                    throw new ArgumentNullException(string.Format($"Can't Find Property Name = {ToProperty.Name} In TResult ({TResultType.Name}). The Property Names Must Match From The Argument PropertiesToSetFromTFrom And In TResult. Pass In Specific Properties That You Want To Set, But TResult Must Contain That Property Name"));
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

    }

}
