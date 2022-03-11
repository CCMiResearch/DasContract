using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class IntField : Field
    {
        [XmlIgnoreAttribute]
        public IList<long> Data { get; set; } = new List<long>();

        public IntField() { }

        public override void SetData(string data)
        {
            Data.Clear();
            Data.Add(Convert.ToInt64(data));
        }

        public override void SetDataList(List<string> data)
        {
            Data = data.Select(d => Convert.ToInt64(d)).ToList();
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
