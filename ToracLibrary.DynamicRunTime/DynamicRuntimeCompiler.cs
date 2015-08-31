using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;

namespace ToracLibrary.DynamicRunTime
{

    /// <summary>
    /// API to allow you to compile code from a string at run time using Roslyn.
    /// </summary>
    public static class DynamicRuntimeCompiler
    {

        /// <summary>
        /// Compile code from a string at run time
        /// </summary>
        /// <param name="CodeToCompile">Code to compile from a string</param>
        /// <param name="TypesToReferenceLocationPath">Types to reference. ie: typeof(object).Assembly.Location</param>
        /// <returns>Compile Time Results. </returns>
        public static DynamicRuntimeResults CompileCodeAtRunTimeLazy(string CodeToCompile, params string[] TypesToReferenceLocationPath)
        {
            //let's parse this into a syntax tree
            var SyntaxTree = CSharpSyntaxTree.ParseText(CodeToCompile);

            //let's go build the metadata references
            var MetaDataReferencesToUse = new List<MetadataReference>();

            //loop through the parameters
            if (TypesToReferenceLocationPath.AnyWithNullCheck())
            {
                //loop through the location paths
                foreach (var ReferenceLocation in TypesToReferenceLocationPath)
                {
                    //add the reference
                    MetaDataReferencesToUse.Add(MetadataReference.CreateFromFile(ReferenceLocation));
                }
            }

            //go compile this thing
            var Compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: new[] { SyntaxTree },
                references: MetaDataReferencesToUse,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            //return errors
            IEnumerable<Diagnostic> FailuresOfCompilation = null;

            //the updated assembly
            Assembly UpdatedAssembly = null;

            //let's go write the result
            using (var MemoryStreamToWrite = new MemoryStream())
            {
                //go emit the result
                var ResultOfCompilation = Compilation.Emit(MemoryStreamToWrite);

                //were we able to compile?
                if (!ResultOfCompilation.Success)
                {
                    //do we have any failures
                    FailuresOfCompilation = ResultOfCompilation.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);
                }
                else
                {
                    //we were able to compile, start at the beg of the memory stream
                    MemoryStreamToWrite.Seek(0, SeekOrigin.Begin);

                    //now load the memory stream we wrote about
                    UpdatedAssembly = Assembly.Load(MemoryStreamToWrite.ToArray());
                }
            }

            //return the model now
            return new DynamicRuntimeResults(FailuresOfCompilation, UpdatedAssembly);
        }

    }

}
