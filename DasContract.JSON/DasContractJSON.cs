using DasContract.Abstraction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasContract.JSON
{
    public static class DasContractJSON
    {
        public static string Serialize(Contract contract)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
        };
            return JsonConvert.SerializeObject(contract, jsonSettings);
        }

        public static Contract Deserialize(string jsonContract)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            };
            return JsonConvert.DeserializeObject<Contract>(jsonContract, jsonSettings);
        }
    }
}
