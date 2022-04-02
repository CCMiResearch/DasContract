using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.ContractManagement
{
    public interface IContractManager
    {

        string GeneratedContract { get; }

        bool IsContractInitialized();
        Task InitAsync();
        Task InitializeNewContract();
        bool ConvertContract(out string data);

        bool CanSafelyExit();
        string SerializeContract();
        void RestoreContract(string contractXML);
        string GetContractName();
        string GetContractId();
        void SetContractName(string name);

    }
}
