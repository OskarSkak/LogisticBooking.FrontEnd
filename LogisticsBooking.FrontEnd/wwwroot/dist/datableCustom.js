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
/******/ 	return __webpack_require__(__webpack_require__.s = "./src/datatableCustom.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./src/datatableCustom.js":
/*!********************************!*\
  !*** ./src/datatableCustom.js ***!
  \********************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("\n\n$('#today').on('click' ,(function () {\n    $('#example').DataTable().clear().destroy();\n    var table = $('#example').DataTable( {\n        \"processing\": true,\n        \"serverSide\": false,\n        \"ajax\": {\n            \"url\": \"BookingOverview?handler=test\",\n            \"type\": \"GET\",\n            \"datatype\": \"application/json\",\n            \"dataSrc\": \"bookings\",\n\n        },\n\n        \"columns\": [\n            {\n                \"className\": 'details-control',\n                \"orderable\": false,\n                \"data\": \"internalId\",\n                \"defaultContent\": '',\n                \"render\": function (data) {\n                    return '<i class=\"fas fa-caret-down\" aria-hidden=\"true\"></i> <input type=\"hidden\" name=\"id\" value=\"' + data + '\">';\n                },\n                width:\"15px\"\n            },\n            {\n                \"data\": \"bookingTime\" , render: function(data ) {\n                    return moment(data).format(\" DD MM\") + '<input type=\"hidden\" name=\"dateTo\" value=\" ' + data + ' \"/>';\n                },\n                width:\"70px\"\n            },\n            { \"data\": \"externalId\" },\n\n            { \"data\": \"totalPallets\" },\n            { \"data\": \"transporterName\" },\n            { \"data\": \"email\" },\n            {\"data\" : \"port\"},\n            {\"data\" : \"interval.startTime\" , render: function(data) {\n                    return moment(data).format(\"HH:MM\")\n                }},\n            {\"data\" : \"actualArrival\" , render: function(data){\n                    return '<input class=\"form-control\" name=\"actualArrival\" value=\"' +moment(data).format(\"HH:mm\")+'\" type=\"time\"/> '\n                } },\n\n            {\"data\" : \"startLoading\" , render: function(data){\n                    return '<input class=\"form-control\" name=\"startLoading\" value=\"' + moment(data).format(\"HH:mm\")+'\"  type=\"time\"/> '\n                }},\n            {\"data\" : \"endLoading\" , render: function(data){\n                    return  ' <input  class=\"form-control\" name=\"endLoading\" value=\"'+  moment(data).format(\"HH:mm\")   +'\" type=\"time\"/>  '\n                }},\n            {\"data\" : null , render: function(data){\n                    return  '<Button  class=\" SubmitButton btn btn-secondary\"  type=\"submit\" >Opdater</button> </form> '\n                }},\n\n\n        ]\n    } );\n\n    $('#SubmitButton').on('click', function() {\n        var data = table.$('input, select').serialize();\n        alert(\n            \"The following data would have been submitted to the server: \\n\\n\"+\n            data.substr( 0, 120 )+'...'\n        );\n        return false;\n    } );\n\n    // Add event listener for opening and closing details\n    $('#example tbody').on('click', 'td.details-control', function () {\n        var tr = $(this).closest('tr');\n        var tdi = tr.find(\"i.fa\");\n        var row = table.row(tr);\n        if (row.child.isShown()) {\n            // This row is already open - close it\n            row.child.hide();\n            tr.removeClass('shown');\n            tdi.first().removeClass('fas fa-caret-up');\n            tdi.first().addClass('fas fa-caret-down');\n        }\n        else {\n            // Open this row\n            row.child(format(row.data())).show();\n            tr.addClass('shown');\n            tdi.first().removeClass('fas fa-caret-down');\n            tdi.first().addClass('fas fa-caret-up');\n        }\n    });\n\n    table.on(\"user-select\", function (e, dt, type, cell, originalEvent) {\n        if ($(cell.node()).hasClass(\"details-control\")) {\n            e.preventDefault();\n        }\n    });\n\n} )); \n    \n\n//# sourceURL=webpack:///./src/datatableCustom.js?");

/***/ })

/******/ });