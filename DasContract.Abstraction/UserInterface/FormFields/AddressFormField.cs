using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class AddressFormField: FormField
    {
        [XmlIgnoreAttribute]
        public string Data { get; set; }

        public override void SetData(string data)
        {
            Data = data;
        }

        public override void SetDataList(List<string> data)
        {
            Data = string.Join(",", data);
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
