using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.ContractManagement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class CallActivityGeneralTab: ComponentBase
    {
        [Parameter]
        public CallActivity CallActivity { get; set; }

        [Inject]
        protected IContractManager ContractManager { get; set; }

        protected IList<CallActivityOption> CreateSelectedProcessList()
        {
            var selectedList = new List<CallActivityOption>();
            if (CallActivity.CalledElement != null) {
                var selectedProcess = ContractManager.GetAllProcesses().Where(p => p.Id == CallActivity.CalledElement).FirstOrDefault();
                if(selectedProcess != null)
                    selectedList.Add(new CallActivityOption { CalledElementId = selectedProcess.Id, CalledElementName = (selectedProcess.Name ?? selectedProcess.Id)});
            }
            return selectedList;
        }
    }
}
