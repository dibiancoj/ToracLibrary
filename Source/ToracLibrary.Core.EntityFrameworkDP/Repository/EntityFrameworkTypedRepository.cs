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
    /// <typeparam name="TEfContextType">Entity framework type</typeparam>
    /// <remarks>Not all methods are added. Add as needed</remarks>
    public class EntityFrameworkTypedRepository<TEfContextType, TRepositoryType> : IEntityFrameworkTypedRepository<TRepositoryType>
        where TEfContextType : DbContext, new()
        where TRepositoryType : class
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ContextToSet">Entity Framework Context</param>
        public EntityFrameworkTypedRepository(EntityFrameworkDP<TEfContextType> ContextToSet)
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

        #region General

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="EntityToAttach">Entity To Attach</param>
        public void Attach(TRepositoryType EntityToAttach)
        {
            Context.Attach(EntityToAttach);
        }

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<TRepositoryType> Fetch(bool TrackRecords)
        {
            return Context.Fetch<TRepositoryType>(TrackRecords);
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="WherePredicate">Criteria to match on</param>
        /// <param name="TrackRecords">Do you want to track the items that are returned so they can be updated and saved after you receive them?</param>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public IQueryable<TRepositoryType> Find(Expression<Func<TRepositoryType, bool>> WherePredicate, bool TrackRecords)
        {
            return Context.Find(WherePredicate, TrackRecords);
        }

        #endregion   

        #region Add

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="EntityToAdd">Entity To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void Add(TRepositoryType EntityToAdd, bool CommitChanges)
        {
            Context.Add(EntityToAdd, CommitChanges);
        }

        /// <summary>
        /// Adds the list of records passed in to the specified entity
        /// </summary>
        /// <param name="EntitiesToAdd">List Of Entities To Add</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void AddRange(IEnumerable<TRepositoryType> EntitiesToAdd, bool CommitChanges)
        {
            Context.AddRange(EntitiesToAdd, CommitChanges);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        public void Delete(TRepositoryType EntityToDelete, bool CommitChanges)
        {
            Context.Delete(EntityToDelete, CommitChanges);
        }

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="EntityToDelete">Entity to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public Task DeleteAsync(TRepositoryType EntityToDelete, bool CommitChanges)
        {
            return Context.DeleteAsync(EntityToDelete, CommitChanges);
        }

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        public void Delete(Expression<Func<TRepositoryType, bool>> Predicate, bool CommitChanges)
        {
            Context.Delete(Predicate, CommitChanges);
        }

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="Predicate">Criteria to match on</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false.</remarks>
        public Task DeleteAsync(Expression<Func<TRepositoryType, bool>> Predicate, bool CommitChanges)
        {
            return Context.DeleteAsync(Predicate, CommitChanges);
        }

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void DeleteRange(IEnumerable<TRepositoryType> EntitiesToDelete, bool CommitChanges)
        {
            Context.DeleteRange(EntitiesToDelete, CommitChanges);
        }

        /// <summary>
        /// Deletes the specified entities passed in
        /// </summary>
        /// <param name="EntitiesToDelete">Entities to delete</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public Task DeleteRangeAsync(IEnumerable<TRepositoryType> EntitiesToDelete, bool CommitChanges)
        {
            return Context.DeleteRangeAsync(EntitiesToDelete, CommitChanges);
        }

        #endregion

        #region Upsert

        /// <summary>
        /// Either Add Or Update An Entity Based On If The Record Exists In The Db Already
        /// </summary>
        /// <param name="EntityToAddOrUpdate">Entity To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void Upsert(TRepositoryType EntityToAddOrUpdate, bool CommitChanges)
        {
            Context.Upsert(EntityToAddOrUpdate, CommitChanges);
        }

        /// <summary>
        /// Either Add Or Update An IEnumerable Of Entity Based On The Func You Pass in
        /// </summary>
        /// <param name="EntitiesToAddOrUpdate">Entities To Either Add Or Update</param>
        /// <param name="CommitChanges">Do you want to commit the changes in this method. If false make sure you call SaveChanges to commit the database</param>
        /// <remarks>Must Call Save Changes To Commit To The Database If CommitChanges is false. Save Changes is called normally. Set to false and call savechanges async if you want to async call save changes</remarks>
        public void UpsertRange(IEnumerable<TRepositoryType> EntitiesToAddOrUpdate, bool CommitChanges)
        {
            Context.Upsert(EntitiesToAddOrUpdate, CommitChanges);
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves all context changes
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// Saves all context changes async
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        /// <remarks>You can call await SaveChangesAsync in the calling method</remarks>
        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
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
