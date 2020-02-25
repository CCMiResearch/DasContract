using System;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Interfaces;

namespace DasContract.Editor.DataPersistence.Repositories.Interfaces
{
    public interface IContractFileSessionRepository : ICRUDInterfaceAsync<ContractFileSession, string>
    {
        
    }
}
