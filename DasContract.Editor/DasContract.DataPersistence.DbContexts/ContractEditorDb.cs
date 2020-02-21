using System;
using DasContract.Editor.DataPersistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DasContract.Editor.DataPersistence.DbContexts
{
    public class ContractEditorDb : DbContext
    {
        public ContractEditorDb(DbContextOptions<ContractEditorDb> options) : base(options)
        {
        }

        public DbSet<ContractFileSession> ContractFileSessions { get; set; }
    }
}
