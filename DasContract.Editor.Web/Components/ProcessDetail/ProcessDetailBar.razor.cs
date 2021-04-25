using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.EditElement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Editor.Web.Components.ProcessDetail
{
    public partial class ProcessDetailBar : ComponentBase, IDisposable
    {
        [Inject]
        private EditElementService EditElementService { get; set; }

        private IList<ProcessDetailTab> _tabs;
        private ProcessDetailTab _activeTab;

        private IProcessElement EditedElement;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditElementService.EditElementChanged += HandleEditedElementChanged;
            EditElementService.EditElementModified += HandleEditedElementModified;
            EditedElement = EditElementService.EditElement;
            CreateTabsList();
        }

        public void Dispose()
        {
            EditElementService.EditElementChanged -= HandleEditedElementChanged;
            EditElementService.EditElementModified -= HandleEditedElementModified;
        }

        private void HandleEditedElementChanged(object sender, EditElementEventArgs e)
        {
            EditedElement = e.processElement;
            CreateTabsList();
            StateHasChanged();
        }

        private void HandleEditedElementModified(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        private void CreateTabsList()
        {
            _tabs = new List<ProcessDetailTab>();
            _tabs.Add(new ProcessDetailTab {TabName = "General" , TabType = ProcessDetailTabType.General});
            switch(EditedElement)
            {
                case ScriptTask:
                    _tabs.Add(new ProcessDetailTab { TabName = "Script", TabType = ProcessDetailTabType.Script });
                    break;
                case UserTask:
                    _tabs.Add(new ProcessDetailTab { TabName = "Validation", TabType = ProcessDetailTabType.UserValidation });
                    break;

            }
            SwitchActive(_tabs.First());
        }

        protected void SwitchActive(ProcessDetailTab newActive)
        {
            _activeTab = newActive;
        }
    }
}
