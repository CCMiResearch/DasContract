var keyCaptureLib;(()=>{"use strict";var e={d:(t,n)=>{for(var o in n)e.o(n,o)&&!e.o(t,o)&&Object.defineProperty(t,o,{enumerable:!0,get:n[o]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};function n(e){document.addEventListener("keydown",(function(t){let n={Type:t.type,AltKey:t.altKey,Key:t.key,CtrlKey:t.ctrlKey};console.log(t),!t.ctrlKey||"z"!==t.key&&"y"!==t.key||t.preventDefault(),e.invokeMethodAsync("HandleKeyInputEvent",n)}))}e.r(t),e.d(t,{setEventHandlerInstance:()=>n}),keyCaptureLib=t})();