using System;
using DasContract.Editor.Tests.Server.ServerFactory;
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
        public void Test1()
        {
            var client = factory.CreateClient();

            var response = client.GetAsync("xxx");

            //...
            throw new NotImplementedException();
        }
    }
}
