using DasContract.Abstraction.Processes.Dmn.Diagram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    [XmlRoot("definitions", Namespace = "https://www.omg.org/spec/DMN/20191111/MODEL/")]
    public class Definitions
    {
        //Attributes
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("namespace")]
        public string Namespace { get; set; } = string.Empty;

        [XmlAttribute("exporter")]
        public string Exporter { get; set; } = string.Empty;

        [XmlAttribute("exporterVersion")]
        public string ExporterVersion { get; set; } = string.Empty;

        //Elements
        [XmlElement("decision")]
        public List<Decision> Decisions { get; set; } = new List<Decision>();

        [XmlElement("inputData")]
        public List<InputData> InputData { get; set; } = new List<InputData>();

        [XmlElement("knowledgeSource")]
        public List<KnowledgeSource> KnowledgeSources { get; set; } = new List<KnowledgeSource>();

        [XmlElement("businessKnowledgeModel")]
        public List<BusinessKnowledgeModel> BusinessKnowledgeModels { get; set; } = new List<BusinessKnowledgeModel>();

        [XmlElement("textAnnotation")]
        public List<TextAnnotation> TextAnnotations { get; set; } = new List<TextAnnotation>();

        [XmlElement("association")]
        public List<Association> Associations { get; set; } = new List<Association>(); 

        [XmlElement("DMNDI", Namespace = "https://www.omg.org/spec/DMN/20191111/DMNDI/")]
        public Dmndi Dmndi { get; set; } = new Dmndi();

        //Methods and Constructors
        public Definitions() { }

        public static Definitions DeserializePlainDefinition(string plainDefinition)
        {
            using (TextReader reader = new StringReader(plainDefinition))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Definitions));
                XmlReader xmlReader = new XmlTextReader(reader);

                return (Definitions)serializer.Deserialize(xmlReader);
            }
        }

        public XElement ToXElement()
        {
            return new XElement("definitions");
        }
    }
}
