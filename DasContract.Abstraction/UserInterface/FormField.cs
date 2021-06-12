using DasContract.Abstraction.Data;
using System.Xml.Linq;

namespace DasContract.Abstraction.UserInterface
{
    public class FormField
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public FormFieldType Type { get; set; }
        /// <summary>
        /// A visible name displayed next to the field. 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// If false, an editor is shown. Otherwise property shown as a label. 
        /// </summary>
        public bool IsReadOnly { get; set; } = true;
        /// <summary>
        /// An expression binding. Entity.Property
        /// </summary>
        public string PropertyExpression { get; set; }

        public string CustomConfiguration { get; set; }

        public FormField() { }
        public FormField(XElement xElement)
        {
            Id = xElement.Attribute("Id")?.Value;
            DisplayName = xElement.Element("DisplayName")?.Value;
            PropertyExpression = xElement.Element("PropertyExpression")?.Value;
            CustomConfiguration = xElement.Element("CustomConfiguration")?.Value;

            if (bool.TryParse(xElement.Element("IsReadOnly")?.Value, out var isReadOnly))
                IsReadOnly = isReadOnly;
            if (int.TryParse(xElement.Element("Order")?.Value, out var order))
                Order = order;
            if (System.Enum.TryParse<FormFieldType>(xElement.Element("Type")?.Value, out var type))
                Type = type;
        }

        public XElement ToXElement()
        {
            return new XElement("FormField",
                new XAttribute("Id", Id),
                new XElement("Type", Type),
                new XElement("Order", Order),
                new XElement("DisplayName", DisplayName),
                new XElement("IsReadOnly", IsReadOnly),
                new XElement("PropertyExpression", PropertyExpression),
                new XElement("CustomConfiguration", CustomConfiguration)
            );
        }
    }
}
