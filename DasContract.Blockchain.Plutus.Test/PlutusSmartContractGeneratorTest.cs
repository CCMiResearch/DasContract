using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            generator.TemplateModel.Name = new string("...");
            generator.TemplateModel.Libraries.Add("Wallet");
            //Choose message used in the log.
            generator.TemplateModel.AddLogAMessageFunction("logAMessage", "...");
            //Choose path to store the smart contract.
            generator.Generate(@"...");
        }
    }
}
