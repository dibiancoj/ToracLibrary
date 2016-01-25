using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExpressionTrees.API.ReMappers
{

    /// <summary>
    /// Let's say i have an IQueryable. Essentially a ef query. I want to tack on a field to my select. My select looks like x => new Record { Id = x.Id};. I want to tack on { Txt = x.Txt} inside the new Record call.
    /// </summary>
    /// <typeparam name="TQueryType">IQueryable Of T. This is the T.</typeparam>
    public class IQueryableMemberInitMerger<TQueryType> : ExpressionVisitor
    {

        #region Documentation 

        //See IQueryableExtensionMethods.AddBindingToSelectInQuery for example call

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FieldNameToSetTheValueToSet">The name of the field you are mapping from. So the table.FieldName</param>
        /// <param name="PropertyNameToSetInSelectProjection">The name of the property you want to set</param>
        public IQueryableMemberInitMerger(string FieldNameToSetTheValueToSet, string PropertyNameToSetInSelectProjection)
        {
            FieldNameToSetTheValue = FieldNameToSetTheValueToSet;
            PropertyNameToSetInProjection = PropertyNameToSetInSelectProjection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the field you are mapping from. So the table.FieldName
        /// </summary>
        public string FieldNameToSetTheValue { get; }

        /// <summary>
        /// The name of the property you want to set
        /// </summary>
        public string PropertyNameToSetInProjection { get; }

        #endregion

        #region Overrides

        /// <summary>
        /// Override the member init expression
        /// </summary>
        /// <param name="Node">Member Init Expression</param>
        /// <returns>new expression</returns>
        protected override Expression VisitMemberInit(MemberInitExpression Node)
        {
            //go find my parameter (the x in x => x.Id)...go start the visitor
            var FindMyParameter = new ParameterExpressionFinder();

            //go visit the expression
            FindMyParameter.Visit(Node);

            //get the type of the node so we can use that
            var TypeOfLambdaParameter = FindMyParameter.ParameterExpressionsThatWeFound.Type;

            //let's grab the property info in a seperate variables so we can raise smart exception if not found
            var FromPropertyInfo = FindMyParameter.ParameterExpressionsThatWeFound.Type.GetProperty(FieldNameToSetTheValue);

            //validate we found the property
            if (FromPropertyInfo == null)
            {
                throw new NullReferenceException($"Can't Find The Property Set In FieldNameToSetTheValue. Trying To Set {FieldNameToSetTheValue} In - {typeof(TQueryType).Name}");
            }

            //grab the property we are putting the value into
            var ToProjectionProperty = typeof(TQueryType).GetProperty(PropertyNameToSetInProjection);

            //validate the projection property
            if (ToProjectionProperty == null)
            {
                throw new NullReferenceException($"Can't Find The Property Set In PropertyNameToSetInProjection. Trying To Set {ToProjectionProperty} In - {FromPropertyInfo.Name}");
            }

            //go build the binding from the x ...to the Txt = x.BlaBlaField
            var BindThisProperty = Expression.Bind(ToProjectionProperty, Expression.MakeMemberAccess(FindMyParameter.ParameterExpressionsThatWeFound, FromPropertyInfo));

            //go merge the bindings
            var MergedBindings = Node.Bindings.Concat(new MemberBinding[] { BindThisProperty });

            //now create a new expression and return it
            return Expression.MemberInit(Expression.New(typeof(TQueryType)), MergedBindings);
        }

        #endregion

    }

}
