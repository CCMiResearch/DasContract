const copyfiles = require("copyfiles");

const filesToCopy = [
    "bpmn-js/dist/bpmn-modeler.production.min.js",
    "bpmn-js/dist/assets/diagram-js.css",
    "bpmn-js/dist/assets/bpmn-font/**",
    "jquery/dist/jquery.slim.min.js",
    "bootstrap/dist/css/bootstrap.min.css",
    "bootstrap/dist/js/bootstrap.bundle.min.js",
    "bootstrap-icons/font/**",
    "dmn-js/dist/assets/**",
    "dmn-js/dist/dmn-modeler.production.min.js",
    "select2/dist/css/select2.min.css",
    "select2/dist/js/select2.min.js",
    "select2-bootstrap-5-theme/dist/select2-bootstrap-5-theme.min.css"
    ];

for (let file of filesToCopy) {
    copyfiles(["node_modules/" + file, "dist/node_packages"], { up: 1 }, (err) => { });
}