export function setEventHandlerInstance(dotNetObjectRef) {
    document.addEventListener('keydown', function (event) {
        let eventObj = {
            "Type": event.type,
            "AltKey": event.altKey,
            "Key": event.key,
            "CtrlKey": event.ctrlKey
        };
        console.log(event);

        if (event.ctrlKey && (event.key === "z" || event.key === "y")) {
            event.preventDefault();
        }

        dotNetObjectRef.invokeMethodAsync("HandleKeyInputEvent", eventObj);
    });
}