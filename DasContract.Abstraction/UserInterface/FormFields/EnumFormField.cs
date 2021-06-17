using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DasContract.Abstraction.UserInterface.FormFields
{
    public class EnumFormField: FormField
    {
        [XmlAttribute("Vertical")]
        public bool Vertical { get; set; } = false;

        [XmlIgnoreAttribute]
        public int Data { get; set; } = -1;
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
            if (Data == 0 && Options.Count > 1)
            {
                Data = 1;
            }
            Data = 0;
        }

        public override object GetData()
        {
            if (Indexed)
            {
                return Data;
            }
            else
            {
                return Options[Data];
            }
        }
    }
}
