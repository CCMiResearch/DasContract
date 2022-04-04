using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.LocalStorage
{
    public interface IContractStorage
    {
        Task<IList<StoredContractLink>> GetAllContractLinks();
        Task StoreContract(string contractId, string contractName, string serializedContract);
        Task RemoveContract(string contractId);
        Task<string> GetContractXml(string contractId);
    }
}
