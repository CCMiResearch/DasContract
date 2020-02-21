using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Repositories;
using DasContract.Editor.Tests.DataPersistence.Repositories.ContextFactory;
using NUnit.Framework;
using System.Linq;

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

        /*
        [Test]
        public async Task GetOne()
        {
            using var provider = new TestControllerProvider();
            var entity = await provider.OfficeController.Get(1);

            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual("Test1", entity.Name);
        }

        [Test]
        public async Task GetNotFound()
        {
            using var provider = new TestControllerProvider();

            try
            {
                var entity = await provider.OfficeController.Get(NotExistingEntityId);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }
        }

        [Test]
        public async Task GetEmptyFilter()
        {
            using var provider = new TestControllerProvider();
            var filterEntities = await provider.OfficeController.Get(new OfficeFilter() { });
            var allEntities = await provider.OfficeController.Get();

            Assert.AreEqual(allEntities.Count(), filterEntities.Entities.Count());
        }

        [Test]
        public async Task GetFilter()
        {
            using var provider = new TestControllerProvider();
            var entities = (await provider.OfficeController.Get(new OfficeFilter()
            {
                Filters = new List<Expression<Func<Office, bool>>>()
                {
                    e => e.Name.StartsWith("Test")
                },
                SortProperty = e => e.Name,
                SortOrder = SortOrder.Descending,
                From = 1,
                Count = 2
            })).Entities;

            Assert.AreEqual(2, entities.Count());

            Assert.AreEqual("Test2", entities.ElementAt(0).Name);
            Assert.AreEqual(2, entities.ElementAt(0).Id);
            Assert.AreEqual("Test1", entities.ElementAt(1).Name);
            Assert.AreEqual(1, entities.ElementAt(1).Id);
        }

        [Test]
        public async Task Insert()
        {
            using var provider = new TestControllerProvider();
            var newEntity = new Office()
            {
                Name = "NewEntity"
            };

            await provider.OfficeController.Insert(newEntity);

            var entity = await provider.OfficeController.Get(newEntity.Id);

            Assert.AreEqual("NewEntity", entity.Name);
            Assert.AreEqual(entity.Id, newEntity.Id);
        }

        [Test]
        public async Task InsertWithNonDefaultId()
        {
            using var provider = new TestControllerProvider();
            var newEntity = new Office()
            {
                Id = 1,
                Name = "NewEntity"
            };

            try
            {
                await provider.OfficeController.Insert(newEntity);
                Assert.Fail();
            }
            catch (BadRequest)
            {

            }
        }

        [Test]
        public async Task Delete()
        {
            using var provider = new TestControllerProvider();

            var entityToDelete = new Office()
            {
                Id = 2,
                Name = "Test2"
            };

            //Temp
            try
            {
                await provider.OfficeController.Delete(1);
                await provider.OfficeController.Delete(entityToDelete);
            }
            catch (NotImplementedException) { Assert.Pass(); }


            try
            {
                var foo = await provider.OfficeController.Get(1);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }

            try
            {
                await provider.OfficeController.Get(entityToDelete.Id);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }
        }

        [Test]
        public async Task DeleteNotFound()
        {
            using var provider = new TestControllerProvider();

            var entityToDelete = new Office()
            {
                Id = NotExistingEntityId,
                Name = "Test2"
            };

            //Temp
            try
            {
                await provider.OfficeController.Delete(NotExistingEntityId);
            }
            catch (NotImplementedException) { Assert.Pass(); }

            try
            {
                await provider.OfficeController.Delete(NotExistingEntityId);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }

            try
            {
                await provider.OfficeController.Delete(entityToDelete);
                Assert.Fail();
            }
            catch (NotFound)
            {

            }
        }

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
