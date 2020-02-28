using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CIndexedList
{
    public partial class IndexedList: LoadableComponent
    {
        [Inject]
        protected IScroll SlowScroll { get; set; }

        [Parameter]
        public RenderFragment<IndexedList> ChildContent { get; set; }

        [Parameter]
        public bool CreateMap { get; set; } = true;

        protected List<IndexedListItem> Items { get; set; } = new List<IndexedListItem>();

        public void AddItem(IndexedListItem item)
        {
            Items.Add(item);
            StateHasChanged();
        }
    }
}
