using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Performance
{

    //[ShortRunJob] - if you want to run a quick test. Not as accurate but faster.
    [MemoryDiagnoser]
    public class PerformanceTest
    {

        [GlobalSetup]
        public void Init()
        {
        }

        private const string TestData = "test 123";

        [Benchmark(Baseline = true)]
        public string SurroundWithExtensionMethod()
        {
            return TestData.SurroundWith("**");
        }

        [Benchmark]
        public string NewSurroundWithExtensionMethod()
        {
            return TestData.SurroundWithNewTest("**");
        }

    }

    public static class NewMethod
    {
        public static string SurroundWithNewTest(this string StringToQuote, string StringToAddAtBegAndEnd)
        {
            return $"{StringToAddAtBegAndEnd}{StringToQuote}{StringToAddAtBegAndEnd}";
        }
    }
}
