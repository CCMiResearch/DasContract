﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DasContract.Editor.Web.Services.BpmnEvents
{
    public class BpmnElementEvent
    {
        public string Type { get; set; }
        public BpmnElement Element { get; set; }
        public string NewId { get; set; }
    }
}
