using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.Mathematical.Age;

namespace ToracLibraryTest.UnitsTest.Core
{

    /// <summary>
    /// Unit test for age functionality
    /// </summary>
    [TestClass]
    public class AgeTest
    {

        #region Age

        /// <summary>
        /// Test Age
        /// </summary>
        [TestCategory("Core.Mathematical.Age")]
        [TestCategory("Core.Mathematical")]
        [TestCategory("Core")]
        [TestMethod]
        public void AgeTest1()
        {
            //should be exactly 10
            Assert.AreEqual(10, AgeCalculation.CalculateAge(DateTime.Now.AddYears(-10)));

            //not there birthday yet for the current year..subtract 1
            Assert.AreEqual(9, AgeCalculation.CalculateAge(DateTime.Now.AddYears(-10).AddDays(1)));

            //birthday passed for the current year
            Assert.AreEqual(10, AgeCalculation.CalculateAge(DateTime.Now.AddYears(-10).AddDays(-1)));

            //birthday passed for the current year
            Assert.AreEqual(20, AgeCalculation.CalculateAge(DateTime.Now.AddYears(-20).AddDays(-1)));

            //birthday passed for the current year
            Assert.AreEqual(25, AgeCalculation.CalculateAge(DateTime.Now.AddYears(-25).AddDays(-1)));
        }

        #endregion

    }

}
