/* JAVASCRIPT PLUGINS GOES HERE */

// jquery
import jQuery from "jquery";
window.$ = window.jQuery = jQuery;

require("popper.js");
require("tooltip.js");
import  "popper.js";
import tooltip from "tooltip.js";

import 'jquery';
// Bootstrap
import "bootstrap";


$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});

$(function () {
    $('[data-toggle="popover"]').popover()
})
