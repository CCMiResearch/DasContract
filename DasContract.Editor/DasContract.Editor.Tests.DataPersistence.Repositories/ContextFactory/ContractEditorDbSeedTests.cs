using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.DataPersistence.DbContexts;
using DasContract.Editor.DataPersistence.Entities;

namespace DasContract.Editor.Tests.DataPersistence.Repositories.ContextFactory
{
    public static class ContractEditorDbSeedTests
    {
        public static void SeedTests(this ContractEditorDb context)
        {
            //Add contracts
            context.ContractFileSessions.Add(new ContractFileSession()
            {
                Id = "expired",
                ExpirationDate = DateTime.MinValue
            });

            context.ContractFileSessions.Add(new ContractFileSession()
            {
                Id = "contract-1",
                SerializedContract = "serialized-contract-1"
            });

            context.ContractFileSessions.Add(new ContractFileSession()
            {
                Id = "contract-2",
                SerializedContract = "serialized-contract-2"
            });

            context.ContractFileSessions.Add(new ContractFileSession()
            {
                Id = "contract-3",
                SerializedContract = "serialized-contract-3"
            });

            context.SaveChanges();
        }
    }
}
