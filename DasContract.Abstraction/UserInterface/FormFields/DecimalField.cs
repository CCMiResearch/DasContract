using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class DecimalField : Field
    {
        [XmlAttribute("Currency")]
        public new bool Currency { get; set; } = false;
        [XmlIgnoreAttribute]
        public IList<decimal> Data { get; set; } = new List<decimal>();

        public DecimalField() { }

        public override void SetData(string data)
        {
            Data.Clear();
            Data.Add(Convert.ToDecimal(data));
        }

        public override void SetDataList(List<string> data)
        {
            Data = data.Select(d => Convert.ToDecimal(d)).ToList();
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
