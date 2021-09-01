window.onload = function () {
    window.addEventListener("beforeunload", async function (e) {
        
        var canExit = await exitGuardLib.contractManagerInstance.invokeMethodAsync("CanSafelyExit")
        console.log("can exit:", canExit);

        if (canExit) {
            return undefined;
        }
       

        var confirmationMessage = 'It looks like you have been editing something. '
            + 'If you leave before saving, your changes will be lost.';

        (e || window.event).returnValue = confirmationMessage; //Gecko + IE
        return confirmationMessage; //Gecko + Webkit, Safari, Chrome etc.
    });
};

export function setContractManagerInstance(dotNetObjectRef) {
    exitGuardLib.contractManagerInstance = dotNetObjectRef;
}

