using DasContract.Abstraction.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services
{
    public interface IContractManager
    {
        public void InitializeNewContract();
        public Process GetProcess();
    }
}
