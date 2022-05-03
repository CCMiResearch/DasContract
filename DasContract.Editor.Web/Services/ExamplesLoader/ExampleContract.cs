using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ExamplesLoader
{
    public class ExampleContract
    {
        public string Name { get; set; }
        [JsonPropertyName("contract_filename")]
        public string ContractFilename { get; set; }
    }
}
