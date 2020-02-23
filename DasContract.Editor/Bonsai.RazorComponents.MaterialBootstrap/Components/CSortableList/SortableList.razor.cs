using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Bonsai.RazorComponents.Interfaces;
using Bonsai.Utils.String;
using System.Globalization;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSortableList
{
    public partial class SortableList<TModel>: LoadableComponent, IIdentifiableComponent
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString().ToIdFriendly();

        /// <summary>
        /// Tells if there should be rendered any control arrow
        /// </summary>
        [Parameter]
        public bool Arrows { get; set; } = true;

        /// <summary>
        /// Tells if the reoder can be done using drag and drop feature
        /// </summary>
        [Parameter]
        public bool DragAndDrop { get; set; } = true;

        /// <summary>
        /// Tells if reorder can be done using manual number input
        /// </summary>
        [Parameter]
        public bool TextInput { get; set; } = false;

        /// <summary>
        /// Template fragment for one item content
        /// </summary>
        [Parameter]
        public RenderFragment<TModel> Item { get; set; }

        /// <summary>
        /// List of models to be sorted
        /// </summary>
        [Parameter]
        public IList<TModel> Items { get; set; } = new List<TModel>();

        public SortableListMediator Positioner { get; set; } = new SortableListMediator();

        [Parameter]
        public EventCallback<SortableListChangeArgs<TModel>> OnReorder { get; set; }

        /// <summary>
        /// Ensures moving one item from a position to a new position
        /// </summary>
        /// <param name="oldPosition">The old position index</param>
        /// <param name="newPosition">The new position index</param>
        public async Task MoveItemToPositionAsync(int oldPosition, int newPosition)
        {
            if (oldPosition < 0 || oldPosition >= Items.Count
                || newPosition < 0 || newPosition >= Items.Count
                || oldPosition == newPosition)
            {
                StateHasChanged();
                return;
            }

            var movedItem = Items[oldPosition];
            
            var itemsTemp = Items;
            //Items = new List<TModel>();
            StateHasChanged();

            Items = itemsTemp;
            Items.RemoveAt(oldPosition);
            Items.Insert(newPosition, movedItem);
            StateHasChanged();

            TModel movedItemLowerNeighbour = default;
            if (newPosition != 0)
                movedItemLowerNeighbour = Items[newPosition - 1];

            TModel movedItemUpperNeighbour = default;
            if (newPosition != Items.Count - 1)
                movedItemUpperNeighbour = Items[newPosition + 1];

            await OnReorder.InvokeAsync(new SortableListChangeArgs<TModel>(oldPosition, newPosition, movedItem, Items) {
                MovedItemLowerNeighbour = movedItemLowerNeighbour,
                MovedItemUpperNeighbour = movedItemUpperNeighbour
            });
        }

        /// <summary>
        /// Ensures moving an item from its position to a new position
        /// </summary>
        /// <param name="model">The item to move</param>
        /// <param name="newPosition">The new position index</param>
        public async Task MoveItemToPositionAsync(TModel model, int newPosition)
        {
            await MoveItemToPositionAsync(Items.IndexOf(model), newPosition);
        }

        /// <summary>
        /// Ensures moving an item from its position to a new position
        /// </summary>
        /// <param name="model">The item to move</param>
        /// <param name="newPosition">The new position index</param>
        public async Task MoveItemToPositionAsync(TModel model, object newPosition)
        {
            if (newPosition == null)
                throw new ArgumentNullException(nameof(newPosition));

            await MoveItemToPositionAsync(Items.IndexOf(model), int.Parse(newPosition.ToString(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Moves an item up in the order
        /// </summary>
        /// <param name="model">The item to move</param>
        public async Task MoveUpAsync(TModel model)
        {
            int modelIndex = Items.IndexOf(model);
            await MoveItemToPositionAsync(modelIndex, modelIndex - 1);
        }

        /// <summary>
        /// Moves an item down in the order
        /// </summary>
        /// <param name="model">The item to move</param>
        public async Task MoveDownAsync(TModel model)
        {
            int modelIndex = Items.IndexOf(model);
            await MoveItemToPositionAsync(modelIndex, modelIndex + 1);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            //Do only on the first render
            if (!firstRender)
                return;

            if (DragAndDrop)
            {
                //Subscribe to positioner
                Positioner.OnSortChange += async (caller, args) =>
                {
                    await MoveItemToPositionAsync(args.OldPosition, args.NewPosition);
                    StateHasChanged();
                };

                var positionerReference = DotNetObjectReference.Create(Positioner);

                string selector = "#" + Id;
                await JSRuntime.InvokeVoidAsync("MaterialBootstrapRazorComponents.SortableList.Create", selector, positionerReference);
            }
        }

    }
}
