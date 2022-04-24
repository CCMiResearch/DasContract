using DasContract.Abstraction.Processes.Dmn;
using System;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class BusinessRuleTask : Task
    {
        /// <summary>
        /// A definition of a business rule in xml format. 
        /// </summary>
        public string BusinessRuleDefinitionXml { get; set; }

        public Definitions BusinessRule { get; set; } = new Definitions();

        public BusinessRuleTask() { }

        public BusinessRuleTask(XElement xElement) : base(xElement)
        {
            BusinessRuleDefinitionXml = xElement.Element("BusinessRuleDefinition")?.Value;
            if (BusinessRuleDefinitionXml != null)
            {
                try
                {
                    BusinessRule = Definitions.DeserializePlainDefinition(BusinessRuleDefinitionXml);
                }
                catch (Exception e) 
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.BUSINESS_RULE_TASK;
            xElement.Add(
                new XElement("BusinessRuleDefinition", BusinessRuleDefinitionXml));
            return xElement;
        }
    }
}
