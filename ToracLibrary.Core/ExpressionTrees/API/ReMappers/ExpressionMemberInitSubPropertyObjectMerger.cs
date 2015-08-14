using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ToracAttributes.ExpressionTreeAttributes;

namespace ToracLibrary.Core.ExpressionTrees.API.ReMappers
{

    /// <summary>
    /// Remaps 2 expressions into 1. Used mainly for dynamic select statements. So i have a base select expression. then i can set another one. then merge the 2. so i can keep adding on to the base select statement
    /// </summary>
    [LinqToObjectsCompatible]
    [EntityFrameworkCompatible]
    public class ExpressionMemberInitSubPropertyObjectMerger<TSource, TBaseDest, TPropertySubClassType> : ExpressionVisitor
    {

        #region Documentation

        /*Example of this.
      * Object
      * PolicyQuery
      *     -PolicyId
      *     -PolicyDate
      *     -PolicyExtensionData (this is an object)
      *          -PolExtId
      *          -PolExtTxt
      * 
      * I want to dynamically build up the select for the PolicyExtensionData.
      * Using the regular merge it will create something like this x => new PolicyQuery { PolicyId = 1, PolicyExtensionData = new PolicyExtData{PolExtId=1}, PolicyExtensionData = new PolicyExtData{PolExtTxt="test"}
      * That won't work with the 2 constructors for policy extension data
      * This method will let you build up the extension data and then combine it and merge it with the policy query data
      * 
      * IE.
      *  Expression<Func<Pol_Policy, PolicyExtension>> z1 = x => new PolicyExtension
                                                                    {
                                                                        PolicyExtensionAttribute3 =.Pol_PolicyExtension.PolicyExtAtt3
                                                                    };
      * 
      * Expression<Func<Pol_Policy, PolicyExtension>> z2 = x => new PolicyExtension
                                                               {
                                                                   PolicyExtensionAttribute2 = x.Pol_PolicyExtension.PolicyExtAtt2
                                                               };
      * 
      * Now we merger the 2 using the regular merge
      * var newExtDataExpression = z1.Merge(z2);
      * 
      * Now we combine it with the PolicyQueryData with the sub Object Merge (base select is some PolicyQuery Selector)
      * var QueryToRun = baseSelect.MergeSubObject(newExtDataExpression, x => x.PolicyExtensionData);
      */

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="BaseExpression">The base expression to merge into the member init list of the extended expression.</param>
        /// <param name="PropertyNameOfSubClass">Holds the property name of the sub class off of the base class. So we will use reflection to grab this property name off of the base object</param>
        /// <param name="MergeSubObjectPosition">Where do you want to merge the second expression. Before Or After The Base (First Expression)</param>
        public ExpressionMemberInitSubPropertyObjectMerger(Expression<Func<TSource, TBaseDest>> BaseExpression, Expression<Func<TBaseDest, TPropertySubClassType>> PropertyNameOfSubClass, ExpressionReMapperShared.ExpressionMemberInitMergerPosition MergeSubObjectPosition)
        {
            //let's set the merge position
            WhichMergeSubObjectPosition = MergeSubObjectPosition;

            //convert the body to a member init expression
            BaseInit = (MemberInitExpression)BaseExpression.Body;

            //now grab the parameter (first) and set the property
            BaseParameter = BaseExpression.Parameters;

            //set the property name
            PropertyNameOfSubClassOffOfBase = PropertyNameOfSubClass;
        }

        #endregion

        #region Variables And Properties

        /// <summary>
        /// Holds the property name of the sub class off of the base class. So we will use reflection to grab this property name off of the base object
        /// </summary>
        private Expression<Func<TBaseDest, TPropertySubClassType>> PropertyNameOfSubClassOffOfBase { get; }

        /// <summary>
        /// Where do you want to merge the second expression. Before Or After The Base (First Expression)
        /// </summary>
        private ExpressionReMapperShared.ExpressionMemberInitMergerPosition WhichMergeSubObjectPosition { get; }

        /// <summary>
        /// Base Member Init Expression
        /// </summary>
        private MemberInitExpression BaseInit { get; }

        /// <summary>
        /// Base Init Expression Parameters
        /// </summary>
        private IReadOnlyList<ParameterExpression> BaseParameter { get; }

        /// <summary>
        /// New Expression Parameter. The expression we are merging
        /// </summary>
        private IReadOnlyList<ParameterExpression> NewParameter { get; set; }

        /// <summary>
        /// Holds the final result. Need this because the TBaseDest is not the same as the extended destination. the visitor pattern requires it to be the same. so we set the property and that is the result
        /// </summary>
        public Expression<Func<TSource, TBaseDest>> FinalResult { get; private set; }

        #endregion

        #region Overrides

        /// <summary>
        /// Pick up the extended expressions range variable.
        /// </summary>
        /// <typeparam name="T">Type Of T To Visit</typeparam>
        /// <param name="Node">Lambda expression node</param>
        /// <returns>Unmodified expression tree</returns>
        protected override Expression VisitLambda<T>(Expression<T> Node)
        {
            //do we have a new parameter?
            if (NewParameter == null)
            {
                //set the new parameters from the node
                NewParameter = Node.Parameters;
            }

            //return the lambda expression
            return base.VisitLambda(Node);
        }

        /// <summary>
        /// Visit the member init expression of the extended expression. Merge the base expression into it.
        /// </summary>
        /// <param name="Node">Member init expression node.</param>
        /// <returns>Merged member init expression.</returns>
        protected override Expression VisitMemberInit(MemberInitExpression Node)
        {
            //rebind the visitor
            var RebindVisitor = new ExpressionParameterRemapper(NewParameter, BaseParameter);

            //case it into a member init expression
            var ReboundBaseInit = (MemberInitExpression)RebindVisitor.Visit(BaseInit);

            //let's go create the base destination. This is the main class
            var NewBaseObject = Expression.New(typeof(TBaseDest));

            //grab the property name
            string PropertyName = ((MemberExpression)PropertyNameOfSubClassOffOfBase.Body).Member.Name;

            //now let's grab the property name which is the sub property off of the base property
            MemberInfo SubPropertyInfo = typeof(TBaseDest).GetProperty(PropertyName);

            //make sure we have that property info
            if (SubPropertyInfo == null)
            {
                throw new IndexOutOfRangeException(string.Format($"Can't Find Property Name {PropertyNameOfSubClassOffOfBase} Off Of The Base Class"));
            }

            //now we need to merge the 2 binding lists
            var MergedBindingLists = new List<MemberBinding>();

            //based on the merge position put the bindings before or after
            if (WhichMergeSubObjectPosition == ExpressionReMapperShared.ExpressionMemberInitMergerPosition.Before)
            {
                //we want to add the sub object first...so add it first
                MergedBindingLists.Add(Expression.Bind(SubPropertyInfo, Node));

                //add the rest of the bindings next
                MergedBindingLists.AddRange(ReboundBaseInit.Bindings);
            }
            else
            {
                //add all the bindings first
                MergedBindingLists.AddRange(ReboundBaseInit.Bindings);

                //now add the sub object property last
                MergedBindingLists.Add(Expression.Bind(SubPropertyInfo, Node));
            }

            //now we build the final lambda...and we set the property. the extension method will return this item
            FinalResult = Expression.Lambda<Func<TSource, TBaseDest>>(Expression.MemberInit(NewBaseObject, MergedBindingLists), NewParameter);

            //return the node so the calling method is happy with type safety...we won't do anything with this node
            return Node;
        }

        #endregion

    }

}
