//Import sevice worker
import "../ServiceWorker/ServiceWorkerRegistrator";

//Import utils
import "./Utils/SaveFileAs";

declare global
{
    interface Window
    {
        DasContractPages: any;
    }
}
window.DasContractPages = {}

//Import styles
import "../../Styles/Global.scss";