using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DasToSolidity.SolidityConverter;
using DasContract.Abstraction;
using Liquid.NET;
using Liquid.NET.Constants;

namespace DasToSolidity
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlString = File.ReadAllText(@"DasContract.Blockchain.Solidity.Test/example.dascontract");
            var contract = ContractFactory.FromDasFile(xmlString);
            var generator = new ProcessConverter(contract);

            var code = generator.GenerateSolidity();
            Console.WriteLine(code);

            File.WriteAllText(@"./code.sol", code);
        }
    }
}
