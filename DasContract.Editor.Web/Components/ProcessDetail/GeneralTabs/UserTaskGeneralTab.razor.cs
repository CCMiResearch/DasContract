using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Processes.Tasks;
using DasContract.Editor.Web.Services.ContractManagement;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;


namespace DasContract.Editor.Web.Components.ProcessDetail.GeneralTabs
{
    public partial class UserTaskGeneralTab: ComponentBase
    {
        [Parameter]
        public UserTask UserTask { get; set; }

        [Inject]
        protected IUserModelManager UserModelManager { get; set; }

        protected List<ProcessUser> CreateAssigneeSelectedList()
        {
            var selected = new List<ProcessUser>();
            if (UserTask.Assignee != null)
                selected.Add(UserTask.Assignee);
            return selected;
        }
    }
}
