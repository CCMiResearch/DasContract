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
            var contractStorage = new ContractStorage(new LocalStorageServiceMock());

            var contract = await ContractReader.ReadContractAsString("example_contract_1.dascontract");
            await contractStorage.StoreContract("aa", "aaa", contract);
        }
    }
}
