using System;
using System.Collections.Generic;
using System.Text;
using DasContract.Editor.Entities.Forms;
using DasContract.Editor.Entities.Processes;

namespace DasContract.Editor.Entities.Integrity.Extensions
{
    public static class ContractProcessesExtensions
    {
        /// <summary>
        /// Returns all property bindings in the contract
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        public static List<ContractPropertyBinding> GetAllPropertyBindings(this ContractProcesses processes)
        {
            if (processes == null)
                throw new ArgumentNullException(nameof(processes));

            var res = new List<ContractPropertyBinding>();
            foreach (var activity in processes.Main.UserActivities)
            {
                foreach(var field in activity.Form.Fields)
                {
                    if (field.PropertyBinding != null)
                        res.Add(field.PropertyBinding);
                }
            }

            return res;
        }
    }
}
