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
    public class DummyObject
    {

        #region Properties

        public int Id { get; set; }

        public string txt { get; set; }

        public Nullable<int> IdNull { get; set; }

        /// <summary>
        /// For Cloning Test
        /// </summary>
        public DummyObject NestedObject { get; set; }

        public string duplicateTxt { get; set; }

        public List<DummyObject> DummyList { get; set; }

        public int LastIndexTest { get; set; }

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
                var NewObject = new DummyObject { Id = i, txt = "Test_" + i.ToString() };

                NewObject.duplicateTxt = "Dup";
                NewObject.NestedObject = new DummyObject { Id = i + 10000 };

                if (i == 1)
                {
                    NewObject.IdNull = i;
                    NewObject.duplicateTxt = "Dup = 1";
                }

                if (i == 2 || i == 3 || i == 4)
                {
                    NewObject.LastIndexTest = 5;
                }

                yield return NewObject;
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
