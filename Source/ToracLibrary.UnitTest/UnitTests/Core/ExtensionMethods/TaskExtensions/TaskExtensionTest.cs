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

        private static async Task<string> AsyncStub1Method()
        {
            await Task.Delay(50);

            return "Test 123";
        }

        private static async Task<string> AsyncStub2Method()
        {
            await Task.Delay(50);

            return "T1";
        }

        [Fact]
        public async Task ThenResultTest()
        {
            Assert.Equal("T", await AsyncStub1Method().Then(tsk => tsk.Substring(0, 1)));
        }

        [Fact]
        public async Task ThenResultAwaitContinuationTest()
        {
            Assert.Equal("T1", await AsyncStub1Method().Then(tsk => AsyncStub2Method()));
        }

        [Fact]
        public async Task ThenResultWithConfigureAwaitTest()
        {
            Assert.Equal("T", await AsyncStub1Method().ConfigureAwait(false).Then(tsk => tsk.Substring(0, 1)));
        }

        [Fact]
        public async Task ConfigureAwaitThenResultWithConfigureAwaitTest()
        {
            Assert.Equal("T1", await AsyncStub1Method().ConfigureAwait(false).Then(tsk => AsyncStub2Method().ConfigureAwait(false)));
        }

    }

}
