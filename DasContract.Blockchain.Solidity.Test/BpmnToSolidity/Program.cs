using System;
using System.Collections.Generic;
using System.IO;
using BpmnToSolidity.SolidityConverter;
using DasContract.Abstraction;
using Liquid.NET;
using Liquid.NET.Constants;

namespace BpmnToSolidity
{
    class Program
    {
        static void Main(string[] args)
        {

            string xmlString = File.ReadAllText(@"../../../testDiagram.bpmn");
            var contract = ContractFactory.FromBpmn(xmlString);
            var generator = new ProcessConverter(contract);

            var code = generator.GenerateSolidity();
            Console.WriteLine(code);

            File.WriteAllText(@"./code.txt", code);
        }
    }
}
