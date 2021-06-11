using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Data
{
    public class Token : Entity
    {
        public string Symbol { get; set; }
        public bool IsFungible { get; set; }
        public bool IsIssued { get; set; }

        public string MintScript { get; set; }
        public string TransferScript { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Token";
            xElement.Add(
                new XElement("Symbol", Symbol),
                new XElement("IsFungible", IsFungible),
                new XElement("IsIssued", IsIssued),
                new XElement("MintScript", MintScript),
                new XElement("TransferScript", TransferScript)
                );
            return xElement;
        }
    }
}
