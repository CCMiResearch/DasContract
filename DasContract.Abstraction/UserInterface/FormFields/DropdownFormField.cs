using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class DropdownFormField: FormField
    {
        [XmlIgnoreAttribute]
        public int? Data { get; set; } = null;
        [XmlElement("Option")]
        public List<string> Options { get; set; } = new List<string>();
        [XmlAttribute("Indexed")]
        public bool Indexed { get; set; } = false;

        public override void SetData(string data)
        {
            for (int i = 0; i < Options.Count; ++i)
            {
                if (Options[i] == data)
                {
                    Data = i;
                    return;
                }
            }
        }

        public override void SetDataList(List<string> data)
        {
            Options = data;
            Data = null;
        }

        public override object GetData()
        {
            if (Indexed || Data == null)
            {
                return Data;
            }
            else
            {
                return Options[(int)Data];
            }
        }

    }
}
