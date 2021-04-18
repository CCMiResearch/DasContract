using DasContract.Abstraction.Processes;
using DasContract.Editor.Web.Services.EditElement;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components
{
    public partial class ProcessDetailBar : ComponentBase, IDisposable
    {
        [Inject]
        private EditElementService EditElementService { get; set; }

        private ProcessElement EditedElement { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditElementService.EditElementChanged += HandleEditedElementChanged;
        }

        public void Dispose()
        {
            EditElementService.EditElementChanged -= HandleEditedElementChanged;
        }

        private void HandleEditedElementChanged(object sender, EditElementEventArgs e)
        {
            EditedElement = e.processElement;
            StateHasChanged();
        }
    }
}
