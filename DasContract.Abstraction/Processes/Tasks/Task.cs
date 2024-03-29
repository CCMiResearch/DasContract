﻿using DasContract.Abstraction.Processes.Tasks;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Tasks
{
    public class Task : ProcessElement
    {
        public InstanceType InstanceType { get; set; }
        /// <summary>
        /// Numbers of repetitions for the process
        /// </summary>
        public string LoopCardinality { get; set; }
        /// <summary>
        /// ID of the referenced collection entity (must be an entity that is saved inside of the contract)
        /// </summary>
        public string LoopCollection { get; set; }

        public Task() { }
        public Task(XElement xElement) : base(xElement)
        {
            LoopCollection = xElement.Element("LoopCollection")?.Value;
            LoopCardinality = xElement.Element("LoopCardinality")?.Value;
            if (System.Enum.TryParse<InstanceType>(xElement.Element("InstanceType")?.Value, out var type))
                InstanceType = type;
        }

        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = ElementNames.TASK;
            xElement.Add(
                new XElement("InstanceType", InstanceType),
                new XElement("LoopCardinality", LoopCardinality),
                new XElement("LoopCollection", LoopCollection));
            return xElement;
        }
    }
}
