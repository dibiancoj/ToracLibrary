using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.DataProviders.EntityFrameworkDP.Repository
{

    /// <summary>
    /// Provides an implementaiton for a typed repository for a single type of T
    /// </summary>
    /// <typeparam name="TRepositoryType">Type of the repository</typeparam>
    /// <remarks>Not all methods are added. Add as needed</remarks>
    public class TypedRepository<TEfContextType, TRepositoryType> : ITypedRepository<TRepositoryType>
        where TEfContextType : DbContext, new()
        where TRepositoryType : class
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ContextToSet">Entity Framework Context</param>
        public TypedRepository(EntityFrameworkDP<TEfContextType> ContextToSet)
        {
            Context = ContextToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Entity Framework Context
        /// </summary>
        private EntityFrameworkDP<TEfContextType> Context { get; }

        #region Disposal Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool Disposed { get; set; }

        #endregion

        #endregion

        #region Add

        public void Add(TRepositoryType EntityToAdd, bool CommitChanges)
        {
            Context.Add(EntityToAdd, CommitChanges);
        }

        public void AddRange(IEnumerable<TRepositoryType> EntitiesToAdd, bool CommitChanges)
        {
            Context.AddRange(EntitiesToAdd, CommitChanges);
        }

        #endregion

        #region General

        public void Attach(TRepositoryType EntityToAttach)
        {
            Context.Attach(EntityToAttach);
        }

        public IQueryable<TRepositoryType> Fetch(bool TrackRecords)
        {
            return Context.Fetch<TRepositoryType>(TrackRecords);
        }

        public IQueryable<TRepositoryType> Find(Expression<Func<TRepositoryType, bool>> WherePredicate, bool TrackRecords)
        {
            return Context.Find(WherePredicate, TrackRecords);
        }

        #endregion   

        #region Delete

        public void Delete(TRepositoryType EntityToDelete, bool CommitChanges)
        {
            Context.Delete(EntityToDelete, CommitChanges);
        }

        public void Delete(Expression<Func<TRepositoryType, bool>> Predicate, bool CommitChanges)
        {
            Context.Delete(Predicate, CommitChanges);
        }

        public Task DeleteAsync(TRepositoryType EntityToDelete, bool CommitChanges)
        {
            return Context.DeleteAsync(EntityToDelete, CommitChanges);
        }

        public Task DeleteAsync(Expression<Func<TRepositoryType, bool>> Predicate, bool CommitChanges)
        {
            return Context.DeleteAsync(Predicate, CommitChanges);
        }

        public Task DeleteRangeAsync(IEnumerable<TRepositoryType> EntitiesToDelete, bool CommitChanges)
        {
            return Context.DeleteRangeAsync(EntitiesToDelete, CommitChanges);
        }

        #endregion

        #region Save

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        #endregion

        #region Upsert

        public void Upsert(TRepositoryType EntityToAddOrUpdate, bool CommitChanges)
        {
            Context.Upsert(EntityToAddOrUpdate, CommitChanges);
        }

        public void UpsertRange(IEnumerable<TRepositoryType> EntitiesToAddOrUpdate, bool CommitChanges)
        {
            Context.Upsert(EntitiesToAddOrUpdate, CommitChanges);
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    //dispose of the ef context
                    this.Context.Dispose();
                }
            }
            this.Disposed = true;
        }

        #endregion

    }
}
