using System.Linq;
using System.Collections.Generic;

namespace DasContract.Blockchain.Plutus
{
    /// <summary>
    /// The object from which the fluid template is created.
    /// </summary>
    public class TemplateModel
    {   
        /// <summary>
        /// The smart contract name
        /// </summary>
        public string Name;
        /// <summary>
        /// Imported libraries and modules in the smart contract.
        /// Two initial libraries for basic compilation in the Plutus Playground. 
        /// </summary>
        public IList<string> Libraries { get; set; } = new List<string>(new string[] { "Language.PlutusTx.Prelude", "Playground.Contract" });
        /// <summary>
        /// Functions of the smart contract.
        /// </summary>
        public IList<Function> Functions { get; set; } = new List<Function>();
        /// <summary>
        /// Functions in the smart contract availible to the blockchain wallet.
        /// </summary>
        public IList<Function> WalletFunctions { get; set; } = new List<Function>();
        /// <summary>
        /// Adds a wallet function that logs a custom message on the blockchain.
        /// </summary>
        /// <param name="name">Name of the function that must be unique both in the functions and the wallet functions.</param>
        /// <param name="message">Message shown in the log.</param>
        public void AddLogAMessageFunction ( string name, string message )
        {
            if (FunctionNameAlreadyExists(name))
            {
                return;
            }

            Function newFunction = new Function();

            newFunction.Name = name;
            newFunction.Head = "MonadWallet m => m()";
            newFunction.Body = "logMsg " + '"' + message + '"' + '\n';

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
