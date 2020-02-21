using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Editor.DataPersistence.Repositories.Interfaces
{
    public interface ICRUDRepositoryAsync<TEntity, TId>
    {
        /// <summary>
        /// Getter for all instances
        /// </summary>
        /// <returns>List of all instances</returns>
        Task<List<TEntity>> GetAsync();

        /// <summary>
        /// Getter for specific instance
        /// </summary>
        /// <param name="id">ID of target instance</param>
        /// <returns>Target instance</returns>
        Task<TEntity> GetAsync(TId id);

        /// <summary>
        /// Updates existing instance
        /// </summary>
        /// <param name="id">Target instance to udpate</param>
        /// <param name="item">Modified instance</param>
        Task UpdateAsync(TId id, TEntity item);

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="item">New item</param>
        Task InsertAsync(TEntity item);

        /// <summary>
        /// Deletes an instance
        /// </summary>
        /// <param name="id">Target instance</param>
        Task DeleteAsync(TId id);
    }
}
