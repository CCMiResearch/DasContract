using DasContract.Abstraction;
using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.Processes
{
    public class ContractManager : IContractManager
    {
        private Contract contract;

        public void InitializeNewContract()
        {
            contract = new Contract();
            contract.Processes.Add(new Process());
        }

        public Process GetProcess()
        {
            return contract.Process;
        }
    }
}
