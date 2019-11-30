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

            string xmlString = File.ReadAllText(@"F:\\Users\\Johny\\Desktop\\diagram_1.bpmn");
            var contract = ContractFactory.FromBpmn(xmlString);
            var generator = new ProcessConverter(contract);
            Console.WriteLine(generator.generateSolidity());



            /*
            foreach(var o in contract.Process.Tasks)
            {
                Console.WriteLine(o.Id);
                foreach(var outg in o.Outgoing)
                {
                    Console.WriteLine(outg);
                }
            }
            */

        }
    }
}
