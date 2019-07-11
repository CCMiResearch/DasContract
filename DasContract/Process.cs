using DasContract.ProcessClasses;
using System;

namespace DasContract
{
    public class Process
    {
        public Guid Id;

        public string Name;

        public Transactor Root;

        public string[] Implementation;
    }
}