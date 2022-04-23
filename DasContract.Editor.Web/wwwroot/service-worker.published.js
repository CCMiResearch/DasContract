self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsIncl = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/,
    /\.json$/, /\.css$/, /\.woff$/, /\.ttf$/, /\.eot$/, /\.woff2$/, /\.svg$/,
    /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/, /\.blat$/, /\.dat$/, /\.dascontract$/,
    /\.xml$/];
const offlineAssetsExcl = [ /^service-worker\.js$/, /^routes\.json$/];

async function onInstall(event) {
    // Cache items from the assets manifest based on regex rules
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsIncl.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExcl.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    // Delete unused caches left behind by inactive service workers
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {
        // For all navigation requests, try to serve index.html from cache
        const shouldServeIndexHtml = event.request.mode === 'navigate';
        let request = shouldServeIndexHtml ? 'index.html' : event.request;
        const cache = await caches.open(cacheName);
        // Try to retrieve the request from cache, ignoring any query params
        cachedResponse = await cache.match(request, {ignoreSearch: true});
    }

    return cachedResponse || fetch(event.request);
}


