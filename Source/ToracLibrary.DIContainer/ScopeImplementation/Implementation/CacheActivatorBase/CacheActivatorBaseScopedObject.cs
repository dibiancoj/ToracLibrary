using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees;

namespace ToracLibrary.DIContainer.ScopeImplementation.Implementation.CacheActivatorBase
{

    /// <summary>
    /// Base class for any implementation that want's to cache the constructor activator. Mainly used so we don't duplicate code
    /// </summary>
    /// <remarks>Only abstract so we can make this class be inherited. I guess it doesn't need to be, but for now it was designed to be inherited for per thread scope and transient scope</remarks>
    internal abstract class CacheActivatorBaseScopedObject
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConstructorToCreateObjectsWith">Constructor information to use to create the object with</param>
        public CacheActivatorBaseScopedObject(ConstructorInfo ConstructorToCreateObjectsWith)
        {
            //go create the cached activator. With the fluent style we dont know if they will pass in there own constructor lambda. so we just build this each time. This is cached only when the app starts so it isn't a performance issue
            CachedActivator = ExpressionTreeHelpers.BuildNewObject(ConstructorToCreateObjectsWith, ConstructorToCreateObjectsWith.GetParameters()).Compile();
        }

        #endregion

        #region Protected Transient Specific Properties

        /// <summary>
        /// Instead of using Activator.CreateInstance, we are going to an expression tree to create a new object. This gets compiled on the first time we request the item
        /// </summary>
        protected Func<object[], object> CachedActivator { get; }

        #endregion

    }

}
