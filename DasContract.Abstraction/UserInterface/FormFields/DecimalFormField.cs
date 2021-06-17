using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class DecimalFormField: FormField
    {
        [XmlAttribute("Currency")]
        public new bool Currency { get; set; } = false;
        [XmlIgnoreAttribute]
        public decimal Data { get; set; }

        public override void SetData(string data)
        {
            Data = Convert.ToDecimal(data);
        }

        public override void SetDataList(List<string> data)
        {
            if (data.Count > 0)
            {
                Data = Convert.ToDecimal(data[0]);
            }
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
