using DasContract.Abstraction.UserInterface.FormFields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface
{
    public class FieldGroup
    {
        [XmlAttribute("Label")]
        public string Label { get; set; } = "";
        [XmlAttribute("Vertical")]
        public bool Vertical { get; set; } = true;
        [XmlAttribute("Displayed")]
        public bool Displayed { get; set; } = true;

        [XmlElement("DateField", typeof(DateField))]
        [XmlElement("AddressField", typeof(AddressField))]
        [XmlElement("SingleLineField", typeof(SingleLineField))]
        [XmlElement("MultiLineField", typeof(MultiLineField))]
        [XmlElement("IntField", typeof(IntField))]
        [XmlElement("DecimalField", typeof(DecimalField))]
        [XmlElement("BoolField", typeof(BoolField))]
        [XmlElement("EnumField", typeof(EnumField))]
        [XmlElement("DropdownField", typeof(DropdownField))]
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}
