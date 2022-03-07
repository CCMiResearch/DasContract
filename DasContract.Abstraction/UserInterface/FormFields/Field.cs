using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public abstract class Field
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
        [XmlAttribute("Multiple")]
        public bool Multiple { get; set; } = false;

        [XmlIgnore]
        public int FieldCount { get; set; } = 1;

        public abstract void SetData(string data);
        public abstract void SetDataList(List<string> data);
        public abstract object GetData();
    }
}
