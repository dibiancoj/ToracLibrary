using Moq;
using System.Linq;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP;
using ToracLibrary.Core.DataProviders.EntityFrameworkDP.Repository;
using ToracLibrary.UnitTest.EntityFramework.DataContext;
using Xunit;

namespace ToracLibrary.UnitTest.Core.DataProviders.EntityFrameworkDP
{

    /// <summary>
    /// Unit test for entity framework repositories
    /// </summary>
    [Collection("DatabaseUnitTests")]
    public class EntityFrameworkRepositoryTest
    {

        #region Framework

        /// <summary>
        /// Repository pattern so we can run a test against the EF Data Repository
        /// </summary>
        private class Ref_TestRepository
        {

            #region Constructor

            public Ref_TestRepository(IEntityFrameworkRepository context)
            {
                Context = context;
            }

            #endregion

            #region Properties

            private IEntityFrameworkRepository Context { get; }

            #endregion

            #region Methods

            public int SumUpIds()
            {
                return Context.Fetch<Ref_Test>(false).Sum(x => x.Id);
            }

            #endregion

        }

        private class Ref_TestTypedRepository
        {

            #region Constructor

            public Ref_TestTypedRepository(IEntityFrameworkTypedRepository<Ref_Test> context)
            {
                Context = context;
            }

            #endregion

            #region Properties

            private IEntityFrameworkTypedRepository<Ref_Test> Context { get; }

            #endregion

            #region Methods

            public int SumUpIds()
            {
                return Context.Fetch(false).Sum(x => x.Id);
            }

            #endregion

        }

        #endregion

        #region Unit Tests

        /// <summary>
        /// Make sure we can mock the entity framework data repository
        /// </summary>
        [Fact]
        public void MockEntityFrameworkRepositoryTest1()
        {
            //mock the ef data repository
            var MockEFDataRepository = new Mock<IEntityFrameworkRepository>();

            //data to return
            var DataToReturn = new[]
                {
                    new Ref_Test {  Id = 1, Description = "Test1"},
                    new Ref_Test {  Id = 2, Description = "Test2"},
                };

            //mock the call
            MockEFDataRepository.Setup(x => x.Fetch<Ref_Test>(false))
                .Returns(DataToReturn.AsQueryable());

            //go make the call
            var SumOfIdsResult = new Ref_TestRepository(MockEFDataRepository.Object).SumUpIds();

            //what was the reesult
            Assert.Equal(DataToReturn.Sum(x => x.Id), SumOfIdsResult);

            //make sure we only call it once
            MockEFDataRepository.Verify(x => x.Fetch<Ref_Test>(false), Times.Once);
        }

        /// <summary>
        /// Make sure we can mock the entity framework Typed data repository
        /// </summary>
        [Fact]
        public void MockEntityFrameworkTypedRepositoryTest1()
        {
            //mock the ef data repository
            var MockEFTypedDataRepository = new Mock<IEntityFrameworkTypedRepository<Ref_Test>>();

            //data to return
            var DataToReturn = new[]
                {
                    new Ref_Test {  Id = 1, Description = "Test1"},
                    new Ref_Test {  Id = 2, Description = "Test2"},
                };

            //mock the call
            MockEFTypedDataRepository.Setup(x => x.Fetch(false))
                .Returns(DataToReturn.AsQueryable());

            //go make the call
            var SumOfIdsResult = new Ref_TestTypedRepository(MockEFTypedDataRepository.Object).SumUpIds();

            //what was the reesult
            Assert.Equal(DataToReturn.Sum(x => x.Id), SumOfIdsResult);

            //make sure we only call it once
            MockEFTypedDataRepository.Verify(x => x.Fetch(false), Times.Once);
        }

        #endregion

    }

}
