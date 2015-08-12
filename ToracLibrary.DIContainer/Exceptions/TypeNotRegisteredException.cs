using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Exceptions
{

    /// <summary>
    /// Exception when you try to resolve a type that has not been registered
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class TypeNotRegisteredException : Exception
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeThatTriedToBeResolvedToSet">Type that tried to be resolved but was not found</param>
        public TypeNotRegisteredException(Type TypeThatTriedToBeResolvedToSet)
        {
            //set the property
            TypeThatTriedToBeResolved = TypeThatTriedToBeResolvedToSet;
        }

        #region Properties

        /// <summary>
        /// Type that tried to be resolved but was not found
        /// </summary>
        public Type TypeThatTriedToBeResolved { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override the exception to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"The type {TypeThatTriedToBeResolved.Name} has not been registered");
        }

        #endregion

    }

}
