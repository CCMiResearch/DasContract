var Sortable = require("sortablejs").Sortable;
window.MaterialBootstrapRazorComponents.SortableList = {
    Create: function (selector, positionerReference) {
        var listWrapper = document.querySelector(selector);
        //let originalChildrenList: Element[] = [];
        var sortable = Sortable.create(listWrapper, {
            animation: 150,
            //ghostClass: 'blue-background-class',
            onStart: function () {
            },
            onEnd: function (args) {
            },
            onUpdate: function (args) {
                var oldIndex = args.oldIndex;
                var newIndex = args.newIndex;
                //Move the items back
                if (args.oldIndex < args.newIndex)
                    listWrapper.insertBefore(listWrapper.children[newIndex], listWrapper.children[oldIndex]);
                else
                    listWrapper.insertBefore(listWrapper.children[newIndex], listWrapper.children[oldIndex + 1]);
                //Invoke Blazor method
                var res = positionerReference.invokeMethodAsync("MoveItemToPosition", args.oldIndex - 1, args.newIndex - 1);
                //console.log("Old index: " + oldIndex);
                //console.log("New index: " + newIndex);
            }
        });
    }
};
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
