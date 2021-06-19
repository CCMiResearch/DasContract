using DasContract.Editor.Web.Services.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.UndoRedo
{
    public abstract class ContractCommand
    {
        protected IContractManager ContractManager { get; set; }

        public ContractCommand(IContractManager contractManager)
        {
            ContractManager = contractManager;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}
