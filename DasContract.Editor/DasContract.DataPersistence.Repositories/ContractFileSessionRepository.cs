using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.DbContexts;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.DataPersistence.Repositories.Interfaces;
using DasContract.Editor.DataPersistence.Repositories.Interfaces.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DasContract.Editor.DataPersistence.Repositories
{
    public class ContractFileSessionRepository : IContractFileSessionRepository
    {
        readonly ContractEditorDb context;

        public ContractFileSessionRepository(ContractEditorDb context)
        {
            this.context = context;
        }

        public async Task RemoveExpiredSessionsAsync()
        {
            var items = await context.ContractFileSessions.ToListAsync();
            foreach (var item in items)
                if (item.IsExpired())
                    context.ContractFileSessions.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<List<ContractFileSession>> GetAsync()
        {
            await RemoveExpiredSessionsAsync();
            return await context.ContractFileSessions.ToListAsync();
        }

        public async Task<ContractFileSession> GetAsync(string id)
        {
            await RemoveExpiredSessionsAsync();
            var res = await context.ContractFileSessions.FindAsync(id);
            if (res == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);
            return res;
        }

        public async Task AddAsync(ContractFileSession item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            await RemoveExpiredSessionsAsync();

            var res = await context.ContractFileSessions.FindAsync(item.Id);
            if (res != null)
                throw new AlreadyExistsException(nameof(ContractFileSession) + " " + item.Id);

            await context.ContractFileSessions.AddAsync(item);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await RemoveExpiredSessionsAsync();

            var res = await context.ContractFileSessions.FindAsync(id);
            if (res == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);

            context.ContractFileSessions.Remove(res);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, ContractFileSession item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (id != item.Id)
                throw new BadRequestException("Ids do not match");

            await RemoveExpiredSessionsAsync();

            var res = await context.ContractFileSessions.FindAsync(id);
            if (res == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);

            context.ContractFileSessions.Update(item);

            await context.SaveChangesAsync();
        }
    }
}
