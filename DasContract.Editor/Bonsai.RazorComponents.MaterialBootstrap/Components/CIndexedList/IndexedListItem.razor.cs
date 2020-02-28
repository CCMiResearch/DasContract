using System;
using System.Collections.Generic;
using System.Text;
using Bonsai.RazorComponents.Interfaces;
using Bonsai.Utils.String;
using Microsoft.AspNetCore.Components;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CIndexedList
{
    public partial class IndexedListItem: RootComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Title { get; set; }

        [CascadingParameter(Name = "IndexedList")]
        [Parameter]
        public IndexedList IndexedList
        {
            get
            {
                return indexedList;
            }
            set
            {
                if (indexedList != value && value != null)
                    value.AddItem(this);
                indexedList = value;
            }
        }
        private IndexedList indexedList = null;

        [Parameter]
        public string Id
        {
            get
            {
                if (id == null)
                    return Title.ToIdFriendly();
                return id;
            }
            set
            {
                id = value;
            }
        }
        private string id = null;

        [Parameter]
        public HeadingLevel HeadingLevel { get; set; } = HeadingLevel.H2;

        [Parameter]
        public bool CreateHeader { get; set; } = true;
    }
}
