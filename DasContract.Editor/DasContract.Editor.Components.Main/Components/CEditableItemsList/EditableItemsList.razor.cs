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
        //                    EDITING
        //--------------------------------------------------
        protected int CurrentlyOpenedEditor { get; set; } = -1;
        protected void ToggleEditor(int index)
        {
            if (CurrentlyOpenedEditor == index)
                CloseEditor();
            else
                CurrentlyOpenedEditor = index;
        }
        protected void CloseEditor()
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
        protected void DeleteItem(int index)
        {
            Items.RemoveAt(index);
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
