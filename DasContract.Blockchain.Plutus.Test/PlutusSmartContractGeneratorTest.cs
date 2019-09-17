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
            PlutusSmartContractGenerator generator = new PlutusSmartContractGenerator();

            //Choose a smart contract name.
            SmartContractModel model = new SmartContractModel() { Name = "..." };

            //Choose a name of the function and a message used in the log.
            model.AddWalletFunction(new LogAMessage() { Name = "...", Message = "...", TemplateSourceCode = Resources.LogAMessage });

            //Choose a path to store the smart contract.
            generator.Generate(model, @"...");
        }
    }
}
