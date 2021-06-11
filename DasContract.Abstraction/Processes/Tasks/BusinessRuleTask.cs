﻿using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class BusinessRuleTask : Task
    {
        /// <summary>
        /// A definition of a business rule in xml format. 
        /// </summary>
        public string BusinessRuleDefinitionXml { get; set; }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "BusinessRuleTask";
            xElement.Add(
                new XElement("BusinessRuleDefinition", BusinessRuleDefinitionXml));
            return xElement;
        }
    }
}
