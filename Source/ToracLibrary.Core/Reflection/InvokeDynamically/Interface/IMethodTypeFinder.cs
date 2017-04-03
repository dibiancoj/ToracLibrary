using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke
{

    /// <summary>
    /// Interface to find the correct method so we can invoke it dynamically
    /// </summary>
    public interface IMethodTypeFinder
    {

        /// <summary>
        /// Find the method we want to invoke
        /// </summary>
        /// <returns>MethodInfo ready to be invoke at run time with everything configured</returns>
        MethodInfo FindMethodToInvoke();

    }

}
