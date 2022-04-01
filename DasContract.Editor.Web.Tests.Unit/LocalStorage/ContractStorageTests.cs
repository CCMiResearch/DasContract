using DasContract.Editor.Web.Services.LocalStorage;
using DasContract.Web.Editor.Tests.Unit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DasContract.Web.Editor.Tests.Unit.LocalStorage
{
    public class ContractStorageTests
    {
        [Fact]
        public async Task StoreContract_ShouldStore()
        {
            const string CONTRACT1_ID = "Contract1-ID";
            const string CONTRACT2_ID = "Contract2-ID";
            var contractStorage = new ContractStorage(new LocalStorageServiceMock());

            await contractStorage.StoreContract(CONTRACT1_ID, "Contract1", "Contract1-Content");
            await contractStorage.StoreContract(CONTRACT2_ID, "Contract2", "Contract2-Content");

            var contractLinks = await contractStorage.GetAllContractLinks();
            var contract1 = await contractStorage.GetContractXml(CONTRACT1_ID);
            var contract2 = await contractStorage.GetContractXml(CONTRACT2_ID);

            Assert.Equal(2, contractLinks.Count);

            var contractLink1 = contractLinks.Single(c => c.ContractId == CONTRACT1_ID);
            var contractLink2 = contractLinks.Single(c => c.ContractId == CONTRACT2_ID);

            Assert.Equal("Contract1", contractLink1.ContractName);
            Assert.Equal("Contract2", contractLink2.ContractName);
            Assert.Equal("Contract1-Content", contract1);
            Assert.Equal("Contract2-Content", contract2);
        }

        [Fact]
        public async Task StoreAndDeleteContract_ShouldDelete()
        {
            const string CONTRACT1_ID = "Contract1-ID";
            var contractStorage = new ContractStorage(new LocalStorageServiceMock());

            await contractStorage.StoreContract(CONTRACT1_ID, "Contract1", "Contract1-Content");

            var contractLinks = await contractStorage.GetAllContractLinks();
            var contract1 = await contractStorage.GetContractXml(CONTRACT1_ID);

            Assert.Equal(1, contractLinks.Count);

            var contractLink1 = contractLinks.Single(c => c.ContractId == CONTRACT1_ID);
            Assert.Equal("Contract1-Content", contract1);

            await contractStorage.RemoveContract(CONTRACT1_ID);

            contractLinks = await contractStorage.GetAllContractLinks();
            contract1 = await contractStorage.GetContractXml(CONTRACT1_ID);

            Assert.Empty(contractLinks);
            Assert.Null(contract1);
        }
    }
}
