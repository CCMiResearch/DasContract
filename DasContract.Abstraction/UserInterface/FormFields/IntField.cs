using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class IntField : Field
    {
        [XmlIgnoreAttribute]
        public long Data { get; set; }

        public override void SetData(string data)
        {
            Data = Convert.ToInt64(data);
        }

        public override void SetDataList(List<string> data)
        {
            if (data.Count > 0)
            {
                Data = Convert.ToInt64(data[0]);
            }
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
