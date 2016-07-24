using System;
using System.Linq;
using ToracLibrary.Core.EnumUtilities;
using Xunit;

namespace ToracLibrary.UnitTest.Core.T4Templates
{

    /// <summary>
    /// Unit test to make sure the t4 template is working
    /// </summary>
    public class T4TemplateUnitTest
    {

        /// <summary>
        /// just going to make sure the t4 templates work
        /// </summary>
        [Fact]
        public void T4TemplateTest()
        {
            //if you ever change the EnumCreator.ttinclude, then you need to copy that file over to the unit test T4Templates template.
            //to run the t4 template, right click "run custom tool"

            //let's go make sure we have the correct enum values
            Assert.NotNull(typeof(T4Template));

            //go grab the enum values
            var EnumValues = EnumUtility.GetValuesLazy<T4Template.T4TemplateTest>().Select(x => x.ToString()).ToArray();

            //now check the values
            Assert.Equal(3, EnumValues.Length);

            //make sure we have Item 1, Item 2, Item 3
            for (int i = 1; i <= 3; i++)
            {
                //make sure we have a value
                Assert.NotNull(EnumValues.Any(x => x == ("Item" + i)));
            }
        }

    }

}