using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class DateFormField: FormField
    {
        [XmlIgnoreAttribute]
        public DateTime Data { get; set; }

        public override void SetData(string data)
        {
            Data = DateTime.Parse(data);
        }

        public override void SetDataList(List<string> data)
        {
            if (data.Count > 0)
            {
                Data = DateTime.Parse(data[0]);
            }
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
