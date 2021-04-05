using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// The Database base table  class and queries
    /// </summary>
    /// <typeparam name="T">Table</typeparam>
    public abstract class RepoBase<T> : IDisposable, IRepo<T> where T : EntityBase, new()
    {


        /// <summary>
        /// The database table to perform queries on
        /// </summary>
        protected DbSet<T> Table;

        /// <summary>
        /// The database representational model for our application
        /// </summary>
        protected readonly ApplicationDbContext  applicationDb;

        /// <summary>
        /// Default constructor
        /// </summary>
        protected RepoBase()
        {
            applicationDb = new ApplicationDbContext();
            

        }

        /// <summary>
        /// Default constructor, expecting database options passed in
        /// </summary>
        /// <param name="options">The database context options</param>
        protected RepoBase(DbContextOptions<ApplicationDbContext> options)
        {
            applicationDb = new ApplicationDbContext(options);
            Table = applicationDb.Set<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        bool _disposed = false;


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                // Free any other managed ints here.
                //
            }
            applicationDb.Dispose();
            _disposed = true;
        }

        public int SaveChanges()
        {
            try
            {
                return applicationDb.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
        }

        public virtual async Task<int> Add(T entity, bool persist = true)
        {

            await Table.AddAsync(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual async Task<int> AddRange(IEnumerable<T> entities, bool persist = true)
        {
            await Table.AddRangeAsync(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }
        public virtual int Delete(T entity, bool persist = true)
        {
           
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }
        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        internal T GetEntryFromChangeTracker(int? id)
        {
            return applicationDb.ChangeTracker.Entries<T>().Select((EntityEntry e) => (T)e.Entity)
            .FirstOrDefault(x => x.Id == id);
        }

        public int Delete(int id, byte[] timeStamp, bool persist = true)
        {
            var entry = GetEntryFromChangeTracker(id);
            if (entry != null)
            {
                if (entry.TimeStamp == timeStamp)
                {
                    return Delete(entry, persist);
                }
                throw new Exception("Unable to delete due to concurrency violation.");
            }
            applicationDb.Entry(new T { Id = id, TimeStamp = timeStamp }).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;

        }

        public async Task<T> Find(int? id) => await Table.FindAsync(id);

        public async Task<T> GetFirst() => await Table.FirstOrDefaultAsync();
        public virtual IEnumerable<T> GetAll() => Table;

        internal IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take) => query.Skip(skip).Take(take);
        public virtual IEnumerable<T> GetRange(int skip, int take) => GetRange(Table, skip, take);

        public bool HasChanges => applicationDb.ChangeTracker.HasChanges();
        public int Count => Table.Count();
    }
}

