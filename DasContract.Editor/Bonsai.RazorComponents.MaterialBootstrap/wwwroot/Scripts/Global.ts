import "jquery"
import "popper.js";
import "arrive";
import "snackbarjs";
import "bootstrap-material-design";
import "../../../Styles/Global.scss";
//import "bootstrap-material-design/dist/css/bootstrap-material-design.min.css";

//IMPORT FONT AWESOME
import "@fortawesome/fontawesome-free/js/fontawesome";
import "@fortawesome/fontawesome-free/js/solid";
import "@fortawesome/fontawesome-free/js/regular";
import "@fortawesome/fontawesome-free/js/brands";

// DECLARE GLOBAL NAMESPACE
declare global
{
    interface Window
    {
        MaterialBootstrapRazorComponents: any;
    }
}
window.MaterialBootstrapRazorComponents = {}


//INITIATE BOOTSTRAP MATERIAL DESIGN
declare let $: any;
$(document).ready(() => { ($("body") as any).bootstrapMaterialDesign() });

//IMPORT SERVICES
import "../../Services/Scroll/SlowScroll";

//IMPORT COMPONENTS
import "../../Components/CAlert/Alert";
import "../../Components/CDialogWindow/DialogWindow";
import "../../Components/CButtonInput/ButtonInput";
import "../../Components/CSnackbar/Snackbar";
import "../../Components/CBreadcrumbs/Breadcrumbs";
import "../../Components/CLoadingScreen/LoadingScreen";
import "../../Components/CModelForm/ModelForm";
import "../../Components/CSortableList/SortableList";
import "../../Components/CPaging/Paging";
import "../../Components/CCard/Card";
import "../../Components/CValueInput/ValueInput";
import "../../Components/CIndexedList/IndexedList";
import "../../Components/CProgressBar/ProgressBar";
import "../../Components/CFileInput/FileInput";
import "../../Components/CTable/Table";
import "../../Components/CHeading/Heading";





