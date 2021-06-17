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

        [XmlElement("DateField", typeof(DateFormField))]
        [XmlElement("AddressField", typeof(AddressFormField))]
        [XmlElement("SingleLineField", typeof(SingleLineFormField))]
        [XmlElement("MultiLineField", typeof(MultiFormLineField))]
        [XmlElement("IntField", typeof(IntFormField))]
        [XmlElement("DecimalField", typeof(DecimalFormField))]
        [XmlElement("BoolField", typeof(BoolFormField))]
        [XmlElement("EnumField", typeof(EnumFormField))]
        [XmlElement("DropdownField", typeof(DropdownFormField))]
        public List<FormField> Fields { get; set; } = new List<FormField>();
    }
}
