using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Tests.Server.ServerFactory;
using DasContract.Editor.Utils.String;
using Newtonsoft.Json;
using Xunit;

namespace DasContract.Editor.Tests.Server
{
    public class ContractFileSessionControllerTests: IClassFixture<ServerWebApplicationFactory>
    {
        readonly ServerWebApplicationFactory factory;

        public ContractFileSessionControllerTests(ServerWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetAll()
        {
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/ContractFileSession");

            Assert.True(response.IsSuccessStatusCode);

            var responseText = await response.Content.ReadAsStringAsync();
            var entities = JsonConvert.DeserializeObject<List<ContractFileSession>>(responseText);

            Assert.True(entities.Count > 0);
            Assert.NotNull(entities.Where(e => e.Id == "contract-1").SingleOrDefault());
            Assert.NotNull(entities.Where(e => e.Id == "contract-2").SingleOrDefault());
            Assert.NotNull(entities.Where(e => e.Id == "contract-3").SingleOrDefault());
        }

        [Theory]
        [InlineData("contract-1")]
        [InlineData("contract-2")]
        [InlineData("contract-3")]
        public async Task GetOne(string id)
        {
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/ContractFileSession/" + id);

            Assert.True(response.IsSuccessStatusCode);

            var responseText = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<ContractFileSession>(responseText);

            Assert.Equal(id, entity.Id);
        }

        [Theory]
        [InlineData("expired")]
        [InlineData("asdifbsdgf")]
        public async Task GetOneNotFound(string id)
        {
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/ContractFileSession/" + id);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Post()
        {
            using var client = factory.CreateClient();

            var response = await client.PostAsync("/api/ContractFileSession", 
                JsonConvert.SerializeObject(new ContractFileSession()
            {
                Id = "new-contract-1"
            }).AsJson());

            Assert.True(response.IsSuccessStatusCode);

            response = await client.GetAsync("/api/ContractFileSession/new-contract-1");

            Assert.True(response.IsSuccessStatusCode);

            var responseText = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<ContractFileSession>(responseText);

            Assert.Equal("new-contract-1", entity.Id);
        }

        [Fact]
        public async Task Put()
        {
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/ContractFileSession/contract-1");
            Assert.True(response.IsSuccessStatusCode);
            var responseText = await response.Content.ReadAsStringAsync();
            var entity = JsonConvert.DeserializeObject<ContractFileSession>(responseText);
            Assert.Equal("contract-1", entity.Id);

            entity.SerializedContract = "xxxyyyzzz";
            response = await client.PutAsync("/api/ContractFileSession", JsonConvert.SerializeObject(entity).AsJson());
            Assert.True(response.IsSuccessStatusCode);

            response = await client.GetAsync("/api/ContractFileSession/contract-1");
            Assert.True(response.IsSuccessStatusCode);
            responseText = await response.Content.ReadAsStringAsync();
            entity = JsonConvert.DeserializeObject<ContractFileSession>(responseText);
            Assert.Equal("contract-1", entity.Id);
            Assert.Equal("xxxyyyzzz", entity.SerializedContract);
        }

        [Fact]
        public async Task Delete()
        {
            using var client = factory.CreateClient();

            var response = await client.DeleteAsync("/api/ContractFileSession/to-delete");
            Assert.True(response.IsSuccessStatusCode);

            response = await client.GetAsync("/api/ContractFileSession/to-delete");
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
