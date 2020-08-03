using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DasContract.Editor.AppLogic.Facades.Interfaces;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.DataPersistence.Repositories.Interfaces;

namespace DasContract.Editor.AppLogic.Facades
{
    public class ContractFileSessionFacade : IContractFileSessionFacade
    {
        readonly IContractFileSessionRepository repository;

        public ContractFileSessionFacade(IContractFileSessionRepository repository)
        {
            this.repository = repository;
        }

        public Task DeleteAsync(string id) => repository.DeleteAsync(id);

        public Task<List<ContractFileSession>> GetAsync() => repository.GetAsync();

        public Task<ContractFileSession> GetAsync(string id) => repository.GetAsync(id);

        public Task InsertAsync(ContractFileSession item) => repository.InsertAsync(item);

        public Task UpdateAsync(ContractFileSession item) => repository.UpdateAsync(item);
    }
}
