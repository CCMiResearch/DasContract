﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DasContract.Abstraction.Processes.Events
{
    public class Event : ProcessElement
    {
        public override XElement ToXElement()
        {
            var xElement = base.ToXElement();
            xElement.Name = "Event";
            return xElement;
        }
    }
}
