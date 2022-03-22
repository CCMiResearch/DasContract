using Blazored.LocalStorage;
using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DasContract.Editor.Web.Services.LocalStorage
{
    public class ContractStorage : IContractStorage
    {
        private const string CONTRACT_LINKS_KEY = "contract-links";
        private const string CONTRACT_PREFIX = "contract";

        private ILocalStorageService _localStorage;

        public ContractStorage(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<IList<StoredContractLink>> GetAllContractLinks()
        {
            var links = await _localStorage.GetItemAsync<IList<StoredContractLink>>(CONTRACT_LINKS_KEY);

            if (links == null)
                links = new List<StoredContractLink>();

            return links.OrderByDescending(l => l.LastChanged).ToList();
        }

        public async Task<string> GetContractXml(string contractId)
        {
            return await _localStorage.GetItemAsStringAsync($"{CONTRACT_PREFIX}-{contractId}");
        }

        public async Task RemoveContract(string contractId)
        {
            var links = await GetAllContractLinks();

            var link = links.SingleOrDefault(l => l.ContractId == contractId);
            if (link == null)
                return;

            links.Remove(link);
            await _localStorage.SetItemAsync(CONTRACT_LINKS_KEY, links);
            await _localStorage.RemoveItemAsync($"{CONTRACT_PREFIX}-{contractId}");
        }

        public async Task StoreContract(string contractId, string contractName, string serializedContract)
        {
            var links = await GetAllContractLinks();

            var contractLink = links.SingleOrDefault(l => l.ContractId == contractId);
            if(contractLink == null)
            {
                contractLink = new StoredContractLink()
                { 
                    ContractId = contractId
                };
                links.Add(contractLink);
            }
            contractLink.ContractName = contractName;
            contractLink.LastChanged = DateTime.Now;

            await _localStorage.SetItemAsync(CONTRACT_LINKS_KEY, links);
            await _localStorage.SetItemAsStringAsync($"{CONTRACT_PREFIX}-{contractId}", serializedContract);
        }
    }
}
