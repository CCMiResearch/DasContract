using DasContract.Blockchain.Plutus.Properties;
using DasContract.Blockchain.Plutus.Functions;
using System.IO;
using Fluid;

namespace DasContract.Blockchain.Plutus
{
    public class PlutusSmartContractGenerator
    {
        public string GeneratedSmartContract { get; set; }

        public string TemplateSourceCode { get; set; }

        public void Generate ( TemplateModel templateModel, string pathString )
        {
            //Loads external template
            TemplateSourceCode = Resources.FluidSmartContractTemplate;

            //Registers data type not known in fluid.
            TemplateContext.GlobalMemberAccessStrategy.Register<Function>();

            //Template generation
            if (FluidTemplate.TryParse(TemplateSourceCode, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(templateModel.GetType());
                context.SetValue("p", templateModel);

                GeneratedSmartContract += template.Render(context);
            }

            string finalPath = Path.Combine(pathString, templateModel.Name) + ".hs";

            //Adding numeric suffix if file already exists.
            uint pathSuffix = 1;
            while (File.Exists(finalPath))
            {
                finalPath = Path.Combine(pathString, templateModel.Name) + pathSuffix++.ToString() + ".hs";
            }

            File.WriteAllText(finalPath, GeneratedSmartContract);
        }
    }
}
