using System;
using System.IO;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction.BPMN.Factory;
using NUnit.Framework;

namespace DasContract.Tests.Blockchain.Solidity
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        public void BPMNCompilation()
        {
            throw new NotImplementedException();

            /*string xmlString = File.ReadAllText(@"../../../DasContract.Blockchain.Solidity.Test/testDiagram.bpmn");
            var contract = ContractFactory.FromBPMN(xmlString);
            var generator = new ProcessConverter(contract);

            var code = generator.GenerateSolidity();

            File.WriteAllText(@"./code.txt", code);*/
        }
    }
}
