using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DasContract.Editor.DataPersistence.Entities;
using DasContract.Editor.Interfaces;
using Newtonsoft.Json;
using DasContract.Editor.Utils.String;

namespace DasContract.Editor.Pages.Main.Services.Entities
{
    public class ContractFileSessionService : ICRUDInterfaceAsync<ContractFileSession, string>
    {
        const string uri = "/api/ContractFileSession/";

        readonly HttpClient http;

        public ContractFileSessionService(HttpClient http)
        {
            this.http = http;
        }

        public async Task DeleteAsync(string id)
        {
            var response = await http.DeleteAsync(uri + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ContractFileSession>> GetAsync()
        {
            var response = await http.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ContractFileSession>>(content);
        }

        public async Task<ContractFileSession> GetAsync(string id)
        {
            var response = await http.GetAsync(uri + id);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ContractFileSession>(content);
        }

        public async Task InsertAsync(ContractFileSession item)
        {
            var response = await http.PostAsync(uri, item.ToJsonContent());
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(ContractFileSession item)
        {
            var response = await http.PutAsync(uri, item.ToJsonContent());
            response.EnsureSuccessStatusCode();
        }
    }
}
