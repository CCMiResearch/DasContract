
// DECLARE GLOBAL NAMESPACE
declare global
{
    interface Window
    {
        DasContractComponents: any;
    }
}
window.DasContractComponents = {}


//Import components
import "../../Components/CContractEditor/ContractEditor";
import "../../Components/CEditableItemsList/EditableItemsList";
import "../../Components/CIntegrityAnalysisResult/IntegrityAnalysisResult";


//Import styles
import "../../../Styles/Global.scss";

//Import icons
import "material-design-icons";
