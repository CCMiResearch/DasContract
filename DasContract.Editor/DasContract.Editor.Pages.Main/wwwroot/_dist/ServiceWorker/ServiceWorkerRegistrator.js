exports.__esModule = true;
var service_worker_registrator_1 = require("@drozdik.m/service-worker-registrator");
new service_worker_registrator_1.ServiceWorkerRegistrator("/ServiceWorker.js").Register();
