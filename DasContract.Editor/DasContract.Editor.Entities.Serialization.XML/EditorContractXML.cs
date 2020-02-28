using System;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using DasContract.Editor.Entities.Forms;
using System.Collections.Generic;
using DasContract.Editor.Entities.DataModels.Entities.Properties;
using System.Xml;

namespace DasContract.Editor.Entities.Serialization.XML
{
    public static class EditorContractXML
    {
        public static readonly Type EditorContractType = typeof(EditorContract);

        /// <summary>
        /// Parses a contract from an xml document and binds all cross-referenced pointers
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static EditorContract From(string xml)
        {
            //Deserialize
            var serializer = new XmlSerializer(EditorContractType);
            using var textReader = new StringReader(xml);
            using var xmlReader = XmlReader.Create(textReader);
            var deserializedContract = serializer.Deserialize(xmlReader) as EditorContract;

            SetReferencesInDataModels(deserializedContract);
            SetReferencesInProcesses(deserializedContract);

            return deserializedContract;
        }

        static void SetReferencesInDataModels(EditorContract contract)
        {
            //Set referenced values in data models
            foreach (var entity in contract.DataModel.Entities)
            {
                //Reference properties
                foreach (var property in entity.ReferenceProperties)
                    if (property.EntityId != null)
                        property.Entity = contract.DataModel.Entities
                            .Where(e => e.Id == property.EntityId)
                            .Single();
            }
        }

        static void SetReferencesInProcesses(EditorContract contract)
        {
            //Gather fields to bind
            var bindings = new List<ContractPropertyBinding>();

            //Start event
            if (contract.Processes.Main != null
                && contract.Processes.Main.StartEvent != null
                && contract.Processes.Main.StartEvent.StartForm != null)
                foreach (var field in contract.Processes.Main.StartEvent.StartForm.Fields)
                    if (field.PropertyBinding != null)
                        bindings.Add(field.PropertyBinding);

            //User forms
            if (contract.Processes.Main != null)
                foreach (var entity in contract.Processes.Main.UserActivities)
                    if (entity.Form != null)
                        foreach (var field in entity.Form.Fields)
                            if (field.PropertyBinding != null)
                                bindings.Add(field.PropertyBinding);

            //Gather all properties
            var properties = new List<ContractProperty>();
            foreach (var entity in contract.DataModel.Entities)
                properties = properties
                    .Concat(entity.PrimitiveProperties)
                    .Concat(entity.ReferenceProperties)
                    .ToList();

            //Bind property bindings
            foreach (var binding in bindings)
                if (binding.PropertyId != null)
                    binding.Property = properties.Where(e => e.Id == binding.PropertyId).Single();
        }

        /// <summary>
        /// Serializes a contract into xml format
        /// </summary>
        /// <param name="contract">The contract to serialize</param>
        /// <returns>Contract as xml</returns>
        public static string To(EditorContract contract)
        {
            var xml = new XmlSerializer(EditorContractType);
            using var textWriter = new StringWriter();
            xml.Serialize(textWriter, contract);
            return textWriter.ToString();
        }
    }
}
