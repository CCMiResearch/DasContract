/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
var exitGuardLib;
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./js/exitGuard.js":
/*!*************************!*\
  !*** ./js/exitGuard.js ***!
  \*************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"confirmDialog\": () => (/* binding */ confirmDialog),\n/* harmony export */   \"setContractManagerInstance\": () => (/* binding */ setContractManagerInstance)\n/* harmony export */ });\nconst DEFAULT_CONFIRMATION = 'It looks like you have been editing something. ' + 'If you leave before saving, your changes will be lost.';\n\nwindow.onload = function () {\n  window.addEventListener(\"beforeunload\", async function (e) {\n    var canExit = await exitGuardLib.contractManagerInstance.invokeMethodAsync(\"CanSafelyExit\");\n    console.log(\"can exit:\", canExit);\n\n    if (canExit) {\n      return undefined;\n    }\n\n    (e || window.event).returnValue = confirmationMessage; //Gecko + IE\n\n    return DEFAULT_CONFIRMATION; //Gecko + Webkit, Safari, Chrome etc.\n  });\n};\n\nfunction setContractManagerInstance(dotNetObjectRef) {\n  exitGuardLib.contractManagerInstance = dotNetObjectRef;\n}\nfunction confirmDialog(confirmationMessage = DEFAULT_CONFIRMATION) {\n  return window.confirm(confirmationMessage);\n}\n\n//# sourceURL=webpack://%5Bname%5DLib/./js/exitGuard.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The require scope
/******/ 	var __webpack_require__ = {};
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./js/exitGuard.js"](0, __webpack_exports__, __webpack_require__);
/******/ 	exitGuardLib = __webpack_exports__;
/******/ 	
/******/ })()
;