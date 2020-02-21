using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using DasContract.Editor.DataPersistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DasContract.Editor.Tests.DataPersistence.Repositories.ContextFactory
{
    class ContractEditorDbTestBuilder: IDisposable
    {
        public ContractEditorDb Build()
        {
            //SQLite
            if (!testingDatabaseAlreadyCreated)
                SQLiteConnection.CreateFile(testingDatabaseName);
            var connection = new SQLiteConnection("DataSource=" + testingDatabaseName);
            connection.Open();
            SQLiteDbConnections.Add(connection);

            var options = new DbContextOptionsBuilder<ContractEditorDb>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            //Build context
            var context = new ContractEditorDb(options);
            context.Database.CloseConnection();
            context.Database.EnsureCreated();
            context.Database.GetDbConnection();

            //Seed
            if (!testingDatabaseAlreadyCreated)
                context.SeedTests();

            contexts.Add(context);
            testingDatabaseAlreadyCreated = true;
            return context;
        }

        private readonly List<ContractEditorDb> contexts = new List<ContractEditorDb>();
        private List<SQLiteConnection> SQLiteDbConnections { get; set; } = new List<SQLiteConnection>();
        private readonly string testingDatabaseName = nameof(ContractEditorDb) + Guid.NewGuid().ToString();
        private bool testingDatabaseAlreadyCreated = false;

        public void Clear()
        {
            foreach (var context in contexts)
                context.Dispose();
            foreach (var connection in SQLiteDbConnections)
            {
                connection.Close();
                connection.Dispose();
            }
            File.Delete(testingDatabaseName);
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
