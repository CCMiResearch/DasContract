using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.Web.Editor.Tests.Unit.Utils
{
    public static class ContractReader
    {
        public async static Task<string> ReadContractAsString(string contractName)
        {
            return await File.ReadAllTextAsync($"resources/{contractName}");
        }
    }
}
