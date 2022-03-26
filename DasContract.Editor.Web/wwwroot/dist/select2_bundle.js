/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
var select2Lib;
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./js/select2.js":
/*!***********************!*\
  !*** ./js/select2.js ***!
  \***********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"addOption\": () => (/* binding */ addOption),\n/* harmony export */   \"getSelectedIds\": () => (/* binding */ getSelectedIds),\n/* harmony export */   \"initializeSelect2\": () => (/* binding */ initializeSelect2),\n/* harmony export */   \"refreshOptions\": () => (/* binding */ refreshOptions),\n/* harmony export */   \"refreshSelectedItems\": () => (/* binding */ refreshSelectedItems),\n/* harmony export */   \"removeOption\": () => (/* binding */ removeOption)\n/* harmony export */ });\nfunction initializeSelect2(componentId, selectedItems, options, multiple, minimumResultsForSearch, dotnetRef) {\n  $(`#${componentId}`).select2({\n    theme: \"bootstrap-5\",\n    width: '100%',\n    multiple: multiple,\n    minimumResultsForSearch: minimumResultsForSearch\n  });\n  options.forEach(option => {\n    var addedOption = new Option(option.text, option.value, false, false);\n    $('#' + componentId).append(addedOption);\n  });\n  $('#' + componentId).val(selectedItems);\n  $('#' + componentId).on('select2:select', function (e) {\n    dotnetRef.invokeMethodAsync('OnSelect', e.params.data.id);\n  });\n  $('#' + componentId).on('select2:unselect', function (e) {\n    console.log(e);\n    dotnetRef.invokeMethodAsync('OnUnselect', e.params.data.id);\n  });\n  $('#' + componentId).on('change', function (e) {\n    dotnetRef.invokeMethodAsync('OnChange');\n  });\n}\nfunction addOption(componentId, value, text) {\n  var addedOption = new Option(text, value, false, false);\n  $('#' + componentId).append(addedOption);\n}\nfunction removeOption(componentId, value) {\n  $('#' + componentId).find('[value=\"' + value + '\"]').remove();\n}\nfunction refreshOptions(componentId, options, selected) {\n  $('#' + componentId + \" option\").remove();\n  options.forEach(option => {\n    var addedOption = new Option(option.text, option.value, false, false);\n    $('#' + componentId).append(addedOption);\n  });\n  refreshSelectedItems(componentId, selected);\n}\nfunction getSelectedIds(id) {\n  return $.makeArray($('#' + id).find(':selected').map(function () {\n    return this.value;\n  }));\n}\nfunction refreshSelectedItems(componentId, selected) {\n  $('#' + componentId).val(null);\n  $('#' + componentId).val(selected); //$('#' + componentId).trigger('change');\n}\n\n//# sourceURL=webpack://%5Bname%5DLib/./js/select2.js?");

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
/******/ 	__webpack_modules__["./js/select2.js"](0, __webpack_exports__, __webpack_require__);
/******/ 	select2Lib = __webpack_exports__;
/******/ 	
/******/ })()
;