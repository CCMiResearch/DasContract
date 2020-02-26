exports.__esModule = true;
//import "@drozdik.m/service-worker/dist/src/OfflineCacheWorker";
require("@drozdik.m/service-worker/dist/src/VersionCacheWorker");
var StaticAssets_1 = require("@drozdik.m/service-worker/dist/src/Libs/StaticAssets");
var Environment_1 = require("../Environment/Environment");
//DEFAULT IMAGE PATH
StaticAssets_1.StaticAssets.defaultNotFoundImage = "/ServiceWorker/Images/DefaultImage.jpg";
//"YOU ARE OFFLINE" PAGE PATH
StaticAssets_1.StaticAssets.defaultOfflinePage = "/error/offline";
StaticAssets_1.StaticAssets.defaultOfflinePageResources = [
    //html
    //favicons
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/apple-touch-icon.png",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/favicon-32x32.png",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/favicon-16x16.png",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/safari-pinned-tab.svg",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/favicon.ico",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/mstile-144x144.png",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Favicon/browserconfig.xml",
    //styles
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Styles/_dist/Global.css",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Styles/_dist/ErrorPage.css",
    //images
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Images/Icons/offline.svg",
    //fonts
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans.eot",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans.svg",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans.ttf",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans.woff",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Bold.eot",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Bold.svg",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Bold.ttf",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Bold.woff",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Extrabold.eot",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Extrabold.svg",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Extrabold.ttf",
    "/_content/" + Environment_1.Environment.ErrorPagesAssemblyName + "/Fonts/OpenSans/OpenSans-Extrabold.woff",
];
//MANIFEST
StaticAssets_1.StaticAssets.manifestPath = "Manifest/Manifest.json";
