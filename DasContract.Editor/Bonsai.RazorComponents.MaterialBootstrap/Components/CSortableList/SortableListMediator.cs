using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.JSInterop;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSortableList
{
    public class SortableListMediator
    {
        public event SortableListMediatorHandler OnSortChange;

        /// <summary>
        /// Serves as a reference method for javascript callback
        /// </summary>
        /// <param name="oldPosition">The old position index</param>
        /// <param name="newPosition">The new position index</param>
        [JSInvokable]
        public void MoveItemToPosition(int oldPosition, int newPosition)
        {
            OnSortChange?.Invoke(this, new SortableListMediatorChangeArgs(oldPosition, newPosition));
        }
    }

    public delegate void SortableListMediatorHandler(SortableListMediator caller, SortableListMediatorChangeArgs args);
}
