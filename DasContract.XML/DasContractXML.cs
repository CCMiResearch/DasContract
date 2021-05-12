using DasContract.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DasContract.XML
{
    public static class DasContractXML
    {
        public static string Serialize(Contract contract)
        {

            var serializer = new DataContractSerializer(typeof(Contract));

            return "";
        }

    }
}
