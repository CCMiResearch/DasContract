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

        public async Task RemoveExpiredSessionsAsync(IEnumerable<ContractFileSession> sessions)
        {
            if (sessions == null)
                throw new ArgumentNullException(nameof(sessions));

            foreach (var item in sessions)
                if (item.IsExpired())
                    context.Entry(item).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task RemoveIfExpired(ContractFileSession session)
        {
            await RemoveExpiredSessionsAsync(new ContractFileSession[] { session });
        }

        public async Task RemoveExpiredSessionsAsync()
        {
            await RemoveExpiredSessionsAsync(await context.ContractFileSessions.ToListAsync());
        }

        public async Task<List<ContractFileSession>> GetAsync()
        {
            await RemoveExpiredSessionsAsync();
            return await context.ContractFileSessions.ToListAsync();
        }

        public async Task<ContractFileSession> GetAsync(string id)
        {
            var toCheck = await context.ContractFileSessions.FindAsync(id);
            if (toCheck == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);
            else
                await RemoveIfExpired(toCheck);

            var res = await context.ContractFileSessions.FindAsync(id);
            if (res == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);

            return res;
        }

        public async Task InsertAsync(ContractFileSession item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var toCheck = await context.ContractFileSessions.FindAsync(item.Id);
            if (toCheck != null)
                await RemoveIfExpired(toCheck);
            
            var res = await context.ContractFileSessions.FindAsync(item.Id);
            if (res != null)
                throw new AlreadyExistsException(nameof(ContractFileSession) + " " + item.Id);

            await context.ContractFileSessions.AddAsync(item);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var toCheck = await context.ContractFileSessions.FindAsync(id);
            if (toCheck != null)
                await RemoveIfExpired(toCheck);

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

            var toCheck = await context.ContractFileSessions
                .AsNoTracking()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();
            if (toCheck == null)
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);

            if (toCheck.IsExpired())
            {
                await RemoveIfExpired(item);
                throw new NotFoundException(nameof(ContractFileSession) + " " + id);
            }

            if (toCheck.ExpirationDate != item.ExpirationDate)
                throw new BadRequestException("Expiration dates do not match");


            context.Entry(item).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }
    }
}
