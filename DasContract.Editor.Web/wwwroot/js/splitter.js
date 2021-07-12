import Split from 'split-grid';

export function createSplit(gutterSelector) {
    Split({
        columnGutters: [{
            track: 1,
            element: document.querySelector(gutterSelector)
        }],
        onDrag: debounce(handleOnDrag, 30)
    })
}

function debounce(func, timeout = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { func.apply(this, args); }, timeout);
    };
}

export function setResizeHandlerInstance(dotNetObjectRef) {
    splitterLib.resizeHandlerRef = dotNetObjectRef;
}

function handleOnDrag() {
    if (splitterLib.resizeHandlerRef != null) {
        splitterLib.resizeHandlerRef.invokeMethodAsync("MainGutterResized");
    }
}