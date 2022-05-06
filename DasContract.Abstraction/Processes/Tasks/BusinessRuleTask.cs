using DasContract.Abstraction.Processes.Dmn;
using System;
using System.Linq;
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
            BusinessRuleDefinitionXml = ChangeDefaultIds(xElement.Element("BusinessRuleDefinition")?.Value);
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
            BusinessRuleDefinitionXml = ChangeDefaultIds(BusinessRuleDefinitionXml);
            xElement.Name = ElementNames.BUSINESS_RULE_TASK;
            xElement.Add(
                new XElement("BusinessRuleDefinition", BusinessRuleDefinitionXml));
            return xElement;
        }

        private string ChangeDefaultIds(string ruleDefinition)
        {
            const string idPool = "abcdefghijklmnopqrstuvwxyz0123456789";
            const string defaultDefinitionsId = "definitions_0qcte86";
            const string defaultDecisionId = "decision_0gdyta1";
            const string defaultTableId = "decisionTable_1v6173a";
            if (ruleDefinition.Contains(defaultDefinitionsId))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                var newDefinition = ruleDefinition;
                var newDefinitionsId = $"definitions_{new string(Enumerable.Range(0, 7).Select(x => idPool[random.Next(0, idPool.Length)]).ToArray())}";
                var newDecisionId = $"decision_{new string(Enumerable.Range(0, 7).Select(x => idPool[random.Next(0, idPool.Length)]).ToArray())}";
                var newTableId = $"decisionTable_{new string(Enumerable.Range(0, 7).Select(x => idPool[random.Next(0, idPool.Length)]).ToArray())}";
                newDefinition = newDefinition.Replace(defaultDefinitionsId, newDefinitionsId);
                newDefinition = newDefinition.Replace(defaultDecisionId, newDecisionId);
                newDefinition = newDefinition.Replace(defaultTableId, newTableId);
                return newDefinition;
            } 
            else
            {
                return ruleDefinition;
            }
        }
    }
}
