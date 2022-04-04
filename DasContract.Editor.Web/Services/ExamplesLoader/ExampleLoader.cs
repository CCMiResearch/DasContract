using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ExamplesLoader
{
    public class ExampleLoader : IExampleLoader
    {
        private const string EXAMPLES_PATH = "dist/examples/";
        private const string MANIFEST_FILE_NAME = "examples-manifest.json";

        private HttpClient _httpClient;

        public ExampleLoader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> ReadContract(string filename)
        {
            return await _httpClient.GetStringAsync(EXAMPLES_PATH + filename);
        }

        public async Task<IList<ExampleContract>> ReadManifest()
        {
            var examplesWrapper = await _httpClient.GetFromJsonAsync<ExamplesWrapper>(EXAMPLES_PATH + MANIFEST_FILE_NAME);
            return examplesWrapper.Examples;
        }
    }
}
