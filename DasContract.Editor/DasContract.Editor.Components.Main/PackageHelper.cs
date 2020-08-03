using System;
using System.Collections.Generic;
using System.Text;

namespace DasContract.Editor.Components.Main
{
    public static class LibraryInfo
    {
        public static string Name { get; set; } = typeof(LibraryInfo).Assembly.GetName().Name;
    }
}

