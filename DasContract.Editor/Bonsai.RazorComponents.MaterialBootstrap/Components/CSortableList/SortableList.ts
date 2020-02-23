let Sortable: ISortable = (require("sortablejs") as any).Sortable;

(window as any).MaterialBootstrapRazorComponents.SortableList = {

    Create: function (selector: string, positionerReference: any)
    {
        let listWrapper = document.querySelector(selector) as HTMLElement;

        //let originalChildrenList: Element[] = [];

        let sortable = Sortable.create(listWrapper, {
            animation: 150,
            //ghostClass: 'blue-background-class',
            onStart: function ()
            {
               
            },
            onEnd: function (args: ISortableOnEndEventArgs)
            {

            },
            onUpdate: function (args: ISortableOnEndEventArgs)
            {
                let oldIndex = args.oldIndex;
                let newIndex = args.newIndex;

                //Move the items back
                if (args.oldIndex < args.newIndex)
                    listWrapper.insertBefore(listWrapper.children[newIndex], listWrapper.children[oldIndex]);
                else
                    listWrapper.insertBefore(listWrapper.children[newIndex], listWrapper.children[oldIndex + 1]);

                //Invoke Blazor method
                let res = (positionerReference as any).invokeMethodAsync("MoveItemToPosition", args.oldIndex - 1, args.newIndex - 1);

                //console.log("Old index: " + oldIndex);
                //console.log("New index: " + newIndex);
            }

        });
    }
}


//SORTABLE INTERFACE
interface ISortable
{
    create(target: HTMLElement, settings: ISortableSettings): ISortable;
}

interface ISortableSettings
{
    animation: number,
    onEnd: ISortableEventCallback<ISortableOnEndEventArgs>
    onStart: Function
    onUpdate: ISortableEventCallback<ISortableOnEndEventArgs>
}

interface ISortableEventCallback<TArgs>
{
    (args: TArgs): void
}

interface ISortableOnEndEventArgs
{
    oldIndex: number; 
    newIndex: number;
}


/*
 // Element dragging ended
	onEnd: function (evt: Event) {
    var itemEl = evt.item;  // dragged HTMLElement
    evt.to;    // target list
    evt.from;  // previous list
    evt.oldIndex;  // element's old index within old parent
    evt.newIndex;  // element's new index within new parent
    evt.oldDraggableIndex; // element's old index within old parent, only counting draggable elements
    evt.newDraggableIndex; // element's new index within new parent, only counting draggable elements
    evt.clone // the clone element
    evt.pullMode;  // when item is in another sortable: `"clone"` if cloning, `true` if moving
},

 */