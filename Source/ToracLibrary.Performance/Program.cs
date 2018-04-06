using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerformanceTest>();

            Console.WriteLine("Performance Test Complete. Press Any Key To Exit.");
            Console.ReadKey();
        }
    }
}
