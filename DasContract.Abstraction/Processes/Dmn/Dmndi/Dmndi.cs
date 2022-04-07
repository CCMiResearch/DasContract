using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class Dmndi
    {
        //Elements
        [XmlElement("DMNDiagram", Namespace = "https://www.omg.org/spec/DMN/20191111/DMNDI/")]
        public DmnDiagram DmnDiagram { get; set; } = new DmnDiagram();

        //Methods and Constructors
        public Dmndi() { }
    }
}
