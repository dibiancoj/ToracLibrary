using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke.Parameters
{

    /// <summary>
    /// Helper class to allow you to pass in the type of T off of the method. Used in Enumerable.Contains(IenumerableOfT Lst, T item) this would be for item
    /// </summary>
    public class GenericMethodTypeParameter : BaseGenericParameter
    {
        public override bool IsGenericType => true;

        public override bool ParametersMatch(ParameterInfo MethodParameterToMatch)
        {
            throw new NotImplementedException();
        }
    }

}
