using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DasContract.Editor.Interfaces.Exceptions;

namespace DasContract.Editor.Interfaces
{
    public interface ICRUDInterfaceAsync<TEntity, TId>
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
        /// <exception cref="NotFoundException"></exception>
        Task<TEntity> GetAsync(TId id);

        /// <summary>
        /// Updates existing instance
        /// </summary>
        /// <param name="item">Modified instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        Task UpdateAsync(TEntity item);

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="item">New item</param>
        /// <exception cref="AlreadyExistsException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task InsertAsync(TEntity item);

        /// <summary>
        /// Deletes an instance
        /// </summary>
        /// <param name="id">Target instance</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        Task DeleteAsync(TId id);
    }
}
