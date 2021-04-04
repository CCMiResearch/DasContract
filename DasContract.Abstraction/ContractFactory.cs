using DasContract.Abstraction.Exceptions;
using DasContract.Abstraction.Processes;
using DasContract.Abstraction.Data;
using System;
using System.Linq;
using System.Xml.Linq;

namespace DasContract.Abstraction
{
    public class ContractFactory
    {
        public static Contract FromDasFile(string dasXml)
        {
            var contract = new Contract();
            var xDoc = XDocument.Parse(dasXml);
            var process = xDoc.Descendants("Main").First();
            var dataModel = xDoc.Descendants("DataModel").First();
            contract.Process = ProcessFactory.FromDasFile(process);
            //TODO: Refactor entity factory
            //contract.DataTypes = DataTypeFactory.FromDasFile(dataModel);
            return contract;
        }
    }
}
