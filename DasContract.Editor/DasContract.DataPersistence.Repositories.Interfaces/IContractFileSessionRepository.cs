using System;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Entities;

namespace DasContract.Editor.DataPersistence.Repositories.Interfaces
{
    public interface IContractFileSessionRepository : ICRUDRepositoryAsync<ContractFileSession, string>
    {
        /// <summary>
        /// Removes all expired sessions
        /// </summary>
        /// <returns>Task</returns>
        Task RemoveExpiredSessionsAsync();


    }
}
