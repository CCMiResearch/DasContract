using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Repositories;
using DasContract.Editor.Tests.DataPersistence.Repositories.ContextFactory;
using NUnit.Framework;
using System.Linq;
using DasContract.Editor.DataPersistence.Repositories.Interfaces.Exceptions;
using DasContract.Editor.DataPersistence.Entities;

namespace DasContract.Editor.Tests.DataPersistence.Repositories
{
    public class ContractFileSessionRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        ContractFileSessionRepository Facade(ContractEditorDbTestBuilder contextBuilder) 
            => new ContractFileSessionRepository(contextBuilder.Build());

        [Test]
        public async Task GetAll()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var entities = await Facade(contextBuilder).GetAsync();

            Assert.NotNull(entities);
            Assert.IsTrue(entities.Count() >= 1);
        }

        [Test]
        public async Task GetAll_CheckExpired()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var entities = await Facade(contextBuilder).GetAsync();

            Assert.NotNull(entities);
            Assert.IsTrue(entities.Count() >= 1);
            Assert.IsTrue(entities.Where(e => e.IsExpired()).Count() == 0);
        }

        
        [Test]
        public async Task GetOne()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var entity = await Facade(contextBuilder).GetAsync("contract-1");

            Assert.NotNull(entity);
            Assert.AreEqual("contract-1", entity.Id);
            Assert.AreEqual("serialized-contract-1", entity.SerializedContract);
        }

        [Test]
        public async Task GetNotFound()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {
                var entity = await Facade(contextBuilder).GetAsync("sajdhaslbsdlghasbef");
                Assert.Fail();
            }
            catch(NotFoundException)
            {

            }

        }

        [Test]
        public async Task GetOne_CheckExpired()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {
                var entity = await Facade(contextBuilder).GetAsync("expired");
                Assert.Fail();
            }
            catch (NotFoundException)
            {

            }

        }

        [Test]
        public async Task Insert()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            await Facade(contextBuilder).InsertAsync(new ContractFileSession()
            {
                Id = "some-id",
                SerializedContract = "xyz"
            });

            var entity = await Facade(contextBuilder).GetAsync("some-id");

            Assert.NotNull(entity);
            Assert.AreEqual("some-id", entity.Id);
            Assert.AreEqual("xyz", entity.SerializedContract);
        }

        [Test]
        public async Task Insert_CheckExpired()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            await Facade(contextBuilder).InsertAsync(new ContractFileSession()
            {
                Id = "expired",
                SerializedContract = "xyz"
            });

            var entity = await Facade(contextBuilder).GetAsync("expired");

            Assert.NotNull(entity);
            Assert.AreEqual("expired", entity.Id);
            Assert.AreEqual("xyz", entity.SerializedContract);
        }

        [Test]
        public async Task InsertButExists()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {

                await Facade(contextBuilder).InsertAsync(new ContractFileSession()
                {
                    Id = "contract-1",
                    SerializedContract = "xyz"
                });
                Assert.Fail();
            }
            catch (AlreadyExistsException) { }

        }

        [Test]
        public async Task Delete()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            await Facade(contextBuilder).DeleteAsync("contract-1");

            try
            {
                await Facade(contextBuilder).GetAsync("contract-1");
            }
            catch(NotFoundException) { }
        }

        [Test]
        public async Task DeleteNotFound()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {
                await Facade(contextBuilder).DeleteAsync("asasdasdasdasd");
            }
            catch (NotFoundException) { }
        }

        [Test]
        public async Task Delete_CheckExpired()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {
                await Facade(contextBuilder).DeleteAsync("expired");
            }
            catch (NotFoundException) { }
        }

        [Test]
        public async Task Update()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();
            
            var contract = await Facade(contextBuilder).GetAsync("contract-1");

            contract.SerializedContract = "new-content";
            await Facade(contextBuilder).UpdateAsync(contract);

            contract = await Facade(contextBuilder).GetAsync("contract-1");

            Assert.AreEqual("contract-1", contract.Id);
            Assert.AreEqual("new-content", contract.SerializedContract);
        }


        [Test]
        public async Task UpdateNotFound()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var contract = await Facade(contextBuilder).GetAsync("contract-1");
            contract.SerializedContract = "new-content";
            await Facade(contextBuilder).DeleteAsync("contract-1");

            try
            {
                await Facade(contextBuilder).UpdateAsync(contract);
                Assert.Fail();
            } catch(NotFoundException) { }
        }

        [Test]
        public async Task UpdateDefaultId()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var contract = await Facade(contextBuilder).GetAsync("contract-1");
            contract.SerializedContract = "new-content";
            contract.Id = null;

            try
            {
                await Facade(contextBuilder).UpdateAsync(contract);
                Assert.Fail();
            }
            catch (BadRequestException) { }
        }

        [Test]
        public async Task UpdateDifferentExpireDates()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            var contract = await Facade(contextBuilder).GetAsync("contract-1");
            contract.SerializedContract = "new-content";
            contract.ExpirationDate = contract.ExpirationDate.AddSeconds(1);

            try
            {
                await Facade(contextBuilder).UpdateAsync(contract);
                Assert.Fail();
            }
            catch (BadRequestException) { }
        }

        [Test]
        public async Task Update_CheckExpired()
        {
            using var contextBuilder = new ContractEditorDbTestBuilder();

            try
            {
                var contract = new ContractFileSession() { Id = "expired" };
                await Facade(contextBuilder).UpdateAsync(contract);
                Assert.Fail();
            }
            catch (NotFoundException) { }
        }

        /*

        [Test]
        public async Task Update()
        {
            using var provider = new TestControllerProvider();

            var entityToUpdate = await provider.OfficeController.Get(1);
            entityToUpdate.Name = "UpdatedEntity";
            await provider.OfficeController.Update(entityToUpdate);

            var updatedEntity = await provider.OfficeController.Get(1);
            Assert.AreEqual("UpdatedEntity", updatedEntity.Name);
        }

        [Test]
        public async Task UpdateNotFound()
        {
            using var provider = new TestControllerProvider();

            var entityToUpdate = await provider.OfficeController.Get(1);
            entityToUpdate.Name = "UpdatedEntity";
            entityToUpdate.Id = NotExistingEntityId;

            try
            {
                await provider.OfficeController.Update(entityToUpdate);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }

            var originalEntity = await provider.OfficeController.Get(1);
            Assert.AreEqual("Test1", originalEntity.Name);
            Assert.AreEqual(1, originalEntity.Id);

        }*/
    }
}
