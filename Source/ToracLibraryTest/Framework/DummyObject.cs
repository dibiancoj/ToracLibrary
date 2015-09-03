
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibraryTest.Framework.DummyObjects
{

    /// <summary>
    /// A dummy object that is used throughout the tests
    /// </summary>
    [Serializable]
    public class DummyObject
    {

        #region Constructor
        public DummyObject(int IdToSet, string DescriptionToSet)
        {
            Id = IdToSet;
            Description = DescriptionToSet;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Description { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a dummy list of ienumerable of objects
        /// </summary>
        /// <param name="HowManyItems">How many items to build</param>
        /// <returns>yield return ienumerable of DummyObjects</returns>
        public static IEnumerable<DummyObject> CreateDummyListLazy(int HowManyItems)
        {
            //loop through however many items you want
            for (int i = 0; i < HowManyItems; i++)
            {
                //create a new record
                yield return new DummyObject(i, "Test_" + i.ToString());
            }
        }

        /// <summary>
        /// Create 1 dummy record
        /// </summary>
        /// <returns>DummyObject</returns>
        public static DummyObject CreateDummyRecord()
        {
            return CreateDummyListLazy(1).ElementAt(0);
        }

        #endregion

    }

}
