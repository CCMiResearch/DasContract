using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Abstraction.Processes
{
    [XmlRoot("definitions")]
    public class BusinessRule
    {
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; } = String.Empty;
        [XmlAttribute("xmlns:dmndi")]
        public string XmlnsDmndi { get; set; } = String.Empty;
        [XmlAttribute("xmlns:dc")]
        public string XmlnsDc { get; set; } = String.Empty;
        [XmlAttribute("xmlns:biodi")]
        public string XmlnsBiodi { get; set; } = String.Empty;
        [XmlAttribute("id")]
        public string Id { get; set; } = String.Empty;
        [XmlAttribute("name")]
        public string Name { get; set; } = String.Empty;
        [XmlAttribute("namespace")]
        public string Namespace { get; set; } = String.Empty;
        [XmlAttribute("exporter")]
        public string Exporter { get; set; } = String.Empty;
        [XmlAttribute("exporterVersion")]
        public string ExporterVersion { get; set; } = String.Empty;


    }
}
