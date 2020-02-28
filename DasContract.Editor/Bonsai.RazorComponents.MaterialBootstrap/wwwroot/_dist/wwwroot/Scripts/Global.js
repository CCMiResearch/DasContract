exports.__esModule = true;
require("jquery");
require("popper.js");
require("arrive");
require("snackbarjs");
require("bootstrap-material-design");
require("../../../Styles/Global.scss");
//import "bootstrap-material-design/dist/css/bootstrap-material-design.min.css";
//IMPORT FONT AWESOME
require("@fortawesome/fontawesome-free/js/fontawesome");
require("@fortawesome/fontawesome-free/js/solid");
require("@fortawesome/fontawesome-free/js/regular");
require("@fortawesome/fontawesome-free/js/brands");
window.MaterialBootstrapRazorComponents = {};
$(document).ready(function () { $("body").bootstrapMaterialDesign(); });
//IMPORT SERVICES
require("../../Services/Scroll/SlowScroll");
//IMPORT COMPONENTS
require("../../Components/CAlert/Alert");
require("../../Components/CDialogWindow/DialogWindow");
require("../../Components/CButtonInput/ButtonInput");
require("../../Components/CSnackbar/Snackbar");
require("../../Components/CBreadcrumbs/Breadcrumbs");
require("../../Components/CLoadingScreen/LoadingScreen");
require("../../Components/CModelForm/ModelForm");
require("../../Components/CSortableList/SortableList");
require("../../Components/CPaging/Paging");
require("../../Components/CCard/Card");
require("../../Components/CValueInput/ValueInput");
require("../../Components/CIndexedList/IndexedList");
require("../../Components/CProgressBar/ProgressBar");
require("../../Components/CFileInput/FileInput");
require("../../Components/CTable/Table");
require("../../Components/CHeading/Heading");
