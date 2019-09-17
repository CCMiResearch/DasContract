using DasContract.Blockchain.Plutus.Functions;
using System.Collections.Generic;
using System.Linq;

namespace DasContract.Blockchain.Plutus
{
    public class SmartContractModel
    {
        public string Name { get; set; }

        public Contract Contract { get; set; }

        public IList<string> Libraries { get; set; } = new List<string>(new string[] { "Language.PlutusTx.Prelude", "Playground.Contract" });

        public IList<Function> Functions { get; set; } = new List<Function>();

        public IList<Function> WalletFunctions { get; set; } = new List<Function>();

        public void AddLibraries ( IList<string> libraries )
        {
            foreach ( string library in libraries)
            {
                if (!LibraryAlreadyIncluded(library))
                {
                    Libraries.Add(library);
                }
            }
        }
        private bool LibraryAlreadyIncluded ( string name )
        {
            if (Libraries.Any(l => name.Contains(l)))
            {
                return true;
            }
            return false;
        }

        public void AddWalletFunction ( Function newFunction )
        {
            if (FunctionNameAlreadyExists(newFunction.Name))
            {
                return;
            }
            AddLibraries(newFunction.Libraries);
            newFunction.RenderTemplate();
            WalletFunctions.Add(newFunction);
        }

        private bool FunctionNameAlreadyExists ( string name )
        {
            if (Functions.Any(f => name.Contains(f.Name) || (WalletFunctions.Any(wf => name.Contains(wf.Name)))))
            {
                return true;
            }
            return false;
        }
    }
}
