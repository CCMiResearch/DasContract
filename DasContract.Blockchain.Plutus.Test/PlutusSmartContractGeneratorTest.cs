using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            //Choose smart contract name.
            TemplateModel template = new TemplateModel("...");

            template.AddLibrary("Wallet");

            //Choose message used in the log.
            template.AddWalletFunction(new LogAMessage("logAMessage", @"..."));

            //Choose path to store the smart contract.
            generator.Generate(template, @"...");
        }
    }
}
