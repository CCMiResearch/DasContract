using System.IO;
using Fluid;

namespace DasContract.Blockchain.Plutus
{
    public class PlutusSmartContractGenerator
    {
        /// <summary>
        /// Full smart contract code.
        /// </summary>
        public string GeneratedSmartContract { get; set; }
        /// <summary>
        /// Fluid smart contract template.
        /// </summary>
        public string TemplateSourceCode { get; set; }
        /// <summary>
        /// Object for filling the fluid template.
        /// </summary>
        public TemplateModel TemplateModel { get; set; } = new TemplateModel();

        public void Generate ( string pathString )
        {
            ProcessDasContract();

            string finalPath = Path.Combine(pathString, TemplateModel.Name) + ".hs";
            uint pathSuffix = 1;

            //Adding numeric suffix if file already exists.
            while (File.Exists(finalPath))
            {
                finalPath = Path.Combine(pathString, TemplateModel.Name) + pathSuffix++.ToString() + ".hs";
            }

            File.WriteAllText(finalPath, GeneratedSmartContract);
        }
        /// <summary>
        /// Creation of the fluid template.
        /// </summary>
        public void ProcessDasContract ()
        {
            GenerateHead();
            GenerateBody();
            RegisterWalletFunctions();

            //Registers data type not known in fluid.
            TemplateContext.GlobalMemberAccessStrategy.Register<Function>();
            
            //Template generation
            if (FluidTemplate.TryParse(TemplateSourceCode, out var template))
            {
                var context = new TemplateContext();
                context.MemberAccessStrategy.Register(TemplateModel.GetType());
                context.SetValue("p", TemplateModel);

                GeneratedSmartContract += template.Render(context);
            }
        }

        public void GenerateHead ()
        {
            //Definition of smart contract name
            TemplateSourceCode += "module {{ p.Name }} where\n\n";
            //Importing libraries and modules
            TemplateSourceCode += "{% for library in p.Libraries %}" +
                                        "import {{library}}\n" +
                                  "{% endfor %}\n";
        }

        public void GenerateBody()
        {
            //Adding wallet functions
            TemplateSourceCode += "--\n--\n" + //Fluid erases whitespaces. Comments added to visually distinguish the head and the body.
                                  "{% for function in p.WalletFunctions %}" +
                                  "{{ function.Name }} :: {{ function.Head }}\n" +
                                  "{{ function.Name }} = {{ function.Body }}\n" +
                                  "{% endfor %}";
        }

        public void RegisterWalletFunctions()
        {
            if (TemplateModel.WalletFunctions.Count == 0)
            {
                return;
            }

            TemplateSourceCode += "$(mkFunctions['" +
                                  "{% assign functionNames = p.WalletFunctions | map: 'Name' %}" +
                                  "{{ functionNames | join: \", '\" }}" +
                                  "])\n";
        }
    }
}
