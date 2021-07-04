var select2Lib =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./js/select2.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./js/select2.js":
/*!***********************!*\
  !*** ./js/select2.js ***!
  \***********************/
/*! exports provided: initializeSelect2, addOption, removeOption, refreshOptions, getSelectedIds, refreshSelectedItems */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"initializeSelect2\", function() { return initializeSelect2; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"addOption\", function() { return addOption; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"removeOption\", function() { return removeOption; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"refreshOptions\", function() { return refreshOptions; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"getSelectedIds\", function() { return getSelectedIds; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"refreshSelectedItems\", function() { return refreshSelectedItems; });\nfunction initializeSelect2(componentId, selectedItems, options, dotnetRef) {\n  $(document).ready(function () {\n    $(`#${componentId}`).select2({\n      theme: \"bootstrap-5\",\n      width: '100%'\n    });\n  });\n  console.log(\"created\" + componentId);\n  options.forEach(option => {\n    var addedOption = new Option(option.text, option.value, false, false);\n    $('#' + componentId).append(addedOption);\n  });\n  $('#' + componentId).val(selectedItems);\n  $('#' + componentId).on('select2:select', function (e) {\n    dotnetRef.invokeMethodAsync('OnSelect', e.params.data.id);\n  });\n  $('#' + componentId).on('select2:unselect', function (e) {\n    dotnetRef.invokeMethodAsync('OnUnselect', e.params.data.id);\n  });\n  $('#' + componentId).on('change', function (e) {\n    dotnetRef.invokeMethodAsync('OnChange');\n  });\n}\nfunction addOption(componentId, value, text) {\n  var addedOption = new Option(text, value, false, false);\n  $('#' + componentId).append(addedOption);\n}\nfunction removeOption(componentId, value) {\n  $('#' + componentId).find('[value=\"' + value + '\"]').remove();\n}\nfunction refreshOptions(componentId, options, selected) {\n  $('#' + componentId + \" option\").remove();\n  options.forEach(option => {\n    var addedOption = new Option(option.text, option.value, false, false);\n    $('#' + componentId).append(addedOption);\n  });\n  refreshSelectedItems(componentId, selected);\n}\nfunction getSelectedIds(id) {\n  return $.makeArray($('#' + id).find(':selected').map(function () {\n    return this.value;\n  }));\n}\nfunction refreshSelectedItems(componentId, selected) {\n  $('#' + componentId).val(null);\n  $('#' + componentId).val(selected); //$('#' + componentId).trigger('change');\n}\n\n//# sourceURL=webpack://%5Bname%5DLib/./js/select2.js?");

/***/ })

/******/ });