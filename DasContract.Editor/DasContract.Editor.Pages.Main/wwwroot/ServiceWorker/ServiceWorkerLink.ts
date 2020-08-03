//import "@drozdik.m/service-worker/dist/src/OfflineCacheWorker";
import "@drozdik.m/service-worker/dist/src/VersionCacheWorker";
import { StaticAssets } from "@drozdik.m/service-worker/dist/src/Libs/StaticAssets";
import { Environment } from "../Environment/Environment";

//DEFAULT IMAGE PATH
StaticAssets.defaultNotFoundImage = "/ServiceWorker/Images/DefaultImage.jpg";

//"YOU ARE OFFLINE" PAGE PATH
StaticAssets.defaultOfflinePage = "/error/offline";
StaticAssets.defaultOfflinePageResources = [

    //html

    //favicons
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/apple-touch-icon.png`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/favicon-32x32.png`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/favicon-16x16.png`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/safari-pinned-tab.svg`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/favicon.ico`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/mstile-144x144.png`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Favicon/browserconfig.xml`,

    //styles
    `/_content/${Environment.ErrorPagesAssemblyName}/Styles/_dist/Global.css`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Styles/_dist/ErrorPage.css`,

    //images
    `/_content/${Environment.ErrorPagesAssemblyName}/Images/Icons/offline.svg`,

    //fonts
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans.eot`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans.svg`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans.ttf`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans.woff`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Bold.eot`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Bold.svg`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Bold.ttf`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Bold.woff`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Extrabold.eot`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Extrabold.svg`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Extrabold.ttf`,
    `/_content/${Environment.ErrorPagesAssemblyName}/Fonts/OpenSans/OpenSans-Extrabold.woff`,
];

//MANIFEST
StaticAssets.manifestPath = `Manifest/Manifest.json`;




    