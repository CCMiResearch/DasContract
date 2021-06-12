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

        public Token() { }
        public Token(XElement xElement) : base(xElement)
        {

            Symbol = xElement.Element("Symbol")?.Value;
            MintScript = xElement.Element("MintScript")?.Value;
            TransferScript = xElement.Element("TransferScript")?.Value;

            if (bool.TryParse(xElement.Element("IsFungible")?.Value, out var isFungible))
                IsFungible = isFungible;
            if (bool.TryParse(xElement.Element("IsIssued")?.Value, out var isIssued))
                IsIssued = isIssued;
        }

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
