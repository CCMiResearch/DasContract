using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.EditElement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Abstraction.Processes.Tasks;

namespace DasContract.Editor.Web.Components
{
    public partial class ProcessDetailBar : ComponentBase, IDisposable
    {
        [Inject]
        private EditElementService EditElementService { get; set; }

        private IProcessElement EditedElement { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditElementService.EditElementChanged += HandleEditedElementChanged;
            EditElementService.EditElementModified += HandleEditedElementModified;
            EditedElement = EditElementService.EditElement;
        }

        public void Dispose()
        {
            EditElementService.EditElementChanged -= HandleEditedElementChanged;
            EditElementService.EditElementModified -= HandleEditedElementModified;
        }

        private void HandleEditedElementChanged(object sender, EditElementEventArgs e)
        {
            EditedElement = e.processElement;
            StateHasChanged();
        }

        private void HandleEditedElementModified(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
