using Microsoft.VisualStudio.TestTools.UnitTesting;
using DasContract.Blockchain.Plutus.Properties;
using DasContract.Blockchain.Plutus.Functions;

namespace DasContract.Blockchain.Plutus.Test
{
    [TestClass]
    public class PlutusSmartContractGeneratorTest
    {
        [TestMethod]
        public void LogMessageSmartContract()
        {
            var generator = new PlutusSmartContractGenerator();

            //Choose a smart contract name.
            var model = new SmartContractModel() { Name = "..." };

            //Choose a name of the function and a message used in the log.
            model.AddWalletFunction(new LogAMessage() { Name = "...", Message = "...", TemplateSourceCode = Resources.LogAMessage });

            //Choose a path to store the smart contract. With '\' at the end.
            generator.Generate(model, @"...");
        }
    }
}
