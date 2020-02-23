using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSortableList
{
    public class SortableListChangeArgs<TModel>
    {
        public int OldPosition { get; protected set; }

        public int NewPosition { get; protected set; }

        public TModel MovedItem { get; protected set; }

        public IList<TModel> UpdatedList { get; }

        public TModel MovedItemLowerNeighbour { get; set; }

        public TModel MovedItemUpperNeighbour { get; set; }

        public SortableListChangeArgs(
            int oldPosition,
            int newPosition,
            TModel movedItem,
            IList<TModel> updatedList)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
            MovedItem = movedItem;
            UpdatedList = updatedList;
        }

        public bool TryGetItemUpperNeighbour(out TModel upperNeighbout)
        {
            upperNeighbout = MovedItemUpperNeighbour;
            return MovedItemUpperNeighbour != null;
        }

        public bool TryGetItemLowerNeighbour(out TModel lowerNeighbout)
        {
            lowerNeighbout = MovedItemLowerNeighbour;
            return MovedItemLowerNeighbour != null;
        }
    }
}
