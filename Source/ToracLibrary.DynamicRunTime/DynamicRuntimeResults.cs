using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.DynamicRunTime
{

    /// <summary>
    /// Holds the results of the dynamic run time compilation
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class DynamicRuntimeResults
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CompileErrorsToSet">Compile Time Errors</param>
        /// <param name="UpdatedAssemblyToSet">Updated Assembly</param>
        public DynamicRuntimeResults(IEnumerable<Diagnostic> CompileErrorsToSet, Assembly UpdatedAssemblyToSet)
        {
            CompileErrors = CompileErrorsToSet;
            UpdatedAssembly = UpdatedAssemblyToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Compile Errors
        /// </summary>
        private IEnumerable<Diagnostic> CompileErrors { get; }

        /// <summary>
        /// Assembly that has the new code
        /// </summary>
        public Assembly UpdatedAssembly { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the compile time errors
        /// </summary>
        /// <returns>yield return Diagnostic. Call CompileError.GetMessage() for full detail</returns>
        public IEnumerable<Diagnostic> CompileTimeErrorsLazy()
        {
            //do we have any errors?
            if (CompileErrors.AnyWithNullCheck())
            {
                //loop through the errors, and yield it
                foreach (var Error in CompileErrors)
                {
                    yield return Error;
                }
            }
        }

        #endregion

    }

}
