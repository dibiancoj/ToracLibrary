using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.IEnumerableExtensions;
using ToracLibrary.DynamicRunTime;

namespace ToracLibraryTest.UnitsTest.DynamicRunTime
{

    /// <summary>
    /// Unit test for dynamic runtime code with Roslyn
    /// </summary>
    [TestClass]
    public class DynamicRunTime
    {

        #region Unit Test

        [TestCategory("DynamicRunTime")]
        [TestMethod]
        public void DynamicRunTimeTestTest1()
        {
            //let's go build our sample code
            var CodeToCompile = @"
                                using System;

                                namespace ToracLibraryTest.UnitsTest.DynamicRunTime
                                {
                                           public class Writer
                                            {
                                                  public string Write(string MessageToDisplay)
                                                  {
                                                       return MessageToDisplay;
                                                  }
                                           }
                                }";

            //let's go compile this
            var ResultOfCompile = DynamicRuntimeCompiler.CompileCodeAtRunTimeLazy(CodeToCompile, typeof(object).Assembly.Location);

            //make sure we have no errors
            Assert.IsFalse(ResultOfCompile.CompileTimeErrorsLazy().Any());

            //grab the assembly reference type
            Type ClassTypeToRun = ResultOfCompile.UpdatedAssembly.GetType("ToracLibraryTest.UnitsTest.DynamicRunTime.Writer");

            //since this is an instance method, let's create the type
            var ClassTypeToRunInstance = Activator.CreateInstance(ClassTypeToRun);

            //grab the method
            var MethodToRun = ClassTypeToRunInstance.GetType().GetMethod("Write");

            //string to pass in
            const string ParameterNameValue = "Test123";

            //go invoke the method
            Assert.AreEqual(ParameterNameValue, MethodToRun.Invoke(ClassTypeToRunInstance, new object[] { ParameterNameValue }));
        }

        #endregion

    }

}
