using DasContract.Abstraction.Data;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public abstract class FormField
    {
        [XmlAttribute("ParamBind")]
        public string ParamBind { get; set; } = "";
        [XmlAttribute("ViewBind")]
        public string ViewBind { get; set; } = "";
        [XmlAttribute("Label")]
        public string Label { get; set; }
        [XmlAttribute("Description")]
        public string Description { get; set; } = "";
        [XmlAttribute("ReadOnly")]
        public bool ReadOnly { get; set; } = false;
        [XmlIgnore]
        public bool Currency { get; set; } = false;

        public abstract void SetData(string data);
        public abstract void SetDataList(List<string> data);
        public abstract object GetData();
    }
}
