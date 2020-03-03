//Import sevice worker
import "../ServiceWorker/ServiceWorkerRegistrator";

declare global
{
    interface Window
    {
        DasContractPages: any;
    }
}
window.DasContractPages = {}

//Import utils
import "./Utils/SaveFileAs";

//Import styles
import "../../Styles/Global.scss";