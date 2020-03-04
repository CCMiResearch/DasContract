using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DasContract.Editor.Components.Main.Components;
using Microsoft.AspNetCore.Components;

namespace DasContract.Editor.Components.Main.Components.CEditableItemsList
{
    public partial class EditableItemsList<TModel> : LoadableComponent
    {

        [Parameter]
        public IList<TModel> Items { get; set; } = new List<TModel>();

        //--------------------------------------------------
        //                   FRAGMENTS
        //--------------------------------------------------
        [Parameter]
        public RenderFragment<TModel> ItemHeading { get; set; }

        [Parameter]
        public RenderFragment<TModel> ItemEdit { get; set; }

        //--------------------------------------------------
        //                    ADDING
        //--------------------------------------------------
        [Parameter]
        public EventCallback OnAdd
        {
            get => onAdd.GetValueOrDefault();
            set => onAdd = value;
        }
        EventCallback? onAdd = null;

        protected async Task AddItemAsync()
        {
            await onAdd?.InvokeAsync(null);
        }

        public bool Extendable => onAdd != null;

        //--------------------------------------------------
        //                    EDITING
        //--------------------------------------------------

        [Parameter]
        public bool Editable { get; set; } = true;

        protected int CurrentlyOpenedEditor { get; set; } = -1;
        protected void ToggleEditor(int index)
        {
            if (CurrentlyOpenedEditor == index)
                CloseEditor();
            else
                CurrentlyOpenedEditor = index;
        }
        public void CloseEditor()
        {
            CurrentlyOpenedEditor = -1;
        }

        protected bool IsOpened(int index)
        {
            return index == CurrentlyOpenedEditor;
        }

        //--------------------------------------------------
        //                   DELETING
        //--------------------------------------------------
        [Parameter]
        public bool Deletable { get; set; } = false;

        [Parameter]
        public EventCallback<int> OnDelete
        {
            get => onDelete.GetValueOrDefault();
            set => onDelete = value;
        }
        EventCallback<int>? onDelete = null;


        protected async Task DeleteItemAsync(int index)
        {
            if (onDelete == null)
                Items.RemoveAt(index);
            else
                await onDelete.Value.InvokeAsync(index);
        }

        //--------------------------------------------------
        //                   ORDERING
        //--------------------------------------------------
        [Parameter]
        public bool Orderable { get; set; } = false;

        protected bool Ordering { get; set; } = false;

        protected void ToggleOrdering()
        {
            Ordering = !Ordering;

            if (!Ordering)
                CloseEditor();
        }


    }
}
