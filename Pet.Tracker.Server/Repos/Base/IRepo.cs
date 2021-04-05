using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pet.Tracker.Server
{
    /// <summary>
    /// The base class of the repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepo<T> where T : EntityBase
    {
        /// <summary>
        /// the count within a table 
        /// </summary>
        int Count { get; }

        /// <summary>
        /// the table has made changes
        /// </summary>
        bool HasChanges { get; }

        /// <summary>
        /// Find by id on the table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Find(int? id);

        /// <summary>
        /// Gets the first item within the table
        /// </summary>
        /// <returns></returns>
        Task<T> GetFirst();

        /// <summary>
        /// Gets everything within the table
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets the range of specified data
        /// </summary>
        /// <param name="skip">data to skip from</param>
        /// <param name="take">data to take from</param>
        /// <returns></returns>
        IEnumerable<T> GetRange(int skip, int take);

        /// <summary>
        /// Adds data on to the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        Task<int> Add(T entity, bool persist = true);

        /// <summary>
        /// Adds a range of data on to the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        Task<int> AddRange(IEnumerable<T> entities, bool persist = true);

        /// <summary>
        /// Updates data on the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        int Update(T entity, bool persist = true);

        /// <summary>
        /// Updates a range data on the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        int UpdateRange(IEnumerable<T> entities, bool persist = true);

        /// <summary>
        /// Deletes data on the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        int Delete(T entity, bool persist = true);

        /// <summary>
        /// Deletes a range of data on the table
        /// </summary>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        int DeleteRange(IEnumerable<T> entities, bool persist = true);

        /// <summary>
        /// deletes data by the given id on the table
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="entity">The base classes model</param>
        /// <param name="persist"></param>
        /// <returns></returns>
        int Delete(int id, byte[] timeStamp, bool persist = true);

        /// <summary>
        /// Saves changes made onto the database
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
