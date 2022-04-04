using DasContract.Abstraction.Data;
using DasContract.Editor.Web.Services.ContractManagement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class TaskGeneralTab: ComponentBase
    {
        [Parameter]
        public Abstraction.Processes.Tasks.Task Task { get; set; }

        [Inject]
        public IContractManager ContractManager { get; set; }

        protected IList<Property> GetSelectedCollectionList()
        {
            var selected = new List<Property>();
            if(Task.LoopCollection != null)
            {
                var selectedCollection = ContractManager.GetPropertyById(Task.LoopCollection);
                if (selectedCollection != null)
                    selected.Add(selectedCollection);
            }
            return selected;
        }
    }
}
