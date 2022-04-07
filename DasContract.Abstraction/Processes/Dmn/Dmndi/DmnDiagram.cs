using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.Processes.Dmn
{
    public class DmnDiagram
    {
        //Elements
        [XmlElement("dmndi:DMNShape", Namespace = "https://www.omg.org/spec/DMN/20191111/DMNDI/")]
        public List<DmnShape> DmnShapes { get; set; } = new List<DmnShape>();

        //Methods and Constructors
        public DmnDiagram() { }
    }
}
