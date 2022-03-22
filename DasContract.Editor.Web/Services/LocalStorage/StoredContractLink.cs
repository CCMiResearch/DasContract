using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.LocalStorage
{
    public class StoredContractLink
    {
        public DateTime LastChanged { get; set; }
        public string ContractName { get; set; }
        public string ContractId { get; set; }
    }
}
