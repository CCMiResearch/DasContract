using System;
using System.Collections.Generic;
using System.Text;

namespace Bonsai.RazorComponents.MaterialBootstrap.Components.CSortableList
{
    public class SortableListMediatorChangeArgs
    {
        public readonly int OldPosition;

        public readonly int NewPosition;

        public SortableListMediatorChangeArgs(int oldPosition, int newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
