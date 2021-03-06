﻿using System;
using System.Linq;
using ToracLibrary.DynamicRunTime;
using Xunit;

namespace ToracLibrary.UnitTest.DynamicRunTime
{

    /// <summary>
    /// Unit test for dynamic runtime code with Roslyn
    /// </summary>
    public class DynamicRunTime
    {

        #region Unit Test

        [Fact(Skip = DisableSpecificUnitTestAreas.DynamicRuntimeUnitTestFlag)]
        public void DynamicRunTimeTestTest1()
        {
            //let's go build our sample code
            var CodeToCompile = @"
                                using System;

                                namespace ToracLibrary.UnitTest.DynamicRunTime
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
            Assert.False(ResultOfCompile.CompileTimeErrorsLazy().Any());

            //grab the assembly reference type
            Type ClassTypeToRun = ResultOfCompile.UpdatedAssembly.GetType("ToracLibrary.UnitTest.DynamicRunTime.Writer");

            //since this is an instance method, let's create the type
            var ClassTypeToRunInstance = Activator.CreateInstance(ClassTypeToRun);

            //grab the method
            var MethodToRun = ClassTypeToRunInstance.GetType().GetMethod("Write");

            //string to pass in
            const string ParameterNameValue = "Test123";

            //go invoke the method
            Assert.Equal(ParameterNameValue, MethodToRun.Invoke(ClassTypeToRunInstance, new object[] { ParameterNameValue }));
        }

        #endregion

    }

}
