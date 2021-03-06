﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.DIContainer.Exceptions
{

    /// <summary>
    /// Exception when you try to resolve a type and we find multiple types. User should use a factory name
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class MultipleTypesFoundException : Exception
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeThatTriedToBeResolvedToSet">Type that tried to be resolved but was not found</param>
        public MultipleTypesFoundException(Type TypeThatTriedToBeResolvedToSet)
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
            return $"The type {TypeThatTriedToBeResolved.Name} has multiple types registered. We weren't able to resolve it down to a single implementation. Please give each registration a unique factory name";
        }

        #endregion

    }

}
