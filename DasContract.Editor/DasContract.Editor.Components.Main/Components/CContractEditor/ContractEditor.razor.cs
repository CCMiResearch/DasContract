using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components;
using DasContract.Editor.Entities;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CContractEditor
{
    public partial class ContractEditor : LoadableComponent
    {
        [Parameter]
        public EditorContract Contract { get; set; }

        public ContractEditorTab OpenedTab { get; protected set; } = ContractEditorTab.General;

        [Parameter]
        public EventCallback<EditorContract> OnSave { get; set; }

        [Parameter]
        public EventCallback<EditorContract> OnDownload { get; set; }

        public async Task SaveAndDownloadAsync()
        {
            await SaveAsync();
            await DownloadAsync();
        }

        public async Task SaveAsync()
        {
            await OnSave.InvokeAsync(Contract);
        }

        public async Task DownloadAsync()
        {
            await OnDownload.InvokeAsync(Contract);
        }

        public void Open(ContractEditorTab tab)
        {
            if (OpenedTab == tab)
                return;

            OpenedTab = tab;
        }
    }
}
