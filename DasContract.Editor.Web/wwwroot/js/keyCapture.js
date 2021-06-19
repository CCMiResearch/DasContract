export function setEventHandlerInstance(dotNetObjectRef) {
    document.addEventListener('keydown', function (event) {
        let eventObj = {
            "Type": event.type,
            "AltKey": event.altKey,
            "Key": event.key,
            "CtrlKey": event.ctrlKey
        };
        console.log(event);
        dotNetObjectRef.invokeMethodAsync("HandleKeyInputEvent", eventObj);
    });
}