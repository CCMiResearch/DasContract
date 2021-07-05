using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class BoolField : Field
    {
        [XmlIgnoreAttribute]
        public bool Data { get; set; }

        public override void SetData(string data)
        {
            Data = Convert.ToBoolean(data);
        }

        public override void SetDataList(List<string> data)
        {
            if (data.Count > 0)
            {
                Data = Convert.ToBoolean(data[0]);
            }
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
