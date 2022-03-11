using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class BoolField : Field
    {
        [XmlIgnoreAttribute]
        public IList<bool> Data { get; set; } = new List<bool>();

        public BoolField( ) { }

        public override void SetData(string data)
        {
            Data.Clear();
            Data.Add(Convert.ToBoolean(data));
        }

        public override void SetDataList(List<string> data)
        {
            Data = data.Select(d => Convert.ToBoolean(d)).ToList();
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
