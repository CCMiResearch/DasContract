using System;
using System.Collections.Generic;
using System.Linq;
using DasContract.Editor.Entities.Exceptions;
using DasContract.Editor.Entities.Integrity.Analysis;
using DasContract.Editor.Entities.Integrity.Analysis.Cases;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Primitive;
using DasContract.Editor.Entities.Integrity.Contract.DataModel.Entities.Properties.Reference;
using DasContract.Editor.Entities.Integrity.Contract.Processes.Process.Activities;
using DasContract.Editor.Entities.Interfaces;
using DasContract.Editor.Entities.Processes.Diagrams;
using DasContract.Editor.Entities.Processes.Factories;
using DasContract.Editor.Entities.Processes.Process;
using DasContract.Editor.Entities.Processes.Process.Activities;

namespace DasContract.Editor.Entities.Integrity.Contract.Processes
{
    public static class ContractProcessesIntegrity
    {
        public static void ReplaceSafely(this EditorContract contract, BPMNProcessDiagram newDiagram)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));

            if (BPMNProcessDiagram.IsNullOrEmpty(newDiagram))
            {
                contract.Processes.Diagram = BPMNProcessDiagram.Default();
                contract.Processes.Main = ContractProcess.Empty();
                return;
            }

            var newMainProcess = contract.ValidatePotentialDiagram(newDiagram);

            if (contract.Processes.Main != null)
                contract.AnalyzeIntegrityWhenReplacedWith(newDiagram).ResolveDeleteRisks();

            contract.Processes.Diagram = newDiagram;

            var oldProcess = contract.Processes.Main;
            var newProcess = newMainProcess;
            contract.Processes.Main = newProcess;

            if (newProcess == null || oldProcess == null)
                return;

            //Update activities
            UpdateMainProcessActivities(oldProcess.ScriptActivities, newProcess.ScriptActivities);
            UpdateMainProcessActivities(oldProcess.BusinessActivities, newProcess.BusinessActivities);
            UpdateMainProcessActivities(oldProcess.UserActivities, newProcess.UserActivities);

            //Update start events
            UpdateMainProcessActivities(oldProcess.StartEvents, newProcess.StartEvents);
        }

        public static ContractProcess ValidatePotentialDiagram(this EditorContract contract, BPMNProcessDiagram newDiagram)
        {
            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));

            var processes = ProcessFactory.FromXML(newDiagram.DiagramXML);
            if (processes.Count() != 1)
                throw new InvalidProcessCountException("The diagram must contain exactly one process");

            var newMainProcess = processes.First();

            if (!newMainProcess.StartEvents.Any())
                throw new AtLeastOneStartEventRequiredException("At least one start event is required");

            if (!newMainProcess.EndEvents.Any())
                throw new AtLeastOneEndEventRequiredException("At least one end event is required");

            return newMainProcess;
        }


        static void UpdateMainProcessActivities<TActivity>(IEnumerable<TActivity> oldActivities, IEnumerable<TActivity> newActivities)
            where TActivity : IDataCopyable<TActivity>, IIdentifiable
        {
            foreach (var item in oldActivities)
            {
                var res = newActivities.Where(e => e.Id == item.Id).SingleOrDefault();
                if (res != null)
                    res.CopyDataFrom(item);
            }
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityWhenReplacedWith(this EditorContract contract, BPMNProcessDiagram newDiagram)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (newDiagram == null)
                throw new ArgumentNullException(nameof(newDiagram));

            var deleteRisks = new List<ContractIntegrityAnalysisDeleteCase>();
            var childrenAnalyses = new List<ContractIntegrityAnalysisResult>();

            //Figure out activities that will be deleted
            var oldActivities = new List<ContractActivity>()
                .Concat(contract.Processes.Main.BusinessActivities)
                .Concat(contract.Processes.Main.UserActivities)
                .Concat(contract.Processes.Main.ScriptActivities)
                .ToList();


            var newMainProcess = ProcessFactory.FromXML(newDiagram.DiagramXML).First();
            var newActivities = new List<ContractActivity>()
                .Concat(newMainProcess.BusinessActivities)
                .Concat(newMainProcess.UserActivities)
                .Concat(newMainProcess.ScriptActivities)
                .ToList();

            var deletedActivities = new List<ContractActivity>();
            foreach(var oldActivity in oldActivities)
            {
                var linkedNewActivity = newActivities.Where(e => e.Id == oldActivity.Id).SingleOrDefault();
                if (linkedNewActivity == null)
                    deletedActivities.Add(oldActivity);
            }

            //Create delete risks
            foreach (var deletedActivity in deletedActivities)
                deleteRisks.Add(new ContractIntegrityAnalysisDeleteCase($"Activity {deletedActivity.Name} will be deleted", () => { }));

            //Add children risks
            foreach (var deletedActivity in deletedActivities)
                childrenAnalyses.Add(contract.AnalyzeIntegrityOf(deletedActivity));

            return new ContractIntegrityAnalysisResult(deleteRisks, childrenAnalyses);
        }
    }
}
