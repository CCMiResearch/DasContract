using DasContract.Blockchain.Plutus.Properties;
using DasContract.Blockchain.Plutus.Functions;
using System.IO;
using Fluid;

namespace DasContract.Blockchain.Plutus
{
    public class PlutusSmartContractFileGenerator
    {
        public string GeneratedSmartContract { get; set; }

        public string TemplateSourceCode { get; set; }

        public void Generate ( SmartContractModel smartContractModel, string pathString )
        {
            var renderer = new FluidTemplateRenderer();
            TemplateSourceCode = Resources.FluidSmartContractStructure;

            TemplateContext.GlobalMemberAccessStrategy.Register<Function>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SCStateMachine>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SCFact>();
            TemplateContext.GlobalMemberAccessStrategy.Register<SCTransactionKind>();
            TemplateContext.GlobalMemberAccessStrategy.Register<TransactionStateMachine>();

            GeneratedSmartContract = renderer.Assemble(TemplateSourceCode, smartContractModel);

            string filePath = ConstructFilePath(smartContractModel.ProcessName, pathString);
            File.WriteAllText(filePath, GeneratedSmartContract);
        }

        string ConstructFilePath ( string smartContractName, string pathString )
        {
            string finalPath = $"{pathString}{smartContractName}.hs";

            //Adding numeric suffix if file already exists.
            uint pathSuffix = 1;
            while (File.Exists(finalPath))
            {
                finalPath = $"{pathString}{smartContractName}{pathSuffix.ToString()}.hs";
                pathSuffix++;
            }

            return finalPath;
        }
    }
}
