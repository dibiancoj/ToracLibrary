using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ReflectionDynamic.Invoke.Parameters;

namespace ToracLibrary.Core.ReflectionDynamic.Invoke.BaseClasses
{

    /// <summary>
    /// Base class for the implementation so they share between the implementations
    /// </summary>
    public class BaseMethodTypeFinder
    {

        /// <summary>
        /// Do the parameters match between the two method signatures
        /// </summary>
        /// <param name="ParametersWeAreLookingFor">Parameter set we are looking for</param>
        /// <param name="ParametersMethodContains">Parameters the method we are checking contains</param>
        /// <returns>true if the parameters match</returns>
        protected static bool ParameterTypesMatch(IList<GenericTypeParameter> ParametersWeAreLookingFor, IList<ParameterInfo> ParametersMethodContains)
        {
            //need to check the parameters now
            for (int i = 0; i < ParametersWeAreLookingFor.Count; i++)
            {
                //is this a generic parameter?
                if (ParametersWeAreLookingFor[i].IsGenericType)
                {
                    //is this T? not ienumerable<T>....just (T item)
                    if (ParametersMethodContains[i].ParameterType.IsGenericParameter)
                    {
                        continue;
                    }

                    //if the parameter types don't match then this method doesn't match
                    if (ParametersMethodContains[i].ParameterType.GetGenericTypeDefinition() != ParametersWeAreLookingFor[i].ParameterType)
                    {
                        return false;
                    }
                }
                else if (ParametersMethodContains[i].ParameterType != ParametersWeAreLookingFor[i].ParameterType)
                {
                    //regular parameter doesn't match
                    return false;
                }
            }

            //everything matched...return treu
            return true;
        }

    }

}
