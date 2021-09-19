using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class DateField : Field
    {
        [XmlIgnoreAttribute]
        public IList<DateTime> Data { get; set; } = new List<DateTime>();

        public DateField() { }

        public override void SetData(string data)
        {
            Data.Clear();
            Data.Add(DateTime.Parse(data));
        }

        public override void SetDataList(List<string> data)
        {
            Data = data.Select(d => DateTime.Parse(d)).ToList();
        }

        public override object GetData()
        {
            return Data;
        }
    }
}
