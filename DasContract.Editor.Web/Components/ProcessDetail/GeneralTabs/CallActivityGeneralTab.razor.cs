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
        protected IProcessModelManager ProcessModelManager { get; set; }

        protected IList<string> CreateSelectedProcessList()
        {
            var selectedList = new List<string>();
            if (CallActivity.CalledElement != null)
            {
                selectedList.Add(CallActivity.CalledElement);
            }
                
            return selectedList;
        }
    }
}
