using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class SingleLineField : Field
    {
        [XmlAttribute("Currency")]
        public new bool Currency { get; set; } = false;

        [XmlIgnoreAttribute]
        public IList<string> Data { get; set; } = new List<string>();

        public SingleLineField() { }

        public override void SetData(string data)
        {
            Data.Clear();
            Data.Add(data);
        }

        public override void SetDataList(List<string> data)
        {
            Data = data;
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
