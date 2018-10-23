using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.TaskExtensions;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    public class TaskExtensionTest
    {

        private static async Task<string> AsyncStubMethod()
        {
            await Task.Delay(50);

            return "Test 123";
        }

        [Fact]
        public async Task ThenResultTest()
        {
            Assert.Equal("T", await AsyncStubMethod().Then(tsk => tsk.Substring(0, 1)));
        }

    }

}
