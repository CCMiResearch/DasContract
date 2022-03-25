using DasContract.Editor.Web.Services.UndoRedo;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Components.Select2
{
    public partial class Select2<TItem>: ComponentBase 
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public IEnumerable<TItem> Options { get; set; }

        [Parameter]
        public IList<TItem> Selected { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter] 
        public EventCallback<IList<TItem>> SelectedChanged { get; set; }

        [Parameter]
        public Func<TItem, string> ValueSelector { get; set; }

        [Parameter]
        public Func<TItem, string> ContentSelector { get; set; }

        [Parameter]
        public EventCallback<string> ItemSelected { get; set; }

        [Parameter]
        public EventCallback<string> ItemUnselected { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected IList<TItem> CurrentSelected
        {
            get => Selected;
            set
            {
                if (value.Count == Selected.Count && !value.Except(Selected).Any())
                    return;
                Selected = value;
                SelectedChanged.InvokeAsync(value);
            }
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                var dotnetRef = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("select2Lib.initializeSelect2", $"select2-{Id}",
                    Selected.Select(s => ValueSelector(s)),
                    Options.Where(o => !string.IsNullOrWhiteSpace(ContentSelector(o)))
                        .Select(o => new Select2Option { Text = ContentSelector(o), Value = ValueSelector(o)}).ToList(),
                    Multiple,
                    dotnetRef);
            }
            await RefreshJsComponent();
        }

        private async Task RefreshJsComponent()
        {
            await JSRuntime.InvokeVoidAsync("select2Lib.refreshOptions", $"select2-{Id}",
                Options.Where(o => !string.IsNullOrWhiteSpace(ContentSelector(o)))
                        .Select(o => new Select2Option { Text = ContentSelector(o), Value = ValueSelector(o) }).ToList(),
                Selected.Select(s => ValueSelector(s)));
        }

        public async void SelectItem(TItem item)
        {
            if(!Selected.Contains(item))
                Selected.Add(item);
            //await JSRuntime.InvokeVoidAsync("select2Lib.refreshSelectedItems", $"select2-{Id}", Selected.Select(s => ValueSelector(s)));
            await RefreshJsComponent();
        }

        public async void UnselectItem(TItem item)
        {
            Selected.Remove(item);
            //await JSRuntime.InvokeVoidAsync("select2Lib.refreshSelectedItems", $"select2-{Id}", Selected.Select(s => ValueSelector(s)));
            await RefreshJsComponent();
        }

        [JSInvokable]
        public void OnSelect(string id)
        {
            Console.WriteLine($"Selected {id}");
            ItemSelected.InvokeAsync(id);
        }

        [JSInvokable]
        public void OnUnselect(string id)
        {
            Console.WriteLine($"Unselected {id}");
            ItemUnselected.InvokeAsync(id);
        }

        [JSInvokable]
        public void OnChange()
        {
            Console.WriteLine("Change happened");
            UpdateSelected();
        }

        private async void UpdateSelected()
        {
            var selectedIds = await GetSelectedIds();
            CurrentSelected = Options.Where(o => selectedIds.Contains(ValueSelector(o))).ToList();
        }

        protected async Task<List<string>> GetSelectedIds()
        {
            return await JSRuntime.InvokeAsync<List<string>>("select2Lib.getSelectedIds", $"select2-{Id}");
        }
    }
}
