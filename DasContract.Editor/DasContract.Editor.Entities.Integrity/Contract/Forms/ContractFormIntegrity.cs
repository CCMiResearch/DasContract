using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Integrity.Analysis;

namespace DasContract.Editor.Entities.Integrity.Contract.Forms
{
    public static class ContractFormIntegrity
    {

        public static void AddSafely(this EditorContract contract, ContractForm form, ContractFormField field)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (field == null)
                throw new ArgumentNullException(nameof(field));

            form.Fields.Add(field);
        }

        public static void RemoveSafely(this EditorContract contract, ContractForm form, ContractFormField field)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (field == null)
                throw new ArgumentNullException(nameof(field));

            //Remove all risks
            contract.AnalyzeIntegrityOf(form, field).ResolveDeleteRisks();

            //Remove this
            form.Fields.Remove(field);
        }

        public static ContractIntegrityAnalysisResult AnalyzeIntegrityOf(this EditorContract contract, ContractForm form, ContractFormField field)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));

            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return ContractIntegrityAnalysisResult.Empty();
        }
    }
}
