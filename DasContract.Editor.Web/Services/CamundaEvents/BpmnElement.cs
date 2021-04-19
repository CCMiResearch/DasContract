using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.CamundaEvents
{
    public class BpmnElement
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsSequential { get; set; }
        public string LoopType { get; set; }
    }
}
