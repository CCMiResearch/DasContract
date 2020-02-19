using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DasContract.Abstraction.Entity;

namespace DasContract.Abstraction.XML.Factory
{
    public static class ContractXML
    {
        public static Contract FromXML(string contractXml)
        {
            var xml = new XmlSerializer(typeof(Contract));
            using var textReader = new StringReader(contractXml);
            var deserializedContract = xml.Deserialize(textReader) as Contract;
            return deserializedContract;
        }

        public static string ToXML(Contract contract)
        {
            var xml = new XmlSerializer(contract.GetType());
            using var textWriter = new StringWriter();
            xml.Serialize(textWriter, contract);
            return textWriter.ToString();
        }
    }
}
