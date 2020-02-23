using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Services.Interfaces
{
    public interface ICRUDService<TEntity>
    {
        /// <summary>
        /// Getter for all instances
        /// </summary>
        /// <returns>IEnumerable object of all instances</returns>
        Task<IEnumerable<TEntity>> GetAsync();

        /// <summary>
        /// Getter for specific instance
        /// </summary>
        /// <param name="id">ID of target instance</param>
        /// <returns>Target instance</returns>
        Task<TEntity> GetAsync(int id);

        /// <summary>
        /// Updates existing instance
        /// </summary>
        /// <param name="id">Target instance</param>
        /// <param name="item">Modified instance</param>
        Task UpdateAsync(int id, TEntity item);

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="item">New item</param>
        Task AddAsync(TEntity item);

        /// <summary>
        /// Deletes an instance
        /// </summary>
        /// <param name="id">Target instance</param>
        Task DeleteAsync(int id);
    }
}
