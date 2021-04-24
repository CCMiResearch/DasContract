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

        private IList<string> _tabs { get; set; }
        private string _activeTab { get; set; }

        private IProcessElement EditedElement { get; set; }

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
            _tabs = new List<string>();
            _tabs.Add("General");
            switch(EditedElement)
            {
                case ScriptTask:
                    _tabs.Add("Script");
                    break;
                case UserTask:
                    _tabs.Add("Validation");
                    break;

            }
            _activeTab = "General";
        }

        protected void SwitchActive(string newActive)
        {
            _activeTab = newActive;
            Console.WriteLine($"New active is {newActive}");
        }
    }
}
